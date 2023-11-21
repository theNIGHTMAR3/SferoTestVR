using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerMode
{
	PlayerPC,
	PlayerVRHome,
	PlayerVRSphere
}

public class PlayerLoader : MonoBehaviour
{
	[SerializeField] GameObject playerPCPrefab;
	[SerializeField] GameObject playerVRHomePrefab;
	[SerializeField] GameObject playerVRSpherePrefab;

	private void Awake()
	{
		if (!PlayerPrefs.HasKey("PlayerMode"))
		{
			Debug.Log("PlayerMode not set, setting it to PlayerPC");
			PlayerPrefs.SetInt("PlayerMode", (int)PlayerMode.PlayerPC);
			PlayerPrefs.Save();
		}

		// 0 - PlayerPC
		// 1 - PlayerVRHome
		// 2 - PlayerVRSphere
		if(GameObject.FindGameObjectWithTag("Player") == null)
		{
			int playerMode = PlayerPrefs.GetInt("PlayerMode");

			switch(playerMode)
			{
				case (int)PlayerMode.PlayerPC:
					Instantiate(playerPCPrefab);
					break;

				case (int)PlayerMode.PlayerVRHome:
					Instantiate(playerVRHomePrefab);
					break;

				case (int)PlayerMode.PlayerVRSphere:
					Instantiate(playerVRSpherePrefab);
					break;
			}
		}	
		


	}

}
