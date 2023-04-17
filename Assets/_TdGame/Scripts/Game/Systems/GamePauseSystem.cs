using Leopotam.EcsLite;
using Zenject;

namespace TdGame
{
    sealed class GamePauseSystem : IEcsInitSystem, IEcsRunSystem
    {
        GameState gameState;
        GameTime gameTime;

        [Inject]
        void Construct(GameState gameState, GameTime gameTime)
        {
            this.gameState = gameState;
            this.gameTime = gameTime;
        }

        public void Init(IEcsSystems systems)
        {
        }

        public void Run(IEcsSystems systems)
        {
            //if (gameState.isGameFinished)
            //    return;

            gameTime.SetTimeScale(gameState.isGamePaused ? 0f : 1f);
        }
    }
}
