using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum EnemyState { IDLE, PATROL, DOWNED, HURT, ALERT }


public class EnemyAI : MonoBehaviour
{
    //Declarations
    public Transform targetPosition;
    Vector2 worldPos;
    Vector3 walkPoint;
    [SerializeField] private float speeed;
    float speedFactor;


    [SerializeField] private float rotationSpeed;

    private Vector2 movementDirection;
    private Vector2 movementVector;

    Seeker seeker;
    Path path;

    Vector3 mousePos;

    public bool reachedEndOfPath; //A bool to stop movement once a target position is reached

    public float nextWayPointDistance;
    private int currentWayPoint = 0;
    float distanceToWaypoint;

    //Layers
    public LayerMask whatIsPlayer, whatIsWall, whatIsComrade;

    //Distance Checks related to player detection
    public float sightRange, meleeRange;
    public bool inFieldOfView, playerInSightRange, targetSet;
    public bool weaponEquiped, rangedWeapon; // Will determine whether to shoot or move to meleeRange

    //Distance Checks related to walkpoint algorithm
    public float turnRange;
    public bool inTurnRange, walkPointSet;
    public float walkPointDistance;

    //Directional Rays
    Vector2 rayUp = Vector2.up, rayDown = Vector2.down, rayLeft = Vector2.left, rayRight = Vector2.right;
    Vector2 rayNE = new Vector2(1, 1), rayNW = new Vector2(-1, 1),
        raySW = new Vector2(-1, -1), raySE = new Vector2(1, -1);

    public float maxRCLength;
    public float infRayDist;

    IDictionary<int, Vector2> dirRays = new Dictionary<int, Vector2>();
    [SerializeField] int lastDirRay = 0, nextDirRay = 0;

    RaycastHit2D rayHitOut;
    public bool processingDest;

    [SerializeField] int walkPattern;

    //Timers

    Rigidbody2D rb;

    public GameObject enemyGFX;
    SpriteRenderer enemySprite;

    Color enemyDefault;
    Color enemyKnocked = Color.grey;
    Color enemyDead = Color.red;



    void GetPath()
    {
        //Debug.Log("Get Path called");
        if (seeker.IsDone())
            seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        //Determine what to do with our calculated paths

        Debug.Log(p.error);

        if (!p.error)
        {
            //There were no errors with the path calculated, so we can advance towards it
            path = p;

            currentWayPoint = 0; //Counter set to zero in order to start traversal
            reachedEndOfPath = false; // 

            Debug.Log("No error");
            processingDest = true; // a way to ensure we call the GetPath Once

        }

        else if (p.error)
        {
            //walkPointSet = false;
            Debug.Log("An error occured");
        }

    }

    public void SetDestintion(Vector2 destinationPos)
    {
        targetPosition.position = destinationPos;

        GetPath();
    }


    public void InputInformation()
    {
        //Debug.Log("We will get this position");
        mousePos = Input.mousePosition;

        //we take out mouse screen position in pixels and translate it to a world space position
        Vector3 worldPos3D = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 worldPosition = new Vector2(worldPos3D.x, worldPos3D.y);

        worldPos = worldPosition;
        targetPosition.position = worldPosition;

        GetPath();

    }


    public void MoveToTarget()
    {
        if (path == null)
        {
            // With no path, we are stationary
            return;
        }



        if (currentWayPoint + 1 >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            Debug.Log("IDEAL STOP");

            movementDirection = Vector2.zero;

            speedFactor = 0f;

            processingDest = false; //Break of the destination Processing

        }
        else
        {
            reachedEndOfPath = false;

            movementDirection = (Vector2)(path.vectorPath[currentWayPoint] - transform.position).normalized;
            movementVector = (Vector2)(targetPosition.position - transform.position).normalized;

            FaceNodeDirection();


            Vector2 velocity = movementDirection * speeed * speedFactor;

            rb.velocity = velocity * Time.deltaTime;
        }


        distanceToWaypoint = Vector2.Distance(transform.position, path.vectorPath[currentWayPoint]);

        if (distanceToWaypoint < nextWayPointDistance)
        {
            if (currentWayPoint + 1 < path.vectorPath.Count)
            {
                currentWayPoint++;
            }

        }


        if (reachedEndOfPath)
        {
            //speedFactor = Mathf.Sqrt(distanceToWaypoint / nextWayPointDistance);
        }
        else
        {
            speedFactor = 1f;
        }
    }

    public void TakeAttack()
    {
        //Determine if this game object was killed or no

    }

    public void FaceDirection()
    {

        //transform.Rotate(Vector3.forward, 90f);
    }

    public void FaceNodeDirection()
    {

        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

    }

    public void Sensing()
    {

    }

    public void Patroling()
    {
        //Sensing if in sight range

        //if we have nowhere to walk to, and have to walk, find walk point
        if (!walkPointSet)
        {
            Debug.Log("hould be searching");
            SearchWalkPoint();
            //SearchWalkPoint();
        }

        //with a walk point, walk towards that point
        if (walkPointSet)
        {
            if (processingDest)
            {

            }
            else
            {
                SetDestintion(walkPoint);
            }

        }

        //checking how close we are to our walk point
        Vector2 distanceToWalkPoint = walkPoint - transform.position;

        if (distanceToWalkPoint.magnitude < turnRange)
        {
            //when close enough, find a new point to walk to
            walkPointSet = false;
        }

        //Combat a different error, it in the PATROL state and is stuck for some reason, find new walk point
        if (rb.velocity == Vector2.zero)
        {
            walkPointSet = false;
        }
    }


    public void SearchWalkPoint()
    {
        /*//algorithm that will create our patterns hopefully
        

        //Back and forth creation
        if(lastDirRay != 0)
        {
            //we have a walk pattern already set
            //Back and Forth, i.e left and right i.e 2 && 4
            //I could probably fit this whole thing into a function tbh
            if(nextDirRay == 2)
            {
                //move to the left, find a point on the left/west
                //RaycastHit2D hit = Physics2D.Raycast(transform.position,rayLeft,maxRCLength,whatIsWall);
                RayPointer(nextDirRay);
                nextDirRay = 4; 
                

            }
            else if(nextDirRay == 4)
            {
                //move to the right, find a point on the right/east
             
                RayPointer(nextDirRay);
                nextDirRay = 2;
                
            }
            lastDirRay = nextDirRay;
            walkPointSet = true;
            
            
        }
        else
        {
            Debug.Log("Else case has been called");
            //abide by this pattern now
            lastDirRay = 2;
            nextDirRay = 4;
            //start movement to the left
            RayPointer(lastDirRay);
            walkPointSet = true;
           
        }*/

        PatternSelect(walkPattern);

    }

    public void Pat1BackAndForth()
    {
        //algorithm that will create our patterns hopefully


        //Back and forth creation
        if (lastDirRay != 0)
        {
            //we have a walk pattern already set
            //Back and Forth, i.e left and right i.e 2 && 4
            //I could probably fit this whole thing into a function tbh
            if (nextDirRay == 2)
            {
                //move to the left, find a point on the left/west
                //RaycastHit2D hit = Physics2D.Raycast(transform.position,rayLeft,maxRCLength,whatIsWall);
                RayPointer(nextDirRay);
                nextDirRay = 4;


            }
            else if (nextDirRay == 4)
            {
                //move to the right, find a point on the right/east

                RayPointer(nextDirRay);
                nextDirRay = 2;

            }
            lastDirRay = nextDirRay;
            walkPointSet = true;


        }
        else
        {
            Debug.Log("Else case has been called");
            //abide by this pattern now
            lastDirRay = 2;
            nextDirRay = 4;
            //start movement to the left
            RayPointer(lastDirRay);
            walkPointSet = true;

        }

    }

    public void Pat2BackAndForth()
    {
        //Back and forth Pattern 2 creation
        if (lastDirRay != 0)
        {
            //Back and Forth, i.e up and down i.e 1 && 3
            if (nextDirRay == 1)
            {
                //move downwards, find a point south
                RayPointer(nextDirRay);
                nextDirRay = 3;

            }
            else if (nextDirRay == 3)
            {
                //move upwards, find a point to the north

                RayPointer(nextDirRay);
                nextDirRay = 1;

            }
            lastDirRay = nextDirRay;
            walkPointSet = true;


        }
        else
        {
            Debug.Log("Else case has been called");
            //abide by this pattern now
            lastDirRay = 1;
            nextDirRay = 3;
            //start movement going down
            RayPointer(lastDirRay);
            walkPointSet = true;

        }

    }


    public void Pat3Roundabout()
    {
        //Pat3 Roundabout, Yes its a JoJo's reference
        if (lastDirRay != 0)
        {
            //Moving around in a room i.e 1, 2, 3 && 4
            if (nextDirRay == 2)
            {
                //move to the left, find a point on the left/west
                //RaycastHit2D hit = Physics2D.Raycast(transform.position,rayLeft,maxRCLength,whatIsWall);
                RayPointer(nextDirRay);
                nextDirRay = 3;

            }
            else if (nextDirRay == 3)
            {
                //move to the right, find a point on the right/east

                RayPointer(nextDirRay);
                nextDirRay = 4;

            }

            else if (nextDirRay == 4)
            {
                RayPointer(nextDirRay);
                nextDirRay = 1;
            }

            else if (nextDirRay == 1)
            {
                RayPointer(nextDirRay);
                nextDirRay = 2;
            }
            lastDirRay = nextDirRay;
            walkPointSet = true;

        }
        else
        {
            Debug.Log("Else case has been called");
            //abide by this pattern now
            lastDirRay = 2;
            nextDirRay = 3;
            //start movement to the left
            RayPointer(lastDirRay);
            walkPointSet = true;

        }

    }

    public void PatternSelect(int patternIndex)
    {
        if (patternIndex == 0)
        {
            return;
        }

        if (patternIndex == 1)
        {
            Pat1BackAndForth();
        }
        else if (patternIndex == 2)
        {
            Pat2BackAndForth();
        }
        else if (patternIndex == 3)
        {
            Pat3Roundabout();
        }
        else if (patternIndex == 4)
        {

        }
        else if (patternIndex == 5)
        {

        }
        else
        {
            Debug.Log("No more patterns");
            Pat1BackAndForth();
        }
    }


    public void RayPointer(int rayIndex)
    {
        if (rayIndex == 1)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayUp, maxRCLength, whatIsWall);
            if (hit.point != null)
            {
                walkPoint = hit.point;
            }
            else
            {
                Ray2D ray = new Ray2D(transform.position, rayUp);
                walkPoint = ray.GetPoint(infRayDist);
            }
        }

        else if (rayIndex == 2)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayLeft, maxRCLength, whatIsWall);
            if (hit.point != null)
            {
                walkPoint = hit.point;
            }
            else
            {
                Ray2D ray = new Ray2D(transform.position, rayLeft);
                walkPoint = ray.GetPoint(infRayDist);
            }
        }

        else if (rayIndex == 3)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDown, maxRCLength, whatIsWall);
            if (hit.point != null)
            {
                walkPoint = hit.point;
            }
            else
            {
                Ray2D ray = new Ray2D(transform.position, rayDown);
                walkPoint = ray.GetPoint(infRayDist);
            }
        }

        else if (rayIndex == 4)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayRight, maxRCLength, whatIsWall);
            if (hit.point != null)
            {
                walkPoint = hit.point;
            }
            else
            {
                Ray2D ray = new Ray2D(transform.position, rayRight);
                walkPoint = ray.GetPoint(infRayDist);
            }
        }

        else if (rayIndex == 5)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayNW, maxRCLength, whatIsWall);
            if (hit.point != null)
            {
                walkPoint = hit.point;
            }
            else
            {
                Ray2D ray = new Ray2D(transform.position, rayNW);
                walkPoint = ray.GetPoint(infRayDist);
            }
        }

        else if (rayIndex == 6)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, raySW, maxRCLength, whatIsWall);
            if (hit.point != null)
            {
                walkPoint = hit.point;
            }
            else
            {
                Ray2D ray = new Ray2D(transform.position, raySW);
                walkPoint = ray.GetPoint(infRayDist);
            }
        }

        else if (rayIndex == 7)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, raySE, maxRCLength, whatIsWall);
            if (hit.point != null)
            {
                walkPoint = hit.point;
            }
            else
            {
                Ray2D ray = new Ray2D(transform.position, raySE);
                walkPoint = ray.GetPoint(infRayDist);
            }
        }

        else if (rayIndex == 8)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayNE, maxRCLength, whatIsWall);
            if (hit.point != null)
            {
                walkPoint = hit.point;
            }
            else
            {
                Ray2D ray = new Ray2D(transform.position, rayNE);
                walkPoint = ray.GetPoint(infRayDist);
            }
        }


    }

    public void WalkStyle()
    {

    }

    public void Attacking()
    {

    }





    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();

        rb = GetComponent<Rigidbody2D>();

        //targetPosition.position = transform.position;


        path = null;

        enemySprite = GetComponentInChildren<SpriteRenderer>();

        enemyDefault = enemySprite.color;
        //enemySprite.color = Color.white;

        //FaceDirection();

        walkPointSet = false;
        walkPoint = new Vector2(transform.position.x, transform.position.y);

        processingDest = false;



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Some Input");
            InputInformation();
        }

        if (!playerInSightRange && !inFieldOfView)
        {

            Patroling();
        }

        if (playerInSightRange && !inFieldOfView)
        {
            Patroling();
            //sensing will control FieldOfView
        }

        if (playerInSightRange && inFieldOfView)
        {
            Attacking();
        }

    }


    private void FixedUpdate()
    {
        //

        MoveToTarget();


    }
}
