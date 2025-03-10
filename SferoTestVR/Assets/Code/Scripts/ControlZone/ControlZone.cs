using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class ControlZone : MonoBehaviour
{
	

	[SerializeField] protected float moveSpeed = 0.1f;
	[SerializeField] protected List<GameObject> middlePoints = new List<GameObject>();

	[SerializeField] protected GameObject startPoint;
	[SerializeField] protected GameObject endPoint;

	private Queue<GameObject> pointsQueue = new Queue<GameObject>();

	protected Rigidbody playerRb;
	protected Player playerController;
	private float offset = 0.5f;
	protected bool isMoving = false;

	// serialize dor debugging
	[SerializeField] protected GameObject targetPoint;
	


	private void FixedUpdate()
	{
		if (isMoving)
		{
			MoveToTarget();

			// check if is close to point
			if (Vector3.Distance(playerRb.position, targetPoint.transform.position) < offset)
			{
				// point reached, selecting next
				if (pointsQueue.Count > 0)
				{
					targetPoint = pointsQueue.Dequeue();
				}
				// out of point, restore control
				else
				{
					RestoreControl();
				}
			}

			// of player accidently dies, restart control
			if(!playerController.GetIsAlive())
			{
				isMoving = false;
				targetPoint = null;
			}
		}	
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !isMoving)
		{
			playerRb = other.GetComponent<Rigidbody>();
			playerController = other.GetComponent<Player>();

			if (playerRb != null && playerController != null)
			{
				playerRb.velocity = Vector3.zero;
				playerRb.angularVelocity = Vector3.zero;
				playerController.SetPlayerControlsSelf(false);

				CreatePointsList();
				isMoving = true;
			}
		}
	}


	/// <summary>
	/// Moves player to set target point
	/// </summary>
	virtual protected void MoveToTarget()
	{
		Vector3 direction = (targetPoint.transform.position - playerRb.position).normalized;
		playerController.Move(direction, moveSpeed);
	}


	/// <summary>
	/// Restores player's control
	/// </summary>
	private void RestoreControl()
	{
		isMoving = false;
		playerController.SetPlayerControlsSelf(true);
	}


	/// <summary>
	/// Creats points list to follow
	/// </summary>
	private void CreatePointsList()
	{
		pointsQueue.Clear();
		pointsQueue.Enqueue(startPoint);
		foreach(GameObject point in middlePoints) 
		{ 
			pointsQueue.Enqueue(point); 
		}
		pointsQueue.Enqueue(endPoint);

		targetPoint = pointsQueue.Dequeue();
	}
}
