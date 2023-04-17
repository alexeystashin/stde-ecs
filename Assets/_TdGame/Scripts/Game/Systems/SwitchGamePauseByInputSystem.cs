using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace TdGame
{
    sealed class SwitchGamePauseByInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        GameState gameState;

        bool isPauseKeyPressed;

        [Inject]
        void Construct(GameState gameState)
        {
            this.gameState = gameState;
        }

        public void Init(IEcsSystems systems)
        {
        }

        public void Run(IEcsSystems systems)
        {
            if (gameState.isGameFinished)
                return;

            var newIsPauseKeyPressed = Input.GetKey(KeyCode.Space);
            if (isPauseKeyPressed != newIsPauseKeyPressed)
            {
                isPauseKeyPressed = newIsPauseKeyPressed;

                gameState.gamePauseCounter += isPauseKeyPressed ? 1 : -1;
            }
        }
    }
}
