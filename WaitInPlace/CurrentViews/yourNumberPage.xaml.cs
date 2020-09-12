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
        int yourNum=0;
        int origNum;
        int waitTimeOrig2;
        string placeInLine2;
        int reachTime;
        static Countdown countdown;
        readonly int counter;
        string tokenId="";

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


        public yourNumberPage(int waitTime, int lineNum,int venue_uid,string address, string pagename)
        {
            InitializeComponent();
            initialization(venue_uid);

            PageName.Text = pagename;
            address1.Text = address;
            Preferences.Set("add", address1.Text);
            placeInLine = (lineNum + 1).ToString();
            waitTimeOrig2 = waitTime;

            reachTime = waitTime - 5;
            placeInLine2 = placeInLine;

            origNum = int.Parse(placeInLine);
            yourNum = Preferences.Get("token_id",0);
            //  place.Text = placeInLine;
            countdown = new Countdown();
            countdown.StartUpdating(waitTimeOrig2);
            cdLabel.SetBinding(Label.TextProperty,
                    new Binding("RemainTime", BindingMode.Default, new CountdownConverter()));
            cdLabel.BindingContext = countdown;
            Device.StartTimer(TimeSpan.FromSeconds(waitTimeOrig2), () =>
            {
                /*if (origNum < yourNum)
                {
                    origNum += 5;
                    return true;
                }*/
                //countdown = new Countdown();
                

                readyButton.Text = "NOW READY";
                readyButton.BackgroundColor = Color.FromHex("#0071BC");
                Navigation.PushAsync(new BarcodePage(waitTimeOrig2, yourNum,PageName.Text));
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
            countdown.StartUpdating(30);

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