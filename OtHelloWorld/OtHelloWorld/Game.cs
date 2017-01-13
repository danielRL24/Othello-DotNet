using System;
using System.Collections.Generic;
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

        enum Colors { white = 0, black }

        public Game ()
        {
            board = new int[8, 8];
            pawnsColor = new List<ImageBrush>();
            pawnsColor.Add(createBrushFromImage("whitePawn.png"));
            pawnsColor.Add(createBrushFromImage("blackPawn.png"));
            isWhite = false;
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
        }

        public ImageBrush getBrush(int color) {
            return pawnsColor[color];
        }

    }
}
