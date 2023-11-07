using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
	[SerializeField] private AudioClip lavaSizzle;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			SoundManager.instance.PlaySound(lavaSizzle);
		}

	}

}
