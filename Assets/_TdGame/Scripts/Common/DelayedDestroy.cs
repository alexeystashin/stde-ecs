using UnityEngine;

namespace Common
{
    public class DelayedDestroy : MonoBehaviour
    {
        public float time;

        void Update()
        {
            time -= Time.deltaTime;
            if(time <= 0)
                GameObject.Destroy(gameObject);
        }
    }
}
