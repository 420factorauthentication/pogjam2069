using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public float shootRate = 3f;
    public Transform gun;
    public bool canShoot = false;
    public float searchRange = 10f;
    public int woodCostToHire = 40;
    public Transform stationedPoint;
    public bool isMovingToPoint = false;
    public float speed = 1.5f;

    public GameObject FabovePlayer;

    public static int numberShotsFired = 0;
    public static bool didbankevennt = false;

    private float timeSinceLastShoot = 0f;
    private bool canPressF = false;

    [SerializeField]
    private Transform _currMoveTarget;
    private RotateToTree rotater;
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
        rotater = GetComponentInChildren<RotateToTree>();
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        if (canShoot)
        {
            // check for enemy. If there is one, then shoot
            Collider2D[] hitobjects = Physics2D.OverlapCircleAll(transform.position, searchRange);
            bool found = false;
            foreach (Collider2D thingsHit in hitobjects)
            {
                if (thingsHit.tag == "Enemy")
                {
                    rotater.tree = thingsHit.transform;
                    found = true;
                }
            }

            rotater.pointToTree = found;

            if (found)
            {
                if (timeSinceLastShoot > shootRate)
                {
                    gun.GetComponent<Gun>().Shoot();
                    timeSinceLastShoot = 0f;
                    numberShotsFired = numberShotsFired + 1;
                }
                timeSinceLastShoot += delta;
            }

            if(numberShotsFired > 20 && !didbankevennt)
            {
                BankEvent();
                didbankevennt = true;
            }
        }

        if(canPressF)
        {
            if(Input.GetKeyUp(KeyCode.F))
            {
                //hire the guy
                if (WoodManager.Wmanager.Wood >= woodCostToHire)
                {
                    canShoot = true;
                    isMovingToPoint = true;
                    TalkHire();
                }
                else
                {
                    talkNotEnough();
                }
            }
        }

        if (isMovingToPoint && Mathf.Abs(Vector2.Distance(stationedPoint.position, transform.position)) < 0.1f)
        {
            isMovingToPoint = false;
            canShoot = true;
        }

        if(isMovingToPoint)
        {
            Move(delta);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canPressF = true;
            FabovePlayer.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canPressF = false;
            FabovePlayer.SetActive(false);
        }
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
            seeker.StartPath(transform.position, stationedPoint.transform.position, OnPathComplete);
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

    /////////////////////
    // EVENT DELEGATES //
    /////////////////////
    private void talkHireGuard()
    {
        Surprise surprise1 = new Surprise(
            "FISHERWOMAN MANN",
            "I`m Fisherwoman Mann. I`m a fisherwoman named Mann. I fish with " +
                "women, man.\n" +
                "If you get me a house, I might be able to join your village.",
            30,
            15, //Player
            false,

            "Continue",
            "",
            null
        );
        SurpriseManager.Smanager.PostSurprise(surprise1, true);
    }

    private void talkNotEnough()
    {
        Surprise surprise1 = new Surprise(
            "FISHERWOMAN MANN",
            "I`m Fisherwoman Mann. I`m a fisherwoman named Mann. I fish with " +
                "women, man.\n" +
                "If you pay me 15 wood, I might be able to help with your bandit problem.",
            30,
            15, //Player
            false,

            "Continue",
            "",
            null
        );
        SurpriseManager.Smanager.PostSurprise(surprise1, true);
    }

    private void TalkHire()
    {
        Surprise surprise1 = new Surprise(
            "FISHERWOMAN MANN",
            "I`m Fisherwoman Mann. I`m a fisherwoman named Mann. I fish with " +
                "women, man.\n" +
                "You Give House. I protect For you.",
            30,
            15, //Player
            false,

            "Continue",
            "",
            null
        );
        SurpriseManager.Smanager.PostSurprise(surprise1, true);
    }

    private void BankEvent()
    {
        Bank bank = FindObjectOfType<Bank>();
        bank.canBeBuilt = true;
        Surprise surprise1 = new Surprise(
            "A Change of Heart",
            "Another great day of watching your guards beat up bandits, or so it seems. One of the bandits suprised you today by asking to work for you. He's always loved finance and had to secretly read accounting textbooks by flashlight when he was younger. It would be great if there were a bank he could work in.",
            30,
            22, //bandit
            false,

            "I'll see what I can do.",
            "",
            null
        );
        SurpriseManager.Smanager.PostSurprise(surprise1, true);
    }
}
