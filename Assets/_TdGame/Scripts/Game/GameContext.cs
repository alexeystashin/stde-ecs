using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TdGame
{
    public class GameContext : IDisposable
    {
        // external objects

        public StaticGameData staticGameData;
        public GameRules gameRules;

        public Camera gameCamera;
        public Canvas gameCanvas;
        public GameInput gameInput;
        public GameUi gameUi;

        // internal objects

        public GameObjectBuilder objectBuilder;

        // game state

        public int currentWave;
        public float currentWaveTime;
        public float currentWaveTimeTotal;

        public int score;

        public bool isGameFinished;
        public bool isWin;

        // cache

        public List<List<int>> creaturesByLine;
        public List<List<int>> turretsByLine;

        public GameContext(EcsWorld world)
        {
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
            objectBuilder.Dispose();
            objectBuilder = null;

            for (var i = 0; i < creaturesByLine.Count; i++)
                creaturesByLine[i].Clear();
            creaturesByLine.Clear();
            creaturesByLine = null;

            for (var i = 0; i < turretsByLine.Count; i++)
                turretsByLine[i].Clear();
            turretsByLine.Clear();
            turretsByLine = null;

            staticGameData = null;
            gameRules = null;

            gameCamera = null;
            gameCanvas = null;
            gameInput = null;
            gameUi = null;
        }
    }
}
