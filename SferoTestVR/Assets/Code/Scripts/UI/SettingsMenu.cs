using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private TextMeshProUGUI sensitivityValue;
    private TextMeshProUGUI diameterValue;

    private void Start()
    {
        sensitivityValue = gameObject.GetNamedChild("SensitivityValue").GetComponent<TextMeshProUGUI>();
        diameterValue = gameObject.GetNamedChild("DiameterValue").GetComponent<TextMeshProUGUI>();
    }

    public void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        PlayerPrefs.Save();
        sensitivityValue.text = sensitivity.ToString();
        Debug.Log("Changed sensitivity to " + sensitivity);
    }

    public void SetSphereDiameter(float diameter)
    {
        float roundedDiameter = (float)Math.Round(diameter,1);

        PlayerPrefs.SetFloat("SphereDiameter", roundedDiameter);
        PlayerPrefs.Save();
        diameterValue.text = roundedDiameter.ToString("0.0").Replace(',','.');
        Debug.Log("Changed sphere diameter to " + roundedDiameter);
    }

}
