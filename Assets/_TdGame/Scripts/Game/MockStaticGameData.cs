using System.Collections.Generic;
using UnityEngine;

namespace TdGame
{
    public class MockStaticGameData
    {
        // project-specific enums

        public enum AreaActionTypeId
        {
            Attack,
            Freeze
        }

        public enum TowerTemplateId
        {
            None,
            MachineGun,
            Rocket,
            Laser,
            Freezer,
            Poisoner
        }

        public enum CreatureTemplateId
        {
            None,
            Creep,
            FastCreep,
            Tank
        }

        public enum AreaTemplateId
        {
            None,
            Bullet,
            Explosion,
            Laser,
            Ice,
            Poison,
        }

        public enum BoltTemplateId
        {
            None,
            Bullet,
            Rocket,
            Laser,
            Ice,
            Poison,
        }

        public static StaticGameData Create()
        {
            var staticGameData = new StaticGameData
            {
                towers = new Dictionary<string, TowerTemplate>(),
                creatures = new Dictionary<string, CreatureTemplate>(),
                areas = new Dictionary<string, AreaTemplate>(),
                bolts = new Dictionary<string, BoltTemplate>()
            };

            // towers

            var tower = new TowerTemplate
            {
                id = TowerTemplateId.MachineGun.ToString(),
                prefabPath = GamePrefabPath.MachineGunTower,
                health = 100,
                size = 3,
                attackBoltId = BoltTemplateId.Bullet.ToString(),
                attackCooldown = 0.25f,
                autoAttack = true,
            };
            staticGameData.towers.Add(tower.id, tower);

            tower = new TowerTemplate
            {
                id = TowerTemplateId.Rocket.ToString(),
                prefabPath = GamePrefabPath.RocketTower,
                health = 100,
                size = 3,
                attackBoltId = BoltTemplateId.Rocket.ToString(),
                attackCooldown = 10f,
            };
            staticGameData.towers.Add(tower.id, tower);

            // creatures

            var creature = new CreatureTemplate
            {
                id = CreatureTemplateId.Creep.ToString(),
                prefabPath = GamePrefabPath.CreepCreature,
                health = 3,
                size = 1,
                moveSpeed = 1.5f,
                attackPower = 1,
                attackCooldown = 0.5f,
                score = 10
            };
            staticGameData.creatures.Add(creature.id, creature);

            creature = new CreatureTemplate
            {
                id = CreatureTemplateId.FastCreep.ToString(),
                prefabPath = GamePrefabPath.FastCreature,
                health = 5,
                size = 1,
                moveSpeed = 2f,
                attackPower = 1,
                attackCooldown = 1f,
                score = 50
            };
            staticGameData.creatures.Add(creature.id, creature);

            creature = new CreatureTemplate
            {
                id = CreatureTemplateId.Tank.ToString(),
                prefabPath = GamePrefabPath.BigCreature,
                health = 30,
                size = 2,
                moveSpeed = 1f,
                attackPower = 5,
                attackCooldown = 2f,
                score = 100
            };
            staticGameData.creatures.Add(creature.id, creature);

            // areas

            var area = new AreaTemplate
            {
                id = AreaTemplateId.Explosion.ToString(),
                prefabPath = GamePrefabPath.ExplosionFx,
                attackPower = 10,
                actionType = AreaActionTypeId.Attack.ToString(),
                size = 3,
                actionCooldown = 999,
                lifetime = 1.0f,
            };
            staticGameData.areas.Add(area.id, area);

            // bolts

            var bolt = new BoltTemplate
            {
                id = BoltTemplateId.Bullet.ToString(),
                prefabPath = GamePrefabPath.BulletBolt,
                size = 0.25f,
                moveSpeed = 30,
                //attackAreaId = null,
                attackPower = 1,
                //pushDistance = 0.1f,
                //pushSize = 0,
            };
            staticGameData.bolts.Add(bolt.id, bolt);

            bolt = new BoltTemplate
            {
                id = BoltTemplateId.Rocket.ToString(),
                prefabPath = GamePrefabPath.RocketBolt,
                size = 0.25f,
                moveSpeed = 15,
                attackAreaId = AreaTemplateId.Explosion.ToString(),
                //attackPower = 0,
                //pushDistance = 0.5f,
                //pushSize = 1,
            };
            staticGameData.bolts.Add(bolt.id, bolt);

            return staticGameData;
        }

        public static GameRules CreateEasyGameRules()
        {
            var rules = new GameRules
            {
                waves = new List<WaveTemplate>(),
                playerTowers = new List<string>()
            };

            rules.playerTowers.Add(TowerTemplateId.Rocket.ToString());
            rules.playerTowers.Add(TowerTemplateId.MachineGun.ToString());
            rules.playerTowers.Add(TowerTemplateId.Rocket.ToString());

            const float cooldownChangePerWave = -0.1f;

            for (var w = 0; w < MagicNumbersGame.easyWavesCount; w++)
            {
                var waveTemplate = new WaveTemplate
                {
                    lineSpawners = new List<List<SpawnerTemplate>>()
                };

                for (var i = 0; i < MagicNumbersGame.lineCount; i++)
                {
                    var spawners = new List<SpawnerTemplate>();

                    // todo: fake data
                    var spawnerTemplate = new SpawnerTemplate
                    {
                        creatureId = CreatureTemplateId.Creep.ToString(),
                        delay = 0,
                        lifetime = 10,
                        cooldownMin = 2.0f + w * cooldownChangePerWave,
                        cooldownMax = 3.0f + w * cooldownChangePerWave
                    };
                    spawners.Add(spawnerTemplate);

                    spawnerTemplate = new SpawnerTemplate
                    {
                        creatureId = CreatureTemplateId.FastCreep.ToString(),
                        delay = 10,
                        lifetime = 10,
                        cooldownMin = 10.0f + w * cooldownChangePerWave,
                        cooldownMax = 15.0f + w * cooldownChangePerWave
                    };
                    spawners.Add(spawnerTemplate);

                    spawnerTemplate = new SpawnerTemplate
                    {
                        creatureId = CreatureTemplateId.Tank.ToString(),
                        delay = 10,
                        lifetime = 10,
                        cooldownMin = 10.0f + w * cooldownChangePerWave,
                        cooldownMax = 15.0f + w * cooldownChangePerWave
                    };
                    spawners.Add(spawnerTemplate);

                    waveTemplate.lineSpawners.Add(spawners);
                }

                rules.waves.Add(waveTemplate);
            }

            return rules;
        }

        public static GameRules CreateHardGameRules()
        {
            var rules = new GameRules
            {
                waves = new List<WaveTemplate>(),
                playerTowers = new List<string>()
            };

            rules.playerTowers.Add(TowerTemplateId.Rocket.ToString());
            rules.playerTowers.Add(TowerTemplateId.MachineGun.ToString());
            rules.playerTowers.Add(TowerTemplateId.Rocket.ToString());
            rules.playerTowers.Add(TowerTemplateId.MachineGun.ToString());
            rules.playerTowers.Add(TowerTemplateId.Rocket.ToString());
            rules.playerTowers.Add(TowerTemplateId.MachineGun.ToString());

            const float cooldownChangePerWave = -0.2f;

            for (var w = 0; w < MagicNumbersGame.hardWavesCount; w++)
            {
                var waveTemplate = new WaveTemplate
                {
                    lineSpawners = new List<List<SpawnerTemplate>>()
                };

                for (var i = 0; i < MagicNumbersGame.lineCount; i++)
                {
                    var spawners = new List<SpawnerTemplate>();

                    // todo: fake data
                    var spawnerTemplate = new SpawnerTemplate
                    {
                        creatureId = CreatureTemplateId.Creep.ToString(),
                        delay = 0,
                        lifetime = 10,
                        cooldownMin = 2.0f + w * cooldownChangePerWave,
                        cooldownMax = 3.0f + w * cooldownChangePerWave
                    };
                    spawners.Add(spawnerTemplate);

                    spawnerTemplate = new SpawnerTemplate
                    {
                        creatureId = CreatureTemplateId.FastCreep.ToString(),
                        delay = 10,
                        lifetime = 10,
                        cooldownMin = 10.0f + w * cooldownChangePerWave,
                        cooldownMax = 15.0f + w * cooldownChangePerWave
                    };
                    spawners.Add(spawnerTemplate);

                    spawnerTemplate = new SpawnerTemplate
                    {
                        creatureId = CreatureTemplateId.Tank.ToString(),
                        delay = 10,
                        lifetime = 10,
                        cooldownMin = 10.0f + w * cooldownChangePerWave,
                        cooldownMax = 15.0f + w * cooldownChangePerWave
                    };
                    spawners.Add(spawnerTemplate);

                    waveTemplate.lineSpawners.Add(spawners);
                }

                rules.waves.Add(waveTemplate);
            }

            return rules;
        }
    }
}
