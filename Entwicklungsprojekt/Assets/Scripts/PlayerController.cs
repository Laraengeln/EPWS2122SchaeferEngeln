using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Initialisierung der oeffentlichen Variablen
    public float startScale = 45f;
    public Animator animator;           //Damit Skript auf Animation zugreifen kann
    public AudioSource jumpAudio;

    // Initialisierung der privaten Variablen
    float horizontalInput;
    float verticalInput;
    float speed = 150f;
    float yRange = 70f;
    float jumpScale = 60f;
    float jumpDuration = 0.75f;
    float jumpingTimeTimer = 0f;
    float jumpTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Timer runterlaufen lassen
        jumpingTimeTimer -= Time.deltaTime;
        jumpTimer -= Time.deltaTime;

        // nach jedem Sprung wieder auf Startgroe�e zur�ck
        if (jumpingTimeTimer <= 0f)
        {
            transform.localScale = new Vector3(startScale, startScale, startScale);
        }

        // Bewegung mit Rotation nach links
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w);
            transform.Translate(Vector3.left * horizontalInput * Time.deltaTime * speed);
        }

        // Bewegung mit Rotation nach rechts
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
            transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);
        }

        // Aufruf der Methoden
        MovePlayer();
        KeepPlayerInBounds(yRange);

        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
        animator.SetFloat("JumpTime", jumpingTimeTimer);
    }

    // Methode um Spieler zu bewegen und zu springen
    void MovePlayer()
    {
        // Input f�r Bewegung speichern
        horizontalInput = Input.GetAxis("Horizontal");


        verticalInput = Input.GetAxis("Vertical");

        // Bewegung nach oben/unten abh�ngig von verticalInput
        //transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);
        transform.Translate(Vector3.up * verticalInput * Time.deltaTime * speed);

        // Bei Dr�cken der Leertaste Skalierung erhoehen, da Player n�her an Kamera erscheinen soll wegen Sprung
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Nur Ausf�hren, wenn jumpTimer unter 0 ist -> Spam der Leertaste verhindern
            // Nach Sprung Timer erneut setzen
            if (jumpTimer <= 0)
            {
                jumpAudio.Play();
                transform.localScale = new Vector3(jumpScale, jumpScale, jumpScale);
                jumpingTimeTimer = jumpDuration;
                jumpTimer = 1.25f;
            }
        }
    }

   //public void OnLanding() 
    //{
    //    animator.SetBool("isJumping", isJumping);
    //}

    // Methode um den Spieler im Spielfeld zu behalten
    void KeepPlayerInBounds(float range)
    {
        if (transform.position.y < -range)
        {
            transform.position = new Vector3(transform.position.x, -range, transform.position.z);
        }

        if (transform.position.y > range)
        {
            transform.position = new Vector3(transform.position.x, range, transform.position.z);
        }


    }
}