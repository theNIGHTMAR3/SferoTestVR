using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtuSphereClient;
using VirtuSphereClient.Events;

public class PlayerVRSphere : Player
{

    [SerializeField] float speedMultiplier = 1;
    [SerializeField] string hostIP="127.0.0.1";
    [SerializeField] int hostPort = 4445;    



    private VirtuSphere virtuSphere;

    //input from
    SpherePoseEvent sphereInput;


    protected override void Start()
    {
        base.Start();

         
        virtuSphere = new VirtuSphere(VirtuSphere.ClientMode.CM_CONTROL_MOTORS);
        virtuSphere.onConnected += onConnected;
        virtuSphere.onDisconnected += onDisconnected;

        virtuSphere.registerEventListener<SpherePoseEvent>(onSpherePoseEvent);

        Debug.Log("CONNECTING");
        virtuSphere.connect(hostIP, hostPort);
         
    }

    /// <summary>
    /// function to analyze input from the sphere. Called in update!
    /// </summary>
    protected override void GetInput()
    {
        if (sphereInput != null)
        {
            //get sphere veloticy direction
            
            Vector2 vel = new Vector2(
                sphereInput.getVelocityVectorX(),
                sphereInput.getVelocityVectorY()
                );
            
            //MAY NEED SOME ADJUSTMENTS f.e.: multiplier or even rotation
            vel *= speedMultiplier;

            //simple rotate the player the same way
            Move(vel);
            
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


    private void onConnected()
    {
        Debug.Log("SPHERE CONNECTED");
        virtuSphere.requestSpherePoseUpdates(20);
    }

    private void onDisconnected()
    {
        Debug.Log("SPHERE DISCONNECTED");
    }
    
}
