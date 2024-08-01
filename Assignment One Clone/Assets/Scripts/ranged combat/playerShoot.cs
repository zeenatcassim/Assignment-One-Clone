using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShoot : MonoBehaviour
{
    //picked up gun
    public pickUpGun picked;

    //firePoint - transform
    public Transform firePoint;

    //prefab - gameobject
    public GameObject projectilePrefab;

    //Firing speed - float
    public float firingSpeed;

    //nrOfRounds- int?

  //  public GameObject playerPos;
    //public GameObject enemyPos;

    // Start is called before the first frame update
    void Start()
    {
        picked = FindAnyObjectByType<pickUpGun>();
    }

    // Update is called once per frame
    void Update()
    {
       

        if (Input.GetMouseButtonDown(0) && picked.pickedUpGun == true) //picked up a gun
        {
           // Shoot();
        }
        else if(picked.pickedUpGun == false )
        {
           // Debug.Log("Gun out of order");
           //can throw gun
        }
    }

    public void Shoot()
    {
        //Instantiate a bullet at firepoint's position + rotation
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
     
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * firingSpeed, ForceMode2D.Impulse);

        picked.ammoAvailable--;

        if (picked.ammoAvailable ==0)
        {
            //  picked.pickedUpGun = false;
            picked.AmmoDepleted();
        }
    }

 
}
