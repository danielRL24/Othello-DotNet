﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtHelloWorld
{
    class Board
    {
        private Pawn[,] pawns;

        public Board ()
        {
            this.pawns = new Pawn[8, 8];
        }
    }
}
