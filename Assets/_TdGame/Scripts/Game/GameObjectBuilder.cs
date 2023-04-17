using Common;
using Leopotam.EcsLite;
using Pump.Unity;
using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace TdGame
{
    public class GameObjectBuilder : IDisposable
    {
        EcsWorld world;

        GameState gameState;
        GameRules gameRules;
        GameUi gameUi;
        StaticGameData staticGameData;
        IUnityPrefabProvider prefabProvider;
        EntityView.Pool entityViewPool;

        EcsPool<Spawner> spawnerPool;
        EcsPool<Creature> creaturePool;
        EcsPool<Turret> turretPool;
        EcsPool<Area> areaPool;
        EcsPool<HitArea> hitAreaPool;
        EcsPool<FreezeArea> freezeAreaPool;
        EcsPool<Bolt> boltPool;
        EcsPool<HitBolt> hitBoltPool;
        EcsPool<AreaBolt> areaBoltPool;
        EcsPool<Position> positionPool;
        EcsPool<Motion> motionPool;
        EcsPool<Health> healthPool;
        EcsPool<Lifetime> lifetimePool;
        EcsPool<Cooldown> cooldownPool;
        EcsPool<View> viewPool;
        EcsPool<TurretUi> turretUiPool;

        [Inject]
        void Construct(EcsWorld world, GameState gameState, GameRules gameRules, GameUi gameUi, StaticGameData staticGameData,
            IUnityPrefabProvider prefabProvider, EntityView.Pool entityViewPool)
        {
            this.world = world;
            this.gameState = gameState;
            this.gameRules = gameRules;
            this.gameUi = gameUi;
            this.staticGameData = staticGameData;
            this.prefabProvider = prefabProvider;
            this.entityViewPool = entityViewPool;

            InitPools();
        }

        void InitPools()
        {
            spawnerPool = world.GetPool<Spawner>();
            creaturePool = world.GetPool<Creature>();
            turretPool = world.GetPool<Turret>();
            areaPool = world.GetPool<Area>();
            hitAreaPool = world.GetPool<HitArea>();
            freezeAreaPool = world.GetPool<FreezeArea>();
            boltPool = world.GetPool<Bolt>();
            hitBoltPool = world.GetPool<HitBolt>();
            areaBoltPool = world.GetPool<AreaBolt>();
            positionPool = world.GetPool<Position>();
            motionPool = world.GetPool<Motion>();
            healthPool = world.GetPool<Health>();
            lifetimePool = world.GetPool<Lifetime>();
            cooldownPool = world.GetPool<Cooldown>();
            viewPool = world.GetPool<View>();
            turretUiPool = world.GetPool<TurretUi>();
        }

        public void CreateInitialEntities()
        {
            // creature spawners
            Debug.Log($"Wave {gameState.currentWave + 1}/{gameRules.waves.Count} started");
            CreateWaveSpawners(gameRules.waves[gameState.currentWave]);

            // player turrets
            var lineId = 0;
            var rowId = 0;
            foreach (var turretId in gameRules.playerTowers)
            {
                CreateTurret(staticGameData.towers[turretId], lineId, rowId);
                lineId++;
                if (lineId >= MagicNumbersGame.lineCount)
                {
                    lineId = 0;
                    rowId++;
                }
            }
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

            //var viewObject = entityViewFactory.Create(template.prefabPath);
            var viewObject = entityViewPool.Spawn(template.prefabPath);

            viewObject.transform.position = GameUtils.PositionToVector3(position.lineId, position.x, position.z);
            viewObject.transform.rotation = Quaternion.identity;
            viewObject.entityId = world.PackEntity(entity);

            ref var view = ref viewPool.Add(entity);
            view.viewObject = viewObject;

            var hudObject = GameObject.Instantiate(prefabProvider.GetPrefab(GamePrefabPath.turretHud),
                GameUtils.PositionToVector3(position.lineId, position.x, position.z),
                Quaternion.identity, gameUi.hudContiner);
            var hud = hudObject.GetComponent<TurretHud>();

            ref var turretUi = ref turretUiPool.Add(entity);
            turretUi.hud = hud;
        }

        public void CreateCreature(CreatureTemplate template, int lineId, float z)
        {
            int entity = world.NewEntity();

            ref var creature = ref creaturePool.Add(entity);
            creature.killScore = template.score;

            ref var position = ref positionPool.Add(entity);
            position.lineId = lineId;
            position.x = Random.Range(MagicNumbersGame.creatureSpawnXMin, MagicNumbersGame.creatureSpawnXMax);
            position.z = z;

            ref var motion = ref motionPool.Add(entity);
            motion.velocityZ = template.moveSpeed * -1f;

            ref var health = ref healthPool.Add(entity);
            health.health = template.health;

            //var viewObject = entityViewFactory.Create(template.prefabPath);
            var viewObject = entityViewPool.Spawn(template.prefabPath);

            viewObject.transform.position = GameUtils.PositionToVector3(position.lineId, position.x, position.z);
            viewObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            viewObject.entityId = world.PackEntity(entity);

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

            //var viewObject = entityViewFactory.Create(template.prefabPath);
            var viewObject = entityViewPool.Spawn(template.prefabPath);

            viewObject.transform.position = GameUtils.PositionToVector3(position.lineId, position.x, position.z);
            viewObject.transform.rotation = Quaternion.identity;
            viewObject.entityId = world.PackEntity(entity);

            ref var view = ref viewPool.Add(entity);
            view.viewObject = viewObject;
        }

        public void CreateArea(AreaTemplate template, int lineId, float z)
        {
            int entity = world.NewEntity();

            ref var area = ref areaPool.Add(entity);

            if (template.actionType == MockStaticGameData.AreaActionTypeId.Hit.ToString())
            {
                ref var hitArea = ref hitAreaPool.Add(entity);
                hitArea.hitPower = template.hitPower;
                hitArea.size = template.size;
                hitArea.cooldown = template.actionCooldown;

                ref var cooldown = ref cooldownPool.Add(entity);
                cooldown.cooldown = 0;
            }
            else if (template.actionType == MockStaticGameData.AreaActionTypeId.Freeze.ToString())
            {
                ref var freezeArea = ref freezeAreaPool.Add(entity);
                //freezeArea.hitPower = template.attackPower;
                freezeArea.size = template.size;
                freezeArea.cooldown = template.actionCooldown;

                ref var cooldown = ref cooldownPool.Add(entity);
                cooldown.cooldown = 0;
            }

            ref var position = ref positionPool.Add(entity);
            position.lineId = lineId;
            position.x = 0;
            position.z = z;

            ref var lifetime = ref lifetimePool.Add(entity);
            lifetime.lifetime = template.lifetime;

            //var viewObject = entityViewFactory.Create(template.prefabPath);
            var viewObject = entityViewPool.Spawn(template.prefabPath);

            viewObject.transform.position = GameUtils.PositionToVector3(position.lineId, position.x, position.z);
            viewObject.transform.rotation = Quaternion.identity;
            viewObject.entityId = world.PackEntity(entity);

            ref var view = ref viewPool.Add(entity);
            view.viewObject = viewObject;
        }

        public void Dispose()
        {
            gameState = null;
            world = null;
            //todo: clear pool references
        }
    }
}
