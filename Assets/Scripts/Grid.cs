using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartition
{
    public class Grid<T>
    {
        private Vector2Int[] _neighbourOffsets =
        {
            Vector2Int.zero,
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.right,
            Vector2Int.left,
            Vector2Int.up + Vector2Int.right,
            Vector2Int.up + Vector2Int.left,
            Vector2Int.down + Vector2Int.right,
            Vector2Int.down + Vector2Int.left
        };

        private Dictionary<Vector2Int, HashSet<T>> _grid = new();

        ///<summary>
        /// Svuoto gli elementi della griglia senza distruggerli
        ///</summary>
        public void ClearNonAlloc()
        {
            foreach (HashSet<T> val in _grid.Values)
            {
                val.Clear();
            }
        }

        ///<summary>
        /// Prende gli elementi alla posizione data
        ///</summary>
        ///<param name = "position"> Posizione da prendere in considerazione </param>
        ///<param name = "result"> Set dove inserire gli elemnti trovati </param>
        private void AddElementAtPosition(Vector2Int position, HashSet<T> result)
        {

            if (!_grid.TryGetValue(position, out HashSet<T> hashSet)) return;
            result.UnionWith(hashSet);
            /*foreach (T val in hashSet)
            {
                result.Add(val);
            }*/
        }

        ///<summary>
        ///Cerca tutti gli elementi che si trovano nelle nove celle in considerazione
        ///</summary>
        ///<param name = "position"> Posizione da prendere in considerazione </param>
        ///<param name = "result"> Set dove inserire gli elemnti trovati </param>
        public void GetNeighbours(Vector2Int position, HashSet<T> result)
        {
            result.Clear();
            foreach (Vector2Int offset in _neighbourOffsets)
            {
                AddElementAtPosition(position + offset, result);
            }
        }

        ///<summary>
        ///Aggiungo l'elemento alla griglia nella posizione corretta
        ///</summary>
        ///<param name = "position"> Posizione da prendere in considerazione </param>
        ///<param name = "value"> Elemento da aggiungere </param>
        public void Add(Vector2Int position, T value)
        {
            if (!_grid.TryGetValue(position, out HashSet<T> hashSet))
            {
                hashSet = new();
                _grid[position] = hashSet;
            }

            hashSet.Add(value);
        }

        ///<summary>
        ///
        ///</summary>
        public static Vector2Int ToGridPosition(Vector3 position, float cellSize)
        {
            var pos = new Vector2(position.x, position.z) / cellSize;
            return Vector2Int.RoundToInt(pos);
        }
    }
}