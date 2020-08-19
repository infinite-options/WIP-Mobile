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

namespace WaitInPlace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class MultipleStorePage : ContentPage
    {
        int lineNum1 = 0, lineNum2 = 0, lineNum3 = 0, waitingTime1=0, waitingTime2=0, waitingTime3=0;
        string lat1="", long1 = "", lat2 = "", long2 = "", lat3 = "", long3 = "", wait11 = "", wait12 = "", wait21 = "", wait22 = "", wait31 = "", wait32 = "";
        string customerId = "";
        DateTime eta = DateTime.Now;


        public ObservableCollection<MultipleStores> MultStores = new ObservableCollection<MultipleStores>();
        public ObservableCollection<GetId> GettingId = new ObservableCollection<GetId>();
        private ObservableCollection<MapMarker> markers;

        protected async Task GetMultStores()
        {
            Console.WriteLine("hie from multiplestores");
            var request = new HttpRequestMessage();
            string venueId = Preferences.Get("venue_id", "");
            request.RequestUri = new Uri("https://61vdohhos4.execute-api.us-west-1.amazonaws.com/dev/api/v2/get_venue/" + venueId);
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var userString = await content.ReadAsStringAsync();
                JObject mult_stores = JObject.Parse(userString);
                this.MultStores.Clear();

                string[] streetArray = { "", "", "" };
                string[] cityArray = { "", "", "" };
                string[] stateArray = { "", "", "" };
                string[] zipArray = { "", "", "" };
                string[] latArray = { "", "", "" };
                string[] longArray = { "", "", "" };
                string[] lineArray = { "", "", "" };
                string[] waitArray = { "", "", "" };
                string[] uidArray = { "", "", "" };


                int i = 0;
                foreach (var m in mult_stores["result"])
                {
                    streetArray[i] = m["street"].ToString();
                    cityArray[i] = m["city"].ToString();
                    stateArray[i] = m["state"].ToString();
                    zipArray[i] = m["zip"].ToString();
                    latArray[i] = m["lattitude"].ToString();
                    longArray[i] = m["longitude"].ToString();

                    lineArray[i] = m["queue_size"].ToString();
                    waitArray[i] = m["wait_time"].ToString();
                    uidArray[i] = m["venue_uid"].ToString();
                    i++;
                }
                address11.Text = streetArray[0] + ", " + cityArray[0] + ", " + stateArray[0] + ", " + zipArray[0];
                address21.Text = streetArray[1] + ", " + cityArray[1] + ", " + stateArray[1] + ", " + zipArray[1];
                address31.Text = streetArray[2] + ", " + cityArray[2] + ", " + stateArray[2] + ", " + zipArray[2];
                people1.Text = lineArray[0];
                people2.Text = lineArray[1];
                people3.Text = lineArray[2];
                lat1 = latArray[0];
                lat2 = latArray[1];
                lat3 = latArray[2];
                long1 = longArray[0];
                long2 = longArray[1];
                long3 = longArray[2];
                Preferences.Set("venue_uid1", uidArray[0]);
                Preferences.Set("venue_uid2", uidArray[1]);
                Preferences.Set("venue_uid3", uidArray[2]);
             //   Console.WriteLine("the uid1 is :" + uidArray[0]);
             //   Console.WriteLine("the uid1 is :" + uidArray[1]);
              //  Console.WriteLine("the uid1 is :" + uidArray[2]);

                //convert waitTime string to minutes
                //1
                int h1, h2, h3, m1, m2, m3;
                wait11 = waitArray[0].Substring(0, 1);
                wait12= waitArray[0].Substring(2, 2);
                bool isParsable1 = Int32.TryParse(wait11, out h1);
                bool isParsable2 = Int32.TryParse(wait12, out m1);
                waitingTime1 = h1 * 60 + m1;
                if (isParsable1 && isParsable2)
                   waitTime1.Text=(waitingTime1).ToString();
                else
                    Console.WriteLine("Could not be parsed.");

                //2
                wait21 = waitArray[1].Substring(0, 1);
                wait22 = waitArray[1].Substring(2, 2);
                isParsable1 = Int32.TryParse(wait21, out h2);
                isParsable2 = Int32.TryParse(wait22, out m2);
                waitingTime2 = h2 * 60 + m2;
                if (isParsable1 && isParsable2)
                    waitTime2.Text = (waitingTime2).ToString();
                else
                    Console.WriteLine("Could not be parsed.");

                //3
                wait31 = waitArray[2].Substring(0, 1);
                wait32 = waitArray[2].Substring(2, 2);
                isParsable1 = Int32.TryParse(wait31, out h3);
                isParsable2 = Int32.TryParse(wait32, out m3);
                waitingTime3 = h3 * 60 + m3;
                if (isParsable1 && isParsable2)
                    waitTime3.Text = (waitingTime3).ToString();
                else
                    Console.WriteLine("Could not be parsed.");
            }
        }
        protected async Task getCustomerId()
        {
            var request = new HttpRequestMessage();
            string custName = "\"" + Preferences.Get("name", "") + "\"";
            string custEmail = "\"" + Preferences.Get("email", "") + "\"";
            string custPhone = "\"" + Preferences.Get("phone", "") + "\"";

            request.RequestUri = new Uri("https://61vdohhos4.execute-api.us-west-1.amazonaws.com/dev/api/v2/get_customer_id/" + custName + "/" + custEmail + "/" + custPhone);
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var userString = await content.ReadAsStringAsync();
                JObject get_id = JObject.Parse(userString);
                this.GettingId.Clear();
                customerId = "";

                foreach (var m in get_id["result"])
                {
                    customerId = m["customer_id"].ToString();
                }
                Console.WriteLine("CUSTOMER_ID IN PARSE FUNC IS: " + customerId);
                Preferences.Set("customer_id", int.Parse(customerId));
            }
        }

        protected async Task setTicketInfo(int venue_uid, double wait_time)
        {
            Console.WriteLine("hie from set tkt");
            Console.WriteLine("starting of the setTicketInfo func!!");
            TicketInfo newTicket = new TicketInfo();
            newTicket.t_user_id = Preferences.Get("customer_id", 0);
            newTicket.t_uid = venue_uid;
            Preferences.Set("v_uid", venue_uid);
            //string now = DateTime.Now.TimeOfDay.ToString("h:mm:ss tt");
            DateTime now = DateTime.Now.ToLocalTime();
            string currentTime = (string.Format("{0}", now));
            Console.WriteLine("The current time is {0}", now);
            newTicket.t_entry_time = "12:02:32" ;// currentTime.Substring(9, 9);
            //Console.WriteLine("the uid1 is :" + venue_uid);
            var newTicketJSONString = JsonConvert.SerializeObject(newTicket);
            var content = new StringContent(newTicketJSONString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://61vdohhos4.execute-api.us-west-1.amazonaws.com/dev/api/v2/add_ticket");
            request.Method = HttpMethod.Post;
            request.Content = content;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            Console.WriteLine("set tktinfo ends");
        }
        private async void getLoaction()
        {
            Console.WriteLine("hie from location");
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout= TimeSpan.FromSeconds(30) 
                    }) ;
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
                Console.WriteLine("the location value is :{0}", location);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Console.WriteLine(" Handle not supported on device exception");

            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Console.WriteLine("Handle not enabled on device exception");

            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Console.WriteLine("Handle permission exception");
            }
            catch (Exception ex)
            {
                // Unable to get location
                Console.WriteLine("Unable to get location");
            }
            Console.WriteLine("hie from end of location");
        }
            public MultipleStorePage(string pageName) {
            InitializeComponent();
            GetMultStores();
            getCustomerId();
            Console.WriteLine("before calling location");
            getLoaction();
            PageName.Text = pageName;
            Console.WriteLine("hie from main");
            var location = new Location(21.705723, 72.998199);
            var otherLocation = new Location(22.3142, 73.1752);
            double distance = location.CalculateDistance(otherLocation, DistanceUnits.Kilometers);



           // Console.WriteLine("the value of wait1 is:" + waitingTime1);
           // Console.WriteLine("the value of wait2 is:" + waitingTime2);
        }

        private void Walk_Selected(object sender, EventArgs e)
        {
            Walk.BackgroundColor = Color.LightGreen;
            Walk.BorderColor = Color.Green;
            Bus.BackgroundColor = Color.White;
            Bus.BorderColor = Color.Black;
            Car.BackgroundColor = Color.White;
            Car.BorderColor = Color.Black;
            Preferences.Set("MOT", "walking");

            travel1.Text = "45 min";
            travel2.Text = "53 min";
            travel3.Text = "65 min";
        }

        private void Bus_Selected(object sender, EventArgs e)
        {
            Bus.BackgroundColor = Color.LightGreen;
            Bus.BorderColor = Color.Green;
            Walk.BackgroundColor = Color.White;
            Walk.BorderColor = Color.Black;
            Car.BackgroundColor = Color.White;
            Car.BorderColor = Color.Black;
            Preferences.Set("MOT", "transit");

            travel1.Text = "35 min";
            travel2.Text = "23 min";
            travel3.Text = "45 min";
        }

        private void Car_Selected(object sender, EventArgs e)
        {
            Car.BackgroundColor = Color.LightGreen;
            Car.BorderColor = Color.Green;
            Walk.BackgroundColor = Color.White;
            Walk.BorderColor = Color.Black;
            Bus.BackgroundColor = Color.White;
            Bus.BorderColor = Color.Black;
            Preferences.Set("MOT", "driving");

            travel1.Text = "7 min";
            travel2.Text = "13 min";
            travel3.Text = "17 min";
        }

        private void Join_Line_1(object sender, EventArgs e)
        {
            Console.WriteLine("hie from join 1");
            //selectedTime = timePicker1.Time;
            if (!double.TryParse(lat1, out double lat)) return;
            if (!double.TryParse(long1, out double lng)) return;
            Preferences.Set("latitude", lat);
            Preferences.Set("longitude", lng);
            int v_uid1 = int.Parse(Preferences.Get("venue_uid1", ""));
            double wait1 = waitingTime1;
            setTicketInfo(v_uid1,wait1);
            Console.WriteLine("set tktinfo in join1 done");
            Navigation.PushAsync(new yourNumberPage(waitingTime1, lineNum1));
            //Navigation.PushAsync(new ConfirmatonPage(lineNum1, travel1.Text, waitingTime1,distance11, address11.Text, selectedTime));
        }
        private void Join_Line_2(object sender, EventArgs e)
        {
            Console.WriteLine("hie from join 2");
            //selectedTime = timePicker2.Time;
            if (!double.TryParse(lat2, out double lat)) return;
            if (!double.TryParse(long2, out double lng)) return;
            Preferences.Set("latitude", lat);
            Preferences.Set("longitude", lng);
            int v_uid2 = int.Parse(Preferences.Get("venue_uid2", ""));
            double wait2 = waitingTime2;
            setTicketInfo(v_uid2,wait2);
            Navigation.PushAsync(new yourNumberPage(waitingTime2, lineNum2));
        }
        private void Join_Line_3(object sender, EventArgs e)
        {
            //selectedTime = timePicker3.Time;
            if (!double.TryParse(lat3, out double lat)) return;
            if (!double.TryParse(long3, out double lng)) return;
            Preferences.Set("latitude", lat); 
            Preferences.Set("longitude", lng);
            int v_uid3 =  int.Parse(Preferences.Get("venue_uid3", ""));
            double wait3 = waitingTime3;
            setTicketInfo(v_uid3,wait3); 
            Navigation.PushAsync(new yourNumberPage(waitingTime3, lineNum3));
        }

        private void main_page5(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}