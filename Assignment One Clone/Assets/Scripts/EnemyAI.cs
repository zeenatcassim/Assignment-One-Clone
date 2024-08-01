using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum EnemyState { IDLE, PATROL, DOWNED, HURT, ALERT, ATTACK, DEAD, UNARMED }


public class EnemyAI : MonoBehaviour
{
    public EnemyState enemyState;
    EnemyState startupState;
    bool seenPlayerOnce;

    //Declarations
    public Transform targetPosition;
    Vector2 worldPos;
    public Vector3 walkPoint;
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
    public LayerMask whatIsPlayer, whatIsWall, whatIsComrade, whatIsNear, whatIsWeapon;

    //Reference To Player, and FOVpivot
    public GameObject playerCharacter;

    //Distance Checks related to player detection
    public Transform meleePoint;
    public float sightRange, meleeRange;
    public bool inFieldOfView, playerInSightRange, inMeleeRange, targetSet;
    public bool weaponEquiped, rangedWeapon; // Will determine whether to shoot or move to meleeRange

    public bool isWeaponAround;

    public Transform referencePoint;
    public float fov_Range;
    Vector2 savedPosition;

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

    //Area Scanning (looking in random direction while patrolling)
    bool scanningArea;
    float scanTimer = 0;
    float maxScanTime = 3f;

    //Timers
    float knockedDownTimer = 0.0f;
    public float downedTime;
    float seenPlayerTimer = 0.0f;
    public float forgetPlayerTime;

    public float reactionTime = 1f; // switch to attack mode/state

    //Attacking control for an enemy
    public float timeBetweenAttacks;
    public float startupMeleeAttack;
    public float timeBetweenRangedAttack;
    public float startupRangedAttack;
    bool alreadyAttacked;

    bool finishEngaged;

    Rigidbody2D rb;

    public GameObject enemyGFX;
    SpriteRenderer enemySprite;

    Color enemyDefaultColor;
    Color enemyKnockedColor = Color.grey;
    Color enemyDeadColor = Color.red;

    //Finishers 
    int bashCounter;
    int bashesRequired; //Function that will set up how many bashes we require

    //Shooting Player
    public GameObject bulletPrefab;
    public float fireSpeed;

    //Picking Up Guns Completion
    gunTrigger gunNearState;

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
    }

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

            //Debug.Log("No error");
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


    /*    public void InputInformation()
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
    */

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
            //Debug.Log("IDEAL STOP");

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


    public void TakeHurt()
    {
        enemyState = EnemyState.HURT;

        enemySprite.color = enemyDeadColor;
        Debug.Log("Player is hurt");
    }

    public void TakeAttack()
    {
        //Determine if this game object was killed or no
        //if not killed
        //enemyState = EnemyState.DOWNED;
        //else enemyState = EnemyState.DEAD;

        //enemyState = EnemyState.HURT;

        //int damage, if damage > etc.

        Debug.Log("Got Hit");
        enemyState = EnemyState.DOWNED;

        enemySprite.color = enemyKnockedColor;

        //Start Timer for them to get back up


    }


    /*  public void PlayerSideNoiseMade()
      {
          RaycastHit2D[] hit = Physics2D.CircleCastAll(this.transform.position, 7f, Vector2.zero, 0, whatIsComrade);
          for (int i = 0; i < hit.Length; i++)
          {
              if (hit[i].collider.CompareTag("Enemy"))
              {
                  GameObject gameObj = hit[i].collider.gameObject;
                  gameObj.GetComponent<EnemyAI>().GoToNoiseLocation(this.transform.position);
              }
          }
      }
  */

    public void GoToNoiseLocation(Vector2 noiseLocation)
    {
        targetPosition.position = noiseLocation;

        enemyState = EnemyState.ALERT;
        //GetPath() to there would be activated

    }


    public void PlayerSideEngageFinisher()
    {

        if (Input.GetKeyDown(KeyCode.Space)) // lock out the function entry or something
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 3f, Vector2.zero, 0, whatIsComrade);
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    GameObject enemyObj = hit.collider.gameObject;

                    bool finisherEngaged = enemyObj.GetComponent<EnemyAI>().TakeFinisher();
                    if (finisherEngaged)
                    {
                        //Call our function that will lock other controls until our enemy is dead.

                    }
                    else
                    {

                    }
                }
            }
            //enemy
        }

        /*if(bashCounter < bashesRequired)
          {
          some things are true


          }
          else if (bashCounter > basehesRequired)
          {
           things be false, call the TakeFinisher Function, avoid null reference error
            

          }

           call a fucntion to reset counter and a nullify a game Object target. 
         */
    }


    public bool TakeFinisher()
    {
        if (!inFieldOfView && enemyState != EnemyState.ATTACK)
        {
            finishEngaged = true;
            return finishEngaged;
        }

        else if (enemyState == EnemyState.DOWNED)
        {
            finishEngaged = true;
            return finishEngaged;
        }
        else
        {
            Debug.Log("Enemy Downed");
            finishEngaged = false;
            return finishEngaged;
        }

    }

    public void EnemyDeath()
    {
        enemyState = EnemyState.DEAD;
        BoxCollider2D enemyCollider = this.gameObject.GetComponent<BoxCollider2D>();

        enemyCollider.enabled = false;
        enemySprite.color = enemyDeadColor;
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

    public void OnYourFeet()
    {
        enemyState = EnemyState.PATROL;
    }

    public void Patroling()
    {

        //Sensing if in sight range

        if (enemyState == EnemyState.IDLE)
        {
            targetPosition.position = transform.position;
        }

        else if (enemyState == EnemyState.PATROL)
        {

            //if we have nowhere to walk to, and have to walk, find walk point
            if (!walkPointSet)
            {
                //Debug.Log("should be searching");
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
                    //Debug.Log("Walk Point" + walkPoint);
                }

            }

            //checking how close we are to our walk point
            Vector2 distanceToWalkPoint = walkPoint - transform.position;

            if ((distanceToWalkPoint.magnitude < turnRange) && !scanningArea) //and the area is not being scanned
            {
                //when close enough, find a new point to walk to
                walkPointSet = false;
            }

            //Combat a different error, it in the PATROL state and is stuck for some reason, find new walk point
            /* if(rb.velocity == Vector2.zero)
             {
                 walkPointSet = false;
             }*/
        }

        else
        {
            Debug.Log("We seem to not be patrolling");
        }

    }


    public void SearchWalkPoint()
    {
        //Debug.Log("Searching for a walk point");
        PatternSelect(walkPattern);

    }

    public void Pat1BackAndForth()
    {
        //algorithm that will create our patterns hopefully
        //Debug.Log("Pattern 1 is being called");

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

            //a case for it being none of those specific values, basically allowing for a transition between walk patterns
            else
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
            //a case for it being none of those specific values, basically allowing for a transition between walk patterns
            else
            {
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

            else
            {
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
            lastDirRay = 2;
            nextDirRay = 3;
            //start movement to the left
            RayPointer(lastDirRay);
            walkPointSet = true;

        }

    }


    public void Pat4ScanPattern()
    {

        ScanArea();

    }

    public void ScanArea() //Make Our Enemy Look in Several Directions, perhaps three random directions
    {
        //We may have to call it repeatedly, Callbacks and/or loops, with timers controlling increments

        scanningArea = true; //making it true

        int xComp, yComp;
        xComp = Random.Range(-1, 1);
        yComp = Random.Range(-1, 1);
        //scanningArea = true;
        scanTimer = 0;

        Vector2 scanDirection = new Vector2(xComp, yComp);


        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, scanDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

        Invoke(nameof(ScanDelay), maxScanTime);

        walkPoint = transform.position;
        walkPointSet = true;

    }


    public void ScanDelay()
    {
        scanningArea = false; //will inform stuff
        WalkStyle(); // change walk pattern called 
    }



    /*  public void IdleLook()
      {
          transform.position = Vector3.zero;
      }*/

    public void PatternSelect(int patternIndex)
    {
        if (patternIndex == 0)
        {
            Pat1BackAndForth();
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
            Pat4ScanPattern();
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
        //Debug.Log("Calling RAY POINTER");

        if (rayIndex == 1)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayUp, maxRCLength, whatIsWall);
            if (hit.collider != null)
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
            if (hit.collider != null)
            {
                walkPoint = hit.point;
                Debug.Log("HitPoint" + hit.point + " Ray direction :" + rayLeft);
            }
            else
            {
                Ray2D ray = new Ray2D(transform.position, rayLeft);
                walkPoint = ray.GetPoint(infRayDist);
                Debug.Log("Ray point" + ray.GetPoint(infRayDist));
            }
        }

        else if (rayIndex == 3)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDown, maxRCLength, whatIsWall);
            if (hit.collider != null)
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
            if (hit.collider != null)
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
            if (hit.collider != null)
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
            if (hit.collider != null)
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
            if (hit.collider != null)
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
            if (hit.collider != null)
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
        //Exists to change our walk pattern
        walkPattern = Random.Range(1, 4);
        Debug.Log("New Walk Pattern");
    }

    public void AlertedWalkStyleChange()
    {

    }



    private void MeleeAttackPlayer()
    {
        if (!alreadyAttacked)
        {

            inMeleeRange = Physics2D.OverlapCircle(meleePoint.position,meleeRange,whatIsPlayer);
            if (inMeleeRange)
            {
                //Game over for our player character
                playerCharacter.GetComponent<playerMovement>().GameOver();
            }

            //Deal Damage to cause the player to have to reset the game
            //Game Over for player

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }


    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    private void RangedAttackPlayer()
    {
        if (!alreadyAttacked)
        {

            //Fire a projectile at the player, projectile will handle player death
            GameObject firedBulled = Instantiate(bulletPrefab, meleePoint.position, meleePoint.rotation);
      
            Rigidbody2D rb = firedBulled.GetComponent<Rigidbody2D>();

            rb.AddForce(meleePoint.up * fireSpeed, ForceMode2D.Impulse);

            RaycastHit2D hitScan = Physics2D.Raycast(meleePoint.position, meleePoint.up, sightRange, whatIsPlayer);

            if(hitScan.collider.CompareTag("Player"))
            {
                //Game over for our player character
                playerCharacter.GetComponent<playerMovement>().GameOver();
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenRangedAttack);
        }
    }

    private void LastKnownLocation(Vector2 position)
    {
        savedPosition = position;
    }





    public void Attacking()
    {
        //enemyState = EnemyState.ATTACK;

        //SetDestintion(playerCharacter.transform.position);

        if (inFieldOfView && playerInSightRange)
        {
            targetPosition.position = playerCharacter.transform.position;  // Add control
            LastKnownLocation(playerCharacter.transform.position); // Nah, I cooked here and with this function

        }
        else
        {
            targetPosition.position = savedPosition;
        }



        inMeleeRange = Physics2D.OverlapCircle(meleePoint.position, meleeRange, whatIsPlayer);

        if (inMeleeRange && !rangedWeapon) //in melee range with a melee weapon
        {
            //Perform Melee attacks
            Invoke(nameof(MeleeAttackPlayer), startupMeleeAttack);

        }

        if (rangedWeapon && playerInSightRange)
        {
            Invoke(nameof(RangedAttackPlayer), startupRangedAttack);
        }

    }


    public void PlayerDetection()
    {
        playerInSightRange = Physics2D.OverlapCircle(transform.position, sightRange, whatIsPlayer);
        // playerInSightRange = Physics2D.CircleCast(transform.position, sightRange, movementDirection, 0, whatIsPlayer);

        if (playerInSightRange)
        {
            Vector2 Vect1 = referencePoint.position - transform.position;
            Vector2 Vect2 = playerCharacter.transform.position - transform.position;

            float fov_angle = Vector2.Angle(Vect1, Vect2); //unsigned check of angles using vector 1 as reference

            if (fov_angle < fov_Range)
            {
                //Debug.Log("In field of view");
                /*We are within the eyeline of our enemy, one last raycast to see if we have a direct line of fire
                between us and them, and if we do, we have indeed seen them, and can indeed chase them down*/
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vect2, sightRange, whatIsNear);
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        //We actually managed to see our player in our peripheral
                        //Become Hostile and attack or move towards player position

                        inFieldOfView = true;
                        seenPlayerOnce = true;
                        //We'll check if we have a weapon
                        if (weaponEquiped)
                        {
                            //enemyState = EnemyState.ATTACK;
                            Invoke(nameof(SetAttackState), reactionTime);
                            //SetAttackState();
                        }

                        seenPlayerTimer = 0; //we reset our seeing player timer everytime we have seen our player


                    }
                    else
                    {
                        //Player is not within their field of view
                        inFieldOfView = false;
                    }
                }

            }

            else
            {
                inFieldOfView = false;


            }


        }

    }




    private void SetAttackState()
    {
        if (enemyState != EnemyState.ATTACK)
        {
            enemyState = EnemyState.ATTACK;
        }

    }

    private void SetUnarmedState()
    {
        if (enemyState != EnemyState.UNARMED)
        {
            enemyState = EnemyState.UNARMED;
        }

    }


    private void FindWeapon() // Mix of Patrol && Others
    {
        isWeaponAround = Physics2D.OverlapCircle(transform.position, sightRange / 2, whatIsWeapon);
       

        if (isWeaponAround)
        {
            RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, sightRange / 2, Vector2.zero, 0, whatIsWeapon);
            //Find out if its in an equipable state. Walk towards it and pick it up
            for(int i = 0; i < hit.Length; i++)
            {
                if(hit[i].collider != null)
                {
                    GameObject getWeapon = hit[i].collider.gameObject;
                    //getWeapon.GetComponent<>  // see if the weapon is not in the players hands, (check if it has enough ammo ?)
                }
            }
          
           

        }
        /* else if()            
         {
             //Check if the player has a weapon equiped and is within our weapon range
             //in such a case, //make cautious walk pattern

         }*/
        else
        {
            //Do some basic patrolling
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();

        rb = GetComponent<Rigidbody2D>();

        //targetPosition.position = transform.position;


        path = null;

        enemySprite = GetComponentInChildren<SpriteRenderer>();

        enemyDefaultColor = enemySprite.color;
        //enemySprite.color = Color.white;

        //FaceDirection();

        walkPointSet = false;
        walkPoint = new Vector2(transform.position.x, transform.position.y);

        processingDest = false;

        seenPlayerOnce = false;

        alreadyAttacked = false;

        scanningArea = false;

        //playerCharacter = GameObject.Find("Player");


        InvokeRepeating("GetPath", 0f, 1f);
        startupState = this.enemyState;

    }

    // Update is called once per frame
    void Update()
    {

        knockedDownTimer += Time.deltaTime;
        seenPlayerTimer += Time.deltaTime;
        scanTimer += Time.deltaTime;


        if (enemyState == EnemyState.DOWNED && !finishEngaged)
        {
            if (knockedDownTimer > downedTime)
            {
                OnYourFeet();

            }
        }

        PlayerDetection();


        if ((!playerInSightRange || !inFieldOfView) && weaponEquiped)
        {

           
                if (seenPlayerTimer > forgetPlayerTime) //aftert this much time of not seeing the player, go back to patrolling
                {

                    enemyState = EnemyState.PATROL; // Patrol if you have seen the player atleast once 

                if (seenPlayerOnce)
                {
                    WalkStyle(); // Change of Walk Pattern, we have to call it only once so we only change walk style when this is called
                    seenPlayerOnce=false; //reset so that we can reactivate it when we see the player again and change our walk pattern again
                }
                  
                    Patroling();
                }

            
            /*     else
                 {

                     *//* if (playerInSightRange || !inFieldOfView)*//*
                     enemyState = startupState;
                     Patroling();
                 }*/


        }

        /*   if (playerInSightRange && inFieldOfView)
           {
               Attacking();
           }*/

        if (enemyState == EnemyState.ATTACK)
        {
            Attacking();
        }


        if (!weaponEquiped)
        {
            SetUnarmedState();
            FindWeapon();
        }

    }


    private void FixedUpdate()
    {
        //


        MoveToTarget();


    }

    /* private void OnDrawGizmos()
     {
         Gizmos.color = Color.yellow;
         Gizmos.DrawWireSphere(transform.position,sightRange);

         Gizmos.color = Color.magenta;
         Gizmos.DrawRay(transform.position, referencePoint.position - transform.position);
         Gizmos.DrawRay(transform.position, playerCharacter.transform.position - transform.position);

     }*/

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; //Sight Range
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.magenta; //Field Of View Angles
        Gizmos.DrawRay(transform.position, referencePoint.position - transform.position);
        Gizmos.DrawRay(transform.position, playerCharacter.transform.position - transform.position);

        Gizmos.color = Color.cyan; // Enemy Melee Range
        Gizmos.DrawWireSphere(meleePoint.position, meleeRange);
    }
}
