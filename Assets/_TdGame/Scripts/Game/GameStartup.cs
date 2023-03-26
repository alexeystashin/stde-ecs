using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class GameStartup : MonoBehaviour
    {
        EcsWorld world;
        GameContext gameContext;
        IEcsSystems systems;

        void Start() {
            world = new EcsWorld();

            gameContext = new GameContext(world);
            gameContext.gameInput = gameObject.AddComponent<GameInput>();

            systems = new EcsSystems(world, gameContext);
            systems
                // register your systems here
                .Add(new SpawnCreatureSystem())
                .Add(new UpdateCreatureListSystem())
                .Add(new UpdateTurretListSystem())
                .Add(new PlayerInputSystem())
                .Add(new FireTurretSystem())
                .Add(new MoveTurretSystem())
                .Add(new MoveEntitySystem())
                .Add(new UpdateViewPositionSystem())
                .Add(new BulletCollisionSystem())
                .Add(new ApplyDamageSystem())
                .Add(new CreatureArriveSystem())
                .Add(new BulletArriveSystem())
                .Add(new DestroyViewSystem())
                .Add(new DestroyEntitySystem())
                
                // register additional worlds here, for example:
                // .AddWorld(new EcsWorld(), "events")
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem("events"))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Init();

            // create initial entities
            gameContext.objectBuilder.CreateInitialEntities();
        }

        void Update() {
            // process systems here.
            if (systems != null)
                systems.Run();
        }

        void OnDestroy() {
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

            if (gameContext != null)
            {
                gameContext.Dispose();
                gameContext = null;
            }
        }
    }
}