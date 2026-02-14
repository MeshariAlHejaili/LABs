using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> asteroidPrefabs = new List<GameObject>();

    private float spawnInterval = 2.0f;
    private float spawnRange = 4.5f;
    private float difficultyTimer = 0f;

    void Start()
    {
        StartCoroutine(SpawnAsteroidRoutine());
    }

    void Update()
    {
        // Gradually increase difficulty by speeding up spawn rates
        difficultyTimer += Time.deltaTime;
        if (difficultyTimer > 30f && spawnInterval > 0.5f)
        {
            spawnInterval -= 0.5f;
            difficultyTimer = 0f;
        }
    }

    IEnumerator SpawnAsteroidRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            int index = Random.Range(0, asteroidPrefabs.Count);

            // Dynamically match the Y position (height) of the selected prefab
            float prefabHeight = asteroidPrefabs[index].transform.position.y;
            Vector3 spawnPos = new Vector3(Random.Range(-spawnRange, spawnRange), prefabHeight, spawnRange + 1);

            GameObject asteroid = Instantiate(asteroidPrefabs[index], spawnPos, asteroidPrefabs[index].transform.rotation);

            // Ensure the asteroid remains level while looking toward the play area
            asteroid.transform.LookAt(new Vector3(spawnPos.x, prefabHeight, 0));
        }
    }
}