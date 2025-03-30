using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudAudioCotroller : MonoBehaviour
{
	[SerializeField] private AudioSource playerMudAudioSource;
	[SerializeField] private float minSpeedSound = 0.25f;
	[SerializeField] private float maxSpeedSound = 5.0f;

	private Rigidbody rigidbody;

	private bool isOnMud = false;

	// Start is called before the first frame update
	void Start()
    {
		rigidbody = GetComponent<Rigidbody>();
	}

    void FixedUpdate()
    {
		CalculateMudVolume();
	}

	/// <summary>
	/// Calculates volume and pitch of mud sound
	/// </summary> 
	protected void CalculateMudVolume()
	{
		float currentPlayerSpeed = rigidbody.velocity.magnitude;
		float normalizedSpeed = Mathf.Clamp01((currentPlayerSpeed - minSpeedSound ) / (maxSpeedSound - minSpeedSound));

		playerMudAudioSource.volume = normalizedSpeed;
		playerMudAudioSource.pitch = Mathf.Lerp(1.0f, 2.0f, normalizedSpeed);

		if (isOnMud)
		{
			if (!playerMudAudioSource.isPlaying)
			{
				playerMudAudioSource.Play();
			}
		}
		else
		{
			playerMudAudioSource.Stop();
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.CompareTag("Mud") && !isOnMud)
		{
			isOnMud = true;
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.CompareTag("Mud"))
		{
			isOnMud = false;
		}
	}


}
