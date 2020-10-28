using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace WaitInPlace
{
    public partial class MainPage : ContentPage
    {
        string customerId = "";
        protected async Task setCustomerInfo()
        {
            UserInfo newUser = new UserInfo();
            newUser.name = Preferences.Get("name", "");
            newUser.email = Preferences.Get("email", "");
            newUser.phone = Preferences.Get("phone", "");
            newUser.current_lat = "0.0";
            newUser.current_long = "0.0";
            var newUserJSONString = JsonConvert.SerializeObject(newUser);
            var content = new StringContent(newUserJSONString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://61vdohhos4.execute-api.us-west-1.amazonaws.com/dev/api/v2/add_customer");
            request.Method = HttpMethod.Post;
            request.Content = content;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
        }

        public ObservableCollection<GetId> GettingId = new ObservableCollection<GetId>();
        protected async Task getCustomerId()
        {

            var request = new HttpRequestMessage();
            string custPhone = "\""+Preferences.Get("phone", "")+"\"";
            request.RequestUri = new Uri("https://61vdohhos4.execute-api.us-west-1.amazonaws.com/dev/api/v2/get_customer_id/" + custPhone);
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
                Preferences.Set("customer_id", int.Parse(customerId));
            }
        }

        protected async Task setLocationInfo()
        {
            LocationInfo newlocation = new LocationInfo();
            newlocation.current_lat =  Preferences.Get("lati", 0.0);
            newlocation.current_long = Preferences.Get("long", 0.0);
            newlocation.customer_id = Preferences.Get("customer_id", 0);
            var newUserJSONString = JsonConvert.SerializeObject(newlocation);
            var content = new StringContent(newUserJSONString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://61vdohhos4.execute-api.us-west-1.amazonaws.com/dev/api/v2/update_customer_coords");
            request.Method = HttpMethod.Put;
            request.Content = content;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
        }


        private async Task getLoaction()
        {
                var locate = await Geolocation.GetLastKnownLocationAsync();

                if (locate == null)
                {
                    locate = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }
                Preferences.Set("lati", locate.Latitude);
                Preferences.Set("long", locate.Longitude);
            
           
        }

        private async Task initializingAsync()
        {

            await getLoaction();
           
                await setCustomerInfo();
                await getCustomerId();
            await setLocationInfo();

        }
        public MainPage()
        {

            InitializeComponent();
           
        }

        void Handle_emailChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var Email = email.Text;

            var emailPattern = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
            if (Regex.IsMatch(Email, emailPattern))
            {
                ErrorLabel.IsVisible = false;
                ErrorLabel.Text = "Email is valid";
            }
            else
            {
                ErrorLabel.IsVisible = true;
                ErrorLabel.Text = "EMail is InValid";
            }
        }
        void Handle_phoneChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var Phone = phone.Text;

            var phonePattern = @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"; 
            if (Regex.IsMatch(Phone, phonePattern))
            {
                wait_in_place.IsEnabled = true;
                wait_in_place.BackgroundColor = Color.FromHex("#0071BC");
                ErrorLabel.IsVisible = false;
                ErrorLabel.Text = "phone number is valid";
            }
            else
            {
                ErrorLabel.IsVisible = true;
                ErrorLabel.Text = "phone number is InValid";
            }
        }
        private void To_Venue_Page(object sender, EventArgs e)
        {
            Preferences.Set("name", name.Text);
            Preferences.Set("email", email.Text);
            Preferences.Set("phone", phone.Text);
            initializingAsync();
            Navigation.PushAsync(new VenuePage());
        }
    }
}
