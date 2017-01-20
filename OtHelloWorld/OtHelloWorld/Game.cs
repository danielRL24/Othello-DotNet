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
        private int[,] pawnToReturn;

        enum Colors { white = 0, black }
        //enum Directions { east, northEast, north, northWest, west, southWest, south, southEast }

        public Game ()
        {
            board = new int[8, 8];
            for(int i=0; i<8;i++)
            {
                for(int j=0; j<8; j++)
                {
                    board[i, j] = -1;
                }
            }
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

        public ImageBrush GetPawnBrush()
        {
            return isWhite ? pawnsColor[(int)Colors.white] : pawnsColor[(int)Colors.black];
        }

        public void play(int x, int y)
        {
            board[x,y] = isWhite ? (int)Colors.white : (int)Colors.black;
            isWhite = !isWhite;
        }

        public ImageBrush GetBrush(int color) {
            return pawnsColor[color];
        }

        public bool IsLegal(int x, int y)  
        {
            int pawnEnemy = isWhite ? (int)Colors.black : (int)Colors.white;
            bool result = false;
            if (y - 1 >= 0 && board[x, y - 1] == pawnEnemy) //sud
            {
                result = playableAxisY(x, y - 1, pawnEnemy, 1);
            }
            if(x - 1 >= 0 && y - 1 >= 0 && board[x - 1, y - 1] == pawnEnemy) //sud ouest
            {
                Console.Write("-> sud ouest");
                result = playableDiag1(x - 1, y - 1, pawnEnemy, 1);
            }
            if (x - 1 >= 0 && board[x - 1, y] == pawnEnemy) // ouest
            {
                result = playableAxisX(x - 1, y, pawnEnemy, -1);
            }
            if (x - 1 >= 0 && y + 1 <= 7 && board[x - 1, y + 1] == pawnEnemy) // nord ouest
            {
                result = playableDiag2(x - 1, y + 1, pawnEnemy, -1);
            }
            if (y + 1 <= 7 && board[x, y + 1] == pawnEnemy) //nord
            {
                result = playableAxisY(x, y + 1, pawnEnemy, -1);
            }
            if (x + 1 <= 7 && y + 1 <= 7 && board[x + 1, y + 1] == pawnEnemy) //nord est
            {
                result = playableDiag1(x + 1, y + 1, pawnEnemy, -1); // c'est ici que ca passe pas!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
            if (x + 1 <= 7 && board[x + 1, y] == pawnEnemy) // est
            {
                result = playableAxisX(x + 1, y, pawnEnemy, 1);
            }
            if (x + 1 <= 7 && y - 1 >= 0 && board[x + 1, y - 1] == pawnEnemy) //sud est
            {
                result = playableDiag2(x + 1, y - 1, pawnEnemy, 1);
            }
            return result;          
        }

        
        private bool playableAxisY(int x, int y,int pawnEnemy, int direction)
        {
            bool result = false;
            int i = y;
            while((i>0 && i < 8) || result == true)
            {
                if(board[x,i] != pawnEnemy && board[x,i] != -1)
                {
                    result = true;
                }
                i += direction;
            }
            return result; 
        }

        private bool playableAxisX(int x, int y, int pawnEnemy, int direction)
        {
            bool result = false;
            int i = x;
            while ((i > 0 && i < 8) || result == true)
            {
                if (board[i, y] != pawnEnemy && board[i, y] != -1)
                {
                    result = true;
                }
                i += direction;
            }
            return result;
        }

        private bool playableDiag1(int x, int y, int pawnEnemy, int direction)
        {
            bool result = false;
            int i = x;
            int j = y;
            while ((i > 0 && i < 8)||(j>0 && j<8)|| result == true)
            {
                if (board[i, j] != pawnEnemy && board[i, j] != -1)
                {
                    result = true;
                }
                i -= direction;
                j += direction;
            }
            return result;
        }

        private bool playableDiag2(int x, int y, int pawnEnemy, int direction)
        {
            bool result = false;
            int i = x;
            int j = y;
            while ((i > 0 && i < 8) || (j > 0 && j < 8) || result == true)
            {
                if (board[i, j] != pawnEnemy && board[i, j] != -1)
                {
                    result = true;
                }
                i += direction;
                j += direction;
            }
            return result;
        }

        public void returnPawn()
        {

        }

        
    }
}
