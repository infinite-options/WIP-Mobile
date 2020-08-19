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
    public partial class GroceryPage : ContentPage
    {
        public ObservableCollection<GroceryCat> GroceryStores = new ObservableCollection<GroceryCat>();

        protected async Task GetGroceryStores()
        {
            var request = new HttpRequestMessage();
            string venueCat = "\"" + Preferences.Get("venueCat","") + "\"";
            request.RequestUri = new Uri("https://61vdohhos4.execute-api.us-west-1.amazonaws.com/dev/api/v2/get_venue/" + venueCat);
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var userString = await content.ReadAsStringAsync();
                JObject grocery_stores = JObject.Parse(userString);
                this.GroceryStores.Clear();

                //Console.WriteLine("user_info['result']: " + grocery_stores["result"]);
                //Console.WriteLine("user_info: " + grocery_stores);

                string[] catArray = { "", "", "", "", "", "" };
                string[] idArray = { "", "", "", "", "", "" };
                int i = 0;
                foreach (var m in grocery_stores["result"])
                {
                    catArray[i] = m["venue_name"].ToString();
                    idArray[i] = m["venue_id"].ToString();
                    /*this.VenueCat.Add(new VenueCategories()
                    {
                        category = m["category"].ToString(),
                    });*/
                    i++;
                }
                Store1.Text = catArray[0];
                Store2.Text = catArray[1];
                Store3.Text = catArray[2];
                Store4.Text = catArray[3];
                Store5.Text = catArray[4];
                Store6.Text = catArray[5];
                id1.Text = idArray[0];
                id2.Text = idArray[1];
                id3.Text = idArray[2];
                id4.Text = idArray[3];
                id5.Text = idArray[4];
                id6.Text = idArray[5];
            }
            
        }

        public GroceryPage()
        {
            InitializeComponent();
             GetGroceryStores();
             catLabel.Text = Preferences.Get("venueCat", "");
        }

        private void To_Safeway_page(object sender, EventArgs e)
        { 
            if (Store1.Text != "")
            {
                Preferences.Set("venue_id", id1.Text);
                Navigation.PushAsync(new MultipleStorePage(Store1.Text));
                
            }

        }
        private void To_TraderJ_page(object sender, EventArgs e)
        {
            if (Store2.Text != "")
            {
                Preferences.Set("venue_id", id2.Text);
                Navigation.PushAsync(new MultipleStorePage(Store2.Text));
            }
        }
        private void To_Sprout_page(object sender, EventArgs e)
        {
            if (Store3.Text != "")
            {
                Preferences.Set("venue_id", id3.Text);
                Navigation.PushAsync(new MultipleStorePage(Store3.Text));
            }
        }
        private void To_WholeFoods_page(object sender, EventArgs e)
        {
            if (Store4.Text != "")
            {
                Preferences.Set("venue_id", id4.Text);
                Navigation.PushAsync(new MultipleStorePage(Store4.Text));
            }
        }
        private void To_Lucky_page(object sender, EventArgs e)
        {
            if (Store5.Text != "")
            {
                Preferences.Set("venue_id", id5.Text);
                Navigation.PushAsync(new MultipleStorePage(Store5.Text));
            }
        }
        private void To_NobHill_page(object sender, EventArgs e)
        {
            if (Store6.Text != "")
            {
                Preferences.Set("venue_id", id6.Text);
                Navigation.PushAsync(new MultipleStorePage(Store6.Text));
            }
        }
        private void main_page3(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}