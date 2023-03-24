using Leopotam.EcsLite;
using TdGame;
using UnityEngine;

namespace Client {
    sealed class GameStartup : MonoBehaviour
    {
        EcsWorld world;
        IEcsSystems systems;

        void Start() {
            world = new EcsWorld();
            systems = new EcsSystems(world);
            systems
                // register your systems here
                .Add (new CreatureSpawnSystem())
                .Add (new CreatureMoveSystem())
                .Add (new ViewPositionUpdateSystem())
                .Add (new CreatureArriveSystem())
                .Add (new DestroyViewSystem())
                .Add (new DestroyEntitySystem())
                
                // register additional worlds here, for example:
                // .AddWorld (new EcsWorld (), "events")
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Init();

            // create initial entities
            var positionPool = world.GetPool<Position>();
            var creatureSpawnerPool = world.GetPool<CreatureSpawner>();
            for(var i = 0; i < 3; i++)
            {
                var entity = world.NewEntity();

                ref var position = ref positionPool.Add(entity);
                position.lineId = i;
                position.x = 0;
                position.z = MagicNumbersGame.creatureSpawnerZ;

                ref var creatureSpawner = ref creatureSpawnerPool.Add(entity);
                creatureSpawner.cooldown = Random.Range(MagicNumbersGame.creatureSpawnerCooldownMin, MagicNumbersGame.creatureSpawnerCooldownMax);
                creatureSpawner.creatureVelocityZ = MagicNumbersGame.creatureVelocityZ;
            }
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
        }
    }
}