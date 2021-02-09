using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    
    private Vector2 direction = Vector2.left;

    private Rigidbody rd2d;

    public Text score;
    public Text lives;
    public Text level;

    private static int scoreValue = 0;
    private static int livesValue = 3;
    public static int levelValue = 1;

    private Animator anim;

    private redGhostController RedGhostController;
    private bastardGhostController BastardGhostController;
    private pinkGhostController PinkGhostController;
    private orangeGhostController OrangeGhostController;
    
    //Needs to be changed to reflect how many pellets are actually in the game, should equal that
    public int pelletsRemaining = 191;
    public int altPellets;

    private int ding = 0;
    private int soundPlayed = 0;
    AudioSource audioSource;
    public AudioClip ding1;
    public AudioClip ding2;
    public AudioClip damagesound;
    public AudioClip bigpellet;
    private static int oneUpped = 0;

    public int face = 1;


    // Start is called before the first frame update
    void Start()
    {
        pelletsRemaining = 191;
        altPellets = 0;
        rd2d = GetComponent<Rigidbody>();
        score.text = "Score: " + scoreValue.ToString();
        level.text = "Level " + levelValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();

        GameObject RedGhostControllerObject = GameObject.FindWithTag("redGhost");
        if (RedGhostControllerObject != null)
        {
            RedGhostController = RedGhostControllerObject.GetComponent<redGhostController>();
        }
        GameObject BastardGhostControllerObject = GameObject.FindWithTag("bastardGhost");
        if (BastardGhostControllerObject != null)
        {
            BastardGhostController = BastardGhostControllerObject.GetComponent<bastardGhostController>();
        }
        GameObject PinkGhostControllerObject = GameObject.FindWithTag("pinkGhost");
        if (PinkGhostControllerObject != null)
        {
            PinkGhostController = PinkGhostControllerObject.GetComponent<pinkGhostController>();
        }
        GameObject OrangeGhostControllerObject = GameObject.FindWithTag("orangeGhost");
        if (OrangeGhostControllerObject != null)
        {
            OrangeGhostController = OrangeGhostControllerObject.GetComponent<orangeGhostController>();
        }

        audioSource = GetComponent<AudioSource>();

         anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput ();

        Move ();
        if (scoreValue >= 5000 && oneUpped == 0)
        {
            livesValue += 1;
            lives.text = "Lives: " + livesValue.ToString();
            oneUpped = 1;
        }
        if (levelValue == 1)
        {
            levelCheck();
        }
    }

    //Pick up code
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "redGhost")
        {
            if (RedGhostController != null)
            {
                if (RedGhostController.mode == 4)
                {
                    scoreValue += 250;
                    score.text = "Score: " + scoreValue.ToString();
                }
                if (RedGhostController.mode == 3 || RedGhostController.mode == 5)
                {

                }
                if (RedGhostController.mode == 1 || RedGhostController.mode == 2)
                {
                    livesValue -= 1;
                    lives.text = "Lives: " + livesValue.ToString();
                    RedGhostController.resetTime();
                    BastardGhostController.resetTime();
                    PinkGhostController.resetTime();
                    OrangeGhostController.resetTime();
                    face = 1;
                    direction = Vector2.left;
                    anim.SetInteger("State", 1);
                    transform.position = new Vector2(-3,-1);
                }
            }
        }
        if (other.tag == "bastardGhost")
        {
            if (BastardGhostController != null)
            {
                if (BastardGhostController.mode == 4)
                {
                    scoreValue += 250;
                    score.text = "Score: " + scoreValue.ToString();
                }
                if (BastardGhostController.mode == 3 || BastardGhostController.mode == 5)
                {

                }
                if (BastardGhostController.mode == 1 || BastardGhostController.mode == 2)
                {
                    livesValue -= 1;
                    lives.text = "Lives: " + livesValue.ToString();
                    BastardGhostController.resetTime();
                    RedGhostController.resetTime();
                    PinkGhostController.resetTime();
                    OrangeGhostController.resetTime();
                    direction = Vector2.left;
                    face = 1;
                    anim.SetInteger("State", 1);
                    transform.position = new Vector2(-3,-1);
                    PlaySound(damagesound);
                }
            }
        }
        if (other.tag == "pinkGhost")
        {
            if (PinkGhostController != null)
            {
                if (PinkGhostController.mode == 4)
                {
                    scoreValue += 250;
                    score.text = "Score: " + scoreValue.ToString();
                }
                if (PinkGhostController.mode == 3 || PinkGhostController.mode == 5)
                {

                }
                if (PinkGhostController.mode == 1 || PinkGhostController.mode == 2)
                {
                    livesValue -= 1;
                    lives.text = "Lives: " + livesValue.ToString();
                    BastardGhostController.resetTime();
                    RedGhostController.resetTime();
                    PinkGhostController.resetTime();
                    OrangeGhostController.resetTime();
                    direction = Vector2.left;
                    face = 1;
                    anim.SetInteger("State", 1);
                    transform.position = new Vector2(-3,-1);
                    PlaySound(damagesound);
                }
            }
        }
        if (other.tag == "orangeGhost")
        {
            if (OrangeGhostController != null)
            {
                if (OrangeGhostController.mode == 4)
                {
                    scoreValue += 250;
                    score.text = "Score: " + scoreValue.ToString();
                }
                if (OrangeGhostController.mode == 3 || OrangeGhostController.mode == 5)
                {

                }
                if (OrangeGhostController.mode == 1 || OrangeGhostController.mode == 2)
                {
                    livesValue -= 1;
                    lives.text = "Lives: " + livesValue.ToString();
                    BastardGhostController.resetTime();
                    RedGhostController.resetTime();
                    PinkGhostController.resetTime();
                    OrangeGhostController.resetTime();
                    direction = Vector2.left;
                    face = 1;
                    anim.SetInteger("State", 1);
                    transform.position = new Vector2(-3,-1);
                    PlaySound(damagesound);
                }
            }
        }
        if (other.tag == "PickUp")
        {
            scoreValue += 20;
            score.text = "Score: " + scoreValue.ToString();
            BastardGhostController.pelletManager();
            OrangeGhostController.pelletManager();
            pelletsRemaining -= 1;
            altPellets += 1;
            if (ding == 0 && soundPlayed == 0)
            {
                ding = ding + 1;
                PlaySound(ding1);
                soundPlayed = 1;
            }
            if (ding == 1 && soundPlayed == 0)
            {
                ding = ding - 1;
                PlaySound(ding2);
                soundPlayed = 1;
            }
            soundPlayed = 0;
            Destroy(other.gameObject);
        }

        if (other.tag == "SuperPellet")
        {
            scoreValue += 30;
            score.text = "Score: " + scoreValue.ToString();
            BastardGhostController.pelletManager();
            OrangeGhostController.pelletManager();
            altPellets += 1;
            pelletsRemaining -= 1;
            PlaySound(bigpellet);

            if (RedGhostController != null)
            {
                if (RedGhostController.mode == 3 || RedGhostController.mode == 4)
                {

                }
                else
                {
                    RedGhostController.modeChange(4);
                }
            }
            if (BastardGhostController != null)
            {
                if (BastardGhostController.mode == 3 || BastardGhostController.mode == 4)
                {

                }
                else
                {
                    BastardGhostController.modeChange(4);
                }
            }
            if (OrangeGhostController != null)
            {
                if (OrangeGhostController.mode == 3 || OrangeGhostController.mode == 4)
                {

                }
                else
                {
                    OrangeGhostController.modeChange(4);
                }
            }
            if (PinkGhostController != null)
            {
                if (PinkGhostController.mode == 3 || PinkGhostController.mode == 4)
                {

                }
                else
                {
                    PinkGhostController.modeChange(4);
                }
            }
            Destroy(other.gameObject);
        }
            

    }
    
    // Controller Code
     void CheckInput ()
    {
        if (Input.GetKeyDown (KeyCode.A))
        {
            direction = Vector2.left;
            face = 0;
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyDown (KeyCode.S))
        {
            direction = Vector2.down;
            face = 2;
            anim.SetInteger("State", 2);
        }

        if (Input.GetKeyDown (KeyCode.D))
        {
            direction = Vector2.right;
            face = 3;
            anim.SetInteger("State", 3);
        }

        if (Input.GetKeyDown (KeyCode.W))
        {
            face = 0;
            direction = Vector2.up;
            anim.SetInteger("State", 4);
        }

        if (Input.GetKeyDown (KeyCode.LeftArrow))
        {
            direction = Vector2.left;
            face = 1;
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyDown (KeyCode.DownArrow))
        {
            direction = Vector2.down;
            face = 2;
            anim.SetInteger("State", 2);
        }

        if (Input.GetKeyDown (KeyCode.RightArrow))
        {
            direction = Vector2.right;
            face = 3;
            anim.SetInteger("State", 3);
        }

        if (Input.GetKeyDown (KeyCode.UpArrow))
        {
            direction = Vector2.up;
            face = 0;
            anim.SetInteger("State", 4);
        }
        if (Input.GetKey ("escape"))
        {
            Application.Quit();
        }
    }

    //Movement Code Function
    void Move ()
    {
        transform.localPosition += (Vector3)(direction * speed) * Time.deltaTime;
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    private void levelCheck()
    {
        if (altPellets == 190 && levelValue ==  2)
        {
            livesValue = 3;
            scoreValue = 0;
            levelValue = 1;
            oneUpped = 0;
            bastardGhostController.bastardPelletCount = 30;
            orangeGhostController.orangePelletCount = 90;
            SceneManager.LoadScene("Win Scene");
        }
        if (pelletsRemaining == 0 && levelValue == 1)
        {
            levelValue += 1;
            SceneManager.LoadScene("Pacman Level 2");
        }
        
        if (livesValue == 0)
        {
            livesValue = 3;
            scoreValue = 0;
            levelValue = 1;
            oneUpped = 0;
            bastardGhostController.bastardPelletCount = 30;
            orangeGhostController.orangePelletCount = 90;
            SceneManager.LoadScene("Lose Scene");
        }
    }
}
