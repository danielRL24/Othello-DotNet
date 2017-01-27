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

        enum Colors { black = 0, white }
        Dictionary<String, bool> directions;

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
            directions.Add("Sud", true);
            directions.Add("SudOuest", true);
            directions.Add("Ouest", true);
            directions.Add("NordOuest", true);
            directions.Add("Nord", true);
            directions.Add("NordEst", true);
            directions.Add("Est", true);
            directions.Add("SudEst", true);
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

        private void initDirections()
        {
            directions["Sud"] = true;
            directions["SudOuest"] = true;
            directions["Ouest"] = true;
            directions["NordOuest"] = true;
            directions["Nord"] = true;
            directions["NordEst"] = true;
            directions["Est"] = true;
            directions["SudEst"] = true;
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
            initDirections();
            bool tmp = false; ;
            int pawnEnemy = isWhite ? (int)Colors.black : (int)Colors.white;
            bool result = false;
            if (y + 1 <8 && board[x, y + 1] == pawnEnemy) //sud
            {
                tmp = playableAxisY(x, y + 1, pawnEnemy, 1);
                directions["Sud"] = tmp;
                result |= tmp;
            }
            if (x - 1 >= 0 && y + 1 <8 && board[x - 1, y + 1] == pawnEnemy) //sud ouest
            {
                tmp = playableDiag1(x - 1, y + 1, pawnEnemy, 1);
                directions["SudOuest"] = tmp;
                result |= tmp;
            }
            if (x - 1 >= 0 && board[x - 1, y] == pawnEnemy) // ouest
            {
                tmp = playableAxisX(x - 1, y, pawnEnemy, -1);
                directions["Ouest"] = tmp;
                result |= tmp;
            }
            if (x - 1 >= 0 && y - 1 >= 0 && board[x - 1, y - 1] == pawnEnemy) // nord ouest
            {
                tmp = playableDiag2(x - 1, y - 1, pawnEnemy, -1);
                directions["NordOuest"] = tmp;
                result |= tmp;
            }
            if (y - 1 >= 0 && board[x, y - 1] == pawnEnemy) //nord
            {
                tmp = playableAxisY(x, y - 1, pawnEnemy, -1);
                directions["Nord"] = tmp;
                result |= tmp;
            }
            if (x + 1 <8 && y - 1 >= 0 && board[x + 1, y - 1] == pawnEnemy) //nord est
            {
                tmp = playableDiag1(x + 1, y - 1, pawnEnemy, -1);
                directions["NordEst"] = tmp;
                result |= tmp;
            }
            if (x + 1 <8 && board[x + 1, y] == pawnEnemy) // est
            {
                tmp = playableAxisX(x + 1, y, pawnEnemy, 1);
                directions["Est"] = tmp;
                result |= tmp;
            }
            if (x + 1 <8 && y + 1 <8 && board[x + 1, y + 1] == pawnEnemy) //sud est
            {
                tmp = playableDiag2(x + 1, y + 1, pawnEnemy, 1);
                directions["SudEst"] = tmp;
                result |= tmp;
            }
            return result;          
        }

        
        private bool playableAxisY(int x, int y,int pawnEnemy, int direction)
        {
            bool result = false;
            int i = y;
            while(i>0 && i < 8)
            {
                if(board[x,i] != pawnEnemy && board[x,i] != -1)
                {
                    result = true;
                    break;
                }
                i += direction;
            }
            return result; 
        }

        private bool playableAxisX(int x, int y, int pawnEnemy, int direction)
        {
            bool result = false;
            int i = x;
            while (i >= 0 && i < 8) 
            {
                if (board[i, y] != pawnEnemy && board[i, y] != -1)
                {
                    result = true;
                    break;
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
            while ((i > 0 && i < 8)&&(j>0 && j<8))
            {
                if (board[i, j] != pawnEnemy && board[i, j] != -1)
                {
                    result = true;
                    break;
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
            while ((i > 0 && i < 8)&& (j > 0 && j < 8))
            {
                if (board[i, j] != pawnEnemy && board[i, j] != -1)
                {
                    result = true;
                    break;
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
