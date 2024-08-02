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

    public LayerMask whatIsComrade;

    public AudioSource gunShot;

    // Start is called before the first frame update
    void Start()
    {
        picked = FindAnyObjectByType<pickUpGun>();

        if(whatIsComrade != LayerMask.NameToLayer("Enemy"))  //can comment out this line after setting layer mask to enemy layer in inspector
        {
            whatIsComrade = LayerMask.NameToLayer("Enemy");
        }
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

        //play animation??
        //sound
        gunShot.Play();

        if (picked.ammoAvailable <=0)
        {
            //  picked.pickedUpGun = false;
            picked.ammoAvailable = 0;
            picked.AmmoDepleted();
        }
        else
        {
            PlayerSideNoiseMade();  // If ammo is available, make noise for our enemy to hear
        }
    }

    public void PlayerSideNoiseMade()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(this.transform.position, 7f, Vector2.zero, 0, whatIsComrade);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.CompareTag("enemyAI"))
            {
                GameObject gameObj = hit[i].collider.gameObject;
                gameObj.GetComponent<EnemyAI>().GoToNoiseLocation(this.transform.position);
            }
        }
    }


}
