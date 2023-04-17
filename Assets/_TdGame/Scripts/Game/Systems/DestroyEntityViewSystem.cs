using Common;
using Leopotam.EcsLite;
using System;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class DestroyEntityViewSystem : IEcsInitSystem, IEcsRunSystem, IDisposable
    {
        EcsWorld world;
        EcsPool<View> viewPool;

        GameState gameState;

        [Inject]
        void Construct(GameState gameState)
        {
            this.gameState = gameState;
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            viewPool = world.GetPool<View>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<DestroyMarker>().Inc<View>().End();

            foreach (int entity in filter)
            {
                ref var view = ref viewPool.Get(entity);
                if (view.viewObject != null)
                {
                    //GameObject.Destroy(view.viewObject.gameObject);
                    view.viewObject.SmoothDispose();
                }
                view.viewObject = null;
                viewPool.Del(entity);
            }
        }

        public void Dispose()
        {
            Debug.LogWarning("DestroyEntityViewSystem.Dispose");
        }
    }
}
