using Leopotam.EcsLite;
using UnityEngine;

namespace TdGame
{
    sealed class UpdateGameUiSystem : IEcsInitSystem, IEcsRunSystem
    {
        GameContext context;

        public void Init(IEcsSystems systems)
        {
            context = systems.GetShared<GameContext>();
        }

        public void Run(IEcsSystems systems)
        {
            var totalWaves = context.gameRules.waves.Count;
            var currentWave = Mathf.Min(context.currentWave + 1, totalWaves);
            context.gameUi.waveCounterText.text = $"Wave {currentWave}/{totalWaves}";

            context.gameUi.waveTimeText.text = VisUtils.FormatTime(
                    (int)(context.currentWaveTimeTotal - context.currentWaveTime));

            context.gameUi.scoreText.text =  context.score.ToString();
        }
    }
}
