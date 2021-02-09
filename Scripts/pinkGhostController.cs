using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class pinkGhostController : MonoBehaviour
{
    /*
    Modes determine how the ghosts act
    scatter is 1
    chase is 2
    eaten is 3
    frightened is 4
    */
    public int mode = 1;

    //speed the ghost travels at
    private float speed = 2.8125f;
    //getting the ghost's target and Pacman for later use; assigned in Unity interface
    public GameObject pinkTarget;
    public GameObject redGhost;
    public GameObject player;
    private PlayerController PacmanController;

    private Rigidbody rd2d;
    Animator animator;

    //Variables for direction checks.
    private bool upIsFine = true;
    private bool leftIsFine = true;
    private bool downIsFine = true;
    private bool rightIsFine = true;

    //variables for target pursuit
    private float testX;
    private float testY;
    private float targetX;
    private float targetY;
    private float test0;
    private float test1;
    private float test2;
    private float test3;

     /*
    randDirec is direction the ghost will be moving, kept separate from face so animation can be linked to face
    0 = up
    1 = left
    2 = down
    3 = right
    */
    private int randDirec;

    //direction ghost is travelling in
    private Vector2 direction = Vector2.left;

    bool specialJunction = false;
    /*
    Face is direction Ghost is facing
    0 = up
    1 = left
    2 = down
    3 = right
    */
    private int face = 1;
    //Time point for Scatter-Chase Pattern
    private float time = 126.0f;
    //Hold proper Scatter-Chase time when frightened or eaten
    private float storedTime;
    //variables for controlling ghost in fright mode
    private float frightenedLength = 15.0f;
    private float frightRemaining;
    private bool frightIsOn = false;

    private int targetCollision = 1;
    private int randCheck = 0;
    private int pinkPelletCount = 30;
    private int currentCount;
    private float altX;
    private float altY;
    private bool starting = true;

    // Start is called before the first frame update
    void Start()
    {
        GameObject PacmanControllerObject = GameObject.FindWithTag("Player");
        if (PacmanControllerObject != null)
        {
            PacmanController = PacmanControllerObject.GetComponent<PlayerController>();
        }
        animator = GetComponent<Animator>();
        animator.SetInteger("face", 2);
        animator.SetInteger("animFright", 0);
        animator.SetInteger("eyed", 0);
        currentCount = pinkPelletCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (starting != true)
        {
            if (mode != 3)
            {
                timeTracker();
                movement();
                frightTime();
            }
            else
            {
                movement();
            }
        }
        else
        {
            initialPrison();
            movement();
            timeTracker();
        }
    }

    //Stealing partner's movement code from player code
    private void movement()
    {
        transform.localPosition += (Vector3)(direction * speed) * Time.deltaTime;
    }
    public void modeChange(int replacementValue)
    {
        mode = replacementValue;
        targetManagement();
        if (mode == 2)
        {
            //whenever ghost enters chase mode, they turn around
        }
        if (mode == 1)
        {
            
        }
        if (mode == 4)
        {
            storedTime = time;
            speed = 1.875f;
            turnabout(face);
        }
    }
    //Junctions at Pacman's spawn and above ghost cage, ghosts can't turn up
    private void intersection()
    {
        //Need to update ghosts targeting when they are about to re-direct
        if (mode != 3)
        {
            targetManagement();
        }
        checkForWalls();
        //If frightened, movement is random
        if (mode == 4)
        {
            //when forward is 0, back is 2, left is 1, and right is 3
            if (face == 0)
            {
                randomDirections(2, 1, 3, 0, leftIsFine,rightIsFine,upIsFine);
            }
            //when forward is 1, back is 3, left is 2, and right is 0
            if (face == 1)
            {
                randomDirections(3, 2, 0, 1, downIsFine, upIsFine, leftIsFine);
            }
            //when forward is 2, back is 0, left is 3, and right is 1
            if (face == 2)
            {
                randomDirections(0, 3, 1, 2, rightIsFine, leftIsFine, downIsFine);
            }
            //when forward is 3, back is 1, left is 0, and right is 2
            if (face == 3)
            {
                randomDirections(1, 0, 2, 3, upIsFine, downIsFine, rightIsFine);
            }
        }
        //else, ghost is meant to pursue target.
        if (mode == 1 || mode == 2 || mode == 3 || mode == 5)
        {
            targetX = pinkTarget.transform.position.x;
            targetY = pinkTarget.transform.position.y;
            //if up is fine, test a distance one movement up for distance from target; same for each direction
            if (upIsFine)
            {
                testX = transform.position.x - targetX;
                testY = transform.position.y + 1 - targetY;
                test0 = Mathf.Sqrt(testX * testX + testY * testY);
            }
            if (upIsFine == false)
            {
                test0= 99999999.0f;
            }
            if (leftIsFine)
            {
                testX = transform.position.x - 1 - targetX;
                testY = transform.position.y - targetY;
                test1 = Mathf.Sqrt(testX * testX + testY * testY);
            }
            if (leftIsFine == false)
            {
                test1= 99999999.0f;
            }
            if (downIsFine)
            {
                testX = transform.position.x - targetX;
                testY = transform.position.y - 1 - targetY;
                test2 = Mathf.Sqrt(testX * testX + testY * testY);
            }
            if (downIsFine == false)
            {
                test2= 99999999.0f;
            }
            if (rightIsFine)
            {
                testX = transform.position.x + 1 - targetX;
                testY = transform.position.y - targetY;
                test3 = Mathf.Sqrt(testX * testX + testY * testY);
            }
            if (rightIsFine == false)
            {
                test3= 99999999.0f;
            }
            // right is smallest distance, go right
            if (test3 < test2 && test3 < test1 && test3 < test0)
            {
                face = 3;
                direction = Vector2.right;
                animator.SetInteger("face", face);
            }
            // if up is smallest or tied for smallest, go up
            else if (test0 <= test1 && test0 <= test2 && test0 <= test3 )
            {
                face = 0;
                direction = Vector2.up;
                animator.SetInteger("face", face);
            }
            else if (test1 < test0 && test1 <= test2 && test1 <= test3)
            {
                face = 1;
                direction = Vector2.left;
                animator.SetInteger("face", face);
            }
            else if (test2 < test0 && test2 < test1 && test2 <= test3)
            {
                face = 2;
                direction = Vector2.down;
                animator.SetInteger("face", face);
            }
        }
    }
    
    private void checkForWalls()
    {
        //set all direction checks to true, so we know what they are
        upIsFine = true;
        leftIsFine = true;
        downIsFine = true;
        rightIsFine = true;
        /* Currently, level design doesn't really work with this mechanic, so I am commenting it out.
        // If co-ordinates of ghost are just above cage or in Pacman's spawning zone, they are in a special junction
        if ( 0.0f < transform.position.x && transform.position.x < 5.0f && 0.0f < transform.position.y && transform.position.y < 5.0f)
        {
            specialJunction = true;
        }
        if ( 10.0f < transform.position.x && transform.position.x < 15.0f && 10.0f < transform.position.y && transform.position.y < 15.0f)
        {
            specialJunction = true;
        }
        */

        //Checking for walls, need to apply wall layer to walls. If Ray collides with a wall, turn its bool to false.
        RaycastHit2D hit0 = Physics2D.Raycast(transform.position, Vector2.up, 2.0f,LayerMask.GetMask("Wall"));
        if (hit0.collider != null)
        {
            upIsFine = false;
        }
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, Vector2.left, 2.0f,LayerMask.GetMask("Wall"));
        if (hit1.collider != null)
        {
            leftIsFine = false;
        }
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.down, 2.0f,LayerMask.GetMask("Wall"));
        if (hit2.collider != null)
        {
            downIsFine = false;
        }
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector2.right, 2.0f,LayerMask.GetMask("Wall"));
        if (hit3.collider != null)
        {
            rightIsFine = false;
        }
        //If special junction, ghost can not turn up
        if (specialJunction)
        {
            upIsFine = false;
        }
        //reset specialJunction back to standard value now that its job is done
        specialJunction = false;
        //We don't want the ghosts to think going backwards is fine, just to be safe.
        if (face == 0)
        {
            downIsFine = false;
        }
        if (face == 1)
        {
            rightIsFine = false;
        }
        if (face == 2)
        {
            upIsFine = false;
        }
        if (face == 3)
        {
            leftIsFine = false;
        }
    }

    //Done in terms of direction the ghost is facing, rather than absolute directions.
    private void randomDirections(int backward, int Left, int Right, int Forward, bool left, bool right, bool forward)
    {
        //set random direction to go backwards, since that is the one direction we don't want to go
        randDirec = backward;
        //while it is trying to go backwards, come up with a new direction
        while (randDirec == backward)
        {
            randDirec = Random.Range(0,4);

            //if left is false and it is trying to go left, make it come up with a new number
            if (left == false && randDirec == Left)
            {
                randDirec = backward;
            }
            if (forward == false && randDirec == Forward)
            {
                randDirec = backward;
            }
            if (right == false && randDirec == Right)
            {
                randDirec = backward;
            }
        }
        //turn the ghost in the direction randomly determined.
        if (randDirec == 0)
        {
            direction = Vector2.up;
            face = 0;
            animator.SetInteger("face", face);
        }
        if (randDirec == 1)
        {
            direction = Vector2.left;
            face = 1;
            animator.SetInteger("face", face);
        }
        if (randDirec == 2)
        {
            direction = Vector2.down;
            face = 2;
            animator.SetInteger("face", face);
        }
        if (randDirec == 3)
        {
            direction = Vector2.right;
            face = 3;
            animator.SetInteger("face", face);
        }
    }

    //Ghosts turn 180 immediately if they enter frightened or chase mode. This handles that.
    private void turnabout(int forward)
    {
        if (forward == 0)
        {
            face = 2;
            direction = Vector2.down;
            animator.SetInteger("face", face);
        }
        if (forward == 1)
        {
            face = 3;
            direction = Vector2.right;
            animator.SetInteger("face", face);
        }
        if (forward == 2)
        {
            face = 0;
            direction = Vector2.up;
            animator.SetInteger("face", face);
        }
        if (forward == 3)
        {
            face = 1;
            direction = Vector2.left;
            animator.SetInteger("face", face);
        }
    }
    private void targetManagement()
    {
        if (mode == 2)
        {
            if (PacmanController.face == 1)
            {
                altX = PacmanController.transform.position.x - 4;
                altY = PacmanController.transform.position.y;
            }
            if (PacmanController.face == 3)
            {
                altX = PacmanController.transform.position.x + 4;
                altY = PacmanController.transform.position.y;
            }
            if (PacmanController.face == 2)
            {
                altX = PacmanController.transform.position.x - 4;
                altY = PacmanController.transform.position.y;
            }
            if (PacmanController.face == 0)
            {
                altX = PacmanController.transform.position.x - 4;
                altY = PacmanController.transform.position.y + 4;
            }
            pinkTarget.transform.position = new Vector2(altX,altY);
        }
        if (mode == 1)
        {
            // This vector 2 needs to be updated to roughly whatever the top left corner is.
            pinkTarget.transform.position = new Vector2(-15,12);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "intersection")
        {
            if (speed == 1.875f)
            {
                Invoke("intersection", .26666666f);
            }
            if (speed == 2.8125f)
            {
                Invoke("intersection", .17777777f);
            }
            if (speed == 5.0f)
            {
                Invoke("intersection", .1f);
            }
        }
        if (other.tag == "Player")
        {
            if (mode == 4)
            {
                eaten();
            }
        }
        if (other.tag == "pinkTarget")
        {
            if (mode == 3 && starting == false)
            {
                if (targetCollision == 3)
                {
                    time = storedTime;
                    mode = 5;
                    speed = 2.8125f;
                    animator.SetInteger("animFright", 0);
                    animator.SetInteger("eyed", 0);
                    timeTracker();
                    targetManagement();
                    targetCollision = 4;
                    //Ghost must turn left or right after leaving cage.
                    Invoke("leavingJail", .2666666666f);
                }
                if (targetCollision == 2)
                {
                    pinkTarget.transform.position = new Vector2(10.5f,3.0f);
                    face = 2;
                    animator.SetInteger("face", face);
                    direction = Vector2.down;
                    targetCollision += 1;
                    //reasoning for staying in jail for other ghosts goes here
                }
                if (targetCollision == 1)
                {
                    face = 0;
                    animator.SetInteger("face", face);
                    direction = Vector2.up;
                    pinkTarget.transform.position = new Vector2(10.5f,5.0f);
                    targetCollision += 1;
                }
            }
            if (starting)
            {
                if (targetCollision == 1)
                {
                    timeTracker();
                    targetManagement();
                    Invoke("leavingJail", .2666666666f);
                }
            }
        }
    }
    private void initialPrison()
    {
        face = 2;
        direction = Vector2.down;
        animator.SetInteger("face", face);
        pinkTarget.transform.position = new Vector2(10.5f,3.0f);
    }
    public void pelletManager()
    {
        currentCount -= 1;
    }
    private void timeTracker()
    {
        time = time - Time.deltaTime;
        if (time > 115.5f && mode != 3 && mode != 4 && mode != 1 && starting == false)
        {
            modeChange(1);
        }
        if (time <= 115.5f && mode != 3 && mode != 4 && mode != 2 && time > 85.5f && starting == false)
        {
            modeChange(2);
            turnabout(face);
        }
        if (time <= 115.5f && time > 75.0f && mode != 3 && mode != 4 && mode != 1 && starting == false)
        {
            if (mode != 5)
            {
                turnabout(face);
            }
            modeChange(1);
        }
        if (time <= 75.0f && time > 45.0f && mode != 3 && mode != 4 && mode != 2 && starting == false)
        {
            modeChange(2);
            turnabout(face);
        }
        if (time <= 45.0f && time > 37.5f && mode != 3 && mode != 4 && mode != 1 && starting == false)
        {
            modeChange(1);
            if (mode != 5)
            {
                turnabout(face);
            }
        }
        if (time <= 37.5f && time > 7.5f && mode != 3 && mode != 4 && mode != 2 && starting == false)
        {  
            modeChange(2);
            turnabout(face);
        }
        if (time <= 7.5f && time > 0.0f && mode != 3 && mode != 4 && mode != 1 && starting == false)
        {
            modeChange(1);
            if (mode != 5)
            {
                turnabout(face);
            }
        }
        if (time <= 0.0f && mode != 3 && mode != 4 && mode != 2 && starting == false)
        {
            modeChange(2);
            turnabout(face);
        }
    }
    //Controls how long fright time lasts
    private void frightTime()
    {
        if (mode == 4 && starting == false)
        {
            if (frightIsOn == false)
            {
                frightRemaining = frightenedLength;
                animator.SetInteger("animFright", 3);
            }
            frightIsOn = true;
            frightRemaining = frightRemaining - Time.deltaTime;
            if (frightRemaining < 0.0f)
            {
                //change mode to an impossible value so timeTracker can reset it properly if it is in scatter.
                mode = 5;
                animator.SetInteger("animFright", 0);
                time = storedTime;
                speed = 2.8125f;
                timeTracker();
                frightIsOn = false;
            }
        }
    }
    private void eaten()
    {
        mode = 3;
        animator.SetInteger("eyed", 1);
        frightIsOn = false;
        speed = 5.0f;
        pinkTarget.transform.position = new Vector2(10.5f,3.0f);
        // rest is handled in trigger collision with target
    }
    public void resetTime()
    {
        time = 126.0f;
        storedTime = time;

        mode = 1;

        face = 1;

        animator.SetInteger("eyed", 0);
        animator.SetInteger("face", face);
        speed = 0.0f;
        transform.position = new Vector2(11f,3.0f);
        direction = Vector2.left;

        targetCollision = 1;
        
        upIsFine = true;
        leftIsFine = true;
        downIsFine = true;
        rightIsFine = true;
        speed = 2.8125f;
    }
    private void leavingJail()
    {
        targetCollision = 1;
        randCheck = Random.Range(0,2);
        if (randCheck == 0)
        {
            face = 1;
            animator.SetInteger("face", face);
            direction = Vector2.left;
        }
        if (randCheck == 1)
        {
            face = 3;
            animator.SetInteger("face", face);
            direction = Vector2.right;
        }
        randCheck = 0;
        starting = false;
    }
}