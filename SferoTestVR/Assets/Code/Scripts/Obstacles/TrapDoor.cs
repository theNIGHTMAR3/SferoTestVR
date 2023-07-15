using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    GameObject door1, door2;

    [SerializeField] float openingTime = 0.5f;
    /// <summary>  in degrees </summary>
    [SerializeField] float angle = 75;
    bool opened = false;



    // Start is called before the first frame update
    void Start()
    {
        door1 = transform.GetChild(0).gameObject;
        door2 = transform.GetChild(1).gameObject;
    }


    IEnumerator Open()
    {
        float startTime = Time.time;
        float lerpedAngle;
        while(Time.time < startTime + openingTime)
        {
            lerpedAngle = Mathf.Lerp(0,angle,(Time.time - startTime)/openingTime);

            door1.transform.localRotation = Quaternion.Euler( new Vector3 (0,0, -lerpedAngle));
            door2.transform.localRotation = Quaternion.Euler( new Vector3 (0,0, lerpedAngle));
            yield return null;  
        }

        door1.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -angle));
        door2.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(opened == false)
        {
            opened = true;
            StartCoroutine(Open()); 
        }
    }
}
