using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    protected Rigidbody rigidbody;
    protected Camera camera;
    protected CameraFollower cameraScript;
    protected UnityEngine.Vector2 playerInput;

    public float moveSpeed = 10f;
    public float sensitivity = 5f;

   
    private Vector3 lastCheckpointPos;
    private Quaternion savedRotation;

    private bool isAlive = true;


    protected virtual void Start()
    {
        camera = Camera.main; //get camera
        rigidbody = GetComponent<Rigidbody>(); //get rigidbody
        cameraScript = camera.GetComponent<CameraFollower>();

        // save player start position
        lastCheckpointPos = transform.position;
    }

    /// <summary>
    /// not modified in derived class
    /// </summary>
    protected void Update()
    {
        GetInput();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Die();
        }

    }

    /// <summary>
    /// Add force to sphere from Vector3
    /// </summary>    
    protected void Move(Vector3 direction)
    {

        if(isAlive)
        {
            Vector3 movement = direction;
            movement.y = 0;
            movement.Normalize();
            movement *= direction.magnitude;

            rigidbody.AddForce(movement);
        }
    }


    /// <summary>
    /// Add force to sphere from Vector2
    /// </summary>
    protected void Move(Vector2 direction)
    {
        rigidbody.AddForce(new Vector3(direction.x, 0, direction.y));
    }


    /// <summary>
    /// starts death coroutine/animation, and after that Revive method
    /// </summary>
    public void Die()
    {
        if(isAlive) 
        {
            //TODO
            // animations
            // death screen
            // saving path
            // other stuff

            isAlive = false;

            Revive();
        }
   
    }

    /// <summary>
    /// starts revival coroutine/animation
    /// </summary>
    private void Revive()
    {
        transform.position = lastCheckpointPos;
        cameraScript.SetCameraRotation(savedRotation);
        StartCoroutine(FreezePlayer(1f));
    }


    /// <summary>
    /// sets new checkpoint
    /// </summary>    
    public void SetNewCheckPoint(GameObject newCheckpoint)
    {
        lastCheckpointPos = newCheckpoint.transform.position;
        savedRotation = newCheckpoint.transform.rotation;
    }

    /// <summary>
    /// checks of player collided with a trap
    /// </summary>    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Threat"))
        {
            Die();
        }
        
    }

    /// <summary>
    /// freezes player for given amount of seconds
    /// </summary> 
    IEnumerator FreezePlayer(float seconds)
    {
        rigidbody.isKinematic = true;
        yield return new WaitForSeconds(seconds);
        rigidbody.isKinematic = false;
        isAlive = true;
    }

    /// <summary>
    /// method for getting input from the device
    /// </summary>
    protected virtual void GetInput() { }
}
