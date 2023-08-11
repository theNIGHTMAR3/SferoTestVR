using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public GameObject deathUI;

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
        deathUI.SetActive(false);
    }

    public void ShowDeathUI()
    {
        deathUI.SetActive(true);
    }

    public void HideDeathUI()
    {
        deathUI.SetActive(false);
    }
}
