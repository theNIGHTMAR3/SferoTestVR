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
    

    [SerializeField] protected AudioSource playerRollingAudioSource;
	[SerializeField] protected float minSpeedSound = 0.5f;
    [SerializeField] protected float maxSpeedSound = 5.0f;

    public float moveSpeed = 10f;
   
    private Vector3 lastCheckpointPos;
    protected  Quaternion savedRotation;

    protected bool isAlive = false;
    protected bool isRespawning = false;
    protected bool hasWon = false;

    protected float respawnHeight = 2f;
    protected float respawnDuration = 4f;

    private bool firstLife = true;

    private int deathsCount = 0;
    private float startTime;


    protected Vector3 addedTorque = Vector3.zero;
    protected virtual void Start()
    {
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody>();
        cameraScript = camera.GetComponent<CameraFollower>();

		// player starts game from spawn
		playerSpawn = GameObject.FindGameObjectWithTag("Respawn");
        SetNewCheckPoint(playerSpawn);
        transform.position = lastCheckpointPos;

        SetPlayerSize();
        StartCoroutine(FreezePlayer(1));
    }

    /// <summary>
    /// not modified in derived class
    /// </summary>
    protected virtual void FixedUpdate()
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

        CalculateRollingVolume();
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
            movement = Quaternion.Euler(0, 90, 0) * movement; //rotate by 90 degrees            
            rigidbody.AddTorque(movement);
            addedTorque = movement;
        }
        else
        {
            addedTorque = Vector3.zero;
        }
    }


    /// <summary>
    /// Add force to sphere from Vector2 (world direction)
    /// </summary>
    protected void Move(Vector2 direction)
    {

        //rigidbody.AddForce(new Vector3(direction.x, 0, direction.y));
        Vector3 movement = new Vector3(direction.x,0,direction.y);                
        movement = Quaternion.Euler(0, 90, 0) * movement; //rotate by 90 degrees            
        rigidbody.AddTorque(movement);
        addedTorque = movement;

    }

	/// <summary>
	/// ckecks if player stands on the ground
	/// </summary>
	protected bool IsGrounded()
	{
        // 0.4f overlap to return grounded when on ramp, need a change later
		return Physics.Raycast(rigidbody.transform.position, Vector3.down, rigidbody.transform.localScale.x+0.4f);
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
    virtual protected void Revive()
    {
        transform.position = lastCheckpointPos;        
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
        StartCoroutine(FreezePlayer(respawnDuration));
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


    /// <summary>
    /// sets player scale
    /// </summary>  
    protected void SetPlayerSize()
    {
        if(!PlayerPrefs.HasKey("SphereDiameter"))
        {
            Debug.LogWarning("PlayerPrefs: player size not set, setting it to 1.0");
            PlayerPrefs.SetFloat("SphereDiameter", 1.0f);
            PlayerPrefs.Save();
            rigidbody.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
        }
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
	/// Calculates volume and pitch of sphere rolling sound
	/// </summary> 
	protected void CalculateRollingVolume()
    {

		float currentPlayerSpeed = rigidbody.velocity.magnitude;

		float normalizedSpeed = Mathf.Clamp01((currentPlayerSpeed - minSpeedSound) / (maxSpeedSound - minSpeedSound));

        playerRollingAudioSource.volume = normalizedSpeed;
        playerRollingAudioSource.pitch = Mathf.Lerp(1.0f, 2.0f, normalizedSpeed);


		if (currentPlayerSpeed > minSpeedSound && IsGrounded())
		{
			if (!playerRollingAudioSource.isPlaying)
			{
				playerRollingAudioSource.Play();
			}
		}
		else
		{
			playerRollingAudioSource.Stop();
		}
	}


    /// <summary>
    /// method for getting input from the device
    /// </summary>
    protected virtual void GetInput() { }
}
