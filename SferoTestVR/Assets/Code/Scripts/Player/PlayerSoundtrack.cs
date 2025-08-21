using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerSoundtrack : MonoBehaviour
{

	[SerializeField] public List<AudioClip> allTracks = new List<AudioClip>();

	[SerializeField] private AudioSource audioSource;

	private Queue<AudioClip> playlist = new Queue<AudioClip>();
	private AudioClip lastPlayed;


	//private void Awake()
	//{
	//	audioSource = GetComponent<AudioSource>();
	//	if (audioSource == null)
	//		audioSource = gameObject.AddComponent<AudioSource>();

	//	audioSource.loop = false;
	//}


	private void Update()
	{
		if (!audioSource.isPlaying && playlist.Count > 0)
		{
			PlayNext();
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			PlayNext();
		}
	}


	/// <summary>
	/// run at the beginning of the round to generate music playlist
	/// </summary>
	public void StartNewRound()
	{
		UnityEngine.Debug.Log("started new round");
		GeneratePlaylist();
		PlayNext();
	}


	/// <summary>
	/// Plays next track and deletes previous one from playlist
	/// </summary>
	private void PlayNext()
	{
		if (playlist.Count == 0)
		{
			GeneratePlaylist();
		}

		if (playlist.Count > 0)
		{
			AudioClip next = playlist.Dequeue();
			audioSource.clip = next;
			audioSource.Play();
			lastPlayed = next;
		}
	}



	/// <summary>
	/// Generates new playlist
	/// </summary>
	private void GeneratePlaylist()
	{
		if (allTracks.Count == 0) return;

		List<AudioClip> shuffled = new List<AudioClip>(allTracks);
		Shuffle(shuffled);

		// if first == lastPlayed, change first
		if (lastPlayed != null && shuffled[0] == lastPlayed && shuffled.Count > 1)
		{
			// zamiana z losowym innym elementem
			int swapIndex = Random.Range(1, shuffled.Count);
			AudioClip temp = shuffled[0];
			shuffled[0] = shuffled[swapIndex];
			shuffled[swapIndex] = temp;
		}

		playlist = new Queue<AudioClip>(shuffled);
	}


	/// <summary>
	/// Shuffle function
	/// </summary>
	private void Shuffle<T>(List<T> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			int rand = Random.Range(i, list.Count);
			T temp = list[i];
			list[i] = list[rand];
			list[rand] = temp;
		}
	}





	
}
