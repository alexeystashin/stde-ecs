using Leopotam.EcsLite;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TdGame
{
    public class GameObjectBuilder : IDisposable
    {
        EcsWorld world;

        EcsPool<Spawner> spawnerPool;
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

            spawnerPool = world.GetPool<Spawner>();
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

                ref var spawner = ref spawnerPool.Add(entity);
                spawner.cooldown = Random.Range(MagicNumbersGame.spawnerCooldownMin, MagicNumbersGame.spawnerCooldownMax);
                spawner.creatureVelocityZ = MagicNumbersGame.creatureVelocityZ;

                ref var position = ref positionPool.Add(entity);
                position.lineId = i;
                position.x = 0;
                position.z = MagicNumbersGame.spawnerZ;
            }

            // player turrets
            for (var i = 0; i < MagicNumbersGame.lineCount - 1; i++)
            {
                var entity = world.NewEntity();

                ref var turret = ref turretPool.Add(entity);
                turret.rowId = 0;
                turret.lineId = i;

                ref var position = ref positionPool.Add(entity);
                position.lineId = i;
                position.x = 0;
                position.z = MagicNumbersGame.turretZ;

                var viewObject = GameObject.Instantiate(MagicNumbersGame.turretPrefab,
                    GameUtils.PositionToVector3(position.lineId, position.x, position.z),
                    Quaternion.identity);
                var entityView = viewObject.AddComponent<EntityView>();
                entityView.entityId = entity;

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
            position.x = Random.Range(MagicNumbersGame.creatureSpawnXMin, MagicNumbersGame.creatureSpawnXMax);
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
        }

        public void Dispose()
        {
        }
    }
}
