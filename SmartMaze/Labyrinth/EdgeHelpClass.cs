using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Labyrinth
{
    class EdgeHelpClass
    {
        public int a;
        public string b;

        public EdgeHelpClass(int aa, string bb)
        {
            a = aa;
            b = bb;
        }

        public EdgeHelpClass(string bb, int aa)
        {
            b = bb;
            a = aa;
        }
    }
}
