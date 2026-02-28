using UnityEngine;
using System.Collections;

namespace Worksheet5And6
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("Controls how fast the player sphere rolls.")]
        [Range(1f, 20f)]
        [SerializeField] private float speed = 5.0f;

        [Tooltip("Multiplies the default Unity gravity for a heavier, less floaty feel.")]
        [SerializeField] private float gravityModifier = 1.0f;

        [HideInInspector] 
        public bool hasPowerup = false; 

        [Header("Powerup Settings")]
        [Tooltip("The impulse force applied to enemies when colliding with a powerup.")]
        [Range(5f, 30f)] 
        [SerializeField] private float powerupStrength = 15.0f;

        [Header("References")]
        [SerializeField] private GameObject powerupIndicator; 
        [SerializeField] private ParticleSystem powerupParticle; 
        [SerializeField] private AudioClip collisionSound; 

        // Cached component references to avoid expensive GetComponent calls during Update()
        private Rigidbody playerRb;
        private GameObject focalPoint;
        private Animator indicatorAnim;
        private AudioSource playerAudio;

        void Start()
        {
            playerRb = GetComponent<Rigidbody>();
            
            // The focal point dictates the 'forward' direction relative to the camera's rotation
            focalPoint = GameObject.Find("Focal Point");

            Physics.gravity *= gravityModifier;

            indicatorAnim = powerupIndicator.GetComponent<Animator>();
            playerAudio = GetComponent<AudioSource>();
        }

        void Update()
        {
            // Gate movement and physics so the player cannot control the sphere during menus or game over states
            if (GameManager.Instance.isGameActive)
            {
                float forwardInput = Input.GetAxis("Vertical");
                playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);

                // Lose condition: Player falls off the arena floor
                if (transform.position.y < -10)
                {
                    GameManager.TriggerGameOver();
                    Destroy(gameObject); 
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Powerup"))
            {
                hasPowerup = true;
                powerupIndicator.SetActive(true);
                powerupParticle.Play(); 
                indicatorAnim.SetBool("isSpinning", true);

                Destroy(other.gameObject);
                StartCoroutine(PowerupCountdownRoutine());
            }
        }

        /// <summary>
        /// Handles the expiration of the powerup state after a set duration.
        /// </summary>
        private IEnumerator PowerupCountdownRoutine()
        {
            yield return new WaitForSeconds(7); 
            
            hasPowerup = false;
            indicatorAnim.SetBool("isSpinning", false);
            powerupIndicator.SetActive(false);
        }

        private void OnCollisionEnter(Collision collision)
        {
            playerAudio.PlayOneShot(collisionSound, 1.0f);

            // Apply knockback only if the player currently holds a powerup
            if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
            {
                Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
                
                // Calculate the pushback vector originating from the player's center
                Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
                
                // Use ForceMode.Impulse for an immediate, burst-like physical reaction
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
        }
    }
}