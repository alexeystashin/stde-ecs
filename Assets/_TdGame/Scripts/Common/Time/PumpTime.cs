using UnityEngine;
using UnityEngine.Assertions;

namespace Pump.Unity
{
    public static class PumpTime
    {
        public static float time => customTime != null ? customTime.time : Time.time;
        public static float timeScale => customTime != null ? customTime.timeScale : Time.timeScale;
        public static float deltaTime => customTime != null ? customTime.deltaTime : Time.deltaTime;
        public static float fixedDeltaTime => customTime != null ? customTime.fixedDeltaTime : Time.fixedDeltaTime;

        private static ITime customTime;

        public static void SetCustomTime(ITime time)
        {
            Assert.IsTrue(time==null || customTime == null);

            if (customTime == time)
                return;

            customTime = time;
        }
    }
}
