using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Info")]
    public string weaponName;
    public string type;
    public int damage;


    [Header("Ammo")]
    public float reloadTime;
    public int maxAmmo;

    [Header("Shooting")]
    public int rounds;
    public float roundDelay;

    [Header("UI")]
    public Image sprite;
}