using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
        }

        if (collision.collider.CompareTag("levelProps"))
        {
            Destroy(gameObject);
        }
    }
}
