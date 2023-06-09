using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rigidbody;
    Camera camera;
    GameObject checkPoint;


    protected void Start()
    {
        
        camera = Camera.main; //get camera
        rigidbody = GetComponent<Rigidbody>(); //get rigidbody
    }

    /// <summary>
    /// not modified in derived class
    /// </summary>
    protected void Update()
    {
        GetInput();
    }

    /// <summary>
    /// movement is 2D, so it call 2D version of Move method
    /// </summary>    
    protected void Move(Vector3 direction)
    {
        Move((Vector2)direction);
    }


    /// <summary>
    /// Add force or momentForce in the inputed direction
    /// </summary>
    /// <param name="direction"></param>
    protected void Move(Vector2 direction)
    {
        //TODO
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
    protected void Revive()
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
