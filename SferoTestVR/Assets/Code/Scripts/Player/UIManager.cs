using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    private GameObject deathUI;
    private Text respawnText;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {

        deathUI = gameObject.GetNamedChild("DeathUI");

        respawnText = deathUI.GetNamedChild("RespawnText").GetComponent<Text>();

        deathUI.SetActive(false);
    }

    /// <summary>
    /// shows death UI text
    /// </summary>    
    public void ShowDeathUI()
    {
        deathUI.SetActive(true);
    }

    /// <summary>
    /// hides death UI text
    /// </summary>    
    public void HideDeathUI()
    {
        deathUI.SetActive(false);
    }

    /// <summary>
    /// updates text afetr player dies
    /// </summary>    
    public void UpdateRespawnText(float seconds)
    {
        respawnText.text = "Respawning in: " + seconds + " seconds";
    }
}
