using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{

    [Header("Weapon Data")]
    [SerializeField] ScriptableObject weaponData;

    [Header("Shooting")]
    [SerializeField] int reloadTime;
    [SerializeField] int maxAmmo;

    [Header("Damage")]
    [SerializeField] int points;

    [Header("Shooting / Attack")]
    [SerializeField] int roundsPerSecond;
    [SerializeField] int roundDelay;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
