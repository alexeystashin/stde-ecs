using Leopotam.EcsLite;
using System.Linq;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class GameStartup : MonoBehaviour
    {
        DiContainer di;

        EcsWorld world;
        GameState gameState;
        GameRules gameRules;
        GameObjectBuilder objectBuilder;

        IEcsSystems systems;

        [Inject]
        void Construct(DiContainer di, EcsWorld world, GameState gameState, GameRules gameRules, GameObjectBuilder objectBuilder)
        {
            this.di = di;
            this.world = world;
            this.gameState = gameState;
            this.gameRules = gameRules;
            this.objectBuilder = objectBuilder;

            InitSystems();

            InitGame();
        }

        void InitGame()
        {
            Debug.LogWarning("Create Game");

            // create initial entities
            objectBuilder.CreateInitialEntities();

            // calculate wave time (duplicate in GameStartup & WaveSystem!)
            gameState.currentWaveTime = 0;
            gameState.currentWaveTimeTotal = 0;
            foreach (var line in gameRules.waves[gameState.currentWave].lineSpawners)
            {
                foreach (var spawnerTemplate in line)
                {
                    gameState.currentWaveTimeTotal = Mathf.Max(gameState.currentWaveTimeTotal,
                        spawnerTemplate.delay + spawnerTemplate.lifetime);
                }
            }
        }

        void InitSystems()
        {
            systems = new EcsSystems(world);
            systems
                // register your systems here
                .Add(di.Instantiate<LifetimeSystem>())
                .Add(di.Instantiate<CooldownSystem>())
                .Add(di.Instantiate<FreezedTimeSystem>())
                .Add(di.Instantiate<SpawnCreatureSystem>())
                .Add(di.Instantiate<UpdateCreatureListSystem>())
                .Add(di.Instantiate<UpdateTurretListSystem>())
                .Add(di.Instantiate<PlayerInputSystem>())
                .Add(di.Instantiate<TriggerAreaByCooldownSystem>())
                .Add(di.Instantiate<TurretFireTriggerByCooldownSystem>())
                .Add(di.Instantiate<TurretFireSystem>())
                .Add(di.Instantiate<MoveTurretSystem>())
                .Add(di.Instantiate<MoveEntitySystem>())
                .Add(di.Instantiate<UpdateEntityViewPositionSystem>())
                .Add(di.Instantiate<UpdateTurretHudSystem>())
                .Add(di.Instantiate<BoltCollisionSystem>())
                .Add(di.Instantiate<ApplyAreaBoltSystem>())
                .Add(di.Instantiate<ApplyHitBoltSystem>())
                .Add(di.Instantiate<ApplyHitAreaSystem>())
                .Add(di.Instantiate<ApplyFreezeAreaSystem>())
                .Add(di.Instantiate<ApplyDamageSystem>())
                .Add(di.Instantiate<CreatureArriveSystem>())
                .Add(di.Instantiate<CheckGameCompleteSystem>())
                .Add(di.Instantiate<WaveSystem>())
                .Add(di.Instantiate<UpdateGameUiSystem>())
                .Add(di.Instantiate<AddScoreForKillSystem>())
                .Add(di.Instantiate<BoltArriveSystem>())
                .Add(di.Instantiate<FinishGameSystem>())
                .Add(di.Instantiate<RemoveComponentSystem<AreaTrigger>>())
                .Add(di.Instantiate<RemoveComponentSystem<BoltTrigger>>())
                .Add(di.Instantiate<RemoveComponentSystem<TurretFireTrigger>>())
                .Add(di.Instantiate<RemoveComponentSystem<GameFinishedEvent>>())
                .Add(di.Instantiate<DestroyEntityUiSystem>())
                .Add(di.Instantiate<DestroyEntityViewSystem>())
                .Add(di.Instantiate<DestroyEntitySystem>())

                // register additional worlds here, for example:
                .AddWorld(new EcsWorld(), "events")
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem("events"))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Init();
        }

        void Update()
        {
            if (gameState.isGameFinished)
                return;

            // process systems here.
            if (systems != null)
                systems.Run();
        }

        // todo: replace a hack by a correct dispose of View objects
        void ForceCleanup()
        {
            if (world == null)
                return;

            gameState.isGameFinished = true;

            var filter = world.Filter<View>().End();
            var destroyMarkerPool = world.GetPool<DestroyMarker>();
            foreach (var entity in filter)
            {
                destroyMarkerPool.GetOrAdd(entity);
            }

            var allSystems = systems.GetAllSystems();
            ((IEcsRunSystem)allSystems.First(s => s is DestroyEntityUiSystem)).Run(systems);
            ((IEcsRunSystem)allSystems.First(s => s is DestroyEntityViewSystem)).Run(systems);
        }

        void OnDestroy()
        {
            Debug.LogWarning("Destroy Game");

            ForceCleanup();

            if (systems != null) {
                systems.Destroy();
                systems = null;
            }
            
            // cleanup custom worlds here.
            
            // cleanup default world.
            if (world != null) {
                world.Destroy();
                world = null;
            }
        }
    }
}