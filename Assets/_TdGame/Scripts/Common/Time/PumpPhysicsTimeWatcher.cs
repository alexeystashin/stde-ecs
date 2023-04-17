using UnityEngine;

namespace Pump.Unity
{
    public class PumpPhysicsTimeWatcher : MonoBehaviour
    {
        float timer;

        void Start()
        {
            Physics2D.simulationMode = SimulationMode2D.Script;
        }

        void Update()
        {
            if (Physics2D.simulationMode != SimulationMode2D.Script)
                return; // do nothing if the automatic simulation is enabled

            if(PumpTime.fixedDeltaTime == 0)
            {
                timer = 0;
                return;
            }

            timer += PumpTime.deltaTime;

            var fixedDeltaTime = PumpTime.fixedDeltaTime;

            while (timer >= fixedDeltaTime)
            {
                timer -= fixedDeltaTime;
                Physics2D.Simulate(fixedDeltaTime);
            }
        }

        void OnDestroy()
        {
            Physics2D.simulationMode = SimulationMode2D.Update;
        }
    }
}
