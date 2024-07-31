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

    //reference to gun ammo script
    public gunAmmo gun;
 

    // Start is called before the first frame update
    void Start()
    {
        picked = FindAnyObjectByType<pickUpGun>();
        gun = FindAnyObjectByType<gunAmmo>();
    }

    // Update is called once per frame
    void Update()
    {
       

        if (Input.GetMouseButtonDown(0) && picked.pickedUpGun == true) //picked up a gun && left mouse button
        {
            Shoot();
        }
       
    }

    public void Shoot()
    {
        //Instantiate a bullet at firepoint's position + rotation
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * firingSpeed, ForceMode2D.Impulse);
        //sound

         picked.ammoAvailable--;

        if (picked.ammoAvailable == 0)
        {
            picked.AmmoDepleted();
        }

    }

 

    void CalculateScore()
    {
        //distance
        // Vector2 playPosition = new Vector2(playerPos.position)
    }
}
