using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartition
{
    public class TestController : MonoBehaviour
    {
        [SerializeField] [Range(1, 1000)] private float _checkDistance = 30;
        [SerializeField] private Transform _player;
        [SerializeField] private GameObject _prefab;

        [SerializeField] private int _numElements = 100;
        [SerializeField] private float _spawnDistance = 50;

        private HashSet<GameObject> _elementSet = new();
        private Grid<GameObject> _grid = new();

        void Start()
        {
            for (int i = 0; i < _numElements; i++)
            {
                GameObject go = Instantiate(_prefab);

                Vector2 position = Random.insideUnitCircle * _spawnDistance;
                go.transform.position = new Vector3(position.x, 0, position.y);

                _elementSet.Add(go);
                go.GetComponent<GameElement>().OnItemDestroy += ItemDestroyed;
            }
        }

        private void ItemDestroyed(GameObject go)
        {
            _elementSet.Remove(go);
        }

        void Update()
        {
            _grid.ClearNonAlloc();

            foreach (var element in _elementSet)
            {
                Vector2Int gridPosition = Grid<GameObject>.ToGridPosition(element.transform.position, _checkDistance / 3);
                _grid.Add(gridPosition, element);
            }

            ///if (Input.GetButtonDown("Fire1"))
            ///{
                UpdateNeighbours();
            //}

            //if (Input.GetButtonDown("Fire2"))
           // {
            //    ShowAll();
            //}
        }

        void ShowAll()
        {
            foreach (GameObject element in _elementSet)
            {
                element.SetActive(true);
            }
        }

        void UpdateNeighbours()
        {
            HashSet<GameObject> set = new();
            Rebuild(_player, set);
            foreach (GameObject element in _elementSet)
            {
                element.SetActive(set.Contains(element));
            }
        }

        public bool Check(Transform pl, Transform el)
        {
            Vector2Int plOnGrid = Grid<GameObject>.ToGridPosition(pl.position, _checkDistance / 3);
            Vector2Int elOnGrid = Grid<GameObject>.ToGridPosition(el.position, _checkDistance / 3);

            return (plOnGrid - elOnGrid).sqrMagnitude <= 2;
        }

        public void Rebuild(Transform pl, HashSet<GameObject> elements)
        {
            Vector2Int positionOnGrid = Grid<GameObject>.ToGridPosition(pl.position, _checkDistance / 3);
            _grid.GetNeighbours(positionOnGrid, elements);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(_player.position, new Vector3(_checkDistance, 2, _checkDistance));
        }
    }
}