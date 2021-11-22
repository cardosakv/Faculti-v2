using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faculti.Helpers
{
    class Randomizer
    {
        public static int GenerateCode(int start, int end)
        {
            Random randomizer = new();
            return randomizer.Next(start, end);
        }
    }
}
