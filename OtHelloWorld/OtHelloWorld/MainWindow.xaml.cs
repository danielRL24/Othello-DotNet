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

            for(int i = 0; i < 8; i++)
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

        private void MenuItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void MenuItem_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void MenuItem_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {

        }

        private void MenuItem_MouseLeftButtonUp_3(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
