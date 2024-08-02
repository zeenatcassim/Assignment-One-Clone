using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSpawn : MonoBehaviour
{
    public GameObject[] bloodPrefabs; // Array to hold different blood splatter prefabs
    public int splatterCount = 5;
    public float spread = 1f;

    public void SpawnBlood(Vector2 position)
    {
        for (int i = 0; i < splatterCount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * spread;
            int randomIndex = Random.Range(0, bloodPrefabs.Length); // Randomly select a blood prefab
            Instantiate(bloodPrefabs[randomIndex], position + randomOffset, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SpawnBlood(transform.position);
        }
    }
}
