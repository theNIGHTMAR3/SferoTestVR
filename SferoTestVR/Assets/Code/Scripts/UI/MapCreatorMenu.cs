using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class MapCreatorMenu : MonoBehaviour
{
	public GameObject tilePrefab; 
	public Transform availableRoomsPanel;
	public Transform currentRoomsPanel;

	private List<GameObject> availableRooms;
	private List<string> availableRoomsString;
	private List<GameObject> currentRooms;
	private static int idCount = 0;


	private void Start()
	{
		currentRooms = new List<GameObject>();
		availableRooms = new List<GameObject>();
		availableRoomsString = MapConfigManager.GetRoomsList();	

		// remove Start and End Rooms from MapCreator
		availableRoomsString.Remove("Start Room");
		availableRoomsString.Remove("End Room");
		GenerateTiles();
	}


	/// <summary>
	/// generates all room tiles
	/// </summary>    
	private void GenerateTiles()
	{
		DeleteContents(availableRoomsPanel);
		foreach(string room in availableRoomsString)
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
		foreach (GameObject room in currentRooms)
		{
			GenerateCurrentRoomTile(room);
		}
	}

	/// <summary>
	/// generates single available room tiles showing romm thumbnail and name
	/// </summary> 
	private void GenerateAvailableRoomTile(string room)
	{

		GameObject tile = GenerateTile(room,availableRoomsPanel);

		tile.GetComponent<RoomTile>().isSelected = false;

		availableRooms.Add(tile);
	}

	/// <summary>
	/// generates single current room tiles showing romm thumbnail and name
	/// </summary> 
	private void GenerateCurrentRoomTile(GameObject roomTile)
	{

		GameObject tile = GenerateTile(roomTile.name,currentRoomsPanel);

		tile.GetComponent<RoomTile>().isSelected = true;
		tile.GetComponent<RoomTile>().tileID = roomTile.GetComponent<RoomTile>().tileID;
	}

	/// <summary>
	/// generates single room tiles
	/// </summary> 
	private GameObject GenerateTile(string room,Transform panel)
	{
		GameObject tile = Instantiate(tilePrefab, panel);

		tile.name = room;
		tile.GetComponentInChildren<TextMeshProUGUI>().text = room;

		tile.GetComponentInChildren<RawImage>().texture = MapConfigManager.GetRoomImage(room);
		tile.GetComponent<Button>().targetGraphic = tile.GetComponentInChildren<RawImage>();

		tile.GetComponent<Button>().onClick.AddListener(() => ClickedTile(tile));

		return tile;
	}

	/// <summary>
	/// Handles logic when clicked on tile
	/// </summary> 
	public void ClickedTile(GameObject tile)
	{
		if (tile == null) return;

		// current room tile
		if(tile.GetComponent<RoomTile>().isSelected)
		{
			GameObject toRemove = currentRooms.FirstOrDefault(r => r.GetComponent<RoomTile>().tileID == tile.GetComponent<RoomTile>().tileID);

			currentRooms.Remove(toRemove);

			Debug.Log("Removed room: " + tile.name + " from current map");

			GenerateCurrentRoomTiles();
		}
		// available room tile
		else
		{
			GameObject selectedTile = Instantiate(tile);
			selectedTile.name = tile.name;
			selectedTile.GetComponent<RoomTile>().tileID = idCount++;	
			selectedTile.GetComponent<RoomTile>().isSelected = true;
			currentRooms.Add(selectedTile);

			Debug.Log("Added room: " + tile.name + " to current map");
			GenerateCurrentRoomTiles();
		}
	}


	/// <summary>
	/// generates map by saving locally room name list
	/// </summary> 
	public void GenerateMap()
	{
		if (currentRooms.Count != 0)
		{
			List<string> selectedRooms = new List<string>();
			foreach(GameObject room in currentRooms)
			{
				selectedRooms.Add(room.name);
			}

			string combinedString = string.Join(",", selectedRooms);
			MapConfigManager.SaveLocally(selectedRooms);
			Debug.Log("Generating map with given rooms: "+ combinedString);
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



}

