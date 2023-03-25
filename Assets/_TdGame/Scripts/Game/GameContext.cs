using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TdGame
{
    public class GameContext : IDisposable
    {
        public GameObjectBuilder objectBuilder;

        public List<List<int>> creaturesByLine;

        public GameContext(EcsWorld world)
        {
            objectBuilder = new GameObjectBuilder(world);

            creaturesByLine = new List<List<int>>();
            for (var i = 0; i < MagicNumbersGame.lineCount; i++)
                creaturesByLine.Add(new List<int>());
        }

        public void Dispose()
        {
            objectBuilder.Dispose();
            objectBuilder = null;

            for (var i = 0; i < creaturesByLine.Count; i++)
                creaturesByLine[i].Clear();
            creaturesByLine.Clear();
            creaturesByLine = null;
        }
    }

    public class GameObjectBuilder : IDisposable
    {
        EcsWorld world;

        EcsPool<CreatureSpawner> creatureSpawnerPool;
        EcsPool<Creature> creaturePool;
        EcsPool<Turret> turretPool;
        EcsPool<Bullet> bulletPool;
        EcsPool<Position> positionPool;
        EcsPool<Motion> motionPool;
        EcsPool<Health> healthPool;
        EcsPool<View> viewPool;

        public GameObjectBuilder(EcsWorld aWorld)
        {
            world = aWorld;

            creatureSpawnerPool = world.GetPool<CreatureSpawner>();
            creaturePool = world.GetPool<Creature>();
            turretPool = world.GetPool<Turret>();
            bulletPool = world.GetPool<Bullet>();
            positionPool = world.GetPool<Position>();
            motionPool = world.GetPool<Motion>();
            healthPool = world.GetPool<Health>();
            viewPool = world.GetPool<View>();
        }

        public void CreateInitialEntities()
        {
            // creature spawners
            for (var i = 0; i < MagicNumbersGame.lineCount; i++)
            {
                var entity = world.NewEntity();

                ref var creatureSpawner = ref creatureSpawnerPool.Add(entity);
                creatureSpawner.cooldown = Random.Range(MagicNumbersGame.creatureSpawnerCooldownMin, MagicNumbersGame.creatureSpawnerCooldownMax);
                creatureSpawner.creatureVelocityZ = MagicNumbersGame.creatureVelocityZ;

                ref var position = ref positionPool.Add(entity);
                position.lineId = i;
                position.x = 0;
                position.z = MagicNumbersGame.creatureSpawnerZ;
            }

            // player turrets
            for (var i = 0; i < MagicNumbersGame.lineCount; i++)
            {
                var entity = world.NewEntity();

                ref var turret = ref turretPool.Add(entity);

                ref var position = ref positionPool.Add(entity);
                position.lineId = i;
                position.x = 0;
                position.z = MagicNumbersGame.turretZ;

                var viewObject = GameObject.Instantiate(MagicNumbersGame.turretPrefab,
                    GameUtils.PositionToVector3(position.lineId, position.x, position.z),
                    Quaternion.identity);
                ref var view = ref viewPool.Add(entity);
                view.viewObject = viewObject;
            }
        }

        public void SpawnCreature(int lineId, float z)
        {
            int entity = world.NewEntity();

            ref var creature = ref creaturePool.Add(entity);

            ref var position = ref positionPool.Add(entity);
            position.lineId = lineId;
            position.x = Random.Range(MagicNumbersGame.creatureSpawnerXMin, MagicNumbersGame.creatureSpawnerXMax);
            position.z = z;

            ref var motion = ref motionPool.Add(entity);
            motion.velocityZ = MagicNumbersGame.creatureVelocityZ;

            ref var health = ref healthPool.Add(entity);
            health.health = MagicNumbersGame.creatureHealth;

            var viewObject = GameObject.Instantiate(MagicNumbersGame.creaturePrefab,
                GameUtils.PositionToVector3(position.lineId, position.x, position.z),
                Quaternion.identity);
            ref var view = ref viewPool.Add(entity);
            view.viewObject = viewObject;
        }

        public void SpawnBullet(int lineId, float z)
        {
            int entity = world.NewEntity();

            ref var bullet = ref bulletPool.Add(entity);

            ref var position = ref positionPool.Add(entity);
            position.lineId = lineId;
            position.x = 0;
            position.z = z;

            ref var motion = ref motionPool.Add(entity);
            motion.velocityZ = MagicNumbersGame.bulletVelocityZ;

            var viewObject = GameObject.Instantiate(MagicNumbersGame.bulletPrefab,
                GameUtils.PositionToVector3(position.lineId, position.x, position.z),
                Quaternion.identity);
            ref var view = ref viewPool.Add(entity);
            view.viewObject = viewObject;

            Debug.Log($"Create Bullet at line {lineId} {position.x}:{position.z}");
        }

        public void Dispose()
        {
            world = null;

            creatureSpawnerPool = null;
            creaturePool = null;
            turretPool = null;
            bulletPool = null;
            positionPool = null;
            motionPool = null;
            healthPool = null;
            viewPool = null;
        }
    }
}
