using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroFiveBit.Utils;

namespace WaitInPlace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BarcodePage : ContentPage
    {
        int yourNum;
        int waitTimeOrig2;
        string yourNumStr;
        int origNum;
        static Countdown countdown;
        public BarcodePage(int waitTimeOrig, int placeInLine)
        {
            InitializeComponent();
            yourNum = placeInLine;
            origNum = placeInLine;
            waitTimeOrig2 = waitTimeOrig;
            countdown = new Countdown();
            countdown.StartUpdating(300);

            cdLabel.SetBinding(Label.TextProperty,
                new Binding("RemainTime", BindingMode.Default, new CountdownConverter()));
            cdLabel.BindingContext = countdown;
            
            Device.StartTimer(TimeSpan.FromMinutes(5), () =>
            {
                if (origNum < yourNum)
                {
                    origNum += 5;
                    return true;
                }

                //Navigation.PushAsync(new BarcodePage(yourNum, waitTimeOrig2));
                return false; // True = Repeat again, False = Stop the timer
            });
        }
        private void bump_in(object sender, EventArgs e)
        {
            yourNum += 5;
            yourNumStr = yourNum.ToString();

            Navigation.PushAsync(new yourNumberPage(waitTimeOrig2, yourNum));
        }

        private void exit_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Exit Store", "You are exitting the store. Thanks for using WIP!.", "Continue");
            Navigation.PushAsync(new MainPage());
        }
    }
}