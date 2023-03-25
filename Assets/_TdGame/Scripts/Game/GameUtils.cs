using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    static class GameUtils
    {
        public static Vector3 PositionToVector3(int lineId, float x, float z)
        {
            return new Vector3(x, 0, z) + new Vector3((lineId - 1) * MagicNumbersGame.lineWidth, 0, 0);
        }
    }
}
