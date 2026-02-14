using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    private float speed = 5f;
    private float bound = 7f;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Destroy asteroid if it drifts too far off-screen
        if (Mathf.Abs(transform.position.x) > bound || Mathf.Abs(transform.position.z) > bound)
        {
            Destroy(gameObject); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Handle Bullet Collision
        if (other.CompareTag("Bullet"))
        {
            other.gameObject.SetActive(false); // Return bullet to object pool
            Destroy(gameObject); // Destroy this asteroid
        }
        // Handle Player Collision
        else if (other.CompareTag("Player"))
        {
            Debug.Log("Game Over!");
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}