using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private int _width = 1;
    [SerializeField] private int _length = 1;
    public int width { get { return _width; } private set { _width = value; } }
    public int length { get { return _length; } private set { _length = value; } }    
}
