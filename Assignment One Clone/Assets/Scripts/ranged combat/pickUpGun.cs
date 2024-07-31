using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpGun : MonoBehaviour
{
    private bool canPickUpGun = false;
   public GameObject gunPrefab;
    private Collider2D currentPickUpCollider;
    private GameObject gunInstance;

    public bool pickedUpGun;
   // public Transform firePoint;
    public int ammoAvailable;

    //reference to gun ammo script
    public gunAmmo gun;

    public float throwForce = 10f;
    public float enemyDetectionRadius = 5f;
    public bool canThrowGun;

    // Start is called before the first frame update
    void Start()
    {
        gun = FindAnyObjectByType<gunAmmo>();   
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
             else if (canThrowGun)
            {
              ThrowGunAtEnemy();
            }

        }
 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GunPickUp"))
        {
            canPickUpGun = true;
            currentPickUpCollider = collision;
            
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
        if (gunPrefab != null && currentPickUpCollider != null)
        {

           // if (gunInstance != null){Destroy(gunInstance); }

        gunInstance = Instantiate(gunPrefab, transform);

        //what needed to initially put in:
        gunInstance.transform.localPosition = new Vector3(-0.01f, 0.11f, 0);
        gunInstance.transform.localScale = new Vector3(0.17f, 0.07f, 0);
        gunInstance.transform.localRotation = Quaternion.identity;

        Destroy(currentPickUpCollider.gameObject);
        canPickUpGun = false;
        pickedUpGun = true;
        }
        
    }

    public void AmmoDepleted()
    {
        ammoAvailable = 0;
        //Destroy(gunInstance);
       // gun = null;
        pickedUpGun = false; 
        canThrowGun = true;
    }

    void ThrowGunAtEnemy()
    {
        if (gunInstance != null)
        {
           
            Collider2D nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                gunInstance.transform.parent = null;

                Rigidbody2D rb = gunInstance.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 direction = (nearestEnemy.transform.position - gunInstance.transform.position).normalized;
                    rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
                }
                Destroy(gunInstance, 2f);

                gunInstance = null;
            }
            else
            {
                Debug.Log("No enemies nearby to throw the gun at");
            }
        }
    }

     Collider2D FindNearestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, enemyDetectionRadius);
        Collider2D nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("enemyAI"))
            {
                Debug.Log("found enemy");
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
        }

        return nearestEnemy;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyDetectionRadius);
    }
}

