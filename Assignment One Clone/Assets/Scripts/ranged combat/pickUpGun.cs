using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpGun : MonoBehaviour
{
   public bool pickedUpGun;
    public GameObject gunPrefab;
    public Transform firePoint;
    public int ammoAvailable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //player must pick up gun when near it - by touching the collider and pressing right mouse button
        if (collision.collider.CompareTag("GunPickUp") && Input.GetButtonDown("Fire1"))
        {
            // Gizmos.color = Color.red;
          //  Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);

            // Debug.Log("Picked up Gun!");

            //player equips gun
            pickedUpGun = true;

            //gun sprite gets destroyed
            Destroy(collision.gameObject);

            // Instantiate() gun in player's hands
            //Instantiate(gunPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
