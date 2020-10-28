using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections;
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
        ViewCell lastCell;
        public ObservableCollection<GroceryCat> GroceryStores = new ObservableCollection<GroceryCat>();
        ArrayList catArray = new ArrayList();
        ArrayList idArray = new ArrayList();

        int i = 0;
        protected async Task GetGroceryStores()
        {
            var request = new HttpRequestMessage();
            string venueCat = "\"" + Preferences.Get("venueCat", "") + "\"";
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

         

                foreach (var m in grocery_stores["result"])
                {
                    catArray.Add(m["venue_name"].ToString());
                    idArray.Add(m["venue_id"].ToString());
                    this.GroceryStores.Add(new GroceryCat()
                    {
                        venue_name = m["venue_name"].ToString(),
                    });
                    i++;
                }
            }
            GroceryListView.ItemsSource = GroceryStores;
        }

        public GroceryPage()
        {
            InitializeComponent();
            GetGroceryStores();
            catLabel.Text = Preferences.Get("venueCat", "");
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

        private void To_Safeway_page(object sender, EventArgs e)
        {
            GroceryCat groceries = new GroceryCat();
            var buttonClickHandler = (Button)sender;
            string venue_id = "";
            if (buttonClickHandler.Text != "")
            {
                for (int j = 0; j < i; j++)
                {
                 if (buttonClickHandler.Text != catArray[j])
                 {
                     continue;
                 }
                else
                 { 

                     venue_id = (string)idArray[j];
                     Preferences.Set("venue_id", venue_id);
                     Navigation.PushAsync(new MultipleStorePage(buttonClickHandler.Text));
                     break;
                 }

             }
            }

        }
        private void main_page3(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}