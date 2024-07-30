using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [TextArea]
    [Tooltip("Doesn't do anything. Just comments shown in inspector")]
    public string Notes = "Press the Right Mouse Button to switch Weapons.";

    [Header("Weapon Spawning")]
    [SerializeField] GameObject[] weaponPrefab;
    [SerializeField] List<WeaponData> weaponData = new List<WeaponData>();
    [SerializeField] List<Transform> weaponSpawnPoints = new List<Transform>();
    GameObject prefabAssign;

    [Header("Audio")]
    [SerializeField] AudioSource backgroundMusic;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < weaponData.Count; i++)
        {
            if (weaponData[i].type == "Gun")
                prefabAssign = weaponPrefab[0];
            else if (weaponData[i].type == "HandHeld")
                prefabAssign = weaponPrefab[1];
            else
                prefabAssign = weaponPrefab[2];

            GameObject newWeapon = Instantiate(prefabAssign, weaponSpawnPoints[i]);
            newWeapon.GetComponent<WeaponController>().ammo = weaponData[i].maxAmmo;
            newWeapon.GetComponent<WeaponController>().maxRounds = weaponData[i].rounds;
            newWeapon.GetComponent<WeaponController>().roundDelay = weaponData[i].rounds;
            newWeapon.GetComponent<WeaponController>().type = weaponData[i].type;
            newWeapon.GetComponent<SpriteRenderer>().sprite = weaponData[i].sprite;
        }

        backgroundMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
