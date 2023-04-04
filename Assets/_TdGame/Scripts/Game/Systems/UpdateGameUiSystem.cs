using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class UpdateGameUiSystem : IEcsInitSystem, IEcsRunSystem
    {
        GameState gameState;
        GameRules gameRules;
        GameUi gameUi;

        [Inject]
        void Construct(GameState gameState, GameRules gameRules, GameUi gameUi)
        {
            this.gameState = gameState;
            this.gameRules = gameRules;
            this.gameUi = gameUi;
        }

        public void Init(IEcsSystems systems)
        {
        }

        public void Run(IEcsSystems systems)
        {
            var totalWaves = gameRules.waves.Count;
            var currentWave = Mathf.Min(gameState.currentWave + 1, totalWaves);
            gameUi.waveCounterText.text = $"Wave {currentWave}/{totalWaves}";

            gameUi.waveTimeText.text = VisUtils.FormatTime(
                    (int)(gameState.currentWaveTimeTotal - gameState.currentWaveTime));

            gameUi.scoreText.text =  gameState.score.ToString();
        }
    }
}
