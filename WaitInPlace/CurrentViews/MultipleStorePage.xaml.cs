using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaitInPlace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class MultipleStorePage : ContentPage
    {
        
        int waitingTime = 0;
        string wait31 = "", wait32 = "";
        double lat3, long3;// dist1;
        double dist;
        int v_uid = 0;
       // int tapCount = 0, 
        int i = 0;
        double travel = 0;
        //double totalwait = 0;
        TimeSpan de_selected_time, select_time;
        string de_selected_time2;
        double waitdouble, de_selected_time_min, de_selected_time_min2;
        ArrayList addressArray = new ArrayList();
        ArrayList uidArray = new ArrayList();
        ArrayList lineArray = new ArrayList();
        ArrayList waitArray = new ArrayList();
        ArrayList latArray = new ArrayList();
        ArrayList longArray = new ArrayList();
        ArrayList streetArray = new ArrayList(); 
        ArrayList apxArray = new ArrayList();
        ViewCell lastCell;
        double speed = 51.0;

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            StoreListView.BeginRefresh();
            MultipleStores multi = new MultipleStores();
            var labelHandler = (Label)sender;
            join_line1.IsEnabled = true;
            join_line1.BackgroundColor = Color.FromHex("#0071BC");
            join_line2.IsEnabled = true;
            join_line2.BackgroundColor = Color.FromHex("#0071BC");
            //schedule.IsVisible = true;
            for (int j = 0; j < i; j++)
            {
                if ((string)addressArray[j] == (string)labelHandler.Text || (string)streetArray[j] == (string)labelHandler.Text)
                {
                    multi.backcolor = Color.White;
                    multi.wait_time = "0";
                    Int32.TryParse((string)uidArray[j], out v_uid);
                    travel = Math.Round(GetTravelTime(getDistance((double)latArray[j], (double)longArray[j]), speed),0);
                    lat3 = (double)latArray[j];
                    long3 = (double)longArray[j];
                    join_line1.Text =  (DateTime.Now.AddMinutes(travel+ (double)waitArray[j])).ToString().Substring(9, 10).TrimStart('0');
                    de_selected_time_min = Math.Round(travel + (double)waitArray[j], 0);
                    de_selected_time_min2 = Math.Round(travel + (double)waitArray[j]+60, 0);
                    de_selected_time = TimeSpan.FromMinutes(de_selected_time_min);
                    join_line2.Text = (DateTime.Now.AddMinutes(travel + (double)waitArray[j] + 60)).ToString().Substring(9, 10).TrimStart('0');
                   select_time = DateTime.Now.TimeOfDay.Add(TimeSpan.FromMinutes(Math.Round(travel + (double)waitArray[j] + 60, 0)));
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
             
                foreach (var m in mult_stores["result"])
                {

                    streetArray.Add(m["street"].ToString() );
                    addressArray.Add(m["city"].ToString() + "," + m["state"].ToString() + "," + m["zip"].ToString());
                    latArray.Add(double.Parse(m["latitude"].ToString()));
                    longArray.Add(double.Parse(m["longitude"].ToString()));
                    lineArray.Add(m["queue_size"].ToString());
                    waitArray.Add(Get_waitingtime(m["wait_time"].ToString()));
                    uidArray.Add(m["venue_uid"].ToString());
                    waitdouble = Get_waitingtime(m["wait_time"].ToString());
                    apxArray.Add(( Math.Round(GetTravelTime(dist, speed), 0)));
           
                    i++;
                    if (v_uid != Int32.Parse(m["venue_uid"].ToString()))
                    {

                        this.MultStores.Add(new MultipleStores()
                        {
                            street = m["street"].ToString() ,
                            city   = m["city"].ToString() + "," + m["state"].ToString() + "," + m["zip"].ToString(),
                            Distance = getDistance(double.Parse(m["latitude"].ToString()), double.Parse(m["longitude"].ToString())).ToString() + " mi from my location",
                            queue_size = Int32.Parse(m["queue_size"].ToString()),
                            wait_time = Get_waitingtime(m["wait_time"].ToString()) + " min",
                            travel_time = Math.Round(GetTravelTime(dist, speed), 0).ToString() + " min",
                            color = Color.Black,
                            backcolor = Color.FromHex("#CCCCCC"),
                            image_line = new Image { Source = "{local:ImageResource WaitInPlace.WIP_Queue_Black.png}" },

                            apx_entry = now.AddMinutes(waitdouble + GetTravelTime(dist, speed)).ToString().Substring(9,10).TrimStart('0'),
                        });
                    }

                    else
                    {
                        this.MultStores.Add(new MultipleStores()
                        {
                            street = m["street"].ToString(),
                            city = m["city"].ToString() + "," + m["state"].ToString() + "," + m["zip"].ToString(),
                            Distance = getDistance(double.Parse(m["latitude"].ToString()), double.Parse(m["longitude"].ToString())).ToString() + " mi from my location",
                            queue_size = Int32.Parse(m["queue_size"].ToString()),
                            wait_time = Get_waitingtime(m["wait_time"].ToString()) + " min",
                            travel_time = Math.Round(GetTravelTime(dist, speed), 0).ToString() + " min",
                            color = Color.White,
                            backcolor = Color.FromHex("#0071BC"),
                            image_line = new Image { Source = "local:ImageResource WaitInPlace.WIP_Queue_White.png" },
                            apx_entry = now.AddMinutes(waitdouble + GetTravelTime(dist, speed)).ToString().Substring(9, 10).TrimStart('0'),
                        }) ;

                    }
                }
                StoreListView.ItemsSource = MultStores;

           

            }

        }

     
        private double Get_waitingtime(string wait1)
        {
            int h, m;
            bool isParsable1, isParsable2;
            if (wait1 == "00:00:00" || wait1 == "")
            {
                return 0;
            }

            wait31 = wait1.Substring(0, 2).Trim(':');
            wait32 = wait1.Substring(3, 3).Trim(':');
      
            isParsable1 = Int32.TryParse(wait31, out h);
            isParsable2 = Int32.TryParse(wait32, out m);
            waitingTime = h * 60 + m;
            if (isParsable1 && isParsable2)
                return (waitingTime);
            else
            {
                Console.WriteLine("Could not be parsed.");
                return 5;
            }
        }

        protected async Task setTicketInfo(int venue_uid)
        {
            TicketInfo newTicket = new TicketInfo();
            newTicket.t_user_id = Preferences.Get("customer_id", 0);
            newTicket.t_uid = venue_uid;
            Preferences.Set("v_uid", venue_uid);
            DateTime now = DateTime.Now.ToLocalTime();
            string currentTime = (string.Format("{0}", now));
            newTicket.commute_time = TimeSpan.FromMinutes(travel).ToString();
            // newTicket.t_scheduled_time = selected_time.ToString();
            var newTicketJSONString = JsonConvert.SerializeObject(newTicket);
            var content = new StringContent(newTicketJSONString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://61vdohhos4.execute-api.us-west-1.amazonaws.com/dev/api/v2/add_ticket");
            request.Method = HttpMethod.Post;
            request.Content = content;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
        }

        double GetTravelTime(double dist, double speed)
        {
            int traveltimeh = 0;
            double traveltime = 0.0;
            double travel = 0;

            traveltime = Math.Round(dist / speed, 2);
            traveltimeh = (int)(traveltime);
            traveltime -= traveltimeh;
            traveltime *= 100 + (traveltimeh * 60);
            travel = (int)traveltime;

            if (travel > 40)
            {
                return 1;//travel / 8;
            }
            else
            {
                return 1;// travel / 5;
            }
        }

        double CarSpeed(double dist)
        {
            if (dist < 10.0)
                return 30.0;
            else
                return 51.0;
        }

        public MultipleStorePage(string pageName)
        {
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
           
            dist = Math.Round(custadd.CalculateDistance(venadd1, DistanceUnits.Miles), 2);
            return dist;

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
            TimeSpan check_time ;
            var getVal = sender as Switch;
            if (getVal.IsToggled)
            {
                check_time = DateTime.Now.TimeOfDay.Add(de_selected_time);
                if (check_time >= TimeSpan.Parse("23:59:59"))
                    check_time -= TimeSpan.Parse("23:59:59");
                selected_time.Time = check_time;    
                schduled_label.IsVisible = false;
                selected_time.IsVisible = true;
                join_line2.IsEnabled = true;
                join_line2.BackgroundColor = Color.FromHex("#0071BC");
            }
            else
            {
                schduled_label.IsVisible = true;
                selected_time.IsVisible = false;
                join_line2.IsEnabled = false;
                join_line2.BackgroundColor = Color.Transparent;
            }
        }

        private void join_line2_Clicked(object sender, EventArgs e)
        {
           // de_selected_time2 = DateTime.Now.TimeOfDay.Subtract(select_time).ToString().Substring(0, 7);
           // double.TryParse(de_selected_time2.ToString(), out de_selected_time_min2);
            Preferences.Set("latitude", lat3);
            Preferences.Set("longitude", long3);
            int vuid = 0;
            for (int j = 0; j < i; j++)
            {
                Int32.TryParse((string)uidArray[j], out vuid);
                if (vuid == v_uid)
                {


                    setTicketInfo(v_uid);
                    Navigation.PushAsync(new yourNumberPage(de_selected_time_min2.ToString(), (string)lineArray[j], v_uid, (string)addressArray[j], PageName.Text));
                    break;
                }
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

        }

        private void selected_time_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            TimeSpan zero_time = TimeSpan.ParseExact("00:00:00", "c", null);
            select_time = selected_time.Time;
            
            join_line2.Text = select_time.ToString().Substring(0,8);
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
            StoreListView.EndRefresh();
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
        }

       
        private void Join_Line(object sender, EventArgs e)
        {
            Preferences.Set("latitude", lat3);
            Preferences.Set("longitude", long3);
            int vuid = 0;
            for (int j = 0; j < i; j++)
            {
                Int32.TryParse((string)uidArray[j], out vuid);
                if (vuid == v_uid)
                {


                    setTicketInfo(v_uid);
                    Navigation.PushAsync(new yourNumberPage(de_selected_time_min.ToString(), (string)lineArray[j], v_uid, (string)addressArray[j], PageName.Text));
                    break;
                }
            }
        }


        private void main_page5(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}