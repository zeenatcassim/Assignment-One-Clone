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


    // Start is called before the first frame update
    void Start()
    {
        gun = FindAnyObjectByType<gunAmmo>();   
    }

    // Update is called once per frame
    void Update()
    {

        if (canPickUpGun && Input.GetMouseButtonDown(1)) //right mouse button
        {
            PickUpGun();
            
            ammoAvailable = gun.maxAmmo;
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
        Destroy(gunInstance);
       // gun = null;
        pickedUpGun = false;
    }

}

