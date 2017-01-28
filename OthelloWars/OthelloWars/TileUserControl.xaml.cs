﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OtHelloWars
{
    /// <summary>
    /// Interaction logic for TileUserControl.xaml
    /// </summary>
    public partial class TileUserControl : UserControl
    {

        private int x;
        private int y;
        private bool isEmpty;

        public TileUserControl()
        {
            InitializeComponent();
        }

        public TileUserControl(int x, int y) : this()
        {
            this.x = x;
            this.y = y;
            this.isEmpty = true;
            //PawnLbl.Content = "("+(char)(col+65)+";"+row+")";
        }

        public int X { get { return x; } }

        public int Y { get { return y; } }

        public bool IsEmpty { get { return this.isEmpty; } set { this.isEmpty = value; } }
    }
}