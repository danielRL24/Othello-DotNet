using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OtHelloWars_IA 
{
    class Game : IPlayable.IPlayable
    {
        private string teamName;
        private int[,] board;
        private List<ImageBrush> pawnsColor;
        private bool isWhite;
        private List<Player> players;
        private List<Tuple<int,int>> toReturn;

        enum Colors { black = 0, white }

        /// <summary>
        /// Default Constructor
        /// </summary>
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
            
            board[3, 3] = 0;
            board[4, 4] = 0;
            board[3, 4] = 1;
            board[4, 3] = 1;
            isWhite = false;
            init();
            players[0].StartTimer();
        }

        /// <summary>
        /// Constructor used to load a game
        /// </summary>
        /// <param name="board">The board</param>
        /// <param name="isWhite">Is white turn</param>
        /// <param name="timePlayer0">First player time</param>
        /// <param name="timePlayer1">Seconde player time</param>
        public Game(int[,] board, bool isWhite, int timePlayer0, int timePlayer1)
        {
            this.board = board;
            this.isWhite = isWhite;
            init();
            this.players[0].Time = timePlayer0;
            this.players[1].Time = timePlayer1;
            if (isWhite)
                players[1].StartTimer();
            else
                players[0].StartTimer();
        }

        /// <summary>
        /// Initialize some attributes
        /// </summary>
        private void init()
        {
            teamName = "02: Nadalin_Rodrigues";
            toReturn = new List<Tuple<int, int>>();
            pawnsColor = new List<ImageBrush>();
            pawnsColor.Add(createBrushFromImage("blackPawn.png"));
            pawnsColor.Add(createBrushFromImage("whitePawn.png"));
            players = new List<Player>();
            players.Add(new Player());
            players.Add(new Player());
            score();
        }

        /// <summary>
        /// Create a brush from a image
        /// </summary>
        /// <param name="filename">Image name</param>
        /// <returns>Created brush</returns>
        private ImageBrush createBrushFromImage(String filename)
        {
            ImageBrush imgBrush = new ImageBrush();
            imgBrush.ImageSource = new BitmapImage(new Uri("../../images/" + filename, UriKind.Relative));

            return imgBrush;
        }

        /// <summary>
        /// Get the current player brush
        /// </summary>
        /// <returns>Current player brush</returns>
        public ImageBrush GetPawnBrush()
        {
            return isWhite ? pawnsColor[(int)Colors.white] : pawnsColor[(int)Colors.black];
        }

        /// <summary>
        /// Put the pawn of current player and move to next player
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Play(int x, int y)
        {
            board[x, y] = isWhite ? (int)Colors.white : (int)Colors.black;
            isWhite = !isWhite;
            score();
            if (isWhite)
            {
                players[0].StopTimer();
                players[1].StartTimer();
            }
            else
            {
                players[1].StopTimer();
                players[0].StartTimer();
            }
        }

        /// <summary>
        /// Calculate the score
        /// </summary>
        private void score()
        {
            int scoreA = 0;
            int scoreB = 0;
            for(int i=0; i<8; i++)
            {
                for(int j=0; j<8; j++)
                {
                    if(board[i,j] == 0)
                    {
                        scoreA++;
                    }
                    else if(board[i,j] == 1)
                    {
                        scoreB++;
                    }
                }
            }
            players[0].Score = scoreA;
            players[1].Score = scoreB;
        }

        /// <summary>
        /// Check if players can play
        /// </summary>
        /// <returns></returns>
        public bool CanPlay()
        {
            bool currentPlayer = false;
            bool nextPlayer = false;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] == -1)
                    {
                        currentPlayer |= IsLegal(i, j, PawnEnemyType());
                        nextPlayer |= IsLegal(i, j, CurrentPawnType());
                    }
                }
            }
            if (currentPlayer)
            {     
                return true;
            }
            else if (nextPlayer)
            {
                isWhite = !isWhite;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get the current player pawn type
        /// </summary>
        /// <returns>Type</returns>
        public int CurrentPawnType()
        {
            return isWhite ? (int)Colors.white : (int)Colors.black;
        }

        /// <summary>
        /// Get the ennemy pawn type
        /// </summary>
        /// <returns></returns>
        public int PawnEnemyType()
        {
            return isWhite ? (int)Colors.black : (int)Colors.white;
        }

        /// <summary>
        /// Get the brush of specified color
        /// </summary>
        /// <param name="color">Specified color</param>
        /// <returns>Brush</returns>
        public ImageBrush GetBrush(int color) {
            return pawnsColor[color];
        }

        /// <summary>
        /// List of players property
        /// </summary>
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

        /// <summary>
        /// Board property
        /// </summary>
        public int[,] Board
        {
            get
            {
                return board;
            }

            set
            {
                board = value;
            }
        }

        /// <summary>
        /// If the current player is white property
        /// </summary>
        public bool IsWhite
        {
            get
            {
                return isWhite;
            }

            set
            {
                isWhite = value;
            }
        }

        /// <summary>
        /// Check if the player is in a legal position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="pawnEnemy"></param>
        /// <returns>If position is legal</returns>
        public bool IsLegal(int x, int y, int pawnEnemy)  
        {
            bool tmp = false; ;
            toReturn = new List<Tuple<int, int>>();
            
            bool result = false;

            // If the case is not empty --> is not legal
            if(board[x, y] != -1)
            {
                return false;
            }

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

        /// <summary>
        /// Check if the player can play in Y axis.
        /// Store the opposite pawn position in a list.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="pawnEnemy"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private bool playableAxisY(int x, int y,int pawnEnemy, int direction)
        {
            bool result = false;
            int i = y;
            List<Tuple<int, int>> tmp = new List<Tuple<int, int>>();
            while(i>0 && i < 8)
            {
                tmp.Add(new Tuple<int, int>(x,i));
                if(board[x,i] != pawnEnemy)
                {
                    result = board[x, i] != -1 ? true : false;
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

        /// <summary>
        /// Check if the player can play in X axis.
        /// Store the opposite pawn position in a list.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="pawnEnemy"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private bool playableAxisX(int x, int y, int pawnEnemy, int direction)
        {
            bool result = false;
            List<Tuple<int, int>> tmp = new List<Tuple<int, int>>();
            int i = x;
            while (i >= 0 && i < 8) 
            {
                tmp.Add(new Tuple<int, int>(i, y));

                if (board[i, y] != pawnEnemy)
                {
                    result = board[i, y] != -1 ? true : false;
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

        /// <summary>
        /// Check if the player can play in first diagonal.
        /// Store the opposite pawn position in a list.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="pawnEnemy"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private bool playableDiag1(int x, int y, int pawnEnemy, int direction)
        {
            bool result = false;
            int i = x;
            List<Tuple<int, int>> tmp = new List<Tuple<int, int>>();
            int j = y;
            while ((i > 0 && i < 8)&&(j>0 && j<8))
            {
                tmp.Add(new Tuple<int, int>(i, j));

                if (board[i, j] != pawnEnemy)
                {
                    result = board[i, j] != -1 ? true : false;
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

        /// <summary>
        /// Check if the player can play in seconde diagonal.
        /// Store the opposite pawn position in a list.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="pawnEnemy"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private bool playableDiag2(int x, int y, int pawnEnemy, int direction)
        {
            bool result = false;
            int i = x;
            List<Tuple<int, int>> tmp = new List<Tuple<int, int>>();
            int j = y;
            while ((i > 0 && i < 8)&& (j > 0 && j < 8))
            {
                tmp.Add(new Tuple<int, int>(i, j));

                if (board[i, j] != pawnEnemy)
                {
                    result = board[i, j] != -1 ? true : false;
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

        /// <summary>
        /// Change pawns
        /// </summary>
        /// <returns></returns>
        public List<Tuple<int,int>> ReturnPawn()
        {
            int pawnType = isWhite ? (int)Colors.white : (int)Colors.black;
            foreach (Tuple<int,int> tuple in toReturn)
            {
                board[tuple.Item1, tuple.Item2] = pawnType;
            }
            return toReturn;
        }


        /// 
        /// INTERFACE IPlayable
        ///

        public string GetName()
        {
            return teamName;
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            this.isWhite = isWhite;
            return IsLegal(column, line, isWhite ? (int)Colors.black : (int)Colors.white);
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            this.isWhite = isWhite;
            bool result = IsLegal(column, line, isWhite ? (int)Colors.black : (int)Colors.white);
            if (result)
            {
                ReturnPawn();
                Play(column, line);
            }

            return result;
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            // TODO ALPHA-BETA
            throw new NotImplementedException();
        }

        public int[,] GetBoard()
        {
            return board;
        }

        public int GetWhiteScore()
        {
            return players[0].Score;
        }

        public int GetBlackScore()
        {
            return players[1].Score;
        }

        ///
        /// IA
        ///

        private Tuple<int, Tuple<int, int>> AlphaBeta(int[,] game, int level, bool whiteTurn, int parentValue)
        {
            int minOrMax = whiteTurn ? 1 : -1;
            if(level == 0 || )
            {
                // TODO
            }
            int optVal = minOrMax * -1;
            Tuple<int, int> optOp = null;

            foreach(Tuple<int, int> op in ValideOp(whiteTurn))
            {
                int[,] newBoard = ApplyOp(op, whiteTurn);
                Tuple<int, Tuple<int, int>> result = AlphaBeta(newBoard, level - 1, !whiteTurn, optVal);
                if (result.Item1 * minOrMax > optVal * minOrMax)
                {
                    optVal = result.Item1;
                    optOp = op;
                    if(optVal * minOrMax > parentValue * minOrMax)
                    {
                        break;
                    }
                }
            }

            return new Tuple<int, Tuple<int, int>>(optVal, optOp);

        }

        private List<Tuple<int, int>> ValideOp(bool whiteTurn)
        {
            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            for(int i=0; i < 8; i++)
            {
                if(IsLegal(i, j, whiteTurn ? (int)Colors.black : (int)Colors.white))
                {
                    list.Add(new Tuple<int, int>(i, j));
                }
            }

            return list;
        }

        private int[,] ApplyOp(Tuple<int, int> op, bool whiteTurn)
        {
            int[,] newBoard = board;

            newBoard[op.Item1, op.Item2] = whiteTurn ? 1 : 0 ;

            return newBoard;
        }
    }
}
