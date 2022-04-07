using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SpatialPartition.EditorFolder
{
    [CustomEditor(typeof(SceneSpawner))]
    public class SceneSpawnerEditor : Editor
    {
        private SceneSpawner _spawner;

        void OnEnable()
        {
            _spawner = target as SceneSpawner;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate"))
            {
                for (int i = 0; i < _spawner.numElements; i++)
                {
                    var go = Instantiate(_spawner.prefab);
                    var pos = Random.insideUnitCircle * _spawner.spawnRange;
                    go.transform.position = new Vector3(pos.x, 0, pos.y);

                    go.name = "Object " + pos; 
                }
            }
        }
    }
}