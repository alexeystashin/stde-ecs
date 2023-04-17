using System;
using System.Collections.Generic;
using UnityEngine;

namespace TdGame
{
    public class GameState : IDisposable
    {
        public int currentWave;
        public float currentWaveTime;
        public float currentWaveTimeTotal;

        public int score;

        public int gamePauseCounter;
        public bool isGameWon;
        public bool isGameLost;

        public bool isGameRunning => !isGamePaused && !isGameFinished;
        public bool isGamePaused => gamePauseCounter > 0;
        public bool isGameFinished => isGameWon || isGameLost;

        // cache

        public List<List<int>> creaturesByLine;
        public List<List<int>> turretsByLine;

        public GameState()
        {
            creaturesByLine = new List<List<int>>();
            for (var i = 0; i < MagicNumbersGame.lineCount; i++)
                creaturesByLine.Add(new List<int>());

            turretsByLine = new List<List<int>>();
            for (var i = 0; i < MagicNumbersGame.lineCount; i++)
                turretsByLine.Add(new List<int>());
        }

        public void Dispose()
        {
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
