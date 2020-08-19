using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroFiveBit.Utils;

namespace WaitInPlace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class yourNumberPage : ContentPage
    {
        string placeInLine;
        int yourNum;
        int origNum;
        int waitTimeOrig2;
        string placeInLine2;
        int reachTime;
        static Countdown countdown;
        int counter;

        public yourNumberPage(int waitTime, int lineNum)
        {
            InitializeComponent();
            placeInLine = (lineNum + 1).ToString();
            waitTimeOrig2 = waitTime;

            reachTime = waitTime - 5;
            placeInLine2 = placeInLine;

            origNum = int.Parse(placeInLine);
            yourNum = int.Parse(placeInLine);
            place.Text = placeInLine;
            countdown = new Countdown();
            countdown.StartUpdating(waitTime);
            cdLabel.SetBinding(Label.TextProperty,
                    new Binding("RemainTime", BindingMode.Default, new CountdownConverter()));
            cdLabel.BindingContext = countdown;
            //DateTime numTime;
            //DateTime.TryParce(time, out numTime);
            //barcode.Source = ImageSource.FromResource("WaitInPlace.QRcode.jpg");
            //barcode.Source = ImageSource.FromResource("WaitInPlace.qrCode.png", typeof(ImageResourceExtension).GetTypeInfo().Assembly);
            Device.StartTimer(TimeSpan.FromSeconds(waitTime), () =>
            {
                if (origNum < yourNum)
                {
                    origNum += 5;
                    return true;
                }
                //countdown = new Countdown();
                

                readyButton.Text = "NOW READY";
                readyButton.BackgroundColor = Color.Green;
                Navigation.PushAsync(new BarcodePage(yourNum, waitTimeOrig2));
                return false; // True = Repeat again, False = Stop the timer
            });
        }

        private void main_page5(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());

        }

        private void bump_in(object sender, EventArgs e)
        {
            yourNum += 5;
            place.Text = yourNum.ToString();
            countdown.StartUpdating(waitTimeOrig2);

            //Navigation.PushAsync(new BarcodePage());
        }


        private void direction_Clicked(object sender, EventArgs e)
        {
            string mot = Preferences.Get("MOT", "default");
            double lat = Preferences.Get("latitude", 0.0);
            double lng = Preferences.Get("longitude", 0.0);
            if (mot == "driving")
            {
                Map.OpenAsync(lat, lng, new MapLaunchOptions { NavigationMode = NavigationMode.Driving });
            }
            else if (mot == "walking")
            {
                Map.OpenAsync(lat, lng, new MapLaunchOptions { NavigationMode = NavigationMode.Walking });
            }
            else if (mot == "transit")
            {
                Map.OpenAsync(lat, lng, new MapLaunchOptions { NavigationMode = NavigationMode.Transit });
            }
            else
            {
                Map.OpenAsync(lat, lng, new MapLaunchOptions { NavigationMode = NavigationMode.Default });
            }
        }
    }
}