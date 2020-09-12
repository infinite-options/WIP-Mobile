using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace WaitInPlace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Final_page : ContentPage
    {
        int yourNum;
        int origNum;
        int waitTimeOrig2;
        int time2;
        string placeInLine2;

        public Final_page(string placeInLine, int time, int waitTimeOrig)
        {
            InitializeComponent();
            waitTimeOrig2 = waitTimeOrig;
            time2 = time;
            placeInLine2 = placeInLine;

            origNum = int.Parse(placeInLine);
            yourNum = int.Parse(placeInLine);
            place.Text = placeInLine;
            //DateTime numTime;
            //DateTime.TryParce(time, out numTime);
            EntranceTime.Text = "Be at the entrance in " + time.ToString() + " min";
            //barcode.Source = ImageSource.FromResource("WaitInPlace.QRcode.jpg");
            //barcode.Source = ImageSource.FromResource("WaitInPlace.qrCode.png", typeof(ImageResourceExtension).GetTypeInfo().Assembly);
            Device.StartTimer(TimeSpan.FromSeconds(waitTimeOrig), () =>
            {
               if(origNum < yourNum)
                 {
                        origNum += 5;
                        return true;
                 }
                readyButton.Text = "NOW READY";
                readyButton.BackgroundColor = Color.Green;
               // Navigation.PushAsync(new BarcodePage(waitTimeOrig2, yourNum));
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
            
            //Navigation.PushAsync(new BarcodePage());
        }
    }
}