using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OtHelloWars
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Game game;

        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }
        
        /// <summary>
        /// Instanciate and initialize the game
        /// </summary>
        private void NewGame()
        {

            this.game = new Game();
            System.Media.SoundPlayer player = new System.Media.SoundPlayer("../../sounds/ImperialMarch.wav");
            player.Play();

            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    TileUserControl tuc = new TileUserControl(i, j);
                    tuc.MouseLeave += TileUC_MouseLeave;
                    tuc.MouseEnter += TileUC_MouseEnter;
                    tuc.MouseLeftButtonUp += TileUC_MouseLeftButtonUp;
                    if (i == 3 && j == 3)
                    {
                        tuc.PawnLbl.Background = this.game.GetBrush(1);
                        tuc.IsEmpty = false;
                    }
                    else if (i == 3 && j == 4)
                    {
                        tuc.PawnLbl.Background = this.game.GetBrush(0);
                        tuc.IsEmpty = false;
                    }
                    else if (i == 4 && j == 3)
                    {
                        tuc.PawnLbl.Background = this.game.GetBrush(0);
                        tuc.IsEmpty = false;
                    }
                    else if (i == 4 && j == 4)
                    {
                        tuc.PawnLbl.Background = this.game.GetBrush(1);
                        tuc.IsEmpty = false;
                    }
                    Grid.SetColumn(tuc, i);
                    Grid.SetRow(tuc, j);
                    BoardGrid.Children.Add(tuc);
                } 
            }

            DataContext = game;
        }

        /// <summary>
        /// Refresh the grid and the display after a load game
        /// </summary>
        private void refreshGrid()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    TileUserControl t = (TileUserControl)getChildren(i, j);
                    if(this.game.Board[i,j] == 0)
                    {
                        t.PawnLbl.Background = this.game.GetBrush(0);
                        t.IsEmpty = false;
                    } else if (this.game.Board[i,j] == 1)
                    {
                        t.PawnLbl.Background = this.game.GetBrush(1);
                        t.IsEmpty = false;
                    } else
                    {
                        t.PawnLbl.Background = null;
                        t.IsEmpty = true;
                    }
                }
            }

            DataContext = game;
        }

        /// <summary>
        /// Verify if the player can do this move
        /// when the mouse enter a compartment of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TileUC_MouseEnter(object sender, MouseEventArgs e)
        {
            TileUserControl tuc = (TileUserControl)sender;
            if (tuc.IsEmpty && this.game.IsLegal(tuc.X, tuc.Y, this.game.PawnEnemyType()))
                tuc.PawnLbl.Background = this.game.GetPawnBrush();
        }

        /// <summary>
        /// Undo the display of the pawn if the player
        /// live the compartment of the grid without playing the move
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TileUC_MouseLeave(object sender, MouseEventArgs e)
        {
            TileUserControl tuc = (TileUserControl)sender;
            if (tuc.IsEmpty)
                tuc.PawnLbl.Background = null;
        }

        /// <summary>
        /// Place a pawn on the mouse click and verify
        /// if there's more moves possible or if it's
        /// the end of the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TileUC_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TileUserControl tuc = (TileUserControl)sender;
            if (tuc.IsEmpty && this.game.IsLegal(tuc.X, tuc.Y, this.game.PawnEnemyType()))
            {
                tuc.PawnLbl.Background = this.game.GetPawnBrush();
                tuc.IsEmpty = false;
                foreach(Tuple<int,int> tuple in this.game.ReturnPawn())
                {
                    TileUserControl t = (TileUserControl)getChildren(tuple.Item1, tuple.Item2);
                    t.PawnLbl.Background = this.game.GetPawnBrush();
                }
                this.game.Play(tuc.X, tuc.Y);
                if(!this.game.CanPlay())
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Vader : " + this.game.Players[0].Score + " points\nLuke : " + this.game.Players[1].Score + " points", "Fin de la partie", MessageBoxButton.OK);
                    if(messageBoxResult == MessageBoxResult.OK)
                    {
                        MessageBoxResult messageBoxResult2 = MessageBox.Show("Voulez-vous recommencer une partie ?", "Nouvelle partie", MessageBoxButton.YesNo);
                        if(messageBoxResult2 == MessageBoxResult.Yes)
                        {
                            BoardGrid.Children.Clear();
                            NewGame();
                        } else
                        {
                            Application.Current.Shutdown();
                        }
                    } else
                    {
                        Application.Current.Shutdown();
                    }
                }
            }
                
        }

        /// <summary>
        /// Get an element of the grid
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private UIElement getChildren(int x, int y)
        {
            foreach(UIElement child in BoardGrid.Children)
            {
                if(Grid.GetRow(child) == y && Grid.GetColumn(child) == x)
                {
                    return child;
                }
            }
            return null;
        }

        /// <summary>
        /// Save the game in a Json file
        /// </summary>
        private void save()
        {
            ObjectToSerialize ots = new ObjectToSerialize();
            ots.Board = this.game.Board;
            ots.IsWhite = this.game.IsWhite;
            ots.TimePlayer0 = this.game.Players[0].Time;
            ots.TimePlayer1 = this.game.Players[1].Time;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json|*.json|Text|*.txt";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                string output = JsonConvert.SerializeObject(ots);
                System.IO.File.WriteAllText(saveFileDialog.FileName, output);
            }
        }

        /// <summary>
        /// Load the game from a Json file
        /// </summary>
        private void load()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json|*.json|Text|*.txt";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                string input = System.IO.File.ReadAllText(openFileDialog.FileName);
                ObjectToSerialize ots= JsonConvert.DeserializeObject<ObjectToSerialize>(input);
                this.game = new Game(ots.Board, ots.IsWhite, ots.TimePlayer0, ots.TimePlayer1);
                refreshGrid();
            }
        }

        /// <summary>
        /// Call the save method when the player
        /// clicks on the save menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            save();
        }

        /// <summary>
        /// Call the load method when the player
        /// clicks on the load menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            load();
        }

        /// <summary>
        /// Ask if the game needs to be saved
        /// before the application shutdown on the click
        /// of the quit menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messsageBoxResult = MessageBox.Show("Voulez-vous sauvergarder la partie en cours ?", "Quitter la partie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messsageBoxResult == MessageBoxResult.Yes)
            {
                save();
            }
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Ask if the player wants to play a new game
        /// and reset the game on the click of the newgame menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messsageBoxResult = MessageBox.Show("Voulez-vous vraiment recommencer une partie ?", "Nouvelle partie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messsageBoxResult == MessageBoxResult.Yes)
            {
                BoardGrid.Children.Clear();
                NewGame();
            }
        }
    }

    /// <summary>
    /// Class used to store the game parameters
    /// and serialize it with Json.Net
    /// </summary>
    public class ObjectToSerialize
    {
        private int timePlayer0;
        private int timePlayer1;
        private bool isWhite;
        private int[,] board;

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

        public int TimePlayer0
        {
            get
            {
                return timePlayer0;
            }

            set
            {
                timePlayer0 = value;
            }
        }

        public int TimePlayer1
        {
            get
            {
                return timePlayer1;
            }

            set
            {
                timePlayer1 = value;
            }
        }
    }
}
