using Leopotam.EcsLite;
using System;
using System.Collections.Generic;

namespace TdGame
{
    public class GameContext : IDisposable
    {
        public StaticGameData staticGameData;
        public GameRules gameRules;

        public GameObjectBuilder objectBuilder;

        public GameInput gameInput;

        // game state

        public int currentWave;

        public bool isGameFinished;
        public bool isWin;

        // cache

        public List<List<int>> creaturesByLine;
        public List<List<int>> turretsByLine;

        public GameContext(EcsWorld world)
        {
            staticGameData = MockStaticGameData.Create();

            gameRules = MockStaticGameData.CreateEasyGameRules();

            objectBuilder = new GameObjectBuilder(this, world);

            creaturesByLine = new List<List<int>>();
            for (var i = 0; i < MagicNumbersGame.lineCount; i++)
                creaturesByLine.Add(new List<int>());

            turretsByLine = new List<List<int>>();
            for (var i = 0; i < MagicNumbersGame.lineCount; i++)
                turretsByLine.Add(new List<int>());
        }

        public void Dispose()
        {
            staticGameData = null;

            gameRules = null;

            objectBuilder.Dispose();
            objectBuilder = null;

            gameInput = null;

            for (var i = 0; i < creaturesByLine.Count; i++)
                creaturesByLine[i].Clear();
            creaturesByLine.Clear();
            creaturesByLine = null;

            for (var i = 0; i < turretsByLine.Count; i++)
                turretsByLine[i].Clear();
            turretsByLine.Clear();
            turretsByLine = null;
        }
    }
}
