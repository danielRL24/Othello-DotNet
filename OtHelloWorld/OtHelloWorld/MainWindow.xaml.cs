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

namespace OtHelloWorld
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
                        tuc.PawnLbl.Background = this.game.GetBrush(0);
                        tuc.IsEmpty = false;
                    }
                    else if (i == 3 && j == 4)
                    {
                        tuc.PawnLbl.Background = this.game.GetBrush(1);
                        tuc.IsEmpty = false;
                    }
                    else if (i == 4 && j == 3)
                    {
                        tuc.PawnLbl.Background = this.game.GetBrush(1);
                        tuc.IsEmpty = false;
                    }
                    else if (i == 4 && j == 4)
                    {
                        tuc.PawnLbl.Background = this.game.GetBrush(0);
                        tuc.IsEmpty = false;
                    }
                    Grid.SetColumn(tuc, i);
                    Grid.SetRow(tuc, j);
                    BoardGrid.Children.Add(tuc);
                } 
            }

            DataContext = game;
        }

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

        private void TileUC_MouseEnter(object sender, MouseEventArgs e)
        {
            TileUserControl tuc = (TileUserControl)sender;
            if (tuc.IsEmpty && this.game.IsLegal(tuc.X, tuc.Y, this.game.PawnEnemyType()))
                tuc.PawnLbl.Background = this.game.GetPawnBrush();
        }

        private void TileUC_MouseLeave(object sender, MouseEventArgs e)
        {
            TileUserControl tuc = (TileUserControl)sender;
            if (tuc.IsEmpty)
                tuc.PawnLbl.Background = null;
        }

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
                    MessageBox.Show("Fin du jeu!", "endGame", MessageBoxButton.OK);
                }
            }
                
        }

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
                //Console.WriteLine(this.game.Players.Count);
                refreshGrid();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            save();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            load();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messsageBoxResult = MessageBox.Show("Voulez-vous sauvergarder la partie en cours ?", "Quitter la partie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messsageBoxResult == MessageBoxResult.Yes)
            {
                save();
            }
            Application.Current.Shutdown();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messsageBoxResult = MessageBox.Show("Voulez-vous vraiment recommencer une partie?", "Nouvelle partie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messsageBoxResult == MessageBoxResult.Yes)
            {
                BoardGrid.Children.Clear();
                NewGame();
            }
        }
    }

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
