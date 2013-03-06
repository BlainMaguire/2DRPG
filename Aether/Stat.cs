using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aether
{
    class Stat
    {
        public int maxValue;
        public int currentValue;

        public Stat(int maxValue, int currentValue)
        {
            this.maxValue = maxValue;
            this.currentValue = currentValue;
        }

    }
}
