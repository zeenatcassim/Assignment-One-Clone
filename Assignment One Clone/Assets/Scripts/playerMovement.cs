using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float originalMoveSpeed; // To store the original move speed

    public Rigidbody2D rb;
    public Camera cam;

    Vector2 movement;
    Vector2 mousePos;

    public int bashCounter;
    public bool isDown = false;

    public Animator animator;
    public GameObject MeleeCollider;

    public LayerMask whatIsComrade;

    public GameObject[] bloodPrefabs; // Array to hold different blood splatter prefabs
    public int splatterCount = 5;
    public float spread = 1f;

    public bool isDead;

    public bool curenemyDown = false;

    public GameObject pauseScreen;
    public GameObject gameOverScreen;

    private bool isPaused = false;

    [Header("Restart UI")]
    [SerializeField] GameManager gameManager;

    void Start()
    {
        originalMoveSpeed = moveSpeed; // Store the original move speed
        pauseScreen.SetActive(false); // Ensure pause screen is initially hidden

        gameOverScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SpawnBlood(Vector2 position)
    {
        for (int i = 0; i < splatterCount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * spread;
            int randomIndex = Random.Range(0, bloodPrefabs.Length); // Randomly select a blood prefab
            Instantiate(bloodPrefabs[randomIndex], position + randomOffset, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (isPaused) return; // Do nothing if the game is paused

        if (!isDown)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            movement = Vector2.zero; // Lock movement when isDown is true
        }

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        bool isWalking = movement.sqrMagnitude > 0;

        animator.SetBool("isWalking", isWalking);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (curenemyDown && isDown)
            {
                bashCounter++;
                SpawnBlood(transform.position);

                if (bashCounter == 3)
                {
                    enemyObj.GetComponent<EnemyAI>().EnemyDeath();
                    Debug.Log("Enemy finished");
                    ResetFinisherState();
                }
            }
            else
            {
                StartCoroutine(PunchAnim());
            }
        }

        if (curenemyDown && !isDown)
        {
            PlayerSideEngageFinisher();
        }

        if (isDead)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                gameOverScreen.SetActive(false);
                Time.timeScale = 1f;
                SceneManager.LoadScene("No Talk F2");
            }
        }
    }

    private IEnumerator PunchAnim()
    {
        animator.SetBool("punch", true);
        MeleeCollider.SetActive(true);
        yield return new WaitForSeconds(.5f);
        animator.SetBool("punch", false);
        MeleeCollider.SetActive(false);
    }

    public GameObject enemyObj;

    public void PlayerSideEngageFinisher()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");

            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 3f, Vector2.zero, 0, whatIsComrade);
            if (hit.collider != null && hit.collider.CompareTag("enemyAI"))
            {
                enemyObj = hit.collider.gameObject;

                bool finisherEngaged = enemyObj.GetComponent<EnemyAI>().TakeFinisher();
                if (finisherEngaged)
                {
                    Debug.Log("Enemy Finishing");
                    isDown = true;

                    // Set the player's position to the enemy's position
                    transform.position = enemyObj.transform.position;

                    // Lock player movement
                    moveSpeed = 0;

                    enemyObj.GetComponent<EnemyAI>().enabled = false;
                    enemyObj.transform.Find("EnemyCharacterGFX").GetComponent<Animator>().SetBool("isWalking", false);
                    enemyObj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    enemyObj.transform.Find("EnemyCharacterGFX").GetComponent<Animator>().SetBool("isDowned", true);
                }
            }
        }
    }

    public void ResetFinisherState()
    {
        bashCounter = 0;
        isDown = false;
        moveSpeed = originalMoveSpeed; // Unlock player movement
        curenemyDown = false;
        enemyObj.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void GameOver()
    {
        StartCoroutine(KillPause());
    }


    private IEnumerator KillPause()
    {
        Debug.Log("Player Would have died/Respawned");
        isDead = true;
        animator.SetBool("isDowned", true);

        yield return new WaitForSeconds(.7f);

        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    private void FixedUpdate()
    {
        if (isPaused) return; // Do nothing if the game is paused

        // Move the player
        rb.MovePosition(rb.position + movement * originalMoveSpeed * Time.deltaTime);

        // Update player rotation based on mouse position
        Vector2 lookDirection = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        // Adjust this offset based on your sprite's default orientation
        float rotationOffset = -360;  // Experiment with this value
        rb.rotation = angle + rotationOffset;
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        pauseScreen.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }
}
