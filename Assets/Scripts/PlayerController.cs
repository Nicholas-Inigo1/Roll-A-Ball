using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.0f;
    private Rigidbody rb;
    private int pickupCount;
    private int totalPickups;
    private float pickupChunk;
    private Timer timer;
    private bool gameOver = false;

    [Header("UI")]
    public TMP_Text pickupText;
    public TMP_Text timerText;
    public Image pickupImage;
    public GameObject winpPanel;
    public TMP_Text winTimeText;
    public GameObject inGamePanel;

    void Start()
    {
        // Gets the rigidbody component attached to this game object
        rb = GetComponent<Rigidbody>();
        //Gets the number of pickups in our scene
        pickupCount = GameObject.FindGameObjectsWithTag("Pickup").Length;
        //Assign the total amount of pickups
        totalPickups = pickupCount;
        //Set the pickup image fill amount to 0
        pickupImage.fillAmount = 0;
        //Work out the pickup chunk fill amount
        pickupChunk = 1.0f / totalPickups;
        //Run the check pickups function
        CheckPickups(); 
        //Gets the timer object
        timer = FindObjectOfType<Timer>();

        //Turn off our win panel
        winpPanel.SetActive(false);
        //Turn on our in game panel
        inGamePanel.SetActive(true);
    }

    private void Update()
    {
        timerText.text = "Time: " + timer.currentTime.ToString("F2");
    }

    void FixedUpdate()
    {
        if (gameOver == true)
            return;

        //Store the horizontal axis value in a float
        float moveHorizontal = Input.GetAxis("Horizontal");
        //Store the vertical axis value in a float
        float moveVertical = Input.GetAxis("Vertical");

        //Create a new Vector 3 based on the horizontal and vertical values
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        //Add force to our rigidbody from our movement vector * speed variable
        rb.AddForce(movement * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pickup");
        {
            //Destroy the collided object
            Destroy(other.gameObject);
            //Decrement the pickup count
            pickupCount--;
            //Increase the full amount of our pickup Image
            pickupImage.fillAmount = pickupImage.fillAmount + pickupChunk;
            //Run the check pickups function
            CheckPickups() ;
        }
    }
    
    private void CheckPickups()
    {
        //Do Text stuff
        pickupText.text = "Pickups left: " + pickupCount.ToString();

        if (pickupCount == 0)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        //Set our game over to be true
        gameOver = true;
        //Stop timer
        timer.StopTimer();
      
        //Display the timer on our win time text
        winTimeText.text = "Your time was: " + timer.GetTime(). ToString("F2");

        //Turn on our win panel
        winpPanel.SetActive(true);
        //Turn off our in game panel
        inGamePanel.SetActive(false);

        //Stop the ball from rolling
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    //Temporary restart function
    public void ResetGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
    