using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OtHelloWorld
{
    class Game
    {
        private int[,] board;
        private List<ImageBrush> pawnsColor;
        private bool isWhite;
        private List<Player> players;

        enum Colors { black = 0, white}

        public Game ()
        {
            board = new int[8, 8];
            pawnsColor = new List<ImageBrush>();
            pawnsColor.Add(createBrushFromImage("blackPawn.png"));
            pawnsColor.Add(createBrushFromImage("whitePawn.png"));
            players = new List<Player>();
            players.Add(new Player());
            players.Add(new Player());
            isWhite = false;
            players[(int)Colors.black].StartTimer();
        }


        private ImageBrush createBrushFromImage(String filename)
        {
            ImageBrush imgBrush = new ImageBrush();
            imgBrush.ImageSource = new BitmapImage(new Uri("../../images/" + filename, UriKind.Relative));

            return imgBrush;
        }

        public ImageBrush getPawnBrush()
        {
            return isWhite ? pawnsColor[(int)Colors.white] : pawnsColor[(int)Colors.black];
        }

        public void play(int x, int y)
        {
            board[x,y] = isWhite ? (int)Colors.white : (int)Colors.black;
            isWhite = !isWhite;
            if (isWhite)
            {
                players[(int)Colors.black].StopTimer();
                //players[(int)Colors.black].Score = players[(int)Colors.black].Score + 1;
                players[(int)Colors.white].StartTimer();

            }
            else
            {
                players[(int)Colors.white].StopTimer();
                //players[(int)Colors.white].Score = players[(int)Colors.white].Score + 1;
                players[(int)Colors.black].StartTimer();
            }
        }

        public ImageBrush getBrush(int color) {
            return pawnsColor[color];
        }

        public List<Player> Players
        {
            get
            {
                return players;
            }

            set
            {
                players = value;
            }
        }

    }
}
