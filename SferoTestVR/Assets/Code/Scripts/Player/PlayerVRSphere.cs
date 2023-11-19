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
	[SerializeField] bool naturalMovementOn = true;


    bool emergencyState = false;
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


    protected override void FixedUpdate()
    {
        base.FixedUpdate();    
        if (virtuSphere != null)
            virtuSphere.onUpdate();

    }



    /***
     * Big disclaimer what's going on with natural movement assist
     * 
     * Basically, engines do whatever they can to keep the spheres speed. The only way 
     * to get the change the player want to make can be made from engines currents. For 
     * example if player wants to break, the engines will try harder, and the current will increase.
     * 
     * To calculate players influence (let's call it Δplayer), we can use the folloging equation:
     * 
     * sfere_speed = engine_speed + Δplayer
     * 
     * thus:
     * 
     * Δplayer = sfere_speed - engine_speed
     * 
     * 
     * where all variables above are vectors indicating the movement, speed direction 
     * 
     * engine_speed vector is an approximation how fast would the sphere rotate without the player's influence, but keeping the 
     * the same current
     * 
     * Each tick we simply do this transformation:
     * 
     * sfere_speed += Δplayer, which can be expanded to:
     * sfere_speed = 2*sfere_speed - engine_speed, or
     * sfere_speed += sfere_speed - engine_speed
     * 
     * 
     * 
     * 
     * Now more complex issue - the influence of inclined surfaces
     * 
     * The spheres speed should technicaly increase on the incline if left alone.
     * To calculate the the influence of the environment, we simply check what was the
     * difference of angular speed between two frames, also taking into account the players influence.
     * 
     * Moreover! The the way we can simulate that is easy!
     * 
     * We need to tweak the equation just a little bit:
     * 
     * sfere_speed += Δplayer + incline_speed
     * 
     * and it should work!
     */


    int counter = 0;
    /// <summary>
    /// function to analyze input from the sphere. Called in update!
    /// </summary>
    protected override void GetInput()
    {
        if (counter == 0)
        {
            if (!emergencyState)
            {
                if (naturalMovementOn)
                {
                    if (sphereInput != null)
                    {
                        //get sphere veloticy direction
                        Quaternion directionQuat = Quaternion.Euler(0.0f, sphereInput.getDirection(), 0.0f);
                        Vector3 sphereDirection = directionQuat * new Vector3(0.0f, 0.0f, sphereInput.getVelocity());

                        //take into account accumulate torque (f.e. from terrain slope)
                        Vector3 accumulatedTorque = GetAccumulatedTorque(); // may need some multipliers

                        //get velocity from from engines currents
                        Vector3 engineVelocity = EstimateEnginesVelocity();

                        Vector3 Δplayer = sphereDirection - engineVelocity;

                        Vector3 actualSphereVelocity = new Vector3(sphereInput.getVelocityVectorX(), sphereInput.getVelocityVectorY(), sphereInput.getVelocityVectorZ());
                        Debug.Log("Actual: " + actualSphereVelocity + " Estimated: " + engineVelocity);

                        sphereDirection += Δplayer + accumulatedTorque / 30.0f;
                        virtuSphere.setSpherePose(sphereDirection.magnitude, Vector3.SignedAngle(Vector3.right, sphereDirection, Vector3.forward)); //is forward in this up vector?

                        //simple rotate the player the same way
                        Move(sphereDirection * speedMultiplier);
                    }
                    else
                    { /*
                //home tests keeping player in the same position (negating slopes)            

                //take into account accumulate torque (f.e. from terrain slope)
                Vector3 accumulatedTorque = GetAccumulatedTorque();

                //simple rotate the player the other way
                Move(-accumulatedTorque);
				*/
                    }


                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        emergencyState = true;
                        virtuSphere.setSpherePose(0, 0);
                        Debug.Log("EMERGENCY BREAK!!");
                    }
                }
                else if (sphereInput != null)
                {
                    //get sphere veloticy direction
                    Quaternion directionQuat = Quaternion.Euler(0.0f, sphereInput.getDirection(), 0.0f);
                    Vector3 sphereDirection = directionQuat * new Vector3(0.0f, 0.0f, sphereInput.getVelocity());

                    //simple rotate the player the same way
                    Move(sphereDirection * speedMultiplier);
                }
            }
            else
            {
                virtuSphere.setSpherePose(0, 0);
            }
        }
        else
        {
            //counter++;
            //if(counter == 20) {
            //    counter = 0;
            //}
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
        virtuSphere.requestMotorStateUpdates(20);
    }

    private void onDisconnected()
    {
        Debug.Log("SPHERE DISCONNECTED");
    }


    #region Natural Movement

        /* id guessing!
                                                          
                **************                         
            **********************                     
         ****************************                 -----> z
      ***********            ***********              |  
     ********    ⎺⎺/|             ********             |
   ********      /                 ********          \|/
  *******     |⎺⎺|           |⎺⎺|      *******          x
 *******      |3|           |4|       *******
 ******       |_|           |_|        ******
******                        \         ******
******                        _\|         ******   --------------> forward
******       _                          ******
******      |\                          ******
******        \                         ******
 ******       |⎺⎺|           |⎺⎺|        ******
 *******      |1|           |2|       *******
  *******     |_|           |_|     *******
   ********                 /      ********
     ********             |/_    ********
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
            MotorStateEvent mot2 = motors[2];
            MotorStateEvent mot4 = motors[4];

            Debug.Log("Vel[2]= " + mot2.getMotorVelocity() + " Curr[2]= " + mot2.getMotorCurrent() + "Vel[4]= " + mot4.getMotorVelocity() + " Curr[4]= " + mot4.getMotorCurrent());

            //get rotating direction
            Vector3 mot2Dir = new Vector3(1,0,-1) * Mathf.Sign(mot2.getMotorVelocity());
            Vector3 mot4Dir = new Vector3(1,0,1 ) * Mathf.Sign(mot4.getMotorVelocity());

            //multiply by current
            mot2Dir *= Mathf.Abs(mot2.getMotorCurrent());
            mot4Dir *= Mathf.Abs(mot4.getMotorCurrent());

            //combine
            Vector3 estimatedDirection = mot2Dir + mot4Dir;

            //multiply by some factor
            estimatedDirection *= 0.1F;

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

        //save for the next frame
        angularVelocity = rigidbody.angularVelocity;
        return TorqueToDirection;

    }
    #endregion
}
