using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public EnemyAI enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = FindAnyObjectByType<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        //bullet gets destroyed after touching enemy
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("enemyAI"))
        {
            Destroy(gameObject);
            enemy.EnemyDeath();
        }

        if (collision.collider.CompareTag("levelProps"))
        {
            Destroy(gameObject);
        }
    }
}
