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
                prefabPath = GamePrefabPath.machineGunTower,
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
                prefabPath = GamePrefabPath.rocketTower,
                health = 100,
                size = 3,
                attackBoltId = BoltTemplateId.Rocket.ToString(),
                attackCooldown = 5f,
            };
            staticGameData.towers.Add(tower.id, tower);

            tower = new TowerTemplate
            {
                id = TowerTemplateId.Freezer.ToString(),
                prefabPath = GamePrefabPath.freezerTower,
                health = 100,
                size = 3,
                attackBoltId = BoltTemplateId.Ice.ToString(),
                attackCooldown = 7f,
            };
            staticGameData.towers.Add(tower.id, tower);

            tower = new TowerTemplate
            {
                id = TowerTemplateId.Poisoner.ToString(),
                prefabPath = GamePrefabPath.poisonerTower,
                health = 100,
                size = 3,
                attackBoltId = BoltTemplateId.Poison.ToString(),
                attackCooldown = 10f,
            };
            staticGameData.towers.Add(tower.id, tower);

            // creatures

            var creature = new CreatureTemplate
            {
                id = CreatureTemplateId.Creep.ToString(),
                prefabPath = GamePrefabPath.creepCreature,
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
                prefabPath = GamePrefabPath.fastCreature,
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
                prefabPath = GamePrefabPath.bigCreature,
                health = 20,
                size = 2,
                moveSpeed = 0.75f,
                attackPower = 5,
                attackCooldown = 2f,
                score = 100
            };
            staticGameData.creatures.Add(creature.id, creature);

            // areas

            var area = new AreaTemplate
            {
                id = AreaTemplateId.Explosion.ToString(),
                prefabPath = GamePrefabPath.explosionArea,
                attackPower = 10,
                actionType = AreaActionTypeId.Attack.ToString(),
                size = 3,
                actionCooldown = 999,
                lifetime = 1.0f,
            };
            staticGameData.areas.Add(area.id, area);

            area = new AreaTemplate
            {
                id = AreaTemplateId.Ice.ToString(),
                prefabPath = GamePrefabPath.iceArea,
                //attackPower = 0,
                actionType = AreaActionTypeId.Freeze.ToString(),
                size = 3,
                actionCooldown = 1.0f,
                lifetime = 5.0f,
            };
            staticGameData.areas.Add(area.id, area);

            area = new AreaTemplate
            {
                id = AreaTemplateId.Poison.ToString(),
                prefabPath = GamePrefabPath.poisonArea,
                attackPower = 2f,
                actionType = AreaActionTypeId.Attack.ToString(),
                size = 4,
                actionCooldown = 0.5f,
                lifetime = 5.0f,
            };
            staticGameData.areas.Add(area.id, area);

            // bolts

            var bolt = new BoltTemplate
            {
                id = BoltTemplateId.Bullet.ToString(),
                prefabPath = GamePrefabPath.bulletBolt,
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
                prefabPath = GamePrefabPath.rocketBolt,
                size = 0.25f,
                moveSpeed = 10,
                attackAreaId = AreaTemplateId.Explosion.ToString(),
                //attackPower = 0,
                //pushDistance = 0.5f,
                //pushSize = 1,
            };
            staticGameData.bolts.Add(bolt.id, bolt);

            bolt = new BoltTemplate
            {
                id = BoltTemplateId.Ice.ToString(),
                prefabPath = GamePrefabPath.iceBolt,
                size = 0.25f,
                moveSpeed = 5,
                attackAreaId = AreaTemplateId.Ice.ToString(),
                //attackPower = 0,
                //pushDistance = 0.5f,
                //pushSize = 1,
            };
            staticGameData.bolts.Add(bolt.id, bolt);

            bolt = new BoltTemplate
            {
                id = BoltTemplateId.Poison.ToString(),
                prefabPath = GamePrefabPath.poisonBolt,
                size = 0.25f,
                moveSpeed = 5,
                attackAreaId = AreaTemplateId.Poison.ToString(),
                //attackPower = 0,
                //pushDistance = 0.5f,
                //pushSize = 1,
            };
            staticGameData.bolts.Add(bolt.id, bolt);

            return staticGameData;
        }

        public static GameRules CreateTutorialGameRules()
        {
            var rules = new GameRules
            {
                waves = new List<WaveTemplate>(),
                towerLines = 1,
                playerTowers = new List<string>()
            };

            rules.playerTowers.Add(TowerTemplateId.MachineGun.ToString());
            rules.playerTowers.Add(TowerTemplateId.MachineGun.ToString());

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
                        delay = 3,
                        lifetime = 10,
                        cooldownMin = 3.0f + w * cooldownChangePerWave,
                        cooldownMax = 4.0f + w * cooldownChangePerWave
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

                    waveTemplate.lineSpawners.Add(spawners);
                }

                rules.waves.Add(waveTemplate);
            }

            return rules;
        }

        public static GameRules CreateEasyGameRules()
        {
            var rules = new GameRules
            {
                waves = new List<WaveTemplate>(),
                towerLines = 1,
                playerTowers = new List<string>()
            };

            rules.playerTowers.Add(TowerTemplateId.MachineGun.ToString());
            rules.playerTowers.Add(TowerTemplateId.Poisoner.ToString());
            rules.playerTowers.Add(TowerTemplateId.MachineGun.ToString());

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
                        delay = 3,
                        lifetime = 10,
                        cooldownMin = 3.0f + w * cooldownChangePerWave,
                        cooldownMax = 4.0f + w * cooldownChangePerWave
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

        public static GameRules CreateMediumGameRules()
        {
            var rules = new GameRules
            {
                waves = new List<WaveTemplate>(),
                towerLines = 2,
                playerTowers = new List<string>()
            };

            rules.playerTowers.Add(TowerTemplateId.Rocket.ToString());
            rules.playerTowers.Add(TowerTemplateId.MachineGun.ToString());
            rules.playerTowers.Add(TowerTemplateId.Freezer.ToString());
            rules.playerTowers.Add(TowerTemplateId.MachineGun.ToString());
            rules.playerTowers.Add(TowerTemplateId.Rocket.ToString());
            rules.playerTowers.Add(TowerTemplateId.MachineGun.ToString());

            const float cooldownChangePerWave = -0.05f;

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
                        delay = 3,
                        lifetime = 20,
                        cooldownMin = 3.0f + w * cooldownChangePerWave,
                        cooldownMax = 4.0f + w * cooldownChangePerWave
                    };
                    spawners.Add(spawnerTemplate);

                    spawnerTemplate = new SpawnerTemplate
                    {
                        creatureId = CreatureTemplateId.FastCreep.ToString(),
                        delay = 10,
                        lifetime = 15,
                        cooldownMin = 5.0f + w * cooldownChangePerWave,
                        cooldownMax = 10 + w * cooldownChangePerWave
                    };
                    spawners.Add(spawnerTemplate);

                    spawnerTemplate = new SpawnerTemplate
                    {
                        creatureId = CreatureTemplateId.Tank.ToString(),
                        delay = 10,
                        lifetime = 15,
                        cooldownMin = 10.0f + w * cooldownChangePerWave,
                        cooldownMax = 15.0f + w * cooldownChangePerWave
                    };
                    spawners.Add(spawnerTemplate);

                    spawnerTemplate = new SpawnerTemplate
                    {
                        creatureId = CreatureTemplateId.Tank.ToString(),
                        delay = 5,
                        lifetime = 20,
                        cooldownMin = 15.0f + w * cooldownChangePerWave,
                        cooldownMax = 20.0f + w * cooldownChangePerWave
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
                towerLines = 2,
                playerTowers = new List<string>()
            };

            rules.playerTowers.Add(TowerTemplateId.Freezer.ToString());
            rules.playerTowers.Add(TowerTemplateId.MachineGun.ToString());
            rules.playerTowers.Add(TowerTemplateId.Rocket.ToString());
            rules.playerTowers.Add(TowerTemplateId.MachineGun.ToString());
            rules.playerTowers.Add(TowerTemplateId.Poisoner.ToString());
            rules.playerTowers.Add(TowerTemplateId.MachineGun.ToString());

            const float cooldownChangePerWave = -0.05f;

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
                        delay = 3,
                        lifetime = 20,
                        cooldownMin = 3.0f + w * cooldownChangePerWave,
                        cooldownMax = 4.0f + w * cooldownChangePerWave
                    };
                    spawners.Add(spawnerTemplate);

                    spawnerTemplate = new SpawnerTemplate
                    {
                        creatureId = CreatureTemplateId.FastCreep.ToString(),
                        delay = 10,
                        lifetime = 15,
                        cooldownMin = 5.0f + w * cooldownChangePerWave,
                        cooldownMax = 10 + w * cooldownChangePerWave
                    };
                    spawners.Add(spawnerTemplate);

                    spawnerTemplate = new SpawnerTemplate
                    {
                        creatureId = CreatureTemplateId.Tank.ToString(),
                        delay = 10,
                        lifetime = 15,
                        cooldownMin = 10.0f + w * cooldownChangePerWave,
                        cooldownMax = 15.0f + w * cooldownChangePerWave
                    };
                    spawners.Add(spawnerTemplate);

                    spawnerTemplate = new SpawnerTemplate
                    {
                        creatureId = CreatureTemplateId.Tank.ToString(),
                        delay = 5,
                        lifetime = 20,
                        cooldownMin = 15.0f + w * cooldownChangePerWave,
                        cooldownMax = 20.0f + w * cooldownChangePerWave
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
