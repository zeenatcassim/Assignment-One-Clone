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

    //LayerMask 
    public LayerMask whatIsComrade;
 

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

    //Call within shoot function, to check if players shot alerted nearby enemies
    public void PlayerSideNoiseMade()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(this.transform.position, 7f, Vector2.zero, 0, whatIsComrade);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("Enemy"))
            {
                GameObject gameObj = hit[i].collider.gameObject;
                gameObj.GetComponent<EnemyAI>().GoToNoiseLocation(this.transform.position);
            }
        }
    }



    void CalculateScore()
    {
        //distance
        // Vector2 playPosition = new Vector2(playerPos.position)
    }
}
