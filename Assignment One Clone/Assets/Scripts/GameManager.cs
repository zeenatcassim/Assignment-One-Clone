using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Weapon Spawning")]
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] List<WeaponData> weaponData = new List<WeaponData>();
    [SerializeField] List<Transform> weaponSpawnPoints = new List<Transform>();

    [Header("Audio")]
    [SerializeField] AudioSource BackGroundMusic;
    [SerializeField] AudioState AudioState;

    [Header("UI")]
    [SerializeField] ScoringDisplay scoreDisplay;

    [Header("Score")]
    [SerializeField] int overallScore = 0;
    [SerializeField] TMP_Text text;
    [SerializeField] GameData gameData;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < weaponData.Count; i++)
        {
            GameObject newWeapon = Instantiate(weaponPrefab, weaponSpawnPoints[i]);
            newWeapon.GetComponent<WeaponController>().ammo = weaponData[i].maxAmmo;
            newWeapon.GetComponent<WeaponController>().maxRounds = weaponData[i].rounds;
            newWeapon.GetComponent<WeaponController>().roundDelay = weaponData[i].rounds;
            newWeapon.GetComponent<WeaponController>().type = weaponData[i].type;
            newWeapon.GetComponent<SpriteRenderer>().sprite = weaponData[i].sprite;
        }

        if (AudioState.state)
            BackGroundMusic.Play();

        ResetGameData(0);
    }

    public void ScoreController(int score)
    {
        scoreDisplay.showScore(score);

        overallScore += score;
        gameData.Score += overallScore;
    }

    public void ResetGameData(int data)
    {
        gameData.Score = data;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = gameData.Score.ToString() + "pts";
    }
}
