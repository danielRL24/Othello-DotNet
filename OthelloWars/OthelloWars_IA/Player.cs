using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace OtHelloIA4
{
    class Player
    {
        private int score;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public Player()
        {
            score = 0;
        }

        /// <summary>
        /// Score property
        /// </summary>
        public int Score
        {
            get
            {
                return score;
            }

            set
            {
                score = value;
            }
        }
    }
}
