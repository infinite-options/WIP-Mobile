using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
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
        int lineNum1 = 0;
        int lineNum2 = 0;
        int lineNum3 = 0;
        int waitingTime1 = 0;
        int waitingTime2 = 0;
        int waitingTime3 = 0;
        int travelTime = 0;
        int waitTimeLeft1 = 0; double  entrytime1=0;
        int waitTimeLeft2 = 0; double entrytime2 = 0;
        int waitTimeLeft3 = 0; double entrytime3 = 0;
        string lat1, long1, lat2, long2, lat3, long3;

        double speed = 0.0;
        double distance11= 2.35, distance22 = 3.45 , distance33= 7.35;

        DateTime eta = DateTime.Now;
        TimeSpan selectedTime;

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

                //Console.WriteLine("user_info['result']: " + mult_stores["result"]);
                //Console.WriteLine("user_info: " + mult_stores);

                string[] streetArray = { "", "", "" };
                string[] cityArray = { "", "", "" };
                string[] stateArray = { "", "", "" };
                string[] zipArray = { "", "", "" };
                string[] latArray = { "", "", "" };
                string[] longArray = { "", "", "" };
                string[] lineArray = { "", "", "" };
                string[] waitArray = { "","",""};


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
                    /*this.VenueCat.Add(new VenueCategories()
                    {
                        category = m["category"].ToString(),
                    });*/
                    i++;
                }
                address11.Text = streetArray[0] + ", " + cityArray[0] + ", " + stateArray[0] + ", " + zipArray[0];
                address21.Text = streetArray[1] + ", " + cityArray[1] + ", " + stateArray[1] + ", " + zipArray[1];
                address31.Text = streetArray[2] + ", " + cityArray[2] + ", " + stateArray[2] + ", " + zipArray[2];
                waitTime1.Text = waitArray[0].Substring(0,4) + " min";
                waitTime2.Text = waitArray[1].Substring(0, 4) + " min";
                waitTime3.Text = waitArray[2].Substring(0, 4) + " min";
                people1.Text = lineArray[0];
                 people2.Text = lineArray[1];
                 people3.Text = lineArray[2];
                //VenueCatListView.ItemsSource = VenueCat;
                lat1 = latArray[0];
                lat2 = latArray[1];
                lat3 = latArray[2];
                long1 = longArray[0];
                long2 = longArray[1];
                long3 = longArray[2];

                //convert waitTime string to minutes
                //1
                int h1,h2,h3, m1,m2,m3;
                if (waitTime1.Text[2] == ':')
                {
                    
                    h1 = int.Parse(waitArray[0].Substring(0, 2));
                    m1 = int.Parse(waitArray[0].Substring(3, 5));


                }
                else if(waitTime1.Text[1] == ':')
                {
                    h1 = int.Parse(waitArray[0].Substring(0, 1));
                    m1 = int.Parse(waitArray[0].Substring(2, 4));
                }
                else
                {
                    h1 =0;
                    m1 =0;
                }
                waitingTime1 = h1 * 60 + m1;
                waitTime1.Text= waitingTime1.ToString();
                //2
                if (waitTime2.Text[2] == ':')
                {

                    h2 = int.Parse(waitArray[0].Substring(0, 2));
                    m2 = int.Parse(waitArray[0].Substring(3, 5));


                }
                else if (waitTime2.Text[1] == ':')
                {
                    h2 = int.Parse(waitArray[0].Substring(0, 1));
                    m2 = int.Parse(waitArray[0].Substring(2, 4));
                }
                else
                {
                    h2 = 1;
                    m2 = 0;
                }
                waitingTime2 = h1 * 60 + m1;
                waitTime2.Text = waitingTime2.ToString();
                //3
                if (waitTime3.Text[2] == ':')
                {

                    h3 = int.Parse(waitArray[0].Substring(0, 2));
                    m3 = int.Parse(waitArray[0].Substring(3, 5));


                }
                else if (waitTime3.Text[1] == ':')
                {
                    h3= int.Parse(waitArray[0].Substring(0, 1));
                    m3 = int.Parse(waitArray[0].Substring(2, 4));
                }
                else
                {
                    h3 = 0;
                    m3 = 0;
                }
                waitingTime3 = h1 * 60 + m1;
                waitTime3.Text = waitingTime3.ToString();
            }
        }

        protected async Task setCustomerInfo()
        {
            int i = 10;
            UserInfo newUser = new UserInfo();
            newUser.name = Preferences.Get("name", "");
            newUser.email = Preferences.Get("email", "");
            newUser.phone = Preferences.Get("phone", "");
            //newUser.customer_id = i + 1;
            //newUser.address = "783 Sample Address";
            var newUserJSONString = JsonConvert.SerializeObject(newUser);
            var content = new StringContent(newUserJSONString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage();
            string venueId = Preferences.Get("venue_id", "");
            request.RequestUri = new Uri("https://61vdohhos4.execute-api.us-west-1.amazonaws.com/dev/api/v2/add_customer");
            request.Method = HttpMethod.Post;
            request.Content = content;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
        }


        public MultipleStorePage(string pageName) {
            InitializeComponent();
            GetMultStores();
            //setCustomerInfo();
            PageName.Text = pageName;
            //lineNum1 = new Random().Next(0, 51);
            //people1.Text = lineNum1.ToString();
            //waitingTime1 = lineNum1 * new Random().Next(1, 7);
            //waitTime1.Text = waitingTime1.ToString() + " min";

            //lineNum2 = new Random().Next(0, 51);
            //people2.Text = lineNum2.ToString();
           // waitingTime2 = lineNum2 * new Random().Next(1, 7);
            //waitTime2.Text = waitingTime2.ToString() + " min";

            //lineNum3 = new Random().Next(0, 51);
            //people3.Text = lineNum3.ToString();
           // waitingTime3 = lineNum3 * new Random().Next(1, 7);
            //waitTime3.Text = waitingTime3.ToString() + " min";

            var location = new Location(21.705723, 72.998199);
            var otherLocation = new Location(22.3142, 73.1752);
            double distance = location.CalculateDistance(otherLocation, DistanceUnits.Kilometers);
            

            //distance1.Text = distance11.ToString() + " mi     away";
            //distance2.Text = distance22.ToString() + " mi     away";
            //distance3.Text = distance33.ToString() + " mi     away";
            //  entrytime = Math.Round(distance / speed, 2);

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
            speed = 5.0;

            // travel time 1
            entrytime1 = Math.Round(distance11/ speed, 2);
            waitTimeLeft1 = (int)(entrytime1);
            entrytime1 -= waitTimeLeft1;
            entrytime1 *= 100 + waitTimeLeft1 * 60;
            travel1.Text = entrytime1.ToString() + " min";


            // travel time 2
            entrytime2 = Math.Round(distance22 / speed, 2);
            waitTimeLeft2 = (int)(entrytime2);
            entrytime2 -= waitTimeLeft2;
            entrytime2 *= 100 + waitTimeLeft2 * 60;
            travel2.Text = entrytime2.ToString() + " min";


            // travel time 1
            entrytime3 = Math.Round(distance33 / speed, 2);
            waitTimeLeft3 = (int)(entrytime3);
            entrytime3 -= waitTimeLeft3;
            entrytime3 *= 100 + waitTimeLeft3 * 60;
            travel3.Text = entrytime3.ToString() + " min";

            //Calculate the estimated entry time, where if waitingTime > travelTime then entry time is current time + the waiting time (and vice versa)

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
            speed = 12.05;

            // travel time 1
            entrytime1 = Math.Round(distance11 / speed, 2);
            waitTimeLeft1 = (int)(entrytime1);
            entrytime1 -= waitTimeLeft1;
            entrytime1 *= 100 + waitTimeLeft1 * 60;
            travel1.Text = entrytime1.ToString() + " min";


            // travel time 2
            entrytime2 = Math.Round(distance22 / speed, 2);
            waitTimeLeft2 = (int)(entrytime2);
            entrytime2 -= waitTimeLeft2;
            entrytime2 *= 100 + waitTimeLeft2 * 60;
            travel2.Text = entrytime2.ToString() + " min";


            // travel time 1
            entrytime3 = Math.Round(distance33 / speed, 2);
            waitTimeLeft3 = (int)(entrytime3);
            entrytime3 -= waitTimeLeft3;
            entrytime3 *= 100 + waitTimeLeft3 * 60;
            travel3.Text = entrytime3.ToString() + " min";
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
            speed = 58.30;

            // travel time 1
            entrytime1 = Math.Round(distance11 / speed, 2);
            waitTimeLeft1 = (int)(entrytime1);
            entrytime1 -= waitTimeLeft1;
            entrytime1 *= 100 + waitTimeLeft1 * 60;
            travel1.Text = entrytime1.ToString() + " min";


            // travel time 2
            entrytime2 = Math.Round(distance22 / speed, 2);
            waitTimeLeft2 = (int)(entrytime2);
            entrytime2 -= waitTimeLeft2;
            entrytime2 *= 100 + waitTimeLeft2 * 60;
            travel2.Text = entrytime2.ToString() + " min";


            // travel time 1
            entrytime3 = Math.Round(distance33 / speed, 2);
            waitTimeLeft3 = (int)(entrytime3);
            entrytime3 -= waitTimeLeft3;
            entrytime3 *= 100 + waitTimeLeft3 * 60;
            travel3.Text = entrytime3.ToString() + " min";
        }

        private void Join_Line_1(object sender, EventArgs e)
        {
           // string name, phone, email;
           // name = Preferences.Get("name1", "name");
            //phone = Preferences.Get("phone1", "phone");
            //email = Preferences.Get("email1", "email");
            travelTime = waitingTime1 - 5;
            selectedTime = timePicker1.Time;
            //Navigation.PushAsync(new ConfirmatonPage(lineNum1, travel1.Text, waitingTime1,distance11, address11.Text, selectedTime));
            if (!double.TryParse(lat1, out double lat)) return;
            if (!double.TryParse(long1, out double lng)) return;
            Preferences.Set("latitude", lat);
            Preferences.Set("longitude", lng);
            setCustomerInfo();
            Navigation.PushAsync(new yourNumberPage(waitingTime1, lineNum1));
        }
        private void Join_Line_2(object sender, EventArgs e)
        {
            travelTime = waitingTime2 - 5;
            selectedTime = timePicker2.Time;
            if (!double.TryParse(lat2, out double lat)) return;
            if (!double.TryParse(long2, out double lng)) return;
            Preferences.Set("latitude", lat);
            Preferences.Set("longitude", lng);
            setCustomerInfo();
            Navigation.PushAsync(new yourNumberPage(waitingTime2, lineNum2));
        }
        private void Join_Line_3(object sender, EventArgs e)
        {
            travelTime = waitingTime3 - 5;
            selectedTime = timePicker3.Time;
            if (!double.TryParse(lat3, out double lat)) return;
            if (!double.TryParse(long3, out double lng)) return;
            Preferences.Set("latitude", lat);
            Preferences.Set("longitude", lng);
            setCustomerInfo();
            Navigation.PushAsync(new yourNumberPage(waitingTime3, lineNum2));
        }

        private void main_page5(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}