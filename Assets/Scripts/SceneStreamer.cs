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
        private List<Vector2Int> _scenePosition = new();
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
                
                var toLoad = newPosition.Except(_scenePosition);
                var toRemove = _scenePosition.Except(newPosition);

                // Carico le scene
                foreach (var ptl in toLoad)
                {
                    var handle = Addressables.LoadSceneAsync("Scene " + ptl, LoadSceneMode.Additive);
                    yield return handle;
                    if(handle.Status == AsyncOperationStatus.Failed) continue;
                    _loadedScenes.Add(ptl, handle.Result);
                }

                // Rimuovo le scene
                foreach (var ptr in toRemove)
                {
                    if(!_loadedScenes.TryGetValue(ptr, out var scene)) continue;
                    _loadedScenes.Remove(ptr);
                    Addressables.UnloadSceneAsync(scene);
                }


                _scenePosition = newPosition;
            }
        }
    }
}