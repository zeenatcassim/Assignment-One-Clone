using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpGun : MonoBehaviour
{
   public bool pickedUpGun;
    public GameObject gunPrefab;
    public Transform firePoint;
    public int ammoAvailable;

    public float interactionPointRadius = 5.0f;
 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
      
         if (collision.collider.CompareTag("GunPickUp") )
        {
          
            if (Input.GetMouseButtonDown(1))
            { 
                
                Debug.Log("Picked up Gun!");
                   Destroy(collision.gameObject);  
                pickedUpGun = true;

            }

          
            // Instantiate() gun in player's hands
            //Instantiate(gunPrefab, firePoint.position, firePoint.rotation);
        }

        
        
    }

    

  
}

