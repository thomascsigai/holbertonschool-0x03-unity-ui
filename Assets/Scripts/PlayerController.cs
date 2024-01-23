using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed = 10.0f;
    private PlayerControls controls;
    private Vector2 move;
    private Rigidbody rb;

    private int score = 0;

    public int health = 5;

    public Text scoreText;

    private void Awake()
    {
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody>();

        if(rb == null)
        {
            Debug.LogError("Rigidbody is NULL");
        }
    }

    private void FixedUpdate()
    {
        move = controls.movementActionMap.Move.ReadValue<Vector2>();
        Vector3 movement = new Vector3(move.x, 0.0f, move.y);
        rb.velocity = movement * speed;
    }

    private void OnEnable()
    {
        controls.movementActionMap.Enable();
    }

    private void OnDisable()
    {
        controls.movementActionMap.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pickup"))
        {
            Destroy(other.gameObject);
            score++;
            ScoreText();
        }

        if(other.gameObject.CompareTag("Trap"))
        {
            health--;
            Debug.Log($"Health: {health}");
        }

        if(other.gameObject.CompareTag("Goal"))
        {
            Debug.Log("You win!");
        }

        if(other.gameObject.CompareTag("Teleporter"))
        {
            if (other.gameObject.GetComponent<TeleporterBehavior>().otherTeleporter.GetComponent<TeleporterBehavior>().enabled)
            {
                GameObject otherTeleporter = other.gameObject.GetComponent<TeleporterBehavior>().otherTeleporter.gameObject;

                otherTeleporter.GetComponent<TeleporterBehavior>().enabled = false;
                other.gameObject.GetComponent<TeleporterBehavior>().enabled = false;

                transform.position = otherTeleporter.transform.position;
            }           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Teleporter"))
        {
            GameObject otherTeleporter = other.gameObject.GetComponent<TeleporterBehavior>().otherTeleporter.gameObject;

            otherTeleporter.GetComponent<TeleporterBehavior>().enabled = true;
        }
    }

    private void Update()
    {
        if(health == 0)
        {
            Debug.Log("Game Over!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void ScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
