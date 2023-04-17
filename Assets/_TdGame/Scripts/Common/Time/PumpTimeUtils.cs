using UnityEngine;

namespace Pump.Unity
{
    public static class PumpTimeUtils
    {
        public static void AddTimeWatcher(Transform tr, bool addToChildren = true)
        {
            if (tr.GetComponent<PumpTimeWatcher>() == null)
            {
                if(tr.GetComponent<Animator>() != null || tr.GetComponent<ParticleSystem>() != null)
                    tr.gameObject.AddComponent<PumpTimeWatcher>();
            }

            if (addToChildren)
            {
                for (var i = 0; i < tr.childCount; i++)
                {
                    AddTimeWatcher(tr.GetChild(i));
                }
            }
        }
    }
}
