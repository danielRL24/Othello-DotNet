using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OtHelloWorld
{
    class Board
    {
        private static int[,] pawns;
        private static List<ImageBrush> pawnsColor;
        private static bool isWhite;

        enum Colors { white = 0, black }

        public Board ()
        {
            pawns = new int[8, 8];
            pawnsColor = new List<ImageBrush>();
            pawnsColor.Add(createBrushFromImage("whitePawn.png"));
            pawnsColor.Add(createBrushFromImage("blackPawn.png"));
            isWhite = true;
        }


        private ImageBrush createBrushFromImage(String filename)
        {
            ImageBrush imgBrush = new ImageBrush();
            imgBrush.ImageSource = new BitmapImage(new Uri("../../images/" + filename, UriKind.Relative));

            return imgBrush;
        }

        public static ImageBrush getPawnBrush()
        {
            return isWhite ? pawnsColor[(int)Colors.white] : pawnsColor[(int)Colors.black];
        }

        public static void play(int x, int y)
        {
            pawns[x,y] = isWhite ? (int)Colors.white : (int)Colors.black;
            isWhite = !isWhite;
        }

        public ImageBrush getBrush(int color) {
            return pawnsColor[color];
        }

    }
}
