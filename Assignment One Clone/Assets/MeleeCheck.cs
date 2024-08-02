using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCheck : MonoBehaviour
{
    public Sprite downedSprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemyAI")
        {
            Debug.Log("colliding with enemy");

            collision.GetComponent<EnemyAI>().TakeAttack();

            //collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //collision.GetComponent<EnemyAI>().enabled = false;
            collision.transform.Find("EnemyCharacterGFX").GetComponent<Animator>().SetBool("isDowned", true);

            GameObject.Find("Player").GetComponent<playerMovement>().curenemyDown = true;
        }
    }
}
