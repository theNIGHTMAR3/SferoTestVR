using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtuSphereClient;
using VirtuSphereClient.Events;
using System.Linq;

/// <summary>
/// smooth queue. Allows only pushing values. If it has
/// too many, it will remove the oldest.
/// It only returns the average of its elements
/// </summary>
public class SmoothQueue
{
    Queue<float> queue;
    int smoothness = 6;

    SmoothQueue()
    {
        queue = new Queue<float>();        
    }

    public void Push(float val)
    {
        queue.Enqueue(val);
        if(queue.Count > smoothness)
        {
            queue.Dequeue();
        }
    }

    public float GetSmooth()
    {
        return queue.Average();
    }
}


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
    Dictionary<int, SmoothQueue> motorCurrents = new Dictionary<int, SmoothQueue>();    

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


        InvokeRepeating("SetSpherePos", 0, 0.05f); //call 20 times per second
        
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();    
        if (virtuSphere != null)
            virtuSphere.onUpdate();

    }

    Vector3 accumulatedTorque = Vector3.zero;
    bool emergencySend = false;
    protected void SetSpherePos()
    {
        if (!emergencyState)
        {
            if (sphereInput != null)
            {
                //get sphere veloticy direction
                Quaternion directionQuat = Quaternion.Euler(0.0f, sphereInput.getDirection(), 0.0f);
                Vector3 sphereDirection = directionQuat * new Vector3(0.0f, 0.0f, sphereInput.getVelocity());

                //get velocity from from engines currents
                Vector3 engineVelocity = EstimateEnginesVelocity();

                Vector3 Δplayer = sphereDirection - engineVelocity;

                Vector3 actualSphereVelocity = new Vector3(sphereInput.getVelocityVectorX(), sphereInput.getVelocityVectorY(), sphereInput.getVelocityVectorZ());
                Debug.Log("Actual: " + actualSphereVelocity + " Estimated: " + engineVelocity);

                //torque seems to be 0 due to drag and angular drag

                sphereDirection += Δplayer + accumulatedTorque / 30.0f;
                virtuSphere.setSpherePose(sphereDirection.magnitude, Vector3.SignedAngle(Vector3.right, sphereDirection, Vector3.forward)); //is forward in this up vector?

                accumulatedTorque = Vector3.zero;
            }
        }
        else
        {
            if (!emergencySend)
            {
                virtuSphere.setSpherePose(0, 0);
                emergencySend = true;
                Debug.Log("EMERGENCY SPHERE STOP!!");
            }
        }
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

        if (naturalMovementOn)
        {
            if (sphereInput != null)
            {
                //get sphere veloticy direction
                Quaternion directionQuat = Quaternion.Euler(0.0f, sphereInput.getDirection(), 0.0f);
                Vector3 sphereDirection = directionQuat * new Vector3(0.0f, 0.0f, sphereInput.getVelocity());
                
                accumulatedTorque += GetAccumulatedTorque();

                //simple rotate the player the same way
                Move(sphereDirection * speedMultiplier);
            }
            Debug.Log(GetAccumulatedTorque());

            if (Input.GetKey(KeyCode.Space))
            {
                emergencyState = true;                
                Debug.Log("EMERGENCY BREAK PRESSED!!");
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
        motorCurrents[evt.getControllerId()].Push(evt.getMotorCurrent());
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

            float mot2Current = motorCurrents[mot2.getControllerId()].GetSmooth();
            float mot4Current = motorCurrents[mot4.getControllerId()].GetSmooth();            

            Debug.Log("Vel[2]= " + mot2.getMotorVelocity() + " Curr[2]= " + mot2Current + "Vel[4]= " + mot4.getMotorVelocity() + " Curr[4]= " + mot4Current);

            //get rotating direction
            Vector3 mot2Dir = new Vector3(1,0,-1) * Mathf.Sign(mot2.getMotorVelocity());
            Vector3 mot4Dir = new Vector3(1,0,1 ) * Mathf.Sign(mot4.getMotorVelocity());

            //multiply by current
            mot2Dir *= Mathf.Abs(mot2Current);
            mot4Dir *= Mathf.Abs(mot4Current);

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

        //E1
        Vector3 angularDrag = rigidbody.angularVelocity * rigidbody.drag;

        //   (w1-w0)/deltaT - E0 + E1  
        Vector3 accumulatedTorque =  angularAcc - playerAngularAcc - angularDrag;  

        //convert it to 3d Vector
        Vector3 TorqueToDirection = Quaternion.Euler(0, -90, 0) * accumulatedTorque;

        //save for the next frame
        angularVelocity = rigidbody.angularVelocity;
        return TorqueToDirection;

    }
    #endregion
}
