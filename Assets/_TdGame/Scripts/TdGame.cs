using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TdGame
{
    static class MagicNumbersGame
    {
        public static string creaturePrefabPath = "Creature";
        public static float creatureVelocityZ = -1f;
        public static float creatureSpawnerXMin = -1f;
        public static float creatureSpawnerXMax = 1f;
        public static float creatureSpawnerZ = 18f;
        public static float creatureSpawnerCooldownMin = 1f;
        public static float creatureSpawnerCooldownMax = 2f;

        public static float lineWidth = 3f;

        public static GameObject creaturePrefab = Resources.Load<GameObject>(creaturePrefabPath);
    }

    struct Health
    {
        public int health;
        public int healthMax;
    }

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

    struct View
    {
        public GameObject viewObject;
    }

    struct CreatureSpawner
    {
        public float cooldown;

        public float creatureVelocityZ;
    }

    struct DestroyMarker
    {
    }

    sealed class CreatureSpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<CreatureSpawner> creatureSpawnerPool;
        EcsPool<Position> positionPool;
        EcsPool<Motion> motionPool;
        EcsPool<View> viewPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            creatureSpawnerPool = world.GetPool<CreatureSpawner>();
            positionPool = world.GetPool<Position>();
            motionPool = world.GetPool<Motion>();
            viewPool = world.GetPool<View>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<CreatureSpawner>().Inc<Position>().End();

            foreach (int entity in filter)
            {
                ref var creatureSpawner = ref creatureSpawnerPool.Get(entity);
                ref var position = ref positionPool.Get(entity);

                if (creatureSpawner.cooldown > 0)
                    creatureSpawner.cooldown = Mathf.Max(0, creatureSpawner.cooldown - Time.deltaTime);

                if (creatureSpawner.cooldown <= 0)
                {
                    SpawnCreature(position.lineId, position.z);
                    creatureSpawner.cooldown = Random.Range(MagicNumbersGame.creatureSpawnerCooldownMin, MagicNumbersGame.creatureSpawnerCooldownMax);
                }
            }
        }

        void SpawnCreature(int lineId, float z)
        {
            int entity = world.NewEntity();

            ref var position = ref positionPool.Add(entity);
            position.lineId = lineId;
            position.x = Random.Range(MagicNumbersGame.creatureSpawnerXMin, MagicNumbersGame.creatureSpawnerXMax);
            position.z = z;

            ref var motion = ref motionPool.Add(entity);
            motion.velocityZ = MagicNumbersGame.creatureVelocityZ;

            var viewObject = GameObject.Instantiate(MagicNumbersGame.creaturePrefab,
                Utils.PositionToVector3(position.lineId, position.x, position.z),
                Quaternion.identity);
            ref var view = ref viewPool.Add(entity);
            view.viewObject = viewObject;

            Debug.Log($"Entity {entity} spawned");
        }
    }

    sealed class CreatureMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsPool<Motion> motionPool;
        EcsPool<Position> positionPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            motionPool = world.GetPool<Motion>();
            positionPool = world.GetPool<Position>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<Motion>().Inc<Position>().End();

            foreach (int entity in filter)
            {
                ref var motion = ref motionPool.Get(entity);
                ref var position = ref positionPool.Get(entity);
                position.z += motion.velocityZ * Time.deltaTime;
            }
        }
    }

    sealed class ViewPositionUpdateSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var viewPool = world.GetPool<View>();
            var positionPool = world.GetPool<Position>();

            var filter = world.Filter<View>().Inc<Position>().End();

            foreach (int entity in filter)
            {
                ref var view = ref viewPool.Get(entity);
                ref var position = ref positionPool.Get(entity);
                view.viewObject.transform.position = Utils.PositionToVector3(position.lineId, position.x, position.z);
            }
        }
    }

    sealed class CreatureArriveSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var positionPool = world.GetPool<Position>();
            var destroyMarkerPool = world.GetPool<DestroyMarker>();

            var filter = world.Filter<Position>().End();

            foreach (int entity in filter)
            {
                ref var position = ref positionPool.Get(entity);
                if (position.z <= 0)
                {
                    destroyMarkerPool.Add(entity);
                    Debug.Log($"Entity {entity} is marked to destroy");
                }
            }
        }
    }

    sealed class DestroyViewSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var viewPool = world.GetPool<View>();

            var filter = world.Filter<DestroyMarker>().Inc<View>().End();

            foreach (int entity in filter)
            {
                ref var view = ref viewPool.Get(entity);
                GameObject.Destroy(view.viewObject);
                view.viewObject = null;
                viewPool.Del(entity);
                Debug.Log($"Entity {entity} view destroyed");
            }
        }
    }

    sealed class DestroyEntitySystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var filter = world.Filter<DestroyMarker>().End();

            foreach (int entity in filter)
            {
                world.DelEntity(entity);
                Debug.Log($"Entity {entity} destroyed");
            }
        }
    }

    static class Utils
    {
        public static Vector3 PositionToVector3(int lineId, float x, float z)
        {
            return new Vector3(x, 0, z) + new Vector3((lineId - 1) * MagicNumbersGame.lineWidth, 0, 0);
        }
    }
}
