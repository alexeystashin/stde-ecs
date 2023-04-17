using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Common
{
    public class MultiPrefabPool<TKey, TValue> where TValue : Component, IPoolable<IMemoryPool>
    {
        Dictionary<TKey, InternalPool> pools = new Dictionary<TKey, InternalPool>();
        DiContainer di;
        MemoryPoolSettings settings;
        IFactory<TKey, TValue> factory;

        public MultiPrefabPool(DiContainer di, MemoryPoolSettings settings, IFactory<TKey, TValue> factory)
        {
            this.di = di;
            this.settings = settings;
            this.factory = factory;
        }

        public TValue Spawn(TKey param)
        {
            if (!pools.TryGetValue(param, out var pool))
            {
                pool = CreatePool(param);
                pools.Add(param, pool);
            }

            return pool.Spawn(pool);
        }

        public void Despawn(TKey param, TValue gameObject)
        {
            pools[param].Despawn(gameObject);
        }

        InternalPool CreatePool(TKey param)
        {
            return di.Instantiate<InternalPool>(new object[] { settings, new FactoryProxy(param, factory) });
        }

        class FactoryProxy : IFactory<TValue> //Factory which proxy all creation to factory
        {
            TKey param;
            IFactory<TKey, TValue> factory;

            public FactoryProxy(TKey param, IFactory<TKey, TValue> factory)
            {
                this.param = param;
                this.factory = factory;
            }

            public TValue Create()
            {
                return factory.Create(param);
            }
        }

        class InternalPool : MonoPoolableMemoryPool<IMemoryPool, TValue>
        {
        }
    }
}
