using UnityEngine;

namespace Pump.Unity
{
    public interface ITime
    {
        float time { get; }
        float timeScale { get; }
        float deltaTime { get; }
        float fixedDeltaTime { get; }
    }
}
