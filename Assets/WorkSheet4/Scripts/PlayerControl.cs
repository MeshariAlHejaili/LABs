using System.Runtime.Serialization;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float turnSpeed = 180f;
    private float xBound = 9.0f; 
    private float zBound = 9.0f;

    void Update()
    {
        // 1. Player Movement & Rotation
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);
        transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);

        // 2. Arena Boundary Clamping
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -xBound, xBound);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, -zBound, zBound);
        transform.position = clampedPosition;

        // 3. Object Pooled Shooting
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject(); 
            if (bullet != null) 
            {
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                bullet.SetActive(true);
            }
        }
    }
}