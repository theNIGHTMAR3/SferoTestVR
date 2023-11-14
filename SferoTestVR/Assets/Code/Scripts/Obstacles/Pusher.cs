using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// placed on a object will move as a pusher!
/// </summary>
public class Pusher : FourActionStateMachine
{

    [SerializeField] float extensionRange = 1f;
    [SerializeField] GameObject bars;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] AudioClip pusherExtending;

    //timers
    [SerializeField] float extendingTime = 0.25f;
    [SerializeField] float extendedTime = 0.25f;
    [SerializeField] float shorteningTime = 1;
    [SerializeField] float shortenTime = 1;

    Vector3 endPos;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	// Start is called before the first frame update
	void Start()
    {
        endPos = new Vector3(0,0, extensionRange);

        //set timers using human readeable names
        action1Time = shortenTime; //waiting to extend
        action2Time = extendingTime; //extending
        action3Time = extendedTime; //waiting to shorten
        action4Time = shorteningTime; //shortening

    }


    //waiting to extend
    protected override void Action1(float percentage)
    {
        SetPositionAndBarsScale(0);
    }

    //extending
    protected override void Action2(float percentage)
    {
        SetPositionAndBarsScale(percentage);
        
    }

	protected override void OnAction2Start()
	{
		audioSource.PlayOneShot(pusherExtending);
	}

	//waiting to shorten
	protected override void Action3(float percentage)
    {
        SetPositionAndBarsScale(1);
    }

    //shortening
    protected override void Action4(float percentage)
    {
        SetPositionAndBarsScale(1 - percentage);
	}

	protected override void OnAction4Start()
	{
		audioSource.PlayOneShot(pusherExtending);
	}

	void SetPositionAndBarsScale(float percentage)
    {
        transform.localPosition = Vector3.Lerp(Vector3.zero, endPos, percentage);

        bars.transform.localScale = new Vector3(1, 1, transform.localPosition.z / 0.1f);

    }    
}
