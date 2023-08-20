using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public void SetSensitivity(float sentivity)
    {
        Debug.Log("Changed sensitivity to " + sentivity);
    }

    public void SetSphereDiameter(float diameter)
    {
        Debug.Log("Changed sphere diameter to " + diameter);
    }

}
