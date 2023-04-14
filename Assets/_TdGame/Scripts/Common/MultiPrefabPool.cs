using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Common
{
    public class MultiPrefabPool<TKey, TValue> where TValue : Component, IPoolable<IMemoryPool>
    {
        Dictionary<TKey, InternalPool<TValue>> _pools = new Dictionary<TKey, InternalPool<TValue>>();
        DiContainer _container;
        MemoryPoolSettings _settings;
        IFactory<TKey, TValue> _factory;

        public MultiPrefabPool(DiContainer container, MemoryPoolSettings settings, IFactory<TKey, TValue> factory)
        {
            _container = container;
            _settings = settings;
            _factory = factory;

        }

        public TValue Spawn(TKey param)
        {
            if (!_pools.TryGetValue(param, out var pool))
            {
                pool = CreatePool(param);
                _pools.Add(param, pool);
            }

            return pool.Spawn(pool);
        }

        public void Despawn(TKey param, TValue gameObject)
        {
            _pools[param].Despawn(gameObject);
        }

        InternalPool<TValue> CreatePool(TKey param)
        {
            return _container.Instantiate<InternalPool<TValue>>(new object[] { _settings, new FactoryProxy<TValue>(param, _factory) });
        }

        class FactoryProxy<TValue> : IFactory<TValue> //Factory which proxy all creation to factory
        {
            TKey _param;
            IFactory<TKey, TValue> _factory;

            public FactoryProxy(TKey param, IFactory<TKey, TValue> factory)
            {
                _param = param;
                _factory = factory;
            }

            public TValue Create()
            {
                return _factory.Create(_param);
            }
        }

        class InternalPool<TValue> : MonoPoolableMemoryPool<IMemoryPool, TValue> where TValue : Component, IPoolable<IMemoryPool>
        {
        }
    }
}
