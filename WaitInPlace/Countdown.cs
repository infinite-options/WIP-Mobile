//
//  Countdown.cs
//  Created by Alexey Kinev on 11 Jan 2015.
//
//    Licensed under The MIT License (MIT)
//    http://opensource.org/licenses/MIT
//
//    Copyright (c) 2015 Alexey Kinev <alexey.rudy@gmail.com>
//
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;

namespace ZeroFiveBit.Utils
{
    /// <summary>
    /// Countdown timer with periodical ticks.
    /// </summary>
    public class Countdown : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the start date time.
        /// </summary>
        public DateTime StartDateTime { get; private set; }

        /// <summary>
        /// Gets the remain time in seconds.
        /// </summary>
        public double RemainTime
        {
            get { return remainTime; }

            private set
            {
                remainTime = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Occurs when completed.
        /// </summary>
        public event Action Completed;

        /// <summary>
        /// Occurs when ticked.
        /// </summary>
        public event Action Ticked;

        /// <summary>
        /// The timer.
        /// </summary>
        Timer timer;

        /// <summary>
        /// The remain time.
        /// </summary>
        double remainTime;

        /// <summary>
        /// The remain time total.
        /// </summary>
        double remainTimeTotal;

        /// <summary>
        /// Starts the updating with specified period, total time and period are specified in seconds.
        /// </summary>
        public void StartUpdating(double total, double period = 1.0)
        {
            if (timer != null)
            {
                StopUpdating();
            }

            remainTimeTotal = total;
            RemainTime = total;

            StartDateTime = DateTime.Now;

            timer = new Timer(period * 1000);
            timer = new Timer();
            timer.Elapsed += (sender, e) => Tick();
            timer.Enabled = true;
        }

        /// <summary>
        /// Stops the updating.
        /// </summary>
        public void StopUpdating()
        {
            RemainTime = 0;
            remainTimeTotal = 0;

            if (timer != null)
            {
                timer.Enabled = false;
                timer = null;
            }
        }

        /// <summary>
        /// Updates the time remain.
        /// </summary>
        public void Tick()
        {
            var delta = (DateTime.Now - StartDateTime).TotalSeconds;

            if (delta < remainTimeTotal)
            {
                RemainTime = remainTimeTotal - delta;

                var ticked = Ticked;
                if (ticked != null)
                {
                    ticked();
                }
            }
            else
            {
                RemainTime = 0;

                var completed = Completed;
                if (completed != null)
                {
                    completed();
                }
            }
        }

        #region INotifyPropertyChanged implementation

        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
