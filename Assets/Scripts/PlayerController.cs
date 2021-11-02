using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


 /* Main Issue: There are areas that can be compressed further
 */
public class PlayerController : MonoBehaviour
{

#region Variables

    // Start() Variables
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private Collider2D coll;
    private float default_jumpForce;
    private CheckNameOfActiveScene checkScene;

    // Finite State Machine
    private enum State {idle, running, jumping, falling, hurt, climb}
    private State state = State.idle;

    [Header("Inspector Variables")]
    [SerializeField] private LayerMask ground;
    /* Main issue: Does not know how to put a variable (Layer, tring) into an array 
        and use it in a condition to compare it or use it as a normal variable
    */


    [Header("Player Variables")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float knockback = 10f;


    [Header("Audio Sources")]
    [SerializeField] private AudioSource collected;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource jump;
    [SerializeField] private AudioSource powerup;
    [SerializeField] private AudioSource hurt;
    /* Main issue: Compress the amount of audio sources into either a single list 
        of audio sources
    */

    [Header("Powerup Variables")]
    [SerializeField] private float PU_jumpForce;
    [SerializeField] private Color PU_Color1;
    [SerializeField] private Color PU_Color2;
    [SerializeField] private float PU_Duration = 5f;
    private float PU_EndTransitionDuration = 5f;
    private float PU_EndTransitionDurationIntoTime;
    private float PU_Start_Duration;
    private float PU_Start_EndTransitionDurationIntoTime;

    [Header("Ladder Variables")]
    [SerializeField] private float climbSpeed = 7f;
    [HideInInspector] public bool canClimb = false;
    [HideInInspector] public bool bottomLadder = false;
    [HideInInspector] public bool topLadder = false;
    public ClimbLadder ladder;
    public float naturalGravity;

#endregion

#region Default Functions
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        checkScene = GameObject.Find("Main Camera").GetComponent<CheckNameOfActiveScene>();
        GetStartingStats();
    } 

    // Update is called once per frame
    void Update()
    {
        if (state == State.climb)
        {
            Climb();
        }
        else if (state != State.hurt)
        {
            // To easily create a method, select all of the text and press the lightbulb in the right
            Movement();
        }
        AnimationState();
        anim.SetInteger("state", (int)state); //Sets animation based on Enumerator State
    }
#endregion

#region Player Movement

    // You can rename multiple same variables by highlighting the text and press F2 then enter
    private void Movement()
    {
        // Grabs the input of Horizontal and turns it into a float
        float hDirection = Input.GetAxisRaw("Horizontal");

        // checks if canClimb is true and if the vertical varibale is greater than .1f
        //      Mathf.Abs turns the variable into a positive value
        if(canClimb && Mathf.Abs(Input.GetAxis("Vertical")) > .1f)
        {
            state = State.climb;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            transform.position  = new Vector3(ladder.transform.position.x, rb.position.y);
            rb.gravityScale = 0f;
        }

        // Going Left
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            sr.flipX = true;
        }
        // Going Right
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            sr.flipX = false;
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
            
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jump.Play();
        state = State.jumping;
    }

#endregion

#region When Touching Player 
     /* Main Issue: Background is being triggered, thus it would be hard to have repeating functions be
        only done once
    */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Collectable")
        {
            collected.Play();
            if (PermanentUI.perm != null)
            {
                PermanentUI.perm.cherries += 1;
                PermanentUI.perm.collectableScore.text = PermanentUI.perm.cherries.ToString();
            }
            Destroy(other.gameObject);
        }
        else if(other.tag == "Collectable_BlueBerry")
        {
            collected.Play();
            if (PermanentUI.perm != null)
            {
                PermanentUI.perm.cherries += 5;
                PermanentUI.perm.collectableScore.text = PermanentUI.perm.cherries.ToString();
            }
            Destroy(other.gameObject);
        }
        else if (other.tag == "Powerup")
        {
            powerup.Play();
            jumpForce = default_jumpForce + PU_jumpForce;
            StartCoroutine(Coroutine_Powerup());
            Destroy(other.gameObject);
        }   
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        // Checks if Tag is equals to Enemy
        if (other.gameObject.tag == "Enemy")
        {
            // Only Defeat Enemy when in Falling State
            if (state == State.falling)
            {
                enemy.JumpedOn(); // This method will work when an enemy has inherited methods from the enemy script
                Jump();
            }
            // Else, get knockbacked
            else
            {
                state = State.hurt;
                HandleHealth();

                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    // Enemy is to my right, therefore I should be damaged and move left
                    rb.velocity = new Vector2(-knockback, rb.velocity.y);
                }
                else
                {
                    // Enemy is to my left, therefore I should be damaged and move right
                    rb.velocity = new Vector2(knockback, rb.velocity.y);
                }
            }

        }
    }


#endregion

#region Extra Methods

    // Responsible for changing the animation of the player 
    //      based on the finiteMachine
    private void AnimationState()
    {
        if (state == State.climb)
        {

        }
        // Jumping and Falling 
        else if(state == State.jumping)
        {
            if(rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        // Falling to Idle when touching ground
        else if (state == State.falling)
        {
            if(coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        // Falling when off the ground
        else if (!coll.IsTouchingLayers(ground) && state != State.hurt)
        {
            state = State.falling;
        }
        // Hurt to Idle when not moving
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        //Mathf.Abs turns the number into a range of 0-1
        // Running when velocity is greater than 2f
        else if(Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.running;
        }
        // If all is false, go to Idle
        else
        {
            state = State.idle;
        }
    }

    // Responsible for handling health
    private void HandleHealth()
    {
        /* Main Issue: Singleton is too clunky to look at.
            Maybe better if this was a variable instead?
        */
        if (PermanentUI.perm != null)
        {
            PermanentUI.perm.health -= 1;
            hurt.Play();
            PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();

            if (PermanentUI.perm.health <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                checkScene.Reset();
            }
        }
    }

    // A corountine that pauses when another coroutine is in play
    private IEnumerator Coroutine_Powerup()
    {
        // Yield return will pause in this line and then will continue 
        //      when the method/coroutine is done
        yield return PowerupChangeColors(); // Lasts until the duration of the powerup

        // Returns variable to normal
        jumpForce = default_jumpForce;
        yield return PowerupDone(); // Transition to default state
    }

    // Color change of the powerup that lasts on the value of PU_Duration
    private IEnumerator PowerupChangeColors()
    {
        PU_Duration = PU_Start_Duration; // Resets the PU_Duration
    
        // While loop that handles a repetition of a function until the duration is up
        while (PU_Duration >= 0)
        {
            sr.color = Color.Lerp(PU_Color1, PU_Color2, Mathf.PingPong(Time.time, 1f)); 
            PU_Duration = PU_Duration - Time.deltaTime; // Reduces the Duration using time
            yield return null; // pauses then returns back to the first line of code inside the while loop
        }
    }    

    // Color transition towards the default state
    private IEnumerator PowerupDone()
    {  
        PU_EndTransitionDurationIntoTime = PU_Start_EndTransitionDurationIntoTime;
        while (PU_EndTransitionDurationIntoTime <= 1)
        {
            sr.color = Color.Lerp(sr.color, Color.white, PU_EndTransitionDurationIntoTime);
            PU_EndTransitionDurationIntoTime = PU_EndTransitionDurationIntoTime + Time.deltaTime; // Reduces the duration (Goes to 1) using time 
            yield return null; 
        }
        
    }

    // Climbing Function
    private void Climb()
    {
        float vDirection = Input.GetAxis("Vertical");
        
        // Jumping
        if (Input.GetButtonDown("Jump"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            canClimb = false;
            rb.gravityScale = naturalGravity;
            anim.speed = 1f;
            Jump();
            return; // breaks out of the function?
        }

        // Climbing up
        if(vDirection > .1f && !topLadder)
        {
            rb.velocity = new Vector2(0f, vDirection * climbSpeed);
            anim.speed = 1f;
        }
        // Climbing down
        else if(vDirection < -.1f && !bottomLadder)
        {
            rb.velocity = new Vector2(0f, vDirection * climbSpeed);
            anim.speed = 1f;
        }
        // Staying still
        else
        {
            anim.speed = 0f;
            rb.velocity = Vector2.zero;
        }
    }

    // Grabs all of the stats
    private void GetStartingStats()
    {
        if (PermanentUI.perm != null)
        {
            PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
        }
        default_jumpForce = jumpForce;
        PU_EndTransitionDurationIntoTime = Time.deltaTime/PU_EndTransitionDuration;
        PU_Start_Duration = PU_Duration;
        PU_Start_EndTransitionDurationIntoTime = PU_EndTransitionDurationIntoTime;
        naturalGravity = rb.gravityScale;
    }

    // Public function that plays in the run animation clip
    public void Footstep()
    {
        footstep.Play();
    }





    
#endregion

}
