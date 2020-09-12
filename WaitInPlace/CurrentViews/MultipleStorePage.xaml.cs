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
        string  wait11 = "", wait12 = "", wait21 = "", wait22 = "", wait31 = "", wait32 = "";
        double lat1,lat2,lat3,long1,long2,long3,dist1,dist2,dist3;
        DateTime eta = DateTime.Now;


        public ObservableCollection<MultipleStores> MultStores = new ObservableCollection<MultipleStores>();


        protected async Task GetMultStores()
        {
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
                double[] latArray = {0,0,0};
                double[] longArray = { 0,0,0};
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
                    latArray[i] =double.Parse( m["lattitude"].ToString());
                    longArray[i] =double.Parse( m["longitude"].ToString());
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

               
                lat1 =latArray[0];
                lat2 = latArray[1];
                lat3 = latArray[2];
                long1 =longArray[0];
                long2 = longArray[1];
                long3 = longArray[2];
                getDistance(lat1, lat2, lat3, long1, long2, long3);
                Preferences.Set("venue_uid1", uidArray[0]);
                Preferences.Set("venue_uid2", uidArray[1]);
                Preferences.Set("venue_uid3", uidArray[2]);

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


        protected async Task setTicketInfo(int venue_uid, double wait_time, TimeSpan selected_time)
        {
            TicketInfo newTicket = new TicketInfo();
            newTicket.t_user_id = Preferences.Get("customer_id", 0);
            newTicket.t_uid = venue_uid;
            Preferences.Set("v_uid", venue_uid);
            //string now = DateTime.Now.TimeOfDay.ToString("h:mm:ss tt");
            DateTime now = DateTime.Now.ToLocalTime();
            string currentTime = (string.Format("{0}", now));
            Console.WriteLine("The current time is {0}", now);
            newTicket.t_entry_time = "12:02:32" ;// currentTime.Substring(9, 9);
            newTicket.t_scheduled_time = selected_time.ToString();
            //Console.WriteLine("the uid1 is :" + venue_uid);
            var newTicketJSONString = JsonConvert.SerializeObject(newTicket);
            var content = new StringContent(newTicketJSONString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://61vdohhos4.execute-api.us-west-1.amazonaws.com/dev/api/v2/add_ticket");
            request.Method = HttpMethod.Post;
            request.Content = content;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
        }

        string GetTravelTime(double dist,double speed)
        {
            int traveltimeh=0;
            double traveltime=0.0;
            string travel="";

            traveltime = Math.Round(dist / speed, 2);
            traveltimeh = (int)(traveltime);
            traveltime -= traveltimeh;
            traveltime *= 100 +( traveltimeh * 60);
            travel = Math.Round(traveltime,0).ToString() + " min";
            Console.WriteLine("travel time is " + travel);

            return travel;
        }

        double CarSpeed(double dist)
        {
            if (dist < 10.0)
                return  30.0;
            else
                return 51.0;
        }

            public MultipleStorePage(string pageName) {
            InitializeComponent();
            GetMultStores();

            PageName.Text = pageName;

        }
        void getDistance(double lat1, double lat2, double lat3, double long1, double long2, double long3)
        {
            Console.WriteLine("hello");
            //for store1
            var custadd = new Location(Preferences.Get("lati", 0.0), Preferences.Get("long", 0.0));
            var venadd1 = new Location(lat1, long1);
            Console.WriteLine("vlat:{0},vlong:{1},clat:{2},clong:{3}", lat1, long1, Preferences.Get("lati", 0.0), Preferences.Get("long", 0.0));


            dist1 = Math.Round(custadd.CalculateDistance(venadd1, DistanceUnits.Miles), 2);
            Console.WriteLine("distance " + dist1);
            distance1.Text = dist1.ToString() + " mi away";

            //for store2
            var venadd2 = new Location(lat2, long2);
            dist2 = Math.Round(custadd.CalculateDistance(venadd2, DistanceUnits.Miles), 2);
            distance2.Text = dist2.ToString() + " mi away";

            //for store3
            var venadd3 = new Location(lat3, long3);
            dist3 = Math.Round(custadd.CalculateDistance(venadd3, DistanceUnits.Miles), 2);
            distance3.Text = dist3.ToString() + " mi away";
        }


        private void Walk_Selected(object sender, EventArgs e)
        {
            Walk.BackgroundColor = Color.FromHex("#0071BC");
            Walk.BorderColor = Color.FromHex("#0071BC");
            Bus.BackgroundColor = Color.White;
            Bus.BorderColor = Color.Black;
            Car.BackgroundColor = Color.White;
            Car.BorderColor = Color.Black;
            Preferences.Set("MOT", "walking");
            double walk = 3.1;
            travel1.Text = GetTravelTime(dist1,walk);
            travel2.Text = GetTravelTime(dist2, walk);
            travel3.Text = GetTravelTime(dist3, walk);
        }

        private void Bus_Selected(object sender, EventArgs e)
        {
            Bus.BackgroundColor = Color.FromHex("#0071BC");
            Bus.BorderColor = Color.FromHex("#0071BC");
            Walk.BackgroundColor = Color.White;
            Walk.BorderColor = Color.Black;
            Car.BackgroundColor = Color.White;
            Car.BorderColor = Color.Black;
            Preferences.Set("MOT", "transit");
            double bus = 13.6;
            travel1.Text = GetTravelTime(dist1, bus);
            travel2.Text = GetTravelTime(dist2, bus);
            travel3.Text = GetTravelTime(dist3, bus);
        }

        private void Car_Selected(object sender, EventArgs e)
        {
            Car.BackgroundColor = Color.FromHex("#0071BC");
            Car.BorderColor = Color.FromHex("#0071BC");
            Walk.BackgroundColor = Color.White;
            Walk.BorderColor = Color.Black;
            Bus.BackgroundColor = Color.White;
            Bus.BorderColor = Color.Black;
            Preferences.Set("MOT", "driving");

            travel1.Text = GetTravelTime(dist1, 51);// CarSpeed(dist1));
            travel2.Text = GetTravelTime(dist2, CarSpeed(dist2));
            travel3.Text = GetTravelTime(dist3, CarSpeed(dist3));
        }

        private void Join_Line_1(object sender, EventArgs e)
        {
            TimeSpan selectedTime = timePicker1.Time;
            Console.WriteLine("selected time {0}",selectedTime);
            //   if (!double.TryParse(lat1, out double lat)) return;
            //   if (!double.TryParse(long1, out double lng)) return;
            Preferences.Set("latitude", lat1);
            Preferences.Set("longitude", long1);
            int v_uid1 = int.Parse(Preferences.Get("venue_uid1", ""));
            double wait1 = waitingTime1;
            setTicketInfo(v_uid1,wait1, selectedTime);
            Navigation.PushAsync(new yourNumberPage(waitingTime1, lineNum1,v_uid1,address11.Text,PageName.Text));
            //Navigation.PushAsync(new ConfirmatonPage(lineNum1, travel1.Text, waitingTime1,distance11, address11.Text, selectedTime));
        }
        private void Join_Line_2(object sender, EventArgs e)
        {
            TimeSpan selectedTime = timePicker2.Time;
          //  if (!double.TryParse(lat2, out double lat)) return;
           // if (!double.TryParse(long2, out double lng)) return;
            Preferences.Set("latitude", lat2);
            Preferences.Set("longitude", long2);
            int v_uid2 = int.Parse(Preferences.Get("venue_uid2", ""));
            double wait2 = waitingTime2;
            setTicketInfo(v_uid2,wait2, selectedTime);
            Navigation.PushAsync(new yourNumberPage(waitingTime2, lineNum2,v_uid2, address21.Text, PageName.Text));
        }
        private void Join_Line_3(object sender, EventArgs e)
        {
            TimeSpan selectedTime = timePicker3.Time;
          //  if (!double.TryParse(lat3, out double lat)) return;
          //  if (!double.TryParse(long3, out double lng)) return;
            Preferences.Set("latitude", lat3); 
            Preferences.Set("longitude", long3);
            int v_uid3 =  int.Parse(Preferences.Get("venue_uid3", ""));
            double wait3 = waitingTime3;
            setTicketInfo(v_uid3,wait3, selectedTime);
            Navigation.PushAsync(new yourNumberPage(waitingTime3, lineNum3,v_uid3, address31.Text, PageName.Text));
        }

        private void main_page5(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}