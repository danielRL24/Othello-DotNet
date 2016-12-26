using System;
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

namespace OtHelloWorld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }

        private void NewGame()
        {

            Game game = new Game();

            for(int i = 0; i <= 8; i++)
            {
                for(int j = 0; j <= 8; j++)
                {
                    TileUserControl tuc = new TileUserControl(i, j);
                    Grid.SetColumn(tuc, i);
                    Grid.SetRow(tuc, j);
                    BoardGrid.Children.Add(tuc);
                } 
            }
        }
    }
}
