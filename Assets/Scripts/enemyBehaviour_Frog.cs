using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBehaviour_Frog : Enemy
{
    // Serialized fields
    [SerializeField] private GameObject gameObjectleftCap;
    [SerializeField] private GameObject gameObjectrightCap;
    [SerializeField] private float jumpLength = 5f;
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private LayerMask ground;

    // Start variables
    private float leftCap;
    private float rightCap;
    private SpriteRenderer sr;

    

    // Private variables
    private bool facingLeft = true;
    
    

    // override allows you to add new stuff while not removing the other stuff in the function
    protected override void Start()
    {
        // base is the inherited script. 
        //This will make this script run the start function of enemy
        base.Start(); 
        leftCap = gameObjectleftCap.transform.position.x;
        rightCap = gameObjectrightCap.transform.position.x;
        sr = GetComponent<SpriteRenderer>();
        
    }

    void Update()
    {
        // Transition from jump to fall
        if(anim.GetBool("Jumping"))
        {
            if(rb.velocity.y < .1f)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }
        // Transition from fall to idle
        if(coll.IsTouchingLayers(ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
        }

    }

    private void Move()
    {
        if (facingLeft)
        {
            // Test to see if we are beyond the leftCap
            // If not, face right
            if (transform.position.x > leftCap)
            {
                sr.flipX = false;
                if (coll.IsTouchingLayers(ground))
                {
                    //jump
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                sr.flipX = true;
                if (coll.IsTouchingLayers(ground))
                {
                    //jump
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }


}
