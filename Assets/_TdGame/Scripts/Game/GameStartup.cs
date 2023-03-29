using Leopotam.EcsLite;
using System.Linq;
using UnityEngine;

namespace TdGame
{
    sealed class GameStartup : MonoBehaviour
    {
        [SerializeField] Camera gameCamera;
        [SerializeField] Canvas gameCanvas;
        [SerializeField] GameInput gameInput;
        [SerializeField] GameUi gameUi;

        EcsWorld world;
        GameContext context;
        IEcsSystems systems;

        void Start()
        {
            world = new EcsWorld();

            context = new GameContext(world);

            context.gameCamera = gameCamera;
            context.gameCanvas = gameCanvas;
            context.gameInput = gameInput;
            context.gameUi = gameUi;

            context.staticGameData = MockStaticGameData.Create();

            //todo: change rules setup
            var level = PlayerPrefs.GetInt("level");
            if (level > 1)
                context.gameRules = MockStaticGameData.CreateHardGameRules();
            else if (level == 1)
                context.gameRules = MockStaticGameData.CreateMediumGameRules();
            else
                context.gameRules = MockStaticGameData.CreateEasyGameRules();

            systems = new EcsSystems(world, context);
            systems
                // register your systems here
                .Add(new LifetimeSystem())
                .Add(new CooldownSystem())
                .Add(new FreezedTimeSystem())
                .Add(new SpawnCreatureSystem())
                .Add(new UpdateCreatureListSystem())
                .Add(new UpdateTurretListSystem())
                .Add(new PlayerInputSystem())
                .Add(new TriggerAreaByCooldownSystem())
                .Add(new TurretFireTriggerByCooldownSystem())
                .Add(new TurretFireSystem())
                .Add(new MoveTurretSystem())
                .Add(new MoveEntitySystem())
                .Add(new UpdateEntityViewPositionSystem())
                .Add(new UpdateTurretHudSystem())
                .Add(new BoltCollisionSystem())
                .Add(new ApplyAreaBoltSystem())
                .Add(new ApplyHitBoltSystem())
                .Add(new ApplyHitAreaSystem())
                .Add(new ApplyFreezeAreaSystem())
                .Add(new ApplyDamageSystem())
                .Add(new CreatureArriveSystem())
                .Add(new CheckGameCompleteSystem())
                .Add(new WaveSystem())
                .Add(new UpdateGameUiSystem())
                .Add(new AddScoreForKillSystem())
                .Add(new BoltArriveSystem())
                .Add(new FinishGameSystem())
                .Add(new RemoveComponentSystem<AreaTrigger>())
                .Add(new RemoveComponentSystem<BoltTrigger>())
                .Add(new RemoveComponentSystem<TurretFireTrigger>())
                .Add(new RemoveComponentSystem<GameFinishedEvent>())
                .Add(new DestroyEntityUiSystem())
                .Add(new DestroyEntityViewSystem())
                .Add(new DestroyEntitySystem())
                
                // register additional worlds here, for example:
                .AddWorld(new EcsWorld(), "events")
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem("events"))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Init();

            // create initial entities
            context.objectBuilder.CreateInitialEntities();

            // calculate wave time (duplicate in GameStartup & WaveSystem!)
            context.currentWaveTime = 0;
            context.currentWaveTimeTotal = 0;
            foreach (var line in context.gameRules.waves[context.currentWave].lineSpawners)
            {
                foreach (var spawnerTemplate in line)
                {
                    context.currentWaveTimeTotal = Mathf.Max(context.currentWaveTimeTotal,
                        spawnerTemplate.delay + spawnerTemplate.lifetime);
                }
            }
        }

        void Update()
        {
            if (context.isGameFinished)
                return;

            // process systems here.
            if (systems != null)
                systems.Run();
        }

        void ForceCleanup()
        {
            if (world == null)
                return;

            context.isGameFinished = true;

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
            ForceCleanup();

            if (systems != null) {
                // list of custom worlds will be cleared
                // during IEcsSystems.Destroy(). so, you
                // need to save it here if you need.
                systems.Destroy();
                systems = null;
            }
            
            // cleanup custom worlds here.
            
            // cleanup default world.
            if (world != null) {
                world.Destroy();
                world = null;
            }

            if (context != null)
            {
                context.Dispose();
                context = null;
            }
        }
    }
}