using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Player-Gun Spawn Transform")]
    [SerializeField] Transform playerTransform;

    [Header("Weapon Switching")]
    [SerializeField] GameObject indicator;
    [SerializeField] bool canCollect = false;
    [SerializeField] int collected = 0;


    public int maxRounds;
    public int ammo;
    public int roundDelay;
    public string type;


    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            canCollect = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            canCollect = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && canCollect == true)
        {
            if (collected == 1) collected = 0;
            else collected = 1;

        }

        if (collected == 1)
            this.transform.position = playerTransform.position;

        if (canCollect)
        {
            
        }
        else
        {
            
        }

        indicator.SetActive(canCollect);
    }
}