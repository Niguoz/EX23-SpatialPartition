using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartition
{
    public class SceneGridGenerator : MonoBehaviour
    {
        public int cellSize = 10;
        public Transform gridContainer;
        public bool cloneElements;
        public List<GameObject> exclusion;
    }
}