using Leopotam.EcsLite;
using System;
using System.Collections.Generic;

namespace TdGame
{
    public class GameContext : IDisposable
    {
        public GameInput gameInput;

        public GameObjectBuilder objectBuilder;

        public List<List<int>> creaturesByLine;
        public List<List<int>> turretsByLine;

        public GameContext(EcsWorld world)
        {
            objectBuilder = new GameObjectBuilder(world);

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
        }
    }
}
