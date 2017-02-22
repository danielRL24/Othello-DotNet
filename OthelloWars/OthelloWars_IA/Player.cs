using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace OtHelloWars_IA
{
    class Player : INotifyPropertyChanged
    {
        private int score;
        private int time;
        private Timer timer;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public Player()
        {
            score = 0;
            time = 0;
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 1000;
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
                NotifyPropertyChanged("Score");
            }
        }

        /// <summary>
        /// Time property
        /// </summary>
        public int Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
                NotifyPropertyChanged("Time");
            }
        }

        /// <summary>
        /// Called every seconds
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            time += 1;
            NotifyPropertyChanged("Time");
        }

        /// <summary>
        /// Start the timer
        /// </summary>
        public void StartTimer()
        {
            timer.Enabled = true;
        }

        /// <summary>
        /// Stop the timer
        /// </summary>
        public void StopTimer()
        {
            timer.Enabled = false;
        }

        /// <summary>
        /// Allow to notify a property change
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Allow to notify a property change
        /// </summary>
        /// <param name="nomPropriete"></param>
        public void NotifyPropertyChanged(string nomPropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nomPropriete));
        }
    }
}
