using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace WaitInPlace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SafewayPage : ContentPage
    {
        int lineNum;
        int reachtime = 0 ;
        double entrytime = 0.0;
        //   int waitTime = 0;
        int waitTimeLeft = 0;
          int waitTimeOrig = 0;
        //int waitTimeRight = 0;
        //string totalWaitTime = "";

        //int waitTimeOrig;
        public SafewayPage(string pageName, int numOfPeople, int waitTime, string address1, string address2, double distance,double speed)
        {
            InitializeComponent();
            PageName.Text = pageName;
            //lineNum = new Random().Next(0,51);
            Line.Text = numOfPeople.ToString();
            //waitTime = lineNum * new Random().Next(1,7);
            //waitTimeOrig = waitTime;
            //waitTimeLeft = waitTime / 60;
            //waitTimeRight = waitTime - waitTimeLeft * 60;

            /*
            if (waitTimeLeft < 10) {
                totalWaitTime = "0" + waitTimeLeft.ToString() + ":" + waitTimeRight.ToString()+"    ";
            }
            else if (waitTimeRight < 10)
            {
                totalWaitTime = waitTimeLeft.ToString() + ":0" + waitTimeRight.ToString() + "    ";
            }
            else if ((waitTimeLeft < 10) && (waitTimeRight < 10))
            {
                totalWaitTime = waitTimeLeft.ToString() + ":" + waitTimeRight.ToString() + "    ";
            }
            else
            {
                totalWaitTime = waitTimeLeft.ToString() + ":" + waitTimeRight.ToString() + "    ";
            }
            */

            waitTimeOrig = waitTime;
            WaitingTime.Text = waitTime.ToString() + " min";

            Time.Text = Preferences.Get("ETA", "00:00 AM") + "    ";
            location1.Text = "                " + address1;
            location2.Text = "                " + address2;
            dist.Text = distance.ToString() + "mi";
            entrytime = Math.Round(distance / speed,2);
            waitTimeLeft = (int)(entrytime);
            entrytime -= waitTimeLeft;
            entrytime *= 100 + waitTimeLeft * 60;

            Time.Text = entrytime.ToString() + " min";
            lineNum = numOfPeople;
            reachtime = waitTime - 5;


          //  DateTime now = DateTime.Now.ToLocalTime();
          //  string currentTime = " " + now.ToString();
          //  string onlytime = currentTime.Substring(9);
          // waitTimeLeft = (int)onlytime.Substring(2);
           //Time1.Text = " " + onlytime.ToString();

        }
        private void main_page4(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

        private void To_final_page(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Final_page(yournum.Text,reachtime, waitTimeOrig));
        }

        private void display_Line_Num(object sender, EventArgs e)
        {
            join.BackgroundColor = Color.Green;
            yournum.Text = (lineNum + 1).ToString();
            LineStack.BackgroundColor = Color.Black;
            //LS1.TextColor = Color.White;
            LS2.TextColor = Color.White;
            yournum.TextColor = Color.White;
            confirmButton.TextColor = Color.White;
            confirmButton.BackgroundColor = Color.Green;
            confirmButton.Clicked += To_final_page;

        }
    }
}