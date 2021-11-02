using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start variables
    protected Animator anim;
    protected Rigidbody2D rb;
    protected AudioSource death;
    protected Collider2D coll;

    // protected allows the children that inherits this script to access it
    // virtual allows over riding things and will not reset the codes of the other function
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        death = GetComponent<AudioSource>();
        coll = GetComponent<Collider2D>();
    }
    public void JumpedOn()
    {
        anim.SetTrigger("Death");
        death.Play();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        coll.isTrigger = true;
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }
}
