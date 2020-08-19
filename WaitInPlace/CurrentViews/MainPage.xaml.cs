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
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace WaitInPlace
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        protected async Task setCustomerInfo()
        {
            UserInfo newUser = new UserInfo();
            newUser.name = Preferences.Get("name", "");
            newUser.email = Preferences.Get("email", "");
            newUser.phone = Preferences.Get("phone", "");

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

        
        public MainPage()
        {
            InitializeComponent();
        }

        private void To_Venue_Page(object sender, EventArgs e)
        {
            Preferences.Set("name", name.Text);
            Preferences.Set("email", email.Text);
            Preferences.Set("phone", phone.Text);
            setCustomerInfo();
            Navigation.PushAsync(new VenuePage());
        }

        private void To_Data_Page(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DataDisplayPage());
        }
    }
}
