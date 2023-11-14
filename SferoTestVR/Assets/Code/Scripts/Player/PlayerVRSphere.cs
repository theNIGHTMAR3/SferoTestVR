using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtuSphereClient;
using VirtuSphereClient.Events;

public class PlayerVRSphere : Player
{

    [SerializeField] float speedMultiplier = 1;


    //connection to sphere
    [SerializeField] string hostIP="127.0.0.1";
    [SerializeField] int hostPort = 4445;


    //sphere measurements
    private VirtuSphere virtuSphere;
    SpherePoseEvent sphereInput;
    Dictionary<int, MotorStateEvent> motors = new Dictionary<int, MotorStateEvent>();

    //natural movement
    Vector2 estimatedDirection;
    
    protected override void Start()
    {
        base.Start();

        //setup sphere 
        virtuSphere = new VirtuSphere(VirtuSphere.ClientMode.CM_CONTROL_MOTORS);
        virtuSphere.onConnected += onConnected;
        virtuSphere.onDisconnected += onDisconnected;

        virtuSphere.registerEventListener<SpherePoseEvent>(onSpherePoseEvent);
        virtuSphere.registerEventListener<MotorStateEvent>(onMotorStateEvent);

        Debug.Log("STARTED CONNECTING");
        virtuSphere.connect(hostIP, hostPort);
         
    }


    protected override void Update()
    {
        base.Update();    
        if (virtuSphere != null)
            virtuSphere.onUpdate();

    }

    /// <summary>
    /// function to analyze input from the sphere. Called in update!
    /// </summary>
    protected override void GetInput()
    {
        if (sphereInput != null)
        {
            //get sphere veloticy direction
            Quaternion directionQuat = Quaternion.Euler(0.0f, sphereInput.getDirection(), 0.0f);
            Vector3 sphereDirection = directionQuat * new Vector3(0.0f, 0.0f, sphereInput.getVelocity());

            //MAY NEED SOME ADJUSTMENTS f.e.: multiplier or even rotation
            sphereDirection *= speedMultiplier;

            //take into account accumulate torque (f.e. from terrain slope)
            Vector3 accumulatedTorque = GetAccumulatedTorque();

            //get velocity from from engines currents
            Vector3 engineVelocity = EstimateEnginesVelocity();

            Vector3 playerVelocity = sphereDirection - engineVelocity;

            Vector3 newSphereVelocity = engineVelocity + playerVelocity + accumulatedTorque; //May need some multipliers               
            virtuSphere.setSpherePose(newSphereVelocity.magnitude, Vector3.SignedAngle(Vector3.right, newSphereVelocity, Vector3.forward)); //is forward in this up vector?

            //simple rotate the player the same way
            Move(sphereDirection);
            


        }
        else
        {
            //home tests for sphere
            //tests keeping player in the same position (negating slopes)            

            //take into account accumulate torque (f.e. from terrain slope)
            Vector3 accumulatedTorque = GetAccumulatedTorque();          

            //simple rotate the player the other way
            Move(-accumulatedTorque);
        }
    }

    
    /// <summary>
    /// Function for async getting input from the sphere
    /// </summary>
    /// <param name="evt"></param>
    private void onSpherePoseEvent(SpherePoseEvent evt)
    {
        sphereInput = evt;
    }


    /// <summary>
    /// Function for async getting engine info from the sphere
    /// </summary>
    /// <param name="evt"></param>
    private void onMotorStateEvent(MotorStateEvent evt)
    {
        motors[evt.getControllerId()]= evt;
    }


    private void onConnected()
    {
        Debug.Log("SPHERE CONNECTED");
        virtuSphere.requestSpherePoseUpdates(20);
    }

    private void onDisconnected()
    {
        Debug.Log("SPHERE DISCONNECTED");
    }


    #region Natural Movement

        /* id guessing!
                                                     /|\ z
                **************                        |
            **********************                    |
         ****************************                 -----> x
      ***********            ***********
     ********    ⎺⎺/|             ********
   ********      /                 ********
  *******     |⎺⎺|           |⎺⎺|      *******
 *******      |2|           |1|       *******
 ******       |_|           |_|        ******
******                         \        ******
******                         _\|        ******   --------------> forward
******                                  ******
******                        ⎺⎺/|       ******
******                        /         ******
 ******       |⎺⎺|           |⎺⎺|        ******
 *******      |3|           |0|       *******
  *******     |_|           |_|     *******
   ********      \                 ********
     ********    _\|             ********
      ***********            ***********
         ****************************
            **********************
                **************


     */


    /// <summary>
    /// Estimates desired velocity basing on engines currents, and rotation directions.   
    /// </summary>
    private Vector3 EstimateEnginesVelocity()
    {        
        if (motors.Count == 4)
        {
            MotorStateEvent mot0 = motors[0];
            MotorStateEvent mot1 = motors[1];


            //get rotating direction
            Vector3 mot0Dir = new Vector3(
                1* Mathf.Sign(mot0.getMotorVelocity()),
                0,
                1* Mathf.Sign(mot0.getMotorVelocity()));
            Vector3 mot1Dir = new Vector3(
                1 * Mathf.Sign(mot0.getMotorVelocity()),
                0,
                -1 * Mathf.Sign(mot0.getMotorVelocity()));

            //multiply by current
            mot0Dir *= mot0.getMotorCurrent();
            mot1Dir *= mot1.getMotorCurrent();

            //combine
            Vector3 estimatedDirection = mot0Dir + mot1Dir;

            //multiply by some factor
            estimatedDirection *= 1;

            return estimatedDirection;
        }
        else
        {
            //wait for all motors to send data
            return Vector3.zero;
        }
    }


    Vector3 angularVelocity=Vector3.zero;
    /// <summary>
    /// Function returning accumulated torque from the environment converted to 3d direction where the sphere should move
    /// w1 = w0+(E0 - E1)*deltaT
    /// w0,w1 - angular velocity before/now
    /// E0 -  player angular acceleration  
    /// E1 - environmetal angular acceleration  
    ///  | 
    ///  |
    /// \|/
    /// 
    /// E_1 = (w1-w0)/deltaT - E0
    /// </summary>
    /// <returns></returns>
    Vector3 GetAccumulatedTorque()
    {
        //           (w1-w0)                w1                        w0
        Vector3 angularVelocityDif = rigidbody.angularVelocity - angularVelocity;

        //    (w1-w0)/deltaT                        deltaT
        Vector3 angularAcc = angularVelocityDif / Time.fixedDeltaTime;

        //          E0
        Vector3 playerAngularAcc = addedTorque;

        //   (w1-w0)/deltaT - E0   
        Vector3 accumulatedTorque=  angularAcc - playerAngularAcc;  // should it also include angulardrag?

        //convert it to 3d Vector
        Vector3 TorqueToDirection = Quaternion.Euler(0, -90, 0) * accumulatedTorque;


        angularVelocity = rigidbody.angularVelocity;
        return TorqueToDirection;

    }
    #endregion
}
