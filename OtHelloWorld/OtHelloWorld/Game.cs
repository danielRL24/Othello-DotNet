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
        private List<Player> players;
        private List<Tuple<int,int>> toReturn;

        enum Colors { black = 0, white }

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
            toReturn = new List<Tuple<int,int>>();
            pawnsColor = new List<ImageBrush>();
            pawnsColor.Add(createBrushFromImage("blackPawn.png"));
            pawnsColor.Add(createBrushFromImage("whitePawn.png"));
            players = new List<Player>();
            players.Add(new Player());
            players.Add(new Player());
            board[3, 3] = 0;
            board[4, 4] = 0;
            board[3, 4] = 1;
            board[4, 3] = 1;
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

        public void Play(int x, int y)
        {
            board[x,y] = isWhite ? (int)Colors.white : (int)Colors.black;
            isWhite = !isWhite;
        }

        public ImageBrush GetBrush(int color) {
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

        public bool IsLegal(int x, int y)  
        {
            bool tmp = false; ;
            toReturn = new List<Tuple<int, int>>();
            int pawnEnemy = isWhite ? (int)Colors.black : (int)Colors.white;
            bool result = false;
            if (y + 1 <8 && board[x, y + 1] == pawnEnemy) //sud
            {
                tmp = playableAxisY(x, y + 1, pawnEnemy, 1);
                result |= tmp;
            }
            if (x - 1 >= 0 && y + 1 <8 && board[x - 1, y + 1] == pawnEnemy) //sud ouest
            {
                tmp = playableDiag1(x - 1, y + 1, pawnEnemy, 1);
                result |= tmp;
            }
            if (x - 1 >= 0 && board[x - 1, y] == pawnEnemy) // ouest
            {
                tmp = playableAxisX(x - 1, y, pawnEnemy, -1);
                result |= tmp;
            }
            if (x - 1 >= 0 && y - 1 >= 0 && board[x - 1, y - 1] == pawnEnemy) // nord ouest
            {
                tmp = playableDiag2(x - 1, y - 1, pawnEnemy, -1);
                result |= tmp;
            }
            if (y - 1 >= 0 && board[x, y - 1] == pawnEnemy) //nord
            {
                tmp = playableAxisY(x, y - 1, pawnEnemy, -1);
                result |= tmp;
            }
            if (x + 1 <8 && y - 1 >= 0 && board[x + 1, y - 1] == pawnEnemy) //nord est
            {
                tmp = playableDiag1(x + 1, y - 1, pawnEnemy, -1);
                result |= tmp;
            }
            if (x + 1 <8 && board[x + 1, y] == pawnEnemy) // est
            {
                tmp = playableAxisX(x + 1, y, pawnEnemy, 1);
                result |= tmp;
            }
            if (x + 1 <8 && y + 1 <8 && board[x + 1, y + 1] == pawnEnemy) //sud est
            {
                tmp = playableDiag2(x + 1, y + 1, pawnEnemy, 1);
                result |= tmp;
            }
            return result;          
        }

        
        private bool playableAxisY(int x, int y,int pawnEnemy, int direction)
        {
            bool result = false;
            int i = y;
            List<Tuple<int, int>> tmp = new List<Tuple<int, int>>();
            while(i>0 && i < 8)
            {
                tmp.Add(new Tuple<int, int>(x,i));
                if(board[x,i] != pawnEnemy && board[x,i] != -1)
                {
                    result = true;
                    break;
                }
                i += direction;
            }
            if(result == true)
            {
               toReturn = toReturn.Concat(tmp).ToList();
            }
            return result; 
        }

        private bool playableAxisX(int x, int y, int pawnEnemy, int direction)
        {
            bool result = false;
            List<Tuple<int, int>> tmp = new List<Tuple<int, int>>();
            int i = x;
            while (i >= 0 && i < 8) 
            {
                tmp.Add(new Tuple<int, int>(i, y));

                if (board[i, y] != pawnEnemy && board[i, y] != -1)
                {
                    result = true;
                    break;
                }
                i += direction;
            }
            if (result == true)
            {
                toReturn = toReturn.Concat(tmp).ToList();
            }
            return result;
        }

        private bool playableDiag1(int x, int y, int pawnEnemy, int direction)
        {
            bool result = false;
            int i = x;
            List<Tuple<int, int>> tmp = new List<Tuple<int, int>>();
            int j = y;
            while ((i > 0 && i < 8)&&(j>0 && j<8))
            {
                tmp.Add(new Tuple<int, int>(i, j));

                if (board[i, j] != pawnEnemy && board[i, j] != -1)
                {
                    result = true;
                    break;
                }
                i -= direction;
                j += direction;
            }
            if (result == true)
            {
                toReturn = toReturn.Concat(tmp).ToList();
            }
            return result;
        }

        private bool playableDiag2(int x, int y, int pawnEnemy, int direction)
        {
            bool result = false;
            int i = x;
            List<Tuple<int, int>> tmp = new List<Tuple<int, int>>();
            int j = y;
            while ((i > 0 && i < 8)&& (j > 0 && j < 8))
            {
                tmp.Add(new Tuple<int, int>(i, j));

                if (board[i, j] != pawnEnemy && board[i, j] != -1)
                {
                    result = true;
                    break;
                }
                i += direction;
                j += direction;
            }
            if (result == true)
            {
                toReturn = toReturn.Concat(tmp).ToList();
            }
            return result;
        }

        public List<Tuple<int,int>> ReturnPawn()
        {
            int pawnType = isWhite ? (int)Colors.white : (int)Colors.black;
            foreach (Tuple<int,int> tuple in toReturn)
            {
                board[tuple.Item1, tuple.Item2] = pawnType;
            }
            return toReturn;
        }

        
    }
}
