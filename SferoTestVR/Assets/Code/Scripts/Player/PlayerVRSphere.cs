#define LOG




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtuSphereClient;
using VirtuSphereClient.Events;
using System.Linq;



public class PlayerVRSphere : Player
{    

    //connection to sphere
    [SerializeField] string hostIP="127.0.0.1";
    [SerializeField] int hostPort = 4445;


    //sphere measurements
    private VirtuSphere virtuSphere;
    SpherePoseEvent sphereInput;
    SmoothVectorQueue sphereDirectionQueue = new SmoothVectorQueue();
    Dictionary<int, MotorStateEvent> motors = new Dictionary<int, MotorStateEvent>();
    Dictionary<int, SmoothQueue> motorCurrents = new Dictionary<int, SmoothQueue>();    

    //natural movement    
	[SerializeField] bool naturalMovementOn = true;
    [SerializeField] float paramA=2, paramB=2;

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

        for (int i = 1; i <= 4; i++)
            motorCurrents.Add(i, new SmoothQueue());

        Debug.Log("STARTED CONNECTING");
        virtuSphere.connect(hostIP, hostPort);


        InvokeRepeating("SetSpherePos", 0, 0.05f); //call 20 times per second
        
    }

    private void OnDisable()
    {
        if (virtuSphere.isConnected())
        {
            virtuSphere.disconnect();
            virtuSphere = null;
        }
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
        if (sphereInput != null)
        {
            //get sphere veloticy direction                
            Vector3 sphereDirection = sphereDirectionQueue.GetSmooth();


            accumulatedTorque += GetAccumulatedTorque();

            //simple rotate the player the same way
            Move(sphereDirection);
        }
        //Debug.Log(GetAccumulatedTorque());

        if (Input.GetKey(KeyCode.Space))
        {
            emergencyState = true;                
            Debug.Log("EMERGENCY BREAK PRESSED!!");
        }  
    }

    
    /// <summary>
    /// Function for async getting input from the sphere
    /// </summary>
    /// <param name="evt"></param>
    private void onSpherePoseEvent(SpherePoseEvent evt)
    {
        sphereInput = evt;        

        sphereDirectionQueue.Push(new Vector3(evt.getVelocityVectorX(),0,evt.getVelocityVectorY()));

    }


    /// <summary>
    /// Function for async getting engine info from the sphere
    /// </summary>
    /// <param name="evt"></param>
    private void onMotorStateEvent(MotorStateEvent evt)
    {
        motors[evt.getControllerId()]= evt;
        float current = evt.getMotorCurrent();
        if (Mathf.Abs(current) < 0.4f) current = 0;
        motorCurrents[evt.getControllerId()].Push(current);
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

    Vector3 accumulatedTorque = Vector3.zero;
    bool emergencySend = false;
    protected void SetSpherePos()
    {
        if (!emergencyState)
        {
            if (sphereInput != null)
            {                
                
                Vector3 sphereDirection = sphereDirectionQueue.GetSmooth();

                //get velocity from from engines currents
                Vector3 engineVelocity = EstimateEnginesVelocity();

                Vector3 Δplayer = sphereDirection - engineVelocity;
                Debug.DrawRay(transform.position - new Vector3(0.0f, 0.5f, 0.0f), sphereDirection, Color.yellow, 0.05f);
                Debug.DrawRay(transform.position - new Vector3(0.0f, 0.5f, 0.0f), engineVelocity, Color.blue, 0.05f);

                Vector3 actualSphereVelocity = new Vector3(sphereInput.getVelocityVectorX(), sphereInput.getVelocityVectorY(), sphereInput.getVelocityVectorZ());
                //Debug.Log("Actual: " + actualSphereVelocity + " Estimated: " + engineVelocity);

                sphereDirection += Δplayer; //+ accumulatedTorque / 30.0f;

                if (naturalMovementOn)
                {
                    virtuSphere.setSpherePose(sphereDirection.magnitude * 0.5f, Vector3.SignedAngle(Vector3.forward, sphereDirection, Vector3.up)); //is forward in this up vector?
                }
                //Debug.DrawRay(transform.position - new Vector3(0.0f, 0.5f, 0.0f), sphereDirection, Color.yellow, 0.05f);

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
******                        _\|        ******   --------------> forward
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
    /// returns the strength on an engine relatively to its current
    /// </summary>
    /// <param name="current">absoulte value of a current</param>
    /// <returns></returns>
    private float EngineStrength(float current)
    {        


#if LINEAR
        return current * paramA;
#elif POWER
        return Mathf.Pow(current,paramA)*paramB;
#elif SQUARE_ROOT
        return Mathf.Sqrt(current) * paramA;
#elif LOG
        return Mathf.Log(current,paramA) * paramB;
#endif

        return 0;
    }


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

            Debug.Log(  "Vel[2]= " + mot2.getMotorVelocity().ToString("00.0") + 
                        " Curr[2]= " + mot2Current.ToString("00.0") + 
                        " Vel[4]= " + mot4.getMotorVelocity().ToString("00.0") + 
                        " Curr[4]= " + mot4Current.ToString("00.0"));

            //get rotating direction
            Vector3 mot2Dir = new Vector3(1,0,-1) * Mathf.Sign(mot2Current);
            Vector3 mot4Dir = new Vector3(1,0,1 ) * Mathf.Sign(mot4Current);

            //multiply by strength 
            mot2Dir *= EngineStrength(Mathf.Abs(mot2Current));
            mot4Dir *= EngineStrength(Mathf.Abs(mot4Current));

            //combine
            Vector3 estimatedDirection = mot2Dir + mot4Dir;            

            Debug.DrawRay(transform.position - new Vector3(0.0f, 0.75f, 0.0f), mot2Dir, Color.red, 0.05f);
            Debug.DrawRay(transform.position - new Vector3(0.0f, 0.75f, 0.0f), mot4Dir, Color.green, 0.05f);

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

        //d
        Vector3 angularDrag = rigidbody.angularVelocity * rigidbody.drag;

        //   E1 = (w1-w0)/deltaT - E0 - d
        Vector3 accumulatedTorque =  angularAcc - playerAngularAcc - angularDrag;  

        //convert it to 3d Vector
        Vector3 TorqueToDirection = Quaternion.Euler(0, -90, 0) * accumulatedTorque;

        //save for the next frame
        angularVelocity = rigidbody.angularVelocity;
        return TorqueToDirection;

    }
#endregion

#region Tracking
    public override MotorRecords GetMotorsRecords()
    {
        MotorRecord motor1 = new MotorRecord(
            motors[1].getMotorCurrent(),
            motors[1].getMotorVoltage(),
            motors[1].getMotorVelocity()
            );
        MotorRecord motor2 = new MotorRecord(
            motors[2].getMotorCurrent(),
            motors[2].getMotorVoltage(),
            motors[2].getMotorVelocity()
            );
        MotorRecord motor3 = new MotorRecord(
            motors[3].getMotorCurrent(),
            motors[3].getMotorVoltage(),
            motors[3].getMotorVelocity()
            );
        MotorRecord motor4 = new MotorRecord(
            motors[4].getMotorCurrent(),
            motors[4].getMotorVoltage(),
            motors[4].getMotorVelocity()
            );


        return new MotorRecords(motor1,motor2,motor3,motor4);
    }

    public override SphereRecord GetSphereRecord()
    {        
        return new SphereRecord(
            sphereInput.getTimestamp(),
            sphereInput.getVelocity(),
            sphereInput.getDirection(),
            sphereInput.getVelocityVectorX(),
            sphereInput.getVelocityVectorY(),
            sphereInput.getVelocityVectorZ());
    }
#endregion
}
