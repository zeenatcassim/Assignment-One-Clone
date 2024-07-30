using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Weapon Spawning")]
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] List<WeaponData> weaponData = new List<WeaponData>();
    [SerializeField] List<Transform> weaponSpawnPoints = new List<Transform>();


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < weaponData.Count; i++)
        {
            GameObject newWeapon = Instantiate(weaponPrefab, weaponSpawnPoints[i]);
            newWeapon.GetComponent<WeaponController>().ammo = weaponData[i].maxAmmo;
            newWeapon.GetComponent<WeaponController>().maxRounds = weaponData[i].rounds;
            newWeapon.GetComponent<WeaponController>().roundDelay = weaponData[i].rounds;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
