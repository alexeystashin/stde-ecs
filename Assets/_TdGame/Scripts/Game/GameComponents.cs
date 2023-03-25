using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    struct Position
    {
        public int lineId;

        // relative to line
        public float x;
        public float z;
    }

    struct Motion
    {
        public float velocityZ;
    }

    struct Health
    {
        public float health;
    }

    struct View
    {
        public GameObject viewObject;
    }

    struct CreatureSpawner
    {
        public float cooldown;

        public float creatureVelocityZ;
    }

    struct Creature
    {
    }

    struct Turret
    {
        public float cooldown;
    }

    struct Bullet
    {
    }

    struct DestroyMarker
    {
    }
}
