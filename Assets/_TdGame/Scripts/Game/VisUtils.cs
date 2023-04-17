using System;
using System.Collections.Generic;
using UnityEngine;

namespace TdGame
{
    public static class VisUtils
    {
        public static string FormatTime(int totalSeconds)
        {
            var timeSpan = TimeSpan.FromSeconds(totalSeconds);

            var days = (int)Math.Floor(timeSpan.TotalDays);
            var hours = timeSpan.Hours;
            var minutes = timeSpan.Minutes;
            var seconds = timeSpan.Seconds;

            if (totalSeconds == 0)
                return "00:00";

            string resultStr;

            if (days > 0)
                resultStr = string.Format("{0}:{1}", days, hours);
            else if (hours > 0)
                resultStr = string.Format("{0}:{1}", hours, minutes);
            else if (minutes > 0)
                resultStr = string.Format("{0}:{1}", minutes.ToString("00"), seconds.ToString("00"));
            else
                resultStr = "00:" + seconds.ToString("00");

            return resultStr;
        }
    }
}
