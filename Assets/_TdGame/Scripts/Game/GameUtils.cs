using Leopotam.EcsLite;
using System;
using UnityEngine;

namespace TdGame
{
    static class GameUtils
    {
        public static ref T GetOrAdd<T>(this EcsPool<T> componentPool, int entity) where T : struct
        {
            if (componentPool.Has(entity))
                return ref componentPool.Get(entity);
            else
                return ref componentPool.Add(entity);
        }

        public static Vector3 PositionToVector3(int lineId, float x, float z)
        {
            return new Vector3(x, 0, z) + new Vector3((lineId - (MagicNumbersGame.lineCount - 1)/2.0f) * MagicNumbersGame.cellSize, 0, 0);
        }

        public static float RowToZ(int rowId)
        {
            var rowWidth = MagicNumbersGame.cellSize;
            return (rowId + 0.5f) * rowWidth;
        }

        public static Vector3 CellToVector3(float line, float row)
        {
            return new Vector3((line - (MagicNumbersGame.lineCount - 1) / 2.0f) * MagicNumbersGame.cellSize, 0, (row + 0.5f) * MagicNumbersGame.cellSize);
        }
    }
}
