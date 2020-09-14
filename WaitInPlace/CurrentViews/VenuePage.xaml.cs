using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public partial class VenuePage : ContentPage
    {
        public ObservableCollection<VenueCategories> VenueCat = new ObservableCollection<VenueCategories>();

        ArrayList catArray = new ArrayList();

        protected async Task GetVenueCat()
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://61vdohhos4.execute-api.us-west-1.amazonaws.com/dev/api/v2/get_categories");
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var userString = await content.ReadAsStringAsync();
                JObject venue_cat = JObject.Parse(userString);
                this.VenueCat.Clear();

                //Console.WriteLine("user_info['result']: " + venue_cat["result"]);
                //Console.WriteLine("user_info: " + venue_cat);

               // string[] catArray = { "", "", "", "", "", "" };
                int i = 0;
                foreach (var m in venue_cat["result"])
                {
                    catArray.Add(m["category"].ToString());
                    this.VenueCat.Add(new VenueCategories()
                    {
                        category = m["category"].ToString(),
                        
                    });;
                    i++;
                }
              /*  cat1.Text = (string)catArray[0];
                cat2.Text = (string)catArray[1];
                cat3.Text = (string)catArray[2];
                cat4.Text = (string)catArray[3];
                cat5.Text = (string)catArray[4];
                cat6.Text = catArray[5];*/
                VenueCatListView.ItemsSource = VenueCat;
            }
        }
        public VenuePage()
        {
            InitializeComponent();
            GetVenueCat();
            VenueCatListView.RefreshCommand = new Command(() =>
            {
                GetVenueCat();
                VenueCatListView.IsRefreshing = false;
            });
        }

        private void main_page2(object sender, EventArgs e)
        {
          //  Preferences.Set("venueCat", cat1.Text);
            Navigation.PushAsync(new MainPage());
        }

        async void display_category(Object sender, EventArgs e) 
        {
            var buttonClickHandler = (Button)sender;
          /*  Console.WriteLine("the text of the button"+ buttonClickHandler.Text);

            StackLayout ParentStackLayout = (StackLayout)buttonClickHandler.Parent;
            Button newcat1 = (Button)ParentStackLayout.Children[0];
            string name = newcat1.Text;
            Console.WriteLine("the 1st name is"+name);*/
            Preferences.Set("venueCat", buttonClickHandler.Text);
            Navigation.PushAsync(new GroceryPage());
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}