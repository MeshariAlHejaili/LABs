using UnityEngine;

namespace Worksheet5And6
{
    public class SpawnManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [Tooltip("The enemy prefab to spawn each wave.")]
        [SerializeField] private GameObject enemyPrefab;
        
        [Tooltip("The powerup prefab to spawn at the start of a wave.")]
        [SerializeField] private GameObject powerupPrefab;
        
        [Header("Spawn Settings")]
        [Tooltip("The maximum X and Z distance from the center where items can spawn.")]
        [SerializeField] private float spawnRange = 9.0f;

        [HideInInspector] 
        public int enemyCount;
        
        [HideInInspector]
        public int waveNumber = 1;

        void Update()
        {
            // Gate spawning logic: only check and spawn if the game is active 
            if (!GameManager.Instance.isGameActive)
            {
                return;
            }

            // Detect when all enemies are defeated by counting objects with the "Enemy" tag
            enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

            // If the arena is clear, progress to the next wave
            if (enemyCount == 0)
            {
                waveNumber++; 
                
                // Update the UI via the GameManager 
                GameManager.Instance.UpdateWave(waveNumber); 
                
                SpawnEnemyWave(waveNumber); 

                // Provide a new powerup for the new wave
                Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
            }
        }

        public void StartSpawning(float spawnRateDelay)
        {
            // Reset wave number for a fresh game start
            waveNumber = 1; 
            GameManager.Instance.UpdateWave(waveNumber); 
            
            SpawnEnemyWave(waveNumber);
            Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
        }

        private void SpawnEnemyWave(int enemiesToSpawn)
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
            }
        }
   
        private Vector3 GenerateSpawnPosition()
        {
            float spawnPosX = Random.Range(-spawnRange, spawnRange);
            float spawnPosZ = Random.Range(-spawnRange, spawnRange);

            return new Vector3(spawnPosX, 1, spawnPosZ);
        }
    }
}