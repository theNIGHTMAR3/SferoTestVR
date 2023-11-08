using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    [SerializeField] private AudioSource playerSource;


	private void Awake()
	{
		if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

		if (!PlayerPrefs.HasKey("Audio"))
		{
			Debug.Log("Audio not set, setting it to 1.0");
			PlayerPrefs.SetFloat("Audio", 1.0f);
			PlayerPrefs.Save();
		}
        ChangeMasterVolume(PlayerPrefs.GetFloat("Audio"));
	}

    public void ChangeMasterVolume(float volume)
    {
        AudioListener.volume = volume;
		Debug.Log("Changed audio to: "+ volume);
	}


	public void PlaySoundOnPlayer(AudioClip clip)
    {
		playerSource.PlayOneShot(clip);
    }
}
