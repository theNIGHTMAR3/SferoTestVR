using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlZone : MonoBehaviour
{

	[SerializeField] private float moveSpeed = 0.1f;

	private GameObject startPoint;
	private GameObject endPoint;

	private Rigidbody playerRb;
	private PlayerPCController playerController;

	
	private bool isMoving = false;
	private bool reachedEnd = false;

	// Start is called before the first frame update
	void Start()
    {
		endPoint = GameObject.Find("EndPoint");
		startPoint = GameObject.Find("StartPoint");
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !isMoving)
		{
			// get players rigidbody and script
			playerRb = other.GetComponent<Rigidbody>();
			playerController = other.GetComponent<PlayerPCController>();

			if (playerRb != null && playerController != null)
			{
				playerRb.velocity = Vector3.zero;
				playerRb.angularVelocity = Vector3.zero;
				playerController.hasControl = false;
				isMoving = true;

				// Najpierw doprowadzenie do StartPoint, potem do EndPoint
				StartCoroutine(MoveToStartPoint());
			}
		}
	}

	/// <summary>
	/// Moves player slowely to starting point
	/// </summary> 
	private IEnumerator MoveToStartPoint()
	{
		while (Vector3.Distance(playerRb.position, startPoint.transform.position) > 0.5f)
		{
			Vector3 direction = (startPoint.transform.position - playerRb.position).normalized;
			playerController.Move(direction, moveSpeed);
			yield return null;
		}

		StartCoroutine(MoveToEndPoint());
	}


	/// <summary>
	/// Moves player from start point to end point
	/// </summary> 
	private IEnumerator MoveToEndPoint()
	{
		while (Vector3.Distance(playerRb.position, endPoint.transform.position) > 0.5f)
		{
			Vector3 direction = (endPoint.transform.position - playerRb.position).normalized;
			playerController.Move(direction, moveSpeed);
			yield return null;
		}

		RestoreControl();
	}

	private void RestoreControl()
	{
		isMoving = false;
		playerController.hasControl = true;
	}

}
