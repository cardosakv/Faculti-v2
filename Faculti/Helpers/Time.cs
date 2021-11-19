using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faculti.Helpers
{
    public class Time
    {
        public static string ToElapsed(DateTime time)
        {
            TimeSpan lastSpan = DateTime.Now - time;
            double lastUpdate = lastSpan.TotalSeconds / 60;
            string lastUpdateString;

            if (lastUpdate >= 1440)
            {
                lastUpdate /= 1440;
                lastUpdateString = $"{Convert.ToInt32(lastUpdate)}d";
            }
            else if (lastUpdate >= 60)
            {
                lastUpdate /= 60;
                lastUpdateString = $"{Convert.ToInt32(lastUpdate)}h";
            }
            else if (Convert.ToInt32(lastUpdate) == 0)
            {
                lastUpdateString = "Now";
            }
            else
            {
                lastUpdateString = $"{Convert.ToInt32(lastUpdate)}m";
            }

            return lastUpdateString;
        }
    }
}
