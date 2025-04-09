using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDeleter : MonoBehaviour
{
    private bool deleted = false;

    private void Start()
    {
        var roomParent = transform.GetComponentInParent<Room>();
        if (roomParent == null)
        {
            Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!deleted && other.gameObject.CompareTag("Player"))
        {
            MapLoader.Instance.DeleteRoom();
            deleted = true;
        }
    }
}
