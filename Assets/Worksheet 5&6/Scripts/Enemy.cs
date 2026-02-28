using UnityEngine;

namespace Worksheet5And6
{

    public class Enemy : MonoBehaviour
    {
        [Header("Enemy Settings")]
        [Tooltip("Controls how fast the enemy accelerates toward the player.")]
        [Range(1f, 10f)]
        [SerializeField] private float speed = 3.0f;

        [Tooltip("Points awarded to the player when this enemy is defeated.")]
        [SerializeField] private int pointValue = 5;

        // Cached references 
        private Rigidbody enemyRb;
        private GameObject player;

        void Start()
        {
            enemyRb = GetComponent<Rigidbody>();
            player = GameObject.Find("Player");

            // Scale the enemy's speed based on the selected difficulty
            speed *= GameManager.Instance.difficultyMultiplier;
        }

        void Update()
        {
            // Gate movement: Only allow the enemy to chase if the game is actively playing 
            if (GameManager.Instance.isGameActive && player != null)
            {
                // Calculate the normalized directional vector pointing from the enemy to the player
                Vector3 lookDirection = (player.transform.position - transform.position).normalized;

                // Apply continuous force to chase the player
                enemyRb.AddForce(lookDirection * speed);
            }

            // Defeat condition: Enemy is pushed off the edge of the arena
            if (transform.position.y < -10)
            {
                // Award points to the player via the GameManager before destroying the object 
                if (GameManager.Instance.isGameActive)
                {
                    GameManager.Instance.UpdateScore(pointValue);
                }

                Destroy(gameObject);
            }
        }
    }
}