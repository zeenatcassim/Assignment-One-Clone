using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Player-Gun Spawn Transform")]
    [SerializeField] Transform playerTransform;

    [Header("Weapon Switching")]
    [SerializeField] bool canCollect = false;
    [SerializeField] int weaponToggle = 0;



    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canCollect = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canCollect = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log("Collect");
        }

        this.transform.position = playerTransform.position;
    }
}
