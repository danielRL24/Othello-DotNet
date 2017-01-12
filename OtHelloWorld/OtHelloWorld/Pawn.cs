using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtHelloWorld
{
    class Pawn
    {
        private int color;
        private int x;
        private int y;

        public int Color { get { return this.color; } set { this.color = value; } }

        public int X { get { return this.x; } set { this.x = value; } }
        public int Y { get { return this.y; } set { this.y = value; } }

    }
}
