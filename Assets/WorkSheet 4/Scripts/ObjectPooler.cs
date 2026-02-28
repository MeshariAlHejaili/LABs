using System.Collections.Generic;
using UnityEngine;
namespace Worksheet4
{
    public class ObjectPooler : MonoBehaviour
    {
        public static ObjectPooler SharedInstance;

        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private int amountToPool = 5;
        private List<GameObject> pooledObjects;

        void Awake()
        {
            SharedInstance = this;
        }

        void Start()
        {
            pooledObjects = new List<GameObject>();
            for (int i = 0; i < amountToPool; i++)
            {
                GameObject obj = Instantiate(bulletPrefab);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }

        public GameObject GetPooledObject()
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                // Return the first available (inactive) bullet in the pool
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }
            return null; // Return null if all bullets are currently active
        }
    }
}