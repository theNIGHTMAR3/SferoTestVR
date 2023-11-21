using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;




public class SettingsMenu : MonoBehaviour
{
	public Slider audioSlider;
	public Slider sensitivitySlider;
	public Slider diameterSlider;
	public Dropdown playerModeDropdown;



	private TextMeshProUGUI audioValue;
	private TextMeshProUGUI sensitivityValue;
    private TextMeshProUGUI diameterValue;

   

	private void Awake()
	{
		audioValue = gameObject.GetNamedChild("AudioValue").GetComponent<TextMeshProUGUI>();
		sensitivityValue = gameObject.GetNamedChild("SensitivityValue").GetComponent<TextMeshProUGUI>();
		diameterValue = gameObject.GetNamedChild("DiameterValue").GetComponent<TextMeshProUGUI>();

		if (!PlayerPrefs.HasKey("Audio"))
		{
			Debug.Log("Audio not set, setting it to 1.0");
			PlayerPrefs.SetFloat("Audio", 1.0f);
			PlayerPrefs.Save();
		}

		if (!PlayerPrefs.HasKey("Sensitivity"))
        {
			Debug.Log("Sensitivity not set, setting it to 1.0");
			PlayerPrefs.SetFloat("Sensitivity", 1.0f);
			PlayerPrefs.Save();
		}

		if (!PlayerPrefs.HasKey("SphereDiameter"))
		{
			Debug.Log("SphereDiameter not set, setting it to 1.0");
			PlayerPrefs.SetFloat("SphereDiameter", 1.0f);
			PlayerPrefs.Save();
		}

		if (!PlayerPrefs.HasKey("PlayerMode"))
		{
			Debug.Log("PlayerMode not set, setting it to PlayerPC");
			PlayerPrefs.SetInt("PlayerMode", (int)PlayerMode.PlayerPC);
			PlayerPrefs.Save();
		}

		audioValue.text = PlayerPrefs.GetFloat("Audio").ToString("0.0").Replace(',', '.');
		audioSlider.value = PlayerPrefs.GetFloat("Audio");
		sensitivityValue.text = PlayerPrefs.GetFloat("Sensitivity").ToString();
		sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
		diameterValue.text = PlayerPrefs.GetFloat("SphereDiameter").ToString("0.0").Replace(',', '.');
		diameterSlider.value = PlayerPrefs.GetFloat("SphereDiameter");
		playerModeDropdown.value = PlayerPrefs.GetInt("PlayerMode");
	}

	private void Start()
    {
        
    }

	public void SetAudio(float volume)
	{
		float roundedVolume = (float)Math.Round(volume, 1);

		PlayerPrefs.SetFloat("Audio", roundedVolume);
		PlayerPrefs.Save();
		audioValue.text = roundedVolume.ToString("0.0").Replace(',', '.');
		Debug.Log("Changed audio volume to " + roundedVolume);
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

	public void SetPlayerMode(int mode)
	{
		PlayerPrefs.SetInt("PlayerMode", mode);
		Debug.Log("Changed player mode to "+ Enum.GetName(typeof(PlayerMode), mode));

		PlayerPrefs.Save();
	}

}
