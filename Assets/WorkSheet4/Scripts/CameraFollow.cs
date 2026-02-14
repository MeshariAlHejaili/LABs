using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player; 
    private Vector3 offset = new Vector3(0, 10, 3.5f);

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}