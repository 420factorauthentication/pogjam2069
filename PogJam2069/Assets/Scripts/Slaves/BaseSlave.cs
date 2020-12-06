using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlaveTask
{
    CutTree,
    CommitTaxFraud
}

public class BaseSlave : MonoBehaviour
{
    public LayerMask AxeHitLayer;
    public bool canDoTask = false;
    public bool isMoving = true;
    public bool isMovingToDepot = false;
    public float taskRate = 1f;
    public int woodGainPerTask = 20;
    public SlaveTask currTask = SlaveTask.CutTree;
    public float speed = 1f;
    public float allowedDiff = 0.1f;
    public float randRadiusAroundPoint = 1f;
    public float randRadiusAroundDepot = 0.3f;
    public float treeIsThereRange = 1.5f;
    public Transform currMoveTarget
    {
        get { return _currMoveTarget; }
        set
        {
            _currMoveTarget = value;
            GetComponent<AIPath>().destination = _currMoveTarget.position;
            GetComponent<AIPath>().SearchPath();
        }
    }
    public Transform currDepotTarget;
    public Animator anim;
    public bool mineSlave = false;
    public GameObject oof;
    public GameObject FabovePlayer;

    private Vector3 targetOffset = new Vector2();
    private Vector3 depotOffset = new Vector2();
    private RotateToTree rotater;
    private bool CanBeHired = false;
    private bool IsHired = false;
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
        targetOffset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * randRadiusAroundPoint;
        depotOffset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * randRadiusAroundDepot;
        rotater = GetComponentInChildren<RotateToTree>();
        rotater.tree = _currMoveTarget;
    }

    void Update()
    {
        if (canDoTask && !isMoving && timeSinceLastTask > taskRate)
        {
            Collider2D[] hitobjects = Physics2D.OverlapCircleAll(transform.position, treeIsThereRange, AxeHitLayer);
            bool treeIsThere = false;
            foreach (Collider2D thingsHit in hitobjects)
            {
                if (thingsHit.GetComponent<Tree>() != null && !thingsHit.GetComponent<Tree>().treeisDead)
                {
                    treeIsThere = true;
                }
            }

            switch (currTask)
            {
                case SlaveTask.CutTree:
                    if (treeIsThere || mineSlave)
                    {
                        anim.SetTrigger("Attack");
                        counter++;
                        if (counter > 1)
                        {
                            rotater.pointToTree = false;
                            isMovingToDepot = true;
                            isMoving = true;
                            counter = 0;
                        }
                    }
                    break;
                case SlaveTask.CommitTaxFraud:
                    Debug.Log("committing tax fraud");
                    break;
            }
            timeSinceLastTask = 0f;
        }

        float deltaTime = Time.deltaTime;
        //Debug.Log(Mathf.Abs(Vector2.Distance(currMoveTarget.position + targetOffset, transform.position)));
        //Debug.Log(currMoveTarget.position + targetOffset);
        //Debug.Log(transform.position);
        if (!isMovingToDepot && Mathf.Abs(Vector2.Distance(currMoveTarget.position + targetOffset, transform.position)) < allowedDiff)
        {
            isMoving = false;
            isMovingToDepot = false;
        }
        else if (isMovingToDepot && Mathf.Abs(Vector2.Distance(currDepotTarget.position + depotOffset, transform.position)) < allowedDiff)
        {
            isMovingToDepot = false;
            Deposit();
            // change destination
            targetOffset = new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f)).normalized * randRadiusAroundPoint;
            depotOffset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * randRadiusAroundDepot;
        }
        else
        {
            isMoving = true;
        }
        if (canDoTask && isMoving)
        {
            // move to target
            Move(deltaTime);
        }

        if(CanBeHired)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                if (mineSlave)
                {
                    GameObject mine = WoodManager.Wmanager.buildings.Find(x => x.GetComponent<Mine>());
                   // GameObject mine = GameObject.Find("Mine");
                    if (mine != null && mine.GetComponent<Mine>().IsBuilt)
                    {
                        if (NpcManager.npcManager.AddNpc(this, mineCheck:true))
                        {
                            canDoTask = true;
                            IsHired = true;
                            isMoving = true;
                            talkMinerHire();
                        }
                        else if (!IsHired)
                        {
                            talkMinerNeedHouse();
                        }
                        else
                        {
                            talkMinerWorking();
                        }
                    }
                    else
                    {
                        talkMinerNeedMine();
                    }
                }
                else if (!mineSlave)
                {
                    if (NpcManager.npcManager.AddNpc(this))
                    {
                        canDoTask = true;
                        IsHired = true;
                        talkFisherHire();
                    }
                    else if (!IsHired)
                    {
                        talkFisherNeedHouse();
                    }
                    else
                    {
                        talkFisherWorking();
                    }
                }
            }
        }

        // upate time
        timeSinceLastTask += deltaTime;
    }

    private void Deposit()
    {
        switch (currTask)
        {
            case SlaveTask.CutTree:
                WoodManager.Wmanager.addWood(woodGainPerTask, isFromMine: mineSlave, isFromTree:!mineSlave);
                rotater.pointToTree = true;
                break;
            case SlaveTask.CommitTaxFraud:
                break;
        }
    }

    private void Move(float deltaTime)
    {
        Vector3 target = isMovingToDepot ? currDepotTarget.position + depotOffset : _currMoveTarget.position + targetOffset;

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CanBeHired = true;
            FabovePlayer.SetActive(true);
        }
        if(collision.tag == "Enemy" && !mineSlave)
        {
            if (!isMoving && timeSinceLastTask > taskRate)
            {
                Instantiate(oof, transform.position, Quaternion.identity);
            }
            isMovingToDepot = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            CanBeHired = false;
            FabovePlayer.SetActive(false);
        }
        if (collision.tag == "Enemy" && !mineSlave)
        {
            if (!isMoving && timeSinceLastTask > taskRate)
            {
                Instantiate(oof, transform.position, Quaternion.identity);
            }
            isMovingToDepot = false;
        }
    }



    /////////////////////
    // EVENT DELEGATES //
    /////////////////////
    private void talkFisherNeedHouse() {
        Surprise surprise1 = new Surprise(
            "Fisherwoman Mann",
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

    private void talkFisherHire() {
        Surprise surprise1 = new Surprise(
            "Fisherwoman Mann",
            "Nice house! I'll get to work.",
            30,
            15, //Player
            false,

            "Continue",
            "",
            null
        );
        SurpriseManager.Smanager.PostSurprise(surprise1, true);
    }

    private void talkFisherWorking() {
        Surprise surprise1 = new Surprise(
            "Fisherwoman Mann",
            "How's it going, boss?",
            30,
            15, //Player
            false,

            "Continue",
            "",
            null
        );
        SurpriseManager.Smanager.PostSurprise(surprise1, true);
    }

    private void talkMinerNeedHouse() {
        Surprise surprise1 = new Surprise(
            "Robot",
            "I`m good with miners.\n" +
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

    private void talkMinerNeedMine() {
        Surprise surprise1 = new Surprise(
            "Robot",
            "I`m good with miners.\n" +
                "If you open the mine, I might be able to join your village.",
            30,
            15, //Player
            false,

            "Continue",
            "",
            null
        );
        SurpriseManager.Smanager.PostSurprise(surprise1, true);
    }

    private void talkMinerHire() {
        Surprise surprise1 = new Surprise(
            "Robot",
            "Nice mine! I'm going in!",
            30,
            15, //Player
            false,

            "Continue",
            "",
            null
        );
        SurpriseManager.Smanager.PostSurprise(surprise1, true);
    }

    private void talkMinerWorking() {
        Surprise surprise1 = new Surprise(
            "Robot",
            "How's it going, boss?",
            30,
            15, //Player
            false,

            "Continue",
            "",
            null
        );
        SurpriseManager.Smanager.PostSurprise(surprise1, true);
    }
}
