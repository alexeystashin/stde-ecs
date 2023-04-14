using Common;
using Leopotam.EcsLite;
using System;
using UnityEngine;
using Zenject;

namespace TdGame
{
    public class EntityView : MonoBehaviour, IDisposable, IPoolable<IMemoryPool>
    {
        public EcsPackedEntity entityId;

        IMemoryPool _pool;

        bool isDisposed;

        [Inject]
        void Construct()
        {
            Debug.Log($"EntityView.Construct");
        }

        public virtual void OnSpawned(IMemoryPool pool)
        {
            Debug.Log($"EntityView.OnSpawned {gameObject.name}");
            isDisposed = false;
            _pool = pool;
        }

        public virtual void OnDespawned()
        {
            Debug.Log($"EntityView.OnDespawned {gameObject.name}");
        }

        public virtual void SmoothDispose()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (isDisposed) return;

            isDisposed = true;

            Debug.Log($"Dispose {gameObject.name}");

            if (_pool != null)
            {
                _pool.Despawn(this);
                _pool = null;
            }
            else
                GameObject.Destroy(gameObject);
        }

        public class Factory : PlaceholderFactory<string, EntityView>
        {
        }

        public class Pool : MultiPrefabPool<string, EntityView>
        {
            [Inject]
            public Pool(DiContainer container, MemoryPoolSettings settings, EntityView.Factory factory) : base(container, settings, factory) { }
        }
    }

    class EntityViewFactory : IFactory<string, EntityView>
    {
        DiContainer di;
        PrefabCache prefabCache;
        Transform rootTransform;

        [Inject]
        public EntityViewFactory(DiContainer container, PrefabCache prefabCache, Transform rootTransform)
        {
            di = container;
            this.prefabCache = prefabCache;
            this.rootTransform = rootTransform;
        }

        public EntityView Create(string prefabPath)
        {
            Debug.Log($"Create EntityView {prefabPath}");
            var prefab = prefabCache.GetPrefab(prefabPath);
            var go = di.InstantiatePrefab(prefab, rootTransform);
            var entityView = go.GetComponent<EntityView>();
            if (entityView == null)
            {
                entityView = go.AddComponent<EntityView>();
                di.Inject(entityView);
            }
            return entityView;
        }
    }
}
