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
        public ObservableCollection<GroceryCat> GroceryStores = new ObservableCollection<GroceryCat>();
       // string[] catArray = { "", "", "", "", "", "" };
       // string[] idArray = { "", "", "", "", "", "" };
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

                //Console.WriteLine("user_info['result']: " + grocery_stores["result"]);
                //Console.WriteLine("user_info: " + grocery_stores);


                foreach (var m in grocery_stores["result"])
                {
                    catArray.Add(m["venue_name"].ToString());
                    idArray.Add(m["venue_id"].ToString());
                    //  catArray[i] = m["venue_name"].ToString();
                    // idArray[i] = m["venue_id"].ToString();
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
                    // Console.WriteLine("the venue id is " + venue_id);
                     Navigation.PushAsync(new MultipleStorePage(buttonClickHandler.Text));
                     break;
                 }

             }
            }

        }

        /* private void To_TraderJ_page(object sender, EventArgs e)
{
    if (Store2.Text != "")
    {
        Preferences.Set("venue_id", idArray[1]);
        Navigation.PushAsync(new MultipleStorePage(Store2.Text));
    }
}
private void To_Sprout_page(object sender, EventArgs e)
{
    if (Store3.Text != "")
    {
        Preferences.Set("venue_id", idArray[2]);
        Navigation.PushAsync(new MultipleStorePage(Store3.Text));
    }
}
private void To_WholeFoods_page(object sender, EventArgs e)
{
    if (Store4.Text != "")
    {
        Preferences.Set("venue_id", idArray[3]);
        Navigation.PushAsync(new MultipleStorePage(Store4.Text));
    }
}
private void To_Lucky_page(object sender, EventArgs e)
{
    if (Store5.Text != "")
    {
        Preferences.Set("venue_id", idArray[4]);
        Navigation.PushAsync(new MultipleStorePage(Store5.Text));
    }
}
private void To_NobHill_page(object sender, EventArgs e)
{
    if (Store6.Text != "")
    {
        Preferences.Set("venue_id", idArray[5
            ]);
        Navigation.PushAsync(new MultipleStorePage(Store6.Text));
    }
}*/
        private void main_page3(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}