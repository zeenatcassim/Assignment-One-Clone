using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwingKO : MonoBehaviour
{
    float koSwingTimer = 0f;
    public float maxSwingTime = 0.55f;

    public bool playerSwungDoor;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerSwungDoor = true;
            koSwingTimer = 0f;
            /*if(playerSwungDoor == true)
            {
                
            }*/
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (playerSwungDoor)
            {
                gameObject.GetComponent<EnemyAI>().TakeAttack();
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        playerSwungDoor = false;
    }

    // Update is called once per frame
    void Update()
    {
        koSwingTimer += Time.deltaTime;


        if (koSwingTimer > maxSwingTime)
        {
            playerSwungDoor = false;
        }
    }
}
