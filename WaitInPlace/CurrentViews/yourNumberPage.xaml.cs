 using GoogleApi.Entities.Maps.StaticMaps.Request;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WaitInPlace.Classes;
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
        int yourNum = 0;
        double waitTime1=0;
        int origNum;
        double waitTimeOrig2;
        string placeInLine2;
        double reachTime;
        static Countdown countdown;
        readonly int counter;
        string tokenId="";
        bool click = false;

        public ObservableCollection<TokenId> TokenId = new ObservableCollection<TokenId>();
        protected async Task getTokenId(int venue_id)
        {
            var request = new HttpRequestMessage();
            int custId = Preferences.Get("customer_id", 0);
            Console.WriteLine("CUSTOMER_ID IN FUNC IS: " + custId);
            int VenueUid = venue_id;
            Console.WriteLine("v_uid IN PARSE FUNC IS: " + VenueUid);
            request.RequestUri = new Uri("https://61vdohhos4.execute-api.us-west-1.amazonaws.com/dev/api/v2/customer_token/" + custId + "/" + VenueUid);
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            Console.WriteLine("its before if");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("its after if");
                HttpContent content = response.Content;
                var userString = await content.ReadAsStringAsync();
                JObject get_Token_id = JObject.Parse(userString);
                this.TokenId.Clear();
                tokenId = "1";

                foreach (var m in get_Token_id["result"])
                {
                    tokenId = m["token_number"].ToString();
                }
                Console.WriteLine("TOKEN IN PARSE FUNC IS: " + tokenId);
                Preferences.Set("token_id", int.Parse(tokenId));
            }
            place.Text = tokenId;
        }

        async void initialization(int venue_uid)
        {
            await getTokenId(venue_uid);
        }

        protected async Task setGetOut(int venue_uid)
        {
            Console.WriteLine("hie from set exit");
            //Console.WriteLine("starting of the get func!!");
            ExitInfo nwexit = new ExitInfo();
            // TicketInfo newtkt = new TicketInfo();
            nwexit.usr_id = Preferences.Get("customer_id", 0);
            nwexit.vnu_uid = venue_uid;
            Console.WriteLine("the venue id and cus id is" + venue_uid.ToString() + nwexit.usr_id.ToString());
            //string now = DateTime.Now.TimeOfDay.ToString("h:mm:ss tt");
            DateTime now = DateTime.Now.ToLocalTime();
            string currentTime = (string.Format("{0}", now));
            //string now = "12:02:32";
            Console.WriteLine("The current time is at exit butto " + now.TimeOfDay);
            nwexit.ext_time = currentTime.Substring(9, 9);
            //Console.WriteLine("the uid1 is :" + venue_uid);
            var newExitJSONString = JsonConvert.SerializeObject(nwexit);
            var content = new StringContent(newExitJSONString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://61vdohhos4.execute-api.us-west-1.amazonaws.com/dev/api/v2/get_out");
            request.Method = HttpMethod.Put;
            request.Content = content;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            Console.WriteLine(response);
        }

        public yourNumberPage(string waitTime, string lineNum,int venue_uid,string address, string pagename)
        {
            InitializeComponent();
            initialization(venue_uid);

            PageName.Text = pagename;
            address1.Text = address;
            Preferences.Set("venue_uid", venue_uid);
            Preferences.Set("add", address1.Text);
            placeInLine = (Int32.Parse(lineNum )+ 1).ToString();
            Console.WriteLine("printing the wait time" + waitTime);
            double.TryParse((String)waitTime,out waitTime1);
            Console.WriteLine("printing the wait time" + waitTime1);
            waitTimeOrig2 = waitTime1;
            Console.WriteLine("printing the waitinf orgin2"+waitTimeOrig2);

            reachTime = waitTime1- 5;
            placeInLine2 = placeInLine;

            origNum = int.Parse(placeInLine);
            yourNum = Preferences.Get("token_id",0);
            //  place.Text = placeInLine;
            countdown = new Countdown();
            countdown.StartUpdating(waitTimeOrig2*60);
            cdLabel.SetBinding(Label.TextProperty,
                    new Binding("RemainTime", BindingMode.Default, new CountdownConverter()));
            cdLabel.BindingContext = countdown;
            Device.StartTimer(TimeSpan.FromMinutes(waitTimeOrig2), () =>
            {
                /*if (origNum < yourNum)
                {
                    origNum += 5;
                    return true;
                }*/
                //countdown = new Countdown();
                if(click == true)
                {
                   return false;
                }

                readyButton.Text = "NOW READY";
                readyButton.BackgroundColor = Color.FromHex("#0071BC");
                Navigation.PushAsync(new BarcodePage(waitTimeOrig2, yourNum,PageName.Text));
                return false; // True = Repeat again, False = Stop the timer
            });
        }

        private void main_page5(object sender, EventArgs e)
        {
            click = true;
            setGetOut(Preferences.Get("venue_uid", 0));
            Navigation.PushAsync(new MainPage());

        }

        private void bump_in(object sender, EventArgs e)
        {
            yourNum += 5;
            place.Text = yourNum.ToString();
            countdown.StartUpdating(30*60);

            Navigation.PushAsync(new BarcodePage(waitTimeOrig2,yourNum,PageName.Text));
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