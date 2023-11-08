using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnPlayerCollision : MonoBehaviour
{

	[SerializeField] private AudioClip audioClip;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private bool playOnPlayer;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			if(playOnPlayer)
			{
				SoundManager.instance.PlaySoundOnPlayer(audioClip);
			}
			else
			{
				audioSource.PlayOneShot(audioClip);
			}
			
		}

	}

}
