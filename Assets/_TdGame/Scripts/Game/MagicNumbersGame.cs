using UnityEngine;

namespace TdGame
{
    static class MagicNumbersGame
    {
        public static float creatureVelocityZ = -1f;
        public static float creatureSpawnerXMin = -0.5f;
        public static float creatureSpawnerXMax = 0.5f;
        public static float creatureSpawnerZ = 24f;
        public static float creatureArriveZ = 0f;
        public static float creatureSpawnerCooldownMin = 1f;
        public static float creatureSpawnerCooldownMax = 2f;
        public static float creatureHealth = 100f;
        public static float turretZ = 1.5f;
        public static float turretFireCooldown = 0.5f;
        public static float bulletVelocityZ = 10f;
        public static float bulletArriveZ = 30f;
        public static float bulletHitPower = 10f;

        public static int lineCount = 3;
        public static float lineWidth = 3f;

        public static string creaturePrefabPath = "Creature";
        public static string turretPrefabPath = "MachineGun";
        public static string bulletPrefabPath = "Bullet";

        public static GameObject creaturePrefab = Resources.Load<GameObject>(creaturePrefabPath);
        public static GameObject turretPrefab = Resources.Load<GameObject>(turretPrefabPath);
        public static GameObject bulletPrefab = Resources.Load<GameObject>(bulletPrefabPath);
    }
}
