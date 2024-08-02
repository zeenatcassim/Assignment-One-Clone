using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

            collision.transform.Find("EnemyCharacterGFX").GetComponent<Animator>().SetBool("isDowned", true);

        }
    }


}
