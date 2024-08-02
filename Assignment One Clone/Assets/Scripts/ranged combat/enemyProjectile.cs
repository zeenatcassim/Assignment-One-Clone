using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyProjectile : MonoBehaviour
{
    public playerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<playerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
            player.GameOver();
            //enemy.EnemyDeath();
        }

        if (collision.collider.CompareTag("levelProps"))
        {
            Destroy(gameObject);
        }

    }
}