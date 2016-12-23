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
    /// Interaction logic for TileUserControl.xaml
    /// </summary>
    public partial class TileUserControl : UserControl
    {

        private int col;
        private int row;

        public TileUserControl()
        {
            InitializeComponent();
        }

        public TileUserControl(int col, int row) : this()
        {
            this.col = col;
            this.row = row;
            LabelName.Content = "("+(char)(col+65)+";"+row+")";
        }

        public int Col { get { return col; } }

        public int Row { get { return row; } }
    }
}
