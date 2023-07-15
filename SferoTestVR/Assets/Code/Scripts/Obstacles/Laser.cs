using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : FourActionStateMachine
{
    //timers
    [SerializeField] float extendingTime = 0.25f;
    [SerializeField] float extendedTime = 0.5f;    
    [SerializeField] float shortenedTime = 1;

    [SerializeField] float range = 2f;
    [SerializeField] float width = 0.25f;


    [SerializeField] LineRenderer laserLine;
    [SerializeField] GameObject collider;
    [SerializeField] ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {        
        //set timers using human readeable names
        action1Time = shortenedTime; //waiting to extend
        action2Time = extendingTime; //extending
        action3Time = extendedTime; //waiting to shorten
        action4Time = extendingTime; //shortening

        laserLine.startWidth = width;
        collider.transform.localScale = new Vector3(width, 0, width);        
        var main = particles.main;
        main.startLifetime = extendedTime;
        main.startSize = width;
    }


    /// <summary>
    /// laser hidden
    /// </summary>  
    protected override void Action1(float percentage)
    {
        laserLine.SetPosition(0, Vector3.zero);
        laserLine.SetPosition(1, Vector3.zero);

        Vector3 scale = collider.transform.localScale;
        scale.y = 0;
        collider.transform.localScale = scale;

        collider.transform.localPosition = Vector3.zero;
    }

    protected override void OnAction2Start()
    {
        particles.Play();
    }

    /// <summary>
    /// extending laser 
    /// </summary>    
    protected override void Action2(float percentage)
    {
        laserLine.SetPosition(0, Vector3.zero);      
        laserLine.SetPosition(1, new Vector3(0, 0, percentage * range));

        Vector3 scale = collider.transform.localScale;
        scale.y = range * percentage;
        collider.transform.localScale = scale;

        collider.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// laser fully on
    /// </summary>   
    protected override void Action3(float percentage)
    {
        laserLine.SetPosition(0, Vector3.zero);
        laserLine.SetPosition(1, new Vector3(0, 0, range));

        Vector3 scale = collider.transform.localScale;
        scale.y = range;
        collider.transform.localScale = scale;
    }

    /// <summary>
    /// shortening laser 
    /// </summary>    
    protected override void Action4(float percentage)
    {
        laserLine.SetPosition(0, new Vector3(0, 0, percentage * range));
        laserLine.SetPosition(1, new Vector3(0, 0, range));

        Vector3 scale = collider.transform.localScale;
        scale.y = range*(1- percentage);
        collider.transform.localScale = scale;

        collider.transform.localPosition = new Vector3(0, range*percentage, 0);
    }    
}
