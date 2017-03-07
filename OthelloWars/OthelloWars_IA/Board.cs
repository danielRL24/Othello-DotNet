using System;
using System.Collections.Generic;
using System.Linq;

namespace OtHelloIA4 
{
    class Board : IPlayable.IPlayable
    {
        private string teamName;
        private int[,] board;
        private bool isWhite;
        private List<Player> players;
        private List<Tuple<int,int>> toReturn;

        enum Colors { white = 0, black }

        private int[,] boardScore = new int[,] { {120, -20, 20, 5, 5, 20, -20, 120},
                                                 {-20, -40, -5, -5, -5, -5, -40, -20 },
                                                 { 20, -5, 15, 3, 3, 15, -5, 20},
                                                 { 5, -5, 3, 3, 3, 3, -5, 5},
                                                 { 5, -5, 3, 3, 3, 3, -5, 5},
                                                 { 20, -5, 15, 3, 3, 15, -5, 20},
                                                 {-20, -40, -5, -5, -5, -5, -40, -20 },
                                                 {120, -20, 20, 5, 5, 20, -20, 120}};

        #region BoardFunctions

        /// <summary>
        /// Default Constructor
        /// Initialize board, list of player and score
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

            teamName = "04_Nadalin_Rodrigues";

            toReturn = new List<Tuple<int, int>>();

            players = new List<Player>();
            players.Add(new Player());
            players.Add(new Player());

            Score(board);
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
                    if(game[i,j] == (int)Colors.white)
                    {
                        scoreA++;
                    }
                    else if(game[i,j] == (int)Colors.black)
                    {
                        scoreB++;
                    }
                }
            }
            players[0].Score = scoreA;
            players[1].Score = scoreB;
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
            if ((x - 1 >= 0 && y + 1 <8) && game[x - 1, y + 1] == pawnEnemy) //sud ouest
            {
                tmp = playableDiag1(game, x - 1, y + 1, pawnEnemy, 1);
                result |= tmp;
            }
            if (x - 1 >= 0 && game[x - 1, y] == pawnEnemy) // ouest
            {
                tmp = playableAxisX(game, x - 1, y, pawnEnemy, -1);
                result |= tmp;
            }
            if ((x - 1 >= 0 && y - 1 >= 0) && game[x - 1, y - 1] == pawnEnemy) // nord ouest
            {
                tmp = playableDiag2(game, x - 1, y - 1, pawnEnemy, -1);
                result |= tmp;
            }
            if (y - 1 >= 0 && game[x, y - 1] == pawnEnemy) //nord
            {
                tmp = playableAxisY(game, x, y - 1, pawnEnemy, -1);
                result |= tmp;
            }
            if ((x + 1 <8 && y - 1 >= 0) && game[x + 1, y - 1] == pawnEnemy) //nord est
            {
                tmp = playableDiag1(game, x + 1, y - 1, pawnEnemy, -1);
                result |= tmp;
            }
            if (x + 1 <8 && game[x + 1, y] == pawnEnemy) // est
            {
                tmp = playableAxisX(game, x + 1, y, pawnEnemy, 1);
                result |= tmp;
            }
            if ((x + 1 <8 && y + 1 <8) && game[x + 1, y + 1] == pawnEnemy) //sud est
            {
                tmp = playableDiag2(game, x + 1, y + 1, pawnEnemy, 1);
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
        private bool playableAxisY(int[,] game, int x, int y,int pawnEnemy, int direction)
        {
            bool result = false;
            int i = y;
            List<Tuple<int, int>> tmp = new List<Tuple<int, int>>();
            while(i >= 0 && i < 8)
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
        /// Check if the player can play in first diagonal (NE - SW).
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
            while ((i >= 0 && i < 8)&&(j >= 0 && j < 8))
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
        /// Check if the player can play in seconde diagonal (NW - SE).
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
            while ((i >= 0 && i < 8)&& (j >= 0 && j < 8))
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

        #endregion

        #region IPlayable

        /////////////////////////////////////////////////////
        // INTERFACE IPlayable
        /////////////////////////////////////////////////////

        /// <summary>
        /// Get name of team
        /// </summary>
        /// <returns>Team's name</returns>
        public string GetName()
        {
            return teamName;
        }

        /// <summary>
        /// Check if is playable
        /// </summary>
        /// <param name="column">Column number</param>
        /// <param name="line">Line number</param>
        /// <param name="isWhite">Is white turn?</param>
        /// <returns>Playable or not</returns>
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
                ReturnPawn(board, isWhite);
                board[column, line] = isWhite ? (int)Colors.white : (int)Colors.black;
                isWhite = !isWhite;
                Score(board);
            }

            return result;
        }

        /// <summary>
        /// Get the next move. Use of alpha-beta algorithm to determine the best next move
        /// </summary>
        /// <param name="game"></param>
        /// <param name="level"></param>
        /// <param name="whiteTurn"></param>
        /// <returns>Move</returns>
        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            this.isWhite = whiteTurn;
            //board = game.Clone() as int[,];
            Tuple<int, Tuple<int, int>> result = AlphaBeta(game, level, whiteTurn, 0);
            return result.Item2;
        }

        /// <summary>
        /// Get the actual board
        /// </summary>
        /// <returns>Board</returns>
        public int[,] GetBoard()
        {
            return board;
        }

        /// <summary>
        /// Get the score of white player
        /// </summary>
        /// <returns>Score</returns>
        public int GetWhiteScore()
        {
            return players[0].Score;
        }

        /// <summary>
        /// Get the score of black player
        /// </summary>
        /// <returns>Score</returns>
        public int GetBlackScore()
        {
            return players[1].Score;
        }

        #endregion

        #region AlphaBeta

        /////////////////////////////////////////////////////
        // IA - Alphabeta
        /////////////////////////////////////////////////////

        /// <summary>
        /// Algorithme alpha-beta 2 
        /// </summary>
        /// <param name="game">Plateau de jeu actuel</param>
        /// <param name="level">Niveau de profondeur</param>
        /// <param name="whiteTurn">Tour du joureur blanc ?</param>
        /// <param name="parentValue">Valeur du parent</param>
        /// <returns></returns>
        private Tuple<int, Tuple<int, int>> AlphaBeta(int[,] game, int level, bool whiteTurn, int parentValue)
        {
            int minOrMax = whiteTurn ? 1 : -1;
            // Si la profondeur est de 0 ou c'est la fin de jeu, on retourne :
            //  - le score du plateau
            //  - la position (-1, -1) -> cette position indique au moteur que le joueur ne peut plus jouer
            if(level == 0 || Final(game, whiteTurn))
            {
                return new Tuple<int, Tuple<int, int>>(Eval(game, whiteTurn), new Tuple<int, int>(-1, -1));
            }
            int optVal = minOrMax * -1;
            Tuple<int, int> optOp = null;

            List<Tuple<int, int>> ops = ValideOp(game, whiteTurn);
            optOp = ops[0];
            // Parcours des coups possibles
            foreach(Tuple<int, int> op in ops)
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

        /// <summary>
        /// Permet d'obtenir la liste d'opération valide pour un plateau de jeu donnée
        /// </summary>
        /// <param name="game">Plateau de jeu</param>
        /// <param name="whiteTurn">Tour du joueur blanc ?</param>
        /// <returns>Liste</returns>
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

        /// <summary>
        /// Applique un coup donnée sur un plateau de jeu donnée
        /// </summary>
        /// <param name="game">Plateau de jeu</param>
        /// <param name="op">Coup à appliquer</param>
        /// <param name="whiteTurn">Tour du joueur blanc ?</param>
        /// <returns>Plateau de jeu une fois le coup joué</returns>
        private int[,] ApplyOp(int[,] game, Tuple<int, int> op, bool whiteTurn)
        {
            int[,] newGame = game.Clone() as int[,];

            if (IsLegal(game, op.Item1, op.Item2, whiteTurn ? (int)Colors.black : (int)Colors.white))
            {
                ReturnPawn(newGame, whiteTurn);
                newGame[op.Item1, op.Item2] = whiteTurn ? (int)Colors.white : (int)Colors.black;
            }
            return newGame;
        }

        /// <summary>
        /// Fonction d'évaluation. 
        /// Permet de donner une valeur au plateau de jeu selon quel jouer joue.
        /// Cette fonction prend en compte plusieurs facteurs pour déterminer le score d'un plateau :
        ///   - Le nombre de pièce du joueur
        ///   - Le score du plateau de jeu (somme de l'importance des cases conquises)
        ///   - Le nombre de coups possible actuellement
        /// </summary>
        /// <param name="game">Plateau de jeu</param>
        /// <param name="whiteTurn">Tour du joueur blanc?</param>
        /// <returns>Valeur du plateau</returns>
        private int Eval(int[,] game, bool whiteTurn)
        {
            Score(game);

            int pieceNumber = whiteTurn ? GetWhiteScore() : GetBlackScore();
            int moveNumber = GetNbMoves(game, whiteTurn);
            int boardScore = GetBoardScore(game, whiteTurn);

            // 1ere moitie de jeu, 1ere strategie => mobilite
            if (GetBlackScore() + GetWhiteScore() < 32)
            {
                return 2 * moveNumber + boardScore;
            }
            // 2eme moitie de jeu, 2eme strategie => nombre de pieces recoltees
            else
            {
                return 2 * pieceNumber + moveNumber + 4 * boardScore;
            }
        }

        /// <summary>
        /// Permet d'obtenir le nombre de coups possible pour un plateau donné selon le joueur
        /// </summary>
        /// <param name="game">Plateau de jeu</param>
        /// <param name="whiteTurn">Tour du joueur blanc?</param>
        /// <returns>Nombre de coups possibles</returns>
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

        /// <summary>
        /// Permet d'obtenir le score d'un plateau.
        /// Le score d'un plateau est la somme du poids des cases conquises par le joueur.
        /// Le point de chaque case est donné par un tableau.
        /// </summary>
        /// <param name="game">Plateau de jeu</param>
        /// <param name="whiteTurn">Tour du joueur blanc?</param>
        /// <returns>Score du plateau de jeu du joueur</returns>
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

        /// <summary>
        /// Permet de déterminer si le joueur peut encore jouer ou pas.
        /// </summary>
        /// <param name="game">Plateau de jeu</param>
        /// <param name="whiteTurn">Tour de joueur blanc?</param>
        /// <returns>Fin de jeu</returns>
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

        #endregion
    }
}
