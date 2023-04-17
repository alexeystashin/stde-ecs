using UnityEngine;

namespace Pump.Unity
{
    public class TweenUtils
    {
        // all values should be in range [0..1]

        public static float EaseInQuad(float value)
        {
            return value * value;
        }

        public static float EaseOutQuad(float value)
        {
            return -1 * value * (value - 2);
        }

        public static float EaseInOutQuad(float value)
        {
            value /= .5f;
            if (value < 1) return 0.5f * value * value;
            value--;
            return -0.5f * (value * (value - 2) - 1);
        }

        public static float EaseInCubic(float value)
        {
            return value * value * value;
        }

        public static float EaseOutCubic(float value)
        {
            value--;
            return (value * value * value + 1);
        }

        public static float EaseInOutCubic(float value)
        {
            value /= .5f;
            if (value < 1) return 0.5f * value * value * value;
            value -= 2;
            return 0.5f * (value * value * value + 2);
        }

        public static float EaseInQuart(float value)
        {
            return value * value * value * value;
        }

        public static float EaseOutQuart(float value)
        {
            value--;
            return -1 * (value * value * value * value - 1);
        }

        public static float EaseInOutQuart(float value)
        {
            value /= .5f;
            if (value < 1) return 0.5f * value * value * value * value;
            value -= 2;
            return -0.5f * (value * value * value * value - 2);
        }

        public static float EaseInQuint(float value)
        {
            return value * value * value * value * value;
        }

        public static float EaseOutQuint(float value)
        {
            value--;
            return (value * value * value * value * value + 1);
        }

        public static float EaseInOutQuint(float value)
        {
            value /= .5f;
            if (value < 1) return 0.5f * value * value * value * value * value;
            value -= 2;
            return 0.5f * (value * value * value * value * value + 2);
        }

        public static float EaseInSine(float value)
        {
            return -Mathf.Cos(value * (Mathf.PI * 0.5f)) + 1;
        }

        public static float EaseOutSine(float value)
        {
            return Mathf.Sin(value * (Mathf.PI * 0.5f));
        }

        public static float EaseInOutSine(float value)
        {
            return -0.5f * (Mathf.Cos(Mathf.PI * value) - 1);
        }

        public static float EaseInExpo(float value)
        {
            return Mathf.Pow(2, 10 * (value - 1));
        }

        public static float EaseOutExpo(float value)
        {
            return -Mathf.Pow(2, -10 * value) + 1;
        }

        public static float EaseInOutExpo(float value)
        {
            value /= .5f;
            if (value < 1) return 0.5f * Mathf.Pow(2, 10 * (value - 1));
            value--;
            return 0.5f * (-Mathf.Pow(2, -10 * value) + 2);
        }

        public static float EaseInCirc(float value)
        {
            return -1 * (Mathf.Sqrt(1 - value * value) - 1);
        }

        public static float EaseOutCirc(float value)
        {
            value--;
            return Mathf.Sqrt(1 - value * value);
        }

        public static float EaseInOutCirc(float value)
        {
            value /= .5f;
            if (value < 1) return -0.5f * (Mathf.Sqrt(1 - value * value) - 1);
            value -= 2;
            return 0.5f * (Mathf.Sqrt(1 - value * value) + 1);
        }

        public static float EaseInBounce(float value)
        {
            return 1 - EaseOutBounce(1 - value);
        }

        public static float EaseOutBounce(float value)
        {
            value /= 1f;
            if (value < (1 / 2.75f))
            {
                return (7.5625f * value * value);
            }
            else if (value < (2 / 2.75f))
            {
                value -= (1.5f / 2.75f);
                return (7.5625f * (value) * value + .75f);
            }
            else if (value < (2.5 / 2.75))
            {
                value -= (2.25f / 2.75f);
                return (7.5625f * (value) * value + .9375f);
            }
            else
            {
                value -= (2.625f / 2.75f);
                return (7.5625f * (value) * value + .984375f);
            }
        }

        public static float EaseInOutBounce(float value)
        {
            if (value < 0.5f) return EaseInBounce(value * 2) * 0.5f;
            else return EaseOutBounce(value * 2 - 1) * 0.5f + 0.5f;
        }

        public static float EaseInBack(float value)
        {
            value /= 1;
            float s = 1.70158f;
            return (value) * value * ((s + 1) * value - s);
        }

        public static float EaseOutBack(float value)
        {
            float s = 1.70158f;
            value = (value) - 1;
            return ((value) * value * ((s + 1) * value + s) + 1);
        }

        public static float EaseInOutBack(float value)
        {
            float s = 1.70158f;
            value /= .5f;
            if ((value) < 1)
            {
                s *= (1.525f);
                return 0.5f * (value * value * (((s) + 1) * value - s));
            }
            value -= 2;
            s *= (1.525f);
            return 0.5f * ((value) * value * (((s) + 1) * value + s) + 2);
        }

        public static float EaseInElastic(float value)
        {
            float d = 1f;
            float p = d * .3f;
            float s = 0;
            float a = 0;

            if (value == 0) return 0;

            if ((value /= d) == 1) return 1;

            if (a == 0f || a < Mathf.Abs(1))
            {
                a = 1;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(1 / a);
            }

            return -(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p));
        }

        public static float EaseOutElastic(float value)
        {
            float d = 1f;
            float p = d * .3f;
            float s = 0;
            float a = 0;

            if (value == 0) return 0;

            if ((value /= d) == 1) return 1;

            if (a == 0f || a < Mathf.Abs(1))
            {
                a = 1;
                s = p * 0.25f;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(1 / a);
            }

            return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + 1);
        }

        public static float EaseInOutElastic(float value)
        {
            float d = 1f;
            float p = d * .3f;
            float s = 0;
            float a = 0;

            if (value == 0) return 0;

            if ((value /= d * 0.5f) == 2) return 1;

            if (a == 0f || a < Mathf.Abs(1))
            {
                a = 1;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(1 / a);
            }

            if (value < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p));
            return a * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + 1;
        }

        public static float Linear(float value)
        {
            return value;
        }

        // first half (val=0..0.5) result is increasing (0 to 1); senond half (val=0.5..1) result is decreasing (1 to 0)
        public static float PingPong(float value)
        {
            value = Mathf.Clamp01(value);
            if (value <= 0.5f)
                return value * 2;

            return 1-(value-0.5f)*2; // 0..0.5
        }

        public static float Spring(float value)
        {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            return value;
        }
    }
}
