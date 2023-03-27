using UnityEngine;

namespace TdGame
{
    static class MagicNumbersGame
    {
        public const int lineCount = 3;
        public const float cellSize = 3f;

        public const int easyWavesCount = 5;
        public const int hardWavesCount = 10;

        public const float creatureVelocityZ = -1f;
        public const float creatureArriveZ = 1.5f;
        public const float creatureSpawnXMin = -0.5f;
        public const float creatureSpawnXMax = 0.5f;

        public const float spawnerZ = 24f;

        public const float turretZ = 1.5f;
        public const float turretMoveTime = 0.25f;

        public const float bulletArriveZ = 30f;

        public const float dragThresold = 0.05f;
    }
}
