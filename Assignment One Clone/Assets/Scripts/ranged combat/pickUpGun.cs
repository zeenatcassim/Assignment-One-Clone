using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpGun : MonoBehaviour
{
    /*public bool pickedUpGun;
     public GameObject gunPrefab;
     public Transform firePoint;
     public int ammoAvailable;*/

    private bool canPickUpGun = false;
    public GameObject gunPrefab;
    private Collider2D currentPickUpCollider;
    private GameObject gunInstance;

    public bool pickedUpGun;
    // public Transform firePoint;
    public int ammoAvailable;

    //reference to gun ammo script
    public gunAmmo gun;

    //reference to playerShoot
    public playerShoot shoot;

    public float throwSpeed = 10f;
   // public float enemyDetectionRadius = 5f;
    public bool canThrowGun;

    public Rigidbody2D rb;


    gunTrigger equipedGun;


    // Start is called before the first frame update
    void Start()
    {
        gun = FindAnyObjectByType<gunAmmo>();
        shoot = FindAnyObjectByType<playerShoot>();
       // canThrowGun = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (canPickUpGun) //right mouse button
            {
                PickUpGun();

                ammoAvailable = gun.maxAmmo;

            }

            if (canThrowGun)
            {
                Debug.Log("Trying to throw the gun");
               ThrowGunAtEnemy();
             }

        }

        if (Input.GetMouseButtonDown(0)) //left mouse button
        {
            if (pickedUpGun)
            {
                shoot.Shoot();
            }

        }
           

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GunPickUp"))
        {
            canPickUpGun = true;
            currentPickUpCollider = collision;
            Debug.Log("pick up");

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("GunPickUp"))
        {
            canPickUpGun = false;
            currentPickUpCollider = null;
        }
    }

    private void PickUpGun()
    { 
        if (gunInstance != null) {
            Debug.Log("Already has a gun");
            return;
        }

        if (gunPrefab != null && currentPickUpCollider != null)
        {
            gunInstance = Instantiate(gunPrefab, transform);

            //what needed to initially put in:
            gunInstance.transform.localPosition = new Vector3(-0.01f, 0.11f, 0);
            gunInstance.transform.localScale = new Vector3(0.17f, 0.07f, 0);
            gunInstance.transform.localRotation = Quaternion.identity;

            //for enemy to know state of gun
            equipedGun = gunInstance.GetComponent<gunTrigger>();
            equipedGun.EquipedGun();

            //  if (gunInstance.GetComponent<Rigidbody2D>() == null)   {   gunInstance.AddComponent<Rigidbody2D>(); }

            Rigidbody2D rb = gunInstance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0;
                rb.isKinematic = true;
            }
            Destroy(currentPickUpCollider.gameObject);
           
            pickedUpGun = true;
            canPickUpGun = false;

           // canThrowGun = true;
        }
       
    }

    public void AmmoDepleted()
    {
        Debug.Log("Ammo depleted");
        ammoAvailable = 0;
        //Destroy(gunInstance);
        // gun = null;
        pickedUpGun = true;

        canThrowGun = true;

        //  if (Input.GetMouseButtonDown(1))  {  ThrowGunAtEnemy();  }
    }

    public void ThrowGunAtEnemy()
    {
        if (gunInstance == null)
        {
            Debug.Log("Gun instance is null");
            return;
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 throwDirection = (mousePosition - (Vector2)transform.position).normalized;

         Debug.Log(mousePosition);
         Debug.Log(throwDirection);

        Debug.Log(gunInstance);

       if (gunInstance.GetComponent<Rigidbody2D>() == null) { gunInstance.AddComponent<Rigidbody2D>(); }
        Rigidbody2D rb = gunInstance.GetComponent<Rigidbody2D>(); 
        Debug.Log(rb);
        
        if (rb != null)
        {
            Debug.Log("can throw");
           //  rb.AddForce(throwDirection * throwSpeed, ForceMode2D.Impulse);

            rb.gravityScale = 0;
            rb.isKinematic = false;
            rb.velocity = throwDirection * throwSpeed;
            Debug.Log(rb.velocity);

            Debug.DrawLine(transform.position, mousePosition, Color.red, 2f);
            Debug.DrawLine(transform.position, (Vector2)transform.position + throwDirection * 5f, Color.green, 2f);


            //Let enemy know they can probably pick up this gun now
            equipedGun.UnEquipGun();


           // Destroy(gunInstance, 2f);
            canThrowGun = false;
            pickedUpGun = false;
        }
        else
        {
            Debug.LogError("Failed to add or find Rigidbody2D on the gun instance.");
        }
       
        
    }

}

