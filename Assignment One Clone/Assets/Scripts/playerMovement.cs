using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;

    public Rigidbody2D rb;
    public Camera cam;

    Vector2 movement;
    Vector2 mousePos;

    public Animator animator;
<<<<<<< Updated upstream
=======
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
>>>>>>> Stashed changes

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        bool isWalking = movement.sqrMagnitude > 0;

        //animator.SetBool("isWalking", isWalking);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(PunchAnim());
        }
    }

    private IEnumerator PunchAnim()
    {
        Debug.Log("punch works");
        animator.SetBool("punch", true);
        yield return new WaitForSeconds(.5f);
        animator.SetBool("punch", false);

    }

    private void FixedUpdate()
    {
        // Move the player
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);

        // Update player rotation based on mouse position
        Vector2 lookDirection = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
}
