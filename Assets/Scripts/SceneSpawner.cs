using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartition
{
    public class SceneSpawner : MonoBehaviour
    {
        [Range(1, 10000)]
        public int numElements = 100;
        public GameObject prefab;
        [Range(10, 500)]
        public float spawnRange = 100;
    }
}
