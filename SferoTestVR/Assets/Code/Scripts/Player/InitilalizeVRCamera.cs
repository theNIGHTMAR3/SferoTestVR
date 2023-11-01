using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitilalizeVRCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
#else
        Display.displays[1].Activate();
#endif
    }

}
