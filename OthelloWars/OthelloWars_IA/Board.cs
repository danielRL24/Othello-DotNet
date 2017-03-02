using System;
using System.Collections.Generic;
using System.Linq;

namespace OtHelloWars_IA 
{
    class Board : IPlayable.IPlayable
    {
        private string teamName;
        private int[,] board;
        private bool isWhite;
        private List<Player> players;
        private List<Tuple<int,int>> toReturn;
        private bool ISLEGAL;

        enum Colors { white = 0, black }

        private int[,] boardScore = new int[,] { {120, -20, 20, 5, 5, 20, -20, 120},
                                                 {-20, -40, -5, -5, -5, -5, -40, -20 },
                                                 { 20, -5, 15, 3, 3, 15, -5, 20},
                                                 { 5, -5, 3, 3, 3, 3, -5, 5},
                                                 { 5, -5, 3, 3, 3, 3, -5, 5},
                                                 { 20, -5, 15, 3, 3, 15, -5, 20},
                                                 {-20, -40, -5, -5, -5, -5, -40, -20 },
                                                 {120, -20, 20, 5, 5, 20, -20, 120}};


        /// <summary>
        /// Default Constructor
        /// </summary>
        public Board ()
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

            ISLEGAL = false;
        }

        /// <summary>
        /// Constructor used to load a game
        /// </summary>
        /// <param name="board">The board</param>
        /// <param name="isWhite">Is white turn</param>
        /// <param name="timePlayer0">First player time</param>
        /// <param name="timePlayer1">Seconde player time</param>
        public Board(int[,] board, bool isWhite, int timePlayer0, int timePlayer1)
        {
            this.board = board;
            this.isWhite = isWhite;
            init();
            this.players[0].Time = timePlayer0;
            this.players[1].Time = timePlayer1;
            if (isWhite)
                players[0].StartTimer();
            else
                players[1].StartTimer();
        }

        /// <summary>
        /// Initialize some attributes
        /// </summary>
        private void init()
        {
            teamName = "02: Nadalin_Rodrigues";
            toReturn = new List<Tuple<int, int>>();
            players = new List<Player>();
            players.Add(new Player());
            players.Add(new Player());
            Score(board);
        }

        /// <summary>
        /// Put the pawn of current player and move to next player
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Play(int x, int y)
        {
            ReturnPawn(board, isWhite);
            board[x, y] = isWhite ? (int)Colors.white : (int)Colors.black;
            isWhite = !isWhite;
            Score(board);
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
        private void Score(int[,] game)
        {
            int scoreA = 0;
            int scoreB = 0;
            for(int i=0; i<8; i++)
            {
                for(int j=0; j<8; j++)
                {
                    if(game[i,j] == 0)
                    {
                        scoreA++;
                    }
                    else if(game[i,j] == 1)
                    {
                        scoreB++;
                    }
                }
            }
            players[0].Score = scoreA;
            players[1].Score = scoreB;
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
        public bool IsLegal(int[,]game, int x, int y, int pawnEnemy)  
        {
            bool tmp = false; ;
            toReturn = new List<Tuple<int, int>>();
            
            bool result = false;

            // If the case is not empty --> is not legal
            if(game[x, y] != -1)
            {
                return false;
            }

            if (y + 1 <8 && game[x, y + 1] == pawnEnemy) //sud
            {
                tmp = playableAxisY(game, x, y + 1, pawnEnemy, 1);
                result |= tmp;
            }
            if (x - 1 >= 0 && y + 1 <8 && game[x - 1, y + 1] == pawnEnemy) //sud ouest
            {
                tmp = playableDiag1(game, x - 1, y + 1, pawnEnemy, 1);
                result |= tmp;
            }
            if (x - 1 >= 0 && game[x - 1, y] == pawnEnemy) // ouest
            {
                tmp = playableAxisX(game, x - 1, y, pawnEnemy, -1);
                result |= tmp;
            }
            if (x - 1 >= 0 && y - 1 >= 0 && game[x - 1, y - 1] == pawnEnemy) // nord ouest
            {
                tmp = playableDiag2(game, x - 1, y - 1, pawnEnemy, -1);
                result |= tmp;
            }
            if (y - 1 >= 0 && game[x, y - 1] == pawnEnemy) //nord
            {
                tmp = playableAxisY(game, x, y - 1, pawnEnemy, -1);
                result |= tmp;
            }
            if (x + 1 <8 && y - 1 >= 0 && game[x + 1, y - 1] == pawnEnemy) //nord est
            {
                tmp = playableDiag1(game, x + 1, y - 1, pawnEnemy, -1);
                result |= tmp;
            }
            if (x + 1 <8 && game[x + 1, y] == pawnEnemy) // est
            {
                tmp = playableAxisX(game, x + 1, y, pawnEnemy, 1);
                result |= tmp;
            }
            if (x + 1 <8 && y + 1 <8 && game[x + 1, y + 1] == pawnEnemy) //sud est
            {
                tmp = playableDiag2(game, x + 1, y + 1, pawnEnemy, 1);
                result |= tmp;
            }

            ISLEGAL = result;

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
        private bool playableAxisY(int[,] game, int x, int y,int pawnEnemy, int direction)
        {
            bool result = false;
            int i = y;
            List<Tuple<int, int>> tmp = new List<Tuple<int, int>>();
            while(i>0 && i < 8)
            {
                tmp.Add(new Tuple<int, int>(x,i));
                if(game[x,i] != pawnEnemy)
                {
                    result = game[x, i] != -1 ? true : false;
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
        private bool playableAxisX(int[,] game, int x, int y, int pawnEnemy, int direction)
        {
            bool result = false;
            List<Tuple<int, int>> tmp = new List<Tuple<int, int>>();
            int i = x;
            while (i >= 0 && i < 8) 
            {
                tmp.Add(new Tuple<int, int>(i, y));

                if (game[i, y] != pawnEnemy)
                {
                    result = game[i, y] != -1 ? true : false;
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
        private bool playableDiag1(int[,] game, int x, int y, int pawnEnemy, int direction)
        {
            bool result = false;
            int i = x;
            List<Tuple<int, int>> tmp = new List<Tuple<int, int>>();
            int j = y;
            while ((i > 0 && i < 8)&&(j>0 && j<8))
            {
                tmp.Add(new Tuple<int, int>(i, j));

                if (game[i, j] != pawnEnemy)
                {
                    result = game[i, j] != -1 ? true : false;
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
        private bool playableDiag2(int[,] game, int x, int y, int pawnEnemy, int direction)
        {
            bool result = false;
            int i = x;
            List<Tuple<int, int>> tmp = new List<Tuple<int, int>>();
            int j = y;
            while ((i > 0 && i < 8)&& (j > 0 && j < 8))
            {
                tmp.Add(new Tuple<int, int>(i, j));

                if (game[i, j] != pawnEnemy)
                {
                    result = game[i, j] != -1 ? true : false;
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
        public List<Tuple<int,int>> ReturnPawn(int[,] game, bool whiteTurn)
        {
            int pawnType = whiteTurn ? (int)Colors.white : (int)Colors.black;
            foreach (Tuple<int,int> tuple in toReturn)
            {
                game[tuple.Item1, tuple.Item2] = pawnType;
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
            return IsLegal(board, column, line, isWhite ? (int)Colors.black : (int)Colors.white);
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            this.isWhite = isWhite;
            bool result = IsLegal(board, column, line, isWhite ? (int)Colors.black : (int)Colors.white);
            if (result)
            {
                Play(column, line);
            }

            return result;
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            this.isWhite = whiteTurn;
            //board = game.Clone() as int[,];
            Tuple<int, Tuple<int, int>> result = AlphaBeta(game, level, whiteTurn, 0);
            return result.Item2;
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
            if(level == 0 || Final(game, whiteTurn))
            {
                return new Tuple<int, Tuple<int, int>>(Eval(game, whiteTurn), null);
            }
            int optVal = minOrMax * -1;
            Tuple<int, int> optOp = null;

            foreach(Tuple<int, int> op in ValideOp(game, whiteTurn))
            {
                int[,] newBoard = ApplyOp(game, op, whiteTurn).Clone() as int [,];
                isWhite = !whiteTurn;
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

        private List<Tuple<int, int>> ValideOp(int[,] game, bool whiteTurn)
        {
            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            for(int i=0; i < 8; i++)
            {
                for(int j=0; j < 8; j++)
                {
                     if(IsLegal(game, i, j, whiteTurn ? (int)Colors.black : (int)Colors.white))
                     {
                        list.Add(new Tuple<int, int>(i, j));
                     }
                }
            }
            return list;
        }

        private int[,] ApplyOp(int[,] game, Tuple<int, int> op, bool whiteTurn)
        {
            int[,] newGame = game.Clone() as int[,];

            if (IsLegal(game, op.Item1, op.Item2, whiteTurn ? (int)Colors.black : (int)Colors.white))
            {
                ReturnPawn(newGame, whiteTurn);
                newGame[op.Item1, op.Item2] = whiteTurn ? 0 : 1;
            }

            return newGame;
        }

        private bool Final(int[,] game, bool whiteTurn)
        {
            bool final = false;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (game[i, j] == -1)
                    {
                        final |= IsLegal(game, i, j, whiteTurn ? (int)Colors.black : (int)Colors.white);
                    }
                }
            }

            return !final;
        }

        private int Eval(int[,] game, bool whiteTurn)
        {
            int pieceNumber = whiteTurn ? GetWhiteScore() : GetBlackScore();
            int moveNumber = GetNbMoves(game, whiteTurn);
            Score(game);
            if(GetBlackScore() + GetWhiteScore() < 32)
            {
                return 2 * moveNumber + GetBoardScore(game, whiteTurn);
            }
            else
            {
                return 2 * pieceNumber + moveNumber + 3 * GetBoardScore(game, whiteTurn);
            }

        }

        private int GetNbMoves(int[,] game, bool whiteTurn)
        {
            int moves = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (IsLegal(game, i, j, whiteTurn ? (int)Colors.black : (int)Colors.white))
                    {
                        moves++;
                    }
                }
            }
            return moves;
        }

        private int GetBoardScore(int[,] game, bool whiteTurn)
        {
            int score = 0;
            int player = whiteTurn ? (int)Colors.white : (int)Colors.black;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (game[i, j] == player)
                    {
                        score += boardScore[i, j];
                    }
                }
            }
            return score;
        }
    }  
}
