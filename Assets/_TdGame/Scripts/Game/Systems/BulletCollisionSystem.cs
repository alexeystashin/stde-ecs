using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class BulletCollisionSystem : IEcsInitSystem, IEcsRunSystem
    {
        GameContext context;
        EcsWorld world;
        EcsPool<Position> positionPool;
        EcsPool<Damage> damagePool;
        EcsPool<DestroyMarker> destroyMarkerPool;

        public void Init(IEcsSystems systems)
        {
            context = systems.GetShared<GameContext>();
            world = systems.GetWorld();
            positionPool = world.GetPool<Position>();
            damagePool = world.GetPool<Damage>();
            destroyMarkerPool = world.GetPool<DestroyMarker>();
        }

        public void Run(IEcsSystems systems)
        {
            var filter = world.Filter<Bullet>().Inc<Position>().End();

            foreach (int entity in filter)
            {
                ref var position = ref positionPool.Get(entity);
                var creaturesByLine = context.creaturesByLine[position.lineId];
                for (var i = 0; i < creaturesByLine.Count; i++)
                {
                    var creatureEntity = creaturesByLine[i];
                    if (destroyMarkerPool.Has(creatureEntity))
                        continue;

                    ref var creaturePosition = ref positionPool.Get(creatureEntity);
                    if (position.z >= creaturePosition.z)
                    {
                        if (damagePool.Has(creatureEntity))
                        {
                            ref var creatureDamage = ref damagePool.Get(creatureEntity);
                            creatureDamage.damage += MagicNumbersGame.bulletHitPower;
                        }
                        else
                        {
                            ref var creatureDamage = ref damagePool.Add(creatureEntity);
                            creatureDamage.damage += MagicNumbersGame.bulletHitPower;
                        }

                        destroyMarkerPool.Add(entity);
                        break;
                    }
                }
            }
        }
    }
}
