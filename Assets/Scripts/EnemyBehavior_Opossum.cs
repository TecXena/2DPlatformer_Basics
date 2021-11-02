using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class EnemyBehavior_Opossum : Enemy
{

#region Variables
    // Start Variables
    private Seeker seeker;
    private SpriteRenderer sr;
    private GameObject target;
    private Transform target_Transform;

    // Public Variables
    
    public float speed;

    // Ai Variables
    public float nextWaypointDistance = 3f;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private bool targetDetected;

#endregion

#region Default Methods
    protected override void Start()
    {
        base.Start(); 
        seeker = GetComponent<Seeker>();
        sr = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player");
        target_Transform = target.transform;

        // Do the method in a certain inteval
        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }

    private void FixedUpdate()
    {
        if (targetDetected)
        {
            if (path == null)
            {
                return;
            }

            if(currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }




            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }

            if (target_Transform.position.x > transform.position.x)
            {
                sr.flipX = true;
            }
            else if(target_Transform.position.x < transform.position.x)
            {
                sr.flipX = false;
            }
        }
        
    }
#endregion

#region Physics Methods
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            targetDetected = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            targetDetected = false;
        }
    }
#endregion

#region Custom Methods
    private void UpdatePath()
    {
        if(seeker.IsDone())
        {
            seeker.StartPath(rb.position, target_Transform.position, OnPathComplete);
        }
        
    }
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
#endregion

}
