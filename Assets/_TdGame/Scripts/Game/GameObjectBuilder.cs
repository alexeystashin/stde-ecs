using Leopotam.EcsLite;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TdGame
{
    public class GameObjectBuilder : IDisposable
    {
        GameContext context;
        EcsWorld world;

        EcsPool<Spawner> spawnerPool;
        EcsPool<Creature> creaturePool;
        EcsPool<Turret> turretPool;
        EcsPool<Area> areaPool;
        EcsPool<HitArea> hitAreaPool;
        EcsPool<AreaTrigger> areaTriggerPool;
        EcsPool<Bolt> boltPool;
        EcsPool<HitBolt> hitBoltPool;
        EcsPool<AreaBolt> areaBoltPool;
        EcsPool<Position> positionPool;
        EcsPool<Motion> motionPool;
        EcsPool<Health> healthPool;
        EcsPool<Lifetime> lifetimePool;
        EcsPool<Cooldown> cooldownPool;
        EcsPool<View> viewPool;

        public GameObjectBuilder(GameContext aContext, EcsWorld aWorld)
        {
            context = aContext;
            world = aWorld;

            spawnerPool = world.GetPool<Spawner>();
            creaturePool = world.GetPool<Creature>();
            turretPool = world.GetPool<Turret>();
            areaPool = world.GetPool<Area>();
            hitAreaPool = world.GetPool<HitArea>();
            areaTriggerPool = world.GetPool<AreaTrigger>();
            boltPool = world.GetPool<Bolt>();
            hitBoltPool = world.GetPool<HitBolt>();
            areaBoltPool = world.GetPool<AreaBolt>();
            positionPool = world.GetPool<Position>();
            motionPool = world.GetPool<Motion>();
            healthPool = world.GetPool<Health>();
            lifetimePool = world.GetPool<Lifetime>();
            cooldownPool = world.GetPool<Cooldown>();
            viewPool = world.GetPool<View>();
        }

        public void CreateInitialEntities()
        {
            // creature spawners
            Debug.Log($"Wave {context.currentWave + 1}/{context.gameRules.waves.Count} started");
            CreateWaveSpawners(context.gameRules.waves[context.currentWave]);

            // player turrets
            CreateTurret(context.staticGameData.towers[MockStaticGameData.TowerTemplateId.MachineGun.ToString()], 0, 0);
            CreateTurret(context.staticGameData.towers[MockStaticGameData.TowerTemplateId.Rocket.ToString()], 1, 0);
            CreateTurret(context.staticGameData.towers[MockStaticGameData.TowerTemplateId.MachineGun.ToString()], 2, 0);
        }

        public void CreateWaveSpawners(WaveTemplate template)
        {
            for (var l = 0; l < MagicNumbersGame.lineCount; l++)
            {
                var lineSpawners = template.lineSpawners[l];
                foreach (var spawnerTemplate in lineSpawners)
                {
                    CreateSpawner(spawnerTemplate, l);
                }
            }
        }

        public void CreateSpawner(SpawnerTemplate template, int lineId)
        {
            var entity = world.NewEntity();

            ref var spawner = ref spawnerPool.Add(entity);
            spawner.template = template;

            ref var lifetime = ref lifetimePool.Add(entity);
            lifetime.lifetime = template.lifetime + template.delay;

            ref var cooldown = ref cooldownPool.Add(entity);
            cooldown.cooldown = template.delay;

            ref var position = ref positionPool.Add(entity);
            position.lineId = lineId;
            position.x = 0;
            position.z = MagicNumbersGame.spawnerZ;
        }

        public void CreateTurret(TowerTemplate template, int lineId, int rowId)
        {
            var entity = world.NewEntity();

            ref var turret = ref turretPool.Add(entity);
            turret.template = template;
            turret.lineId = lineId;
            turret.rowId = rowId;

            ref var cooldown = ref cooldownPool.Add(entity);
            cooldown.cooldown = 0;

            ref var position = ref positionPool.Add(entity);
            position.lineId = lineId;
            position.x = 0;
            position.z = GameUtils.RowToZ(rowId);

            var viewObject = GameObject.Instantiate(PrefabCache.instance.GetPrefab(template.prefabPath),
                GameUtils.PositionToVector3(position.lineId, position.x, position.z),
                Quaternion.identity);
            var entityView = viewObject.AddComponent<EntityView>();
            entityView.entityId = world.PackEntity(entity);

            ref var view = ref viewPool.Add(entity);
            view.viewObject = viewObject;
        }

        public void CreateCreature(CreatureTemplate template, int lineId, float z)
        {
            int entity = world.NewEntity();

            ref var creature = ref creaturePool.Add(entity);

            ref var position = ref positionPool.Add(entity);
            position.lineId = lineId;
            position.x = Random.Range(MagicNumbersGame.creatureSpawnXMin, MagicNumbersGame.creatureSpawnXMax);
            position.z = z;

            ref var motion = ref motionPool.Add(entity);
            motion.velocityZ = template.moveSpeed * -1f;

            ref var health = ref healthPool.Add(entity);
            health.health = template.health;

            var viewObject = GameObject.Instantiate(PrefabCache.instance.GetPrefab(template.prefabPath),
                GameUtils.PositionToVector3(position.lineId, position.x, position.z),
                Quaternion.Euler(0, 180, 0));
            ref var view = ref viewPool.Add(entity);
            view.viewObject = viewObject;
        }

        public void CreateBolt(BoltTemplate template, int lineId, float z)
        {
            int entity = world.NewEntity();

            ref var bolt = ref boltPool.Add(entity);

            if (template.attackPower > 0)
            {
                ref var hitBolt = ref hitBoltPool.Add(entity);
                hitBolt.hitPower = template.attackPower;
            }

            if (!string.IsNullOrEmpty(template.attackAreaId))
            {
                ref var areaBolt = ref areaBoltPool.Add(entity);
                areaBolt.areaTemplateId = template.attackAreaId;
            }

            ref var position = ref positionPool.Add(entity);
            position.lineId = lineId;
            position.x = 0;
            position.z = z;

            ref var motion = ref motionPool.Add(entity);
            motion.velocityZ = template.moveSpeed;

            var viewObject = GameObject.Instantiate(PrefabCache.instance.GetPrefab(template.prefabPath),
                GameUtils.PositionToVector3(position.lineId, position.x, position.z),
                Quaternion.identity);
            ref var view = ref viewPool.Add(entity);
            view.viewObject = viewObject;
        }

        public void CreateArea(AreaTemplate template, int lineId, float z)
        {
            int entity = world.NewEntity();

            ref var area = ref areaPool.Add(entity);

            if (template.attackPower > 0)
            {
                ref var hitArea = ref hitAreaPool.Add(entity);
                hitArea.hitPower = template.attackPower;
                hitArea.size = template.size;

                // todo: add by colldown via separate system
                ref var areaTrigger = ref areaTriggerPool.Add(entity);
            }

            ref var position = ref positionPool.Add(entity);
            position.lineId = lineId;
            position.x = 0;
            position.z = z;

            ref var lifetime = ref lifetimePool.Add(entity);
            lifetime.lifetime = template.lifetime;

            var viewObject = GameObject.Instantiate(PrefabCache.instance.GetPrefab(template.prefabPath),
                GameUtils.PositionToVector3(position.lineId, position.x, position.z),
                Quaternion.identity);
            ref var view = ref viewPool.Add(entity);
            view.viewObject = viewObject;
        }

        public void Dispose()
        {
            context = null;
            world = null;
            //todo: clear pool references
        }
    }
}
