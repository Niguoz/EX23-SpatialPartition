using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace SpatialPartition
{
    public class SceneStreamer : MonoBehaviour
    {
        public Transform player;
        public float refreshTime = .5f;
        public float checkDistance = 30;

        private Grid<GameObject> _grid = new();
        private List<Vector2> _scenePosition = new();
        private Dictionary<Vector2Int, SceneInstance> _loadedScenes = new();

        void Start()
        {
            StartCoroutine(nameof(UpdateScene));
        }

        IEnumerator UpdateScene()
        {
            while(true)
            {
                yield return new WaitForSeconds(refreshTime);

                // Logica check griglia e caricamento scene
                var pos = Grid<GameObject>.ToGridPosition(player.position, checkDistance / 3);
                var newPosition = _grid.Offsets.Select(offset => pos + offset).ToList();
                
            }
        }
    }
}