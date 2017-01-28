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

        /// <summary>
        /// Default constructor
        /// </summary>
        public TileUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with possibiliy to define position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public TileUserControl(int x, int y) : this()
        {
            this.x = x;
            this.y = y;
            this.isEmpty = true;
        }

        /// <summary>
        /// X property
        /// </summary>
        public int X { get { return x; } }

        /// <summary>
        /// Y property
        /// </summary>
        public int Y { get { return y; } }

        /// <summary>
        /// Is empty property
        /// </summary>
        public bool IsEmpty { get { return this.isEmpty; } set { this.isEmpty = value; } }
    }
}
