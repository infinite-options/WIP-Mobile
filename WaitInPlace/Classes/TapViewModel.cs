using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace WaitInPlace.Classes
{
    public class TapViewModel : INotifyPropertyChanged
    {
        int taps = 0;
        ICommand tapCommand;
        public TapViewModel()
        {
            // configure the TapCommand with a method
            tapCommand = new Command(OnTapped);
        }
        public ICommand TapCommand
        {
            get { return tapCommand; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnTapped(object s)
        {
            taps++;
            Debug.WriteLine("parameter: " + s);
        }
        //region INotifyPropertyChanged code omitted
    }
}
