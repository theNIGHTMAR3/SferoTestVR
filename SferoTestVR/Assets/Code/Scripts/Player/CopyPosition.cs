using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField] GameObject gameObject;
    [SerializeField] Vector3 offset = Vector3.zero;


    // Update is called once per frame
    void Update()
    {
        transform.position = gameObject.transform.position + offset;
    }
}
