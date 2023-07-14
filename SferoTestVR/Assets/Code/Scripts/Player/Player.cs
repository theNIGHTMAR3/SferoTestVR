using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    protected Rigidbody rigidbody;
    protected Camera camera;
    protected GameObject checkPoint;

    public float moveSpeed = 10f;
    public float sensitivity = 5f;

    protected UnityEngine.Vector2 playerInput;


    protected virtual void Start()
    {
        camera = Camera.main; //get camera
        rigidbody = GetComponent<Rigidbody>(); //get rigidbody

        // make sure cursor is locked and invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// not modified in derived class
    /// </summary>
    protected void Update()
    {
        GetInput();
    }

    /// <summary>
    /// Add force to sphere from Vector3
    /// </summary>    
    protected void Move(Vector3 direction)
    {

        Vector3 movement = direction;
        movement.y = 0;
        movement.Normalize();
        movement *= direction.magnitude;
        
        rigidbody.AddForce(movement);
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
        //TODO
    }

    /// <summary>
    /// starts revival coroutine/animation
    /// </summary>
    private void Revive()
    {
        //TODO
    }


    /// <summary>
    /// sets new checkpoint
    /// </summary>    
    public void SetNewCheckPoint(GameObject newCheckpoint)
    {
        //TODO
    }

    /// <summary>
    /// method for getting input from the device
    /// </summary>
    protected virtual void GetInput() { }
}
