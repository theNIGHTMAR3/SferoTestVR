using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MapCreatorMenu : MonoBehaviour
{
	public GameObject tilePrefab; 
	public Transform availableRoomsPanel;
	public Transform currentRoomsPanel;

	private List<String> availableRooms;
	private List<String> currentRooms;


	private void Start()
	{
		currentRooms = new List<String>();
		availableRooms = MapConfigManager.GetRoomsList();
		GenerateTiles();
	}


	/// <summary>
	/// generates all room tiles
	/// </summary>    
	private void GenerateTiles()
	{
		DeleteContents(availableRoomsPanel);
		foreach(string room in availableRooms)
		{
			GenerateAvailableRoomTile(room);	
		}
	}

	/// <summary>
	/// generates all current room tiles
	/// </summary>    
	private void GenerateCurrentRoomTiles()
	{
		DeleteContents(currentRoomsPanel);
		foreach (string room in currentRooms)
		{
			GenerateCurrentRoomTile(room);
		}
	}

	/// <summary>
	/// generates single available room tiles showing romm thumbnail and name
	/// </summary> 
	private void GenerateAvailableRoomTile(string room)
	{
		GameObject tile = Instantiate(tilePrefab, availableRoomsPanel);

		tile.GetComponentInChildren<RawImage>().texture = MapConfigManager.GetRoomImage(room);

		tile.GetComponent<Button>().targetGraphic = tile.GetComponentInChildren<RawImage>();

		tile.GetComponentInChildren<TextMeshProUGUI>().text = room;


		tile.GetComponent<Button>().onClick.AddListener(() => AddRoomToCurrentMap(room));
	}

	/// <summary>
	/// generates single current room tiles showing romm thumbnail and name
	/// </summary> 
	private void GenerateCurrentRoomTile(string room)
	{
		GameObject tile = Instantiate(tilePrefab, currentRoomsPanel);

		tile.GetComponentInChildren<RawImage>().texture = MapConfigManager.GetRoomImage(room);

		tile.GetComponent<Button>().targetGraphic = tile.GetComponentInChildren<RawImage>();

		tile.GetComponentInChildren<TextMeshProUGUI>().text = room;


		tile.GetComponent<Button>().onClick.AddListener(() => RemoveRoomFromCurrentMap(room));
	}



	/// <summary>
	/// Adds selected room to current map
	/// </summary> 
	public void AddRoomToCurrentMap(string roomName)
	{
		Debug.Log("Adding room: " + roomName+ " to current map");
		currentRooms.Add(roomName);
		GenerateCurrentRoomTiles();
	}


	/// <summary>
	/// Rremoves selected roomt from current map
	/// </summary> 
	public void RemoveRoomFromCurrentMap(string roomName)
	{
		Debug.Log("Removing room: " + roomName + " from current map");
		currentRooms.Remove(roomName);
		GenerateCurrentRoomTiles();
	}


	/// <summary>
	/// generates map by saving locally room name list
	/// </summary> 
	public void GenerateMap()
	{
		if (currentRooms.Count != 0)
		{
			string combinedString = string.Join(",", currentRooms);
			Debug.Log("Generating map with given rooms: "+ combinedString);
			MapConfigManager.SaveLocally(currentRooms);
		}
		else
		{
			Debug.Log("Can not generate empty map");
		}
	}

	/// <summary>
	/// deletes contents from tile current rooms panel
	/// </summary> 
	public void ClearCurrentRooms()
	{
		currentRooms.Clear();
		DeleteContents(currentRoomsPanel);
		Debug.Log("Cleared all current rooms");
	}

	/// <summary>
	/// deletes contents from tile panel
	/// </summary> 
	private void DeleteContents(Transform panel)
	{
		foreach (Transform child in panel)
		{
			Destroy(child.gameObject);
		}
	}



	/*for (int i = 0; i<availableRooms.Count; i++)
		{
			GameObject tile = Instantiate(tilePrefab, contentParent);

	tile.GetComponentInChildren<RawImage>().texture = MapConfigManager.GetRoomImage(availableRooms[i]);
			
			tile.GetComponentInChildren<TextMeshProUGUI>().text = availableRooms[i];



			int roomIndex = i; // Aby przekazać poprawny indeks do funkcji
							   //tile.GetComponent<Button>().onClick.AddListener(() => OnTileClicked(roomIndex));
	}*/
}

