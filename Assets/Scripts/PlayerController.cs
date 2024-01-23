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
    public Text healthText;
    public Text WinLoseText;

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
        controls.UIInputsActionMap.Enable();

        controls.UIInputsActionMap.BackToMenu.performed += ctx => SceneManager.LoadScene("menu");
    }

    private void OnDisable()
    {
        controls.movementActionMap.Disable();
        controls.UIInputsActionMap.Disable();
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
            HealthText();
        }

        if(other.gameObject.CompareTag("Goal"))
        {
            WinText();
            StartCoroutine(LoadScene(3));
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
            LoseText();
            StartCoroutine(LoadScene(3));
        }
    }

    void ScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void HealthText()
    {
        healthText.text = "Health: " + health.ToString();
    }

    void WinText()
    {
        WinLoseText.transform.parent.gameObject.SetActive(true);
        WinLoseText.text = "You Win!";
        WinLoseText.color = Color.black;
        WinLoseText.GetComponentInParent<Image>().color = Color.green;
    }

    void LoseText()
    {
        WinLoseText.transform.parent.gameObject.SetActive(true);
        WinLoseText.text = "Game Over!";
        WinLoseText.color = Color.white;
        WinLoseText.GetComponentInParent<Image>().color = Color.red;
    }

    IEnumerator LoadScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
