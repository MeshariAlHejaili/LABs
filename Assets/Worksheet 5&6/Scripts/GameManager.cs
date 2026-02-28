using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

namespace Worksheet5And6
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; } // Singleton pattern for easy access

        public static event Action OnGameOver;

        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private GameObject titleScreen;
        [SerializeField] private GameObject gameOverScreen;
        [SerializeField] private CanvasGroup gameOverCanvasGroup;

        [Header("Game Settings")]
        [Tooltip("Controls the base delay between enemy spawns")]
        [Range(1f, 5f)]
        [SerializeField] private float spawnRate = 2f;

        [HideInInspector]
        public bool isGameActive;

        [HideInInspector]
        public float difficultyMultiplier = 1f;

        private int score;
        private int currentWave = 1;

        void Awake()
        {
            // Set up the Singleton instance so other scripts can easily call GameManager.Instance
            if (Instance == null) Instance = this;
        }

        void OnEnable()
        {
            // Subscribe the local UI method to your static Game Over event
            OnGameOver += HandleGameOverUI;
        }

        void OnDisable()
        {
            // Unsubscribe to prevent memory leaks
            OnGameOver -= HandleGameOverUI;
        }

        public static void TriggerGameOver()
        {
            if (OnGameOver != null)
            {
                OnGameOver();
                Debug.Log("Game Over!");
            }
        }

        // Called by the DifficultyButton script
        public void StartGame(int difficulty)
        {
            isGameActive = true;
            score = 0;
            difficultyMultiplier = difficulty;

            titleScreen.gameObject.SetActive(false);
            UpdateScore(0);
            UpdateWave(1);

            // Starts the spawner (Assumes SpawnManager is in the scene)
            FindAnyObjectByType<SpawnManager>().StartSpawning(spawnRate);
        }

        public void UpdateScore(int scoreToAdd)
        {
            if (!isGameActive) return;

            score += scoreToAdd;
            scoreText.text = "Score: " + score;

            // Play advanced UI animation
            StartCoroutine(PunchScoreText());
        }

        public void UpdateWave(int wave)
        {
            currentWave = wave;
            waveText.text = "Wave: " + currentWave;
        }

        // This triggers when your existing OnGameOver event is fired
        private void HandleGameOverUI()
        {
            isGameActive = false;
            gameOverScreen.gameObject.SetActive(true);
            StartCoroutine(FadeIn(gameOverCanvasGroup, 0.5f));
        }

        // Called by the Restart Button
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // --- Advanced Extension: Coroutine UI Animations ---

        private IEnumerator FadeIn(CanvasGroup cg, float dur)
        {
            float t = 0;
            while (t < dur)
            {
                t += Time.deltaTime;
                cg.alpha = t / dur;
                yield return null; // Wait one frame
            }
        }

        private IEnumerator PunchScoreText()
        {
            // Instantly scale up the text, then smoothly scale it back down
            scoreText.transform.localScale = Vector3.one * 1.5f;
            float t = 0;
            while (t < 0.2f)
            {
                t += Time.deltaTime;
                scoreText.transform.localScale = Vector3.Lerp(Vector3.one * 1.5f, Vector3.one, t / 0.2f);
                yield return null;
            }
            scoreText.transform.localScale = Vector3.one;
        }
    }
}