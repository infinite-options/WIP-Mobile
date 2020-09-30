using GoogleApi.Entities.Maps.StaticMaps.Request;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
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
       int waitingTime=0,waitingTime3=0,lineNum3=0;
        string  wait11 = "", wait12 = "", wait21 = "", wait22 = "", wait31 = "", wait32 = "";
        double lat1,lat2,lat3,long1,long2,long3,dist1,dist2,dist3;
        double dist;
        int v_uid = 0;
        int tapCount = 0, i = 0;
        double travel = 0;
        double totalwait =0;
        double waitdouble;
        ArrayList addressArray = new ArrayList();
        ArrayList uidArray = new ArrayList();
        ArrayList lineArray = new ArrayList();
        ArrayList waitArray = new ArrayList();
        ArrayList latArray = new ArrayList();
        ArrayList longArray = new ArrayList();
        ViewCell lastCell;
        double speed = 51.0;

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Console.WriteLine("I am alive");
            StoreListView.BeginRefresh();
            string wait1="0:16:40",selectedTime ="";
            MultipleStores multi = new MultipleStores();
            var labelHandler = (Label)sender;
            Console.WriteLine(labelHandler.Text);
            join_line.IsEnabled = true;
            join_line.BackgroundColor = Color.FromHex("#0071BC");
            for (int j = 0; j < i; j++)
            {
                if ((string)addressArray[j] == (string)labelHandler.Text)
                {
                    multi.backcolor = Color.White;
                    multi.wait_time = "0";
                    Int32.TryParse((string)uidArray[j], out v_uid);
                    travel = GetTravelTime(getDistance((double)latArray[j], (double)longArray[j]), speed);
                   // Console.WriteLine("travel@click is " + travel.ToString() );
                 
                }
               
            }

            StoreListView.EndRefresh();
        }

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

                DateTime now = DateTime.Now.ToLocalTime();
                totalwait = waitdouble + (GetTravelTime(dist, speed))/60;
                Console.WriteLine("the total wait is " + totalwait);
               // DateTime time = now.TimeOfDay;

                /*  string[] streetArray = { "", "", "" };
                  string[] cityArray = { "", "", "" };
                  string[] stateArray = { "", "", ""};
                  string[] zipArray = { "", "", ""};
                  double[] latArray = {0,0,0};
                  double[] longArray = { 0, 0, 0};
                  string[] lineArray = { "", "", ""};
                  string[] waitArray = { "", "", ""};*/

                foreach (var m in mult_stores["result"])
                {
                    
                    addressArray.Add( m["street"].ToString()+","+m["city"].ToString()+","+ m["state"].ToString()+ "," + m["zip"].ToString());
                    latArray.Add( double.Parse(m["latitude"].ToString()));
                    longArray.Add( double.Parse(m["longitude"].ToString()));
                    lineArray.Add (m["queue_size"].ToString());
                    waitArray.Add(Get_waitingtime(m["wait_time"].ToString()));
                    uidArray.Add( m["venue_uid"].ToString());
                    Double.TryParse(Get_waitingtime(m["wait_time"].ToString()), out waitdouble);
                    i++;
                    if (v_uid != Int32.Parse(m["venue_uid"].ToString()))
                    {

                       // var image = new Image { Source = "{local:ImageResource WaitInPlace.WIP_Queue_Black.png}" };
                        this.MultStores.Add(new MultipleStores()
                        {
                            street = m["street"].ToString() + "," + m["city"].ToString() + "," + m["state"].ToString() + "," + m["zip"].ToString(),
                            Distance = getDistance(double.Parse(m["latitude"].ToString()), double.Parse(m["longitude"].ToString())).ToString() + " mi from my location",
                            queue_size = Int32.Parse(m["queue_size"].ToString()),
                            wait_time = Get_waitingtime(m["wait_time"].ToString()) + " min",
                            travel_time = GetTravelTime(dist, speed).ToString() + " min",
                            color = Color.Black,
                            backcolor = Color.FromHex("#CCCCCC"),
                            image_line = new Image { Source = "{local:ImageResource WaitInPlace.WIP_Queue_Black.png}" },

                            apx_entry = now.AddMinutes(waitdouble + GetTravelTime(dist, speed)).ToString().Substring(9,9),
                        }) ;
                        Console.WriteLine(now);
                    }
                    
                    else
                    {
                        //var image = new Image { Source = "WIP_Queue_White.png" };
                        this.MultStores.Add(new MultipleStores()
                        {
                            street = m["street"].ToString() + "," + m["city"].ToString() + "," + m["state"].ToString() + "," + m["zip"].ToString(),
                            Distance = getDistance(double.Parse(m["latitude"].ToString()), double.Parse(m["longitude"].ToString())).ToString()+ " mi from my location",
                            queue_size = Int32.Parse(m["queue_size"].ToString()),
                            wait_time = Get_waitingtime(m["wait_time"].ToString()) + " min",
                            travel_time = GetTravelTime(dist, speed).ToString() + " min",
                            color = Color.White,
                            backcolor = Color.FromHex("#0071BC"),
                            image_line = new Image { Source = "local:ImageResource WaitInPlace.WIP_Queue_White.png" },
                            apx_entry = now.AddMinutes(waitdouble + GetTravelTime(dist, speed)).ToString().Substring(9,9)
                        });

                    }
                }
                StoreListView.ItemsSource = MultStores;

              /*  address11.Text = streetArray[0] + ", " + cityArray[0] + ", " + stateArray[0] + ", " + zipArray[0];
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
                Preferences.Set("venue_uid3", uidArray[2]);*/

            
            }
          
        }

        private string Get_waitingtime(string wait1)
        {
            int h, m;
            bool isParsable1, isParsable2;
           if(wait1 == "00:00:00" || wait1 == "")
            {
                return "0";
            }
            
            wait31 = wait1.Substring(0, 2).Trim(':');
            wait32 = wait1.Substring(3, 3).Trim(':');
          //  Console.WriteLine(wait31);
          //  Console.WriteLine(wait32);

            isParsable1 = Int32.TryParse(wait31, out h);
            isParsable2 = Int32.TryParse(wait32, out m);
            waitingTime = h * 60 + m;
            if (isParsable1 && isParsable2)
                return (waitingTime).ToString();
            else
            {
                Console.WriteLine("Could not be parsed.");
                return "5";
            }
        }

        void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            Console.WriteLine("here now"); 
            tapCount++;
            //var imageSender = (Image)sender;
            var storeview = (Image)sender;
            // watch the monkey go from color to black&white!
            if (tapCount % 2 == 0)
            {
                storeview.Source = "local:ImageResource WaitInPlace.WIP_Queue_Black.png";
            }
            else
            {
                storeview.Source = "WIP_Queue_Black.png";
            }
        }


        protected async Task setTicketInfo(int venue_uid)
        {
            double comm_time = 0;
            TicketInfo newTicket = new TicketInfo();
            newTicket.t_user_id = Preferences.Get("customer_id", 0);
            newTicket.t_uid = venue_uid;
            Preferences.Set("v_uid", venue_uid);
            //string now = DateTime.Now.TimeOfDay.ToString("h:mm:ss tt");
            DateTime now = DateTime.Now.ToLocalTime();
            string currentTime = (string.Format("{0}", now));
            Console.WriteLine("The current time is {0}", now);
            newTicket.commute_time = "00:00:00";//TimeSpan.FromMinutes(travel).ToString();
            // newTicket.t_scheduled_time = selected_time.ToString();
            Console.WriteLine("the comm is :" + newTicket.commute_time);
            var newTicketJSONString = JsonConvert.SerializeObject(newTicket);
            var content = new StringContent(newTicketJSONString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://61vdohhos4.execute-api.us-west-1.amazonaws.com/dev/api/v2/add_ticket");
            request.Method = HttpMethod.Post;
            request.Content = content;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
        }

        double GetTravelTime(double dist,double speed)
        {
            Console.WriteLine("dist is " + dist.ToString());
            Console.WriteLine("speed is " + speed.ToString());
            int traveltimeh=0;
            double traveltime=0.0;
            double travel=0;

            traveltime = Math.Round(dist / speed, 2);
            traveltimeh = (int)(traveltime);
            traveltime -= traveltimeh;
            traveltime *= 100 +( traveltimeh * 60);
            travel = (int)traveltime;
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

            StoreListView.RefreshCommand = new Command(() =>
            {
                GetMultStores();
                StoreListView.IsRefreshing = false;
            });
        }
        double getDistance(double lati, double longi)
        {

            var custadd = new Location(Preferences.Get("lati", 0.0), Preferences.Get("long", 0.0));
            var venadd1 = new Location(lati, longi);
            Console.WriteLine("vlat:{0},vlong:{1},clat:{2},clong:{3}", lati, longi, Preferences.Get("lati", 0.0), Preferences.Get("long", 0.0));


            dist = Math.Round(custadd.CalculateDistance(venadd1, DistanceUnits.Miles), 2);
            Console.WriteLine("distance " + dist);
            return dist;

        }
        void getDistance(double lat1, double lat2, double lat3, double long1, double long2, double long3)
        {
           // Console.WriteLine("hello");
            //for store1
            var custadd = new Location(Preferences.Get("lati", 0.0), Preferences.Get("long", 0.0));
            var venadd1 = new Location(lat1, long1);
            Console.WriteLine("vlat:{0},vlong:{1},clat:{2},clong:{3}", lat1, long1, Preferences.Get("lati", 0.0), Preferences.Get("long", 0.0));


            dist1 = Math.Round(custadd.CalculateDistance(venadd1, DistanceUnits.Miles), 2);
            Console.WriteLine("distance " + dist1);
          //  distance1.Text = dist1.ToString() + " mi away";

            //for store2
            var venadd2 = new Location(lat2, long2);
            dist2 = Math.Round(custadd.CalculateDistance(venadd2, DistanceUnits.Miles), 2);
         //   distance2.Text = dist2.ToString() + " mi away";

            //for store3
            var venadd3 = new Location(lat3, long3);
            dist3 = Math.Round(custadd.CalculateDistance(venadd3, DistanceUnits.Miles), 2);
        //   distance3.Text = dist3.ToString() + " mi away";
        }

       
        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.LightGray;
            }
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var getVal = sender as Switch;
            if (getVal.IsToggled)
            {
                App.Current.Resources["LabelColor"] = "#0071BC  ";
            }
            else
            {
                App.Current.Resources["LabelColor"] = "#CCCCCC";
            }
        }
        private void Walk_Selected(object sender, EventArgs e)
        {
            StoreListView.BeginRefresh();
            Walk.BackgroundColor = Color.FromHex("#0071BC");
            Walk.BorderColor = Color.FromHex("#0071BC");
            Bus.BackgroundColor = Color.White;
            Bus.BorderColor = Color.Black;
            Car.BackgroundColor = Color.White;
            Car.BorderColor = Color.Black;
            Preferences.Set("MOT", "walking");
            speed = 3.1;
            StoreListView.EndRefresh();

            // travel1.Text = GetTravelTime(dist1,walk);
            // travel2.Text = GetTravelTime(dist2, walk);
            //  travel3.Text = GetTravelTime(dist3, walk);
        }

        private void Bus_Selected(object sender, EventArgs e)
        {
            StoreListView.BeginRefresh();
            Bus.BackgroundColor = Color.FromHex("#0071BC");
            Bus.BorderColor = Color.FromHex("#0071BC");
            Walk.BackgroundColor = Color.White;
            Walk.BorderColor = Color.Black;
            Car.BackgroundColor = Color.White;
            Car.BorderColor = Color.Black;
            Preferences.Set("MOT", "transit");
           speed = 13.6;
            Console.WriteLine("hello");
            StoreListView.EndRefresh();
            // travel1.Text = GetTravelTime(dist1, bus);
            //  travel2.Text = GetTravelTime(dist2, bus);
            //travel3.Text = GetTravelTime(dist3, bus);
        }

        private void Car_Selected(object sender, EventArgs e)
        {
            StoreListView.BeginRefresh();
            Car.BackgroundColor = Color.FromHex("#0071BC");
            Car.BorderColor = Color.FromHex("#0071BC");
            Walk.BackgroundColor = Color.White;
            Walk.BorderColor = Color.Black;
            Bus.BackgroundColor = Color.White;
            Bus.BorderColor = Color.Black;
            Preferences.Set("MOT", "driving");
            speed = 51.0;
            StoreListView.EndRefresh();
            // travel1.Text = GetTravelTime(dist1, 51);// CarSpeed(dist1));
            // travel2.Text = GetTravelTime(dist2, CarSpeed(dist2));
            // travel3.Text = GetTravelTime(dist3, CarSpeed(dist3));
        }

          /* private void Join_Line_1(object sender, EventArgs e)
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
          */
        private void Join_Line(object sender, EventArgs e)
        {
           // TimeSpan selectedTime = timePicker3.Time;
          //  if (!double.TryParse(lat3, out double lat)) return;
          //  if (!double.TryParse(long3, out double lng)) return;
            Preferences.Set("latitude", lat3); 
            Preferences.Set("longitude", long3);
            int vuid = 0;
            for (int j = 0; j < i; j++)
            {
                Int32.TryParse((string)uidArray[j], out vuid);
                if (vuid == v_uid)
                {


                    setTicketInfo(v_uid);
                    Navigation.PushAsync(new yourNumberPage(totalwait.ToString(), (string)lineArray[j], v_uid, (string)addressArray[j], PageName.Text));
                    break;
                }
            }
            /*
            int v_uid3 = int.Parse(Preferences.Get("venue_uid1", ""));
            double wait3 = waitingTime3;
            setTicketInfo(v_uid3);
            Navigation.PushAsync(new yourNumberPage(waitingTime3.ToString(), lineNum3.ToString(),v_uid3, "", PageName.Text));*/
        }


        private void main_page5(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}