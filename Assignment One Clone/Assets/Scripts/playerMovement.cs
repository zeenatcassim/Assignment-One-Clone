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

    public LayerMask whatIsComrade;


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

    public void GameOver()
    {
        Debug.Log("Player Would have died/Respawned");
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
