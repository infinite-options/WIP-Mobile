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
    public partial class ConfirmatonPage : ContentPage
    {
        string placeInLine;
        int yourNum;
        int origNum;
        int waitTimeOrig2;
     //   int travelTime2;
        string placeInLine2;
        int reachTime;
        double distance2;

        public ConfirmatonPage(int lineNum, string travelTime, int waitTime, double distance, string address, TimeSpan pickerTime)
        {
            placeInLine = lineNum.ToString();
            InitializeComponent();
            waitTimeOrig2 = waitTime;
            reachTime = waitTime - 5;
            placeInLine2 = placeInLine;
            distance2 = distance;

            origNum = int.Parse(placeInLine);
            yourNum = int.Parse(placeInLine);
            place.Text = placeInLine;
            address11.Text = address;
            distance1.Text = distance.ToString() + " mi";
            waitTime1.Text = waitTime.ToString() + "  min";
            travel1.Text = travelTime;

            SelectedTime.Text = pickerTime.ToString();
          //  people1.Text = lineNum.ToString();
            //DateTime numTime;
            //DateTime.TryParce(time, out numTime);
            //barcode.Source = ImageSource.FromResource("WaitInPlace.QRcode.jpg");
            //barcode.Source = ImageSource.FromResource("WaitInPlace.qrCode.png", typeof(ImageResourceExtension).GetTypeInfo().Assembly);
            
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

        private void confirmNumber(object sender, EventArgs e)
        {
            confirmButton.TextColor = Color.White;
            confirmButton.BackgroundColor = Color.Green;
            Navigation.PushAsync(new yourNumberPage(waitTimeOrig2, yourNum));
        }
    }
}