using System.Collections.Generic;

namespace TdGame
{
    public class StaticGameData
    {
        public Dictionary<string, TowerTemplate> towers;
        public Dictionary<string, CreatureTemplate> creatures;
        public Dictionary<string, BoltTemplate> bolts;
        public Dictionary<string, AreaTemplate> areas;
    }
}
