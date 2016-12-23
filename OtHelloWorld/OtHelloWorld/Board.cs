using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtHelloWorld
{
    class Board
    {
        private TileUserControl[,] tiles;

        public Board ()
        {
            tiles = new TileUserControl[8, 8];
        }

        public void addTilesToBoard(TileUserControl tile)
        {
            this.tiles[tile.Col, tile.Row] = tile;
        }
    }
}
