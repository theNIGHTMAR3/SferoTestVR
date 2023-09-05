using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    protected Rigidbody rigidbody;
    protected Camera camera;
    protected CameraFollower cameraScript;
    protected Vector2 playerInput;
    private GameObject playerSpawn;

    public float moveSpeed = 10f;
   
    private Vector3 lastCheckpointPos;
    private Quaternion savedRotation;

    protected bool isAlive = false;
    protected bool isRespawning = false;
    protected bool hasWon = false;

    protected float respawnHeight = 2f;
    protected float respawnDuration = 4f;

    private bool firstLife = true;

    private int deathsCount = 0;
    private float startTime;


    protected virtual void Start()
    {
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody>();
        cameraScript = camera.GetComponent<CameraFollower>();

        // player starts game from spawn
        playerSpawn = GameObject.FindGameObjectWithTag("Respawn");
        SetNewCheckPoint(playerSpawn);
        transform.position = lastCheckpointPos;

        Debug.Log("SphereDiameter: "+PlayerPrefs.GetFloat("SphereDiameter"));
        SetPlayerSize();
        StartCoroutine(FreezePlayer(1));
    }

    /// <summary>
    /// not modified in derived class
    /// </summary>
    protected void Update()
    {
        GetInput();

        if(Input.GetKeyDown(KeyCode.Space) && isAlive && !hasWon)
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.Q) && isRespawning)
        {
            StopRespawning();
        }

        if (Input.GetKeyDown(KeyCode.Space) && hasWon)
        {
            GoBackToMainMenu();
        }

    }

    /// <summary>
    /// Add force to sphere from Vector3
    /// </summary>    
    protected void Move(Vector3 direction)
    {

        if(isAlive && !hasWon)
        {
            Vector3 movement = direction;
            movement.y = 0;
            movement.Normalize();
            movement *= direction.magnitude;

            rigidbody.AddForce(movement);
        }
    }


    /// <summary>
    /// Add force to sphere from Vector2 (world direction)
    /// </summary>
    protected void Move(Vector2 direction)
    {
        rigidbody.AddForce(new Vector3(direction.x, 0, direction.y));
    }


    /// <summary>
    /// starts death coroutine/animation, and after that Revive method
    /// </summary>
    public void Die()
    {
        if(isAlive) 
        {
            deathsCount++;
            isAlive = false;
            UIManager.Instance.ShowDeathUI();
            StartCoroutine(DieCoroutine());
            //TODO
            // animations
            // saving path
            // other stuff
        }
    }

    /// <summary>
    /// starts revival coroutine/animation
    /// </summary>
    protected void Revive()
    {
        transform.position = lastCheckpointPos;
        cameraScript.SetCameraRotation(savedRotation);
        transform.rotation = Quaternion.Euler(-90,savedRotation.eulerAngles.y,0);

        StartCoroutine(FreezePlayer(1f));
    }


    /// <summary>
    /// sets new checkpoint
    /// </summary>    
    public void SetNewCheckPoint(GameObject newCheckpoint)
    {
        lastCheckpointPos = newCheckpoint.transform.position;
        savedRotation = newCheckpoint.transform.rotation;
    }

    /// <summary>
    /// checks of player collided with an object
    /// </summary>    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Threat"))
        {
            Die();
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            Debug.Log("Player reached the end");
            Win();
        }

    }

    /// <summary>
    /// freezes player for given amount of seconds
    /// </summary> 
    protected IEnumerator FreezePlayer(float seconds)
    {
        rigidbody.isKinematic = true;
        yield return new WaitForSeconds(seconds);
        if(firstLife)
        {
            firstLife= false;
            startTime = Time.time;
        }
        rigidbody.isKinematic = false;
        isAlive = true;
    }

    /// <summary>
    /// handles logic after player dies
    /// </summary>    
    protected virtual IEnumerator DieCoroutine()
    {
        Vector3 originalPosition = rigidbody.position;
        isRespawning = true;
        float elapsedTime = 0;

        rigidbody.isKinematic = true;
        while (elapsedTime < respawnDuration)
        {
            if (!isRespawning)
                break;

            float t = elapsedTime / respawnDuration;
            Vector3 newPos = Vector3.Lerp(originalPosition, originalPosition + Vector3.up * respawnHeight, t);
            rigidbody.position = newPos;

            elapsedTime += Time.deltaTime;
            UIManager.Instance.UpdateRespawnText(Mathf.Ceil(respawnDuration - elapsedTime));
            yield return null;
        }
        
        StopRespawning();
        rigidbody.isKinematic = false;

        Revive();
    }


    protected void SetPlayerSize()
    {
        float playerSize = PlayerPrefs.GetFloat("SphereDiameter");

        Vector3 playerScale = new Vector3(playerSize, playerSize, playerSize);

        rigidbody.transform.localScale= playerScale;
    }

    /// <summary>
    /// stops respawning procedure
    /// </summary>    
    protected void StopRespawning()
    {
        isRespawning = false;
        UIManager.Instance.HideDeathUI();   
    }

    /// <summary>
    /// logic when player finnishes the level
    /// </summary>    
    protected void Win()
    {
        hasWon = true;
        UIManager.Instance.ShowWinningUI();
        UIManager.Instance.SetSummaryText(Time.time-startTime,deathsCount);
        StartCoroutine(FreezePlayer(5f));
    }

    /// <summary>
    /// moves user to Main Menu
    /// </summary>    
    protected void GoBackToMainMenu()
    {
        cameraScript.UnlockCursor();
        SceneManager.LoadScene("MainMenu");
    }


    /// <summary>
    /// method for getting input from the device
    /// </summary>
    protected virtual void GetInput() { }
}
