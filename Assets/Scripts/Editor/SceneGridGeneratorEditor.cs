using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

namespace SpatialPartition.EditorFolder
{
    [CustomEditor(typeof(SceneGridGenerator))]
    public class SceneGridGeneratorEditor : Editor
    {
        SceneGridGenerator _generator;
        void OnEnable()
        {
            _generator = target as SceneGridGenerator;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!GUILayout.Button("Generate Grid")) return;

            _generator.gridContainer.position = Vector3.zero;
            _generator.gridContainer.rotation = Quaternion.identity;
            _generator.gridContainer.localScale = Vector3.one;

            var allElements = SceneManager.GetActiveScene().GetRootGameObjects();
            var grid = new Grid<GameObject>();

            foreach (var go in allElements)
            {
                if (_generator.exclusion.Contains(go)) continue;
                var pos = Grid<GameObject>.ToGridPosition(go.transform.position, _generator.cellSize);
                grid.Add(pos, go);
            }

            // creo cartelle che servono
            var sectionFolderPath = Application.dataPath + "/Resources/Sections/";
            var scenesFolderPath = Application.dataPath + "/Addressables/Scenes/";

            if (!Directory.Exists(sectionFolderPath)) Directory.CreateDirectory(sectionFolderPath);
            if (!Directory.Exists(scenesFolderPath)) Directory.CreateDirectory(scenesFolderPath);

            // creo prefab
            foreach (var key in grid.Keys)
            {
                var set = new HashSet<GameObject>();
                grid.AddElementAtPosition(key, set);

                var section = new GameObject("Section " + key);
                section.transform.position = new Vector3(key.x * _generator.cellSize, 0, key.y * _generator.cellSize);
                section.transform.SetParent(_generator.gridContainer);

                foreach (var go in set)
                {
                    if (_generator.cloneElements)
                    {
                        var clone = Instantiate(go, section.transform);
                        clone.transform.localPosition = Vector3.zero;
                    }
                    else
                    {
                        go.transform.SetParent(section.transform);
                    }
                }

                var prefabPath = sectionFolderPath + section.name + ".prefab";
                PrefabUtility.SaveAsPrefabAsset(section, prefabPath);
            }


            // creo scene 

            foreach (var key in grid.Keys)
            {
                var sceneName = "Scene " + key + ".unity";
                var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
                scene.name = sceneName;
                var go = Resources.Load<GameObject>("Sections/Section " + key);
                Instantiate(go);
                

                var scenePath = scenesFolderPath + sceneName;
                EditorSceneManager.SaveScene(scene, scenePath);
                
            }

            // Refresh Database
            AssetDatabase.Refresh();
        }
    }
}