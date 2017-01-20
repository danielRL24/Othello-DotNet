﻿using System;
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
                        tuc.PawnLbl.Background = this.game.getBrush(0);
                        tuc.IsEmpty = false;
                    }
                    else if (i == 3 && j == 4)
                    {
                        tuc.PawnLbl.Background = this.game.getBrush(1);
                        tuc.IsEmpty = false;
                    }
                    else if (i == 4 && j == 3)
                    {
                        tuc.PawnLbl.Background = this.game.getBrush(1);
                        tuc.IsEmpty = false;
                    }
                    else if (i == 4 && j == 4)
                    {
                        tuc.PawnLbl.Background = this.game.getBrush(0);
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
            if (tuc.IsEmpty)
                tuc.PawnLbl.Background = this.game.getPawnBrush();
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
            tuc.PawnLbl.Background = this.game.getPawnBrush();
            tuc.IsEmpty = false;
            this.game.play(tuc.Y, tuc.X);
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
