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
        ViewCell lastCell;
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

        private void main_page2(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

        async void display_category(Object sender, EventArgs e) 
        {
            var buttonClickHandler = (Button)sender;
            Preferences.Set("venueCat", buttonClickHandler.Text);
            Navigation.PushAsync(new GroceryPage());
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}