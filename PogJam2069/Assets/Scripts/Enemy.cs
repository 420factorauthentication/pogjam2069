using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int currHealth = 3;
    public int maxHealth = 3;
    public float searchRange = 5f;
    public bool isActive = true;
    public float speed = 1.5f;
    public bool isUnderAttack = false;
    public float closeEnough = 1f;
    public float refreshrate = 0.5f;

    [SerializeField]
    private GameObject target;
    private Vector3 actualTarget;

    [SerializeField]
    private Transform _currMoveTarget;
    private RotateToTree rotater;
    private float timeSinceLastTask = 0f;
    private Path currentPath;
    private Seeker seeker;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private float nextWaypointDistance = 1f;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
        seeker = GetComponent<Seeker>();
        rotater = GetComponentInChildren<RotateToTree>();
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        if(isActive)
        {
            Collider2D[] hitobjects = Physics2D.OverlapCircleAll(transform.position, searchRange);

            foreach (Collider2D thingsHit in hitobjects)
            {
                if(isUnderAttack && thingsHit.tag == "Guard")
                {
                    target = thingsHit.gameObject;
                    rotater.tree = target.transform;
                    rotater.pointToTree = false;
                }
                else if(!isUnderAttack && thingsHit.tag == "Npc")
                {
                    target = thingsHit.gameObject;
                    rotater.tree = target.transform;
                    rotater.pointToTree = false;
                }
            }

            if(target != null)
            {
                // randomize movement if we are close to target to make it look cool
                if (Mathf.Abs(Vector2.Distance(target.transform.position, transform.position)) < closeEnough)
                {
                    if(timeSinceLastTask > refreshrate)
                    {
                        Vector3 targetOffset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * closeEnough;
                        actualTarget = target.transform.position + targetOffset;
                        timeSinceLastTask = 0f;
                    }
                }
                else
                {
                    actualTarget = target.transform.position;
                }
                Move(delta);
            }
        }
        else
        {
            target = null;
        }
        timeSinceLastTask += delta;
    }

    public void TakeDamage(int damage)
    {
        currHealth -= damage;
        isUnderAttack = true;
        if(currHealth <= 0)
        {
            // time to die
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);
    }

    private void OnPathComplete(Path p)
    {
        p.Claim(this);
        if (!p.error)
        {
            if (currentPath != null) currentPath.Release(this);
            currentPath = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
        else
        {
            p.Release(this);
        }
    }

    private void Move(float deltaTime)
    {
        if (seeker.IsDone())
        {
            // Start a new path to the targetPosition, call the the OnPathComplete function
            // when the path has been calculated (which may take a few frames depending on the complexity)
            seeker.StartPath(transform.position, actualTarget, OnPathComplete);
        }

        if (currentPath == null)
        {
            // We have no path to follow yet, so don't do anything
            return;
        }

        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        reachedEndOfPath = false;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        while (true)
        {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector3.Distance(transform.position, currentPath.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < currentPath.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        Vector3 dir = (currentPath.vectorPath[currentWaypoint] - transform.position).normalized;
        // Multiply the direction by our desired speed to get a velocity
        Vector3 velocity = dir * speed * speedFactor;

        transform.position += velocity * deltaTime;
    }
}
