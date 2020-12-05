using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlaveTask
{
    Inactive,
    CutTree,
    CommitTaxFraud
}

public class BaseSlave : MonoBehaviour
{
    public bool canDoTask = false;
    public bool isMoving = true;
    public bool isMovingToDepot = false;
    public float taskRate = 1f;
    public int woodGainPerTask = 20;
    public SlaveTask currTask = SlaveTask.Inactive;
    public float speed = 1f;
    public float allowedDiff = 0.1f;
    public Vector3 targetOffset = new Vector2();
    public Vector3 depotOffset = new Vector2();
    public Transform currMoveTarget
    {
        get { return _currMoveTarget;  }
        set
        {
            _currMoveTarget = value;
            GetComponent<AIPath>().destination = _currMoveTarget.position;
            GetComponent<AIPath>().SearchPath();
        }
    }
    public Transform currDepotTarget;
    public Animator anim;

    [SerializeField]
    private Transform _currMoveTarget;
    private float timeSinceLastTask = 0f;
    private Path currentPath;
    private Seeker seeker;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private float nextWaypointDistance = 1f;
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
    }

    void Update()
    {
        if (canDoTask && !isMoving && timeSinceLastTask > taskRate)
        {
            switch (currTask)
            {
                case SlaveTask.CutTree:
                    Debug.Log("cuttinng tree");
                    anim.SetTrigger("Attack");
                    counter++;
                    if(counter > 2)
                    {
                        isMovingToDepot = true;
                        isMoving = true;
                        counter = 0;
                    }
                    break;
                case SlaveTask.CommitTaxFraud:
                    Debug.Log("committing tax fraud");
                    break;
            }
            timeSinceLastTask = 0f;
        }

        float deltaTime = Time.deltaTime;
        if (canDoTask && isMoving)
        {
            // move to target
            Move(deltaTime);
        }
        if (!isMovingToDepot && Mathf.Abs(Vector2.Distance(currMoveTarget.position, transform.position)) < allowedDiff)
        {
            isMoving = false;
        }
        else if(isMovingToDepot && Mathf.Abs(Vector2.Distance(currDepotTarget.position, transform.position)) < allowedDiff)
        {
            isMovingToDepot = false;
            Deposit();
        }
        else
        {
            isMoving = true;
        }

        // upate time
        timeSinceLastTask += deltaTime;
    }

    private void Deposit()
    {
        switch (currTask)
        {
            case SlaveTask.CutTree:
                Debug.Log("deposited");
                WoodManager.Wmanager.addWood(woodGainPerTask);
                break;
            case SlaveTask.CommitTaxFraud:
                break;
        }
    }

    private void Move(float deltaTime)
    {
        Vector3 target = isMovingToDepot ? currDepotTarget.position : _currMoveTarget.position;

        if (timeSinceLastTask > taskRate && seeker.IsDone())
        {
            // Start a new path to the targetPosition, call the the OnPathComplete function
            // when the path has been calculated (which may take a few frames depending on the complexity)
            seeker.StartPath(transform.position, target, OnPathComplete);
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

}
