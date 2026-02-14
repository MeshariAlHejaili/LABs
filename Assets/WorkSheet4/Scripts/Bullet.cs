using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    private float bound = 12f;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Deactivate instead of destroying to return to the Object Pool
        if (Mathf.Abs(transform.position.x) > bound || Mathf.Abs(transform.position.z) > bound)
        {
            gameObject.SetActive(false);
        }
    }
}