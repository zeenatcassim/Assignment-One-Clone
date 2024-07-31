using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpGun : MonoBehaviour
{
    public bool pickedUpGun = false;
   public GameObject gunPrefab;
    private Collider2D currentPickUpCollider;
    private GameObject gunInstance;

    public Transform firePoint;
    public int ammoAvailable;

  
 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (pickedUpGun && Input.GetMouseButtonDown(1))
        {
            PickUpGun();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GunPickUp"))
        {
            pickedUpGun = true;
            currentPickUpCollider = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("GunPickUp"))
        {
            pickedUpGun = false;
            currentPickUpCollider = null;
        }
    }

    private void PickUpGun()
    {
        gunInstance = Instantiate(gunPrefab, transform);

        //what needed to initially put in:
        gunInstance.transform.localPosition = new Vector3(-0.01f, 0.11f, 0);
        gunInstance.transform.localScale = new Vector3(0.17f, 0.07f, 0);
        gunInstance.transform.localRotation = Quaternion.identity;

        Destroy(currentPickUpCollider.gameObject);
        pickedUpGun = false;
    }

}

