using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace OtHelloWars
{
    class Player : INotifyPropertyChanged
    {
        private int score;
        private int time;
        private Timer timer;
        

        public Player()
        {
            score = 0;
            time = 0;
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 1000;
        }

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

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            time += 1;
            NotifyPropertyChanged("Time");
        }

        public void StartTimer()
        {
            timer.Enabled = true;
        }

        public void StopTimer()
        {
            timer.Enabled = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nomPropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nomPropriete));
        }
    }
}
