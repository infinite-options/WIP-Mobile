using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaitInPlace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataDisplayPage : ContentPage
    {
        public ObservableCollection<UserData> UserInfo = new ObservableCollection<UserData>();
        protected async Task GetUserData()
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://61vdohhos4.execute-api.us-west-1.amazonaws.com/dev/api/v2/all_users");
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var userString = await content.ReadAsStringAsync();
                JObject user_info = JObject.Parse(userString);
                this.UserInfo.Clear();

                //Console.WriteLine("user_info['result']: " + user_info["result"]);
                //Console.WriteLine("user_info: " + user_info);
                
                foreach (var m in user_info["result"])
                {
                    this.UserInfo.Add(new UserData()
                        {
                            name = m["name"].ToString(),
                            email = m["email"].ToString(),
                            phone = m["phone"].ToString(),
                            address = m["address"].ToString(),
                            eta = m["eta"].ToString(),
                            transport = m["transport"].ToString(),
                        });
                }
                UserDataListView.ItemsSource = UserInfo;
            }
            //First Attempt
            //HttpClient client = new HttpClient();
            //var response = await client.GetStringAsync("https://61vdohhos4.execute-api.us-west-1.amazonaws.com/dev/api/v2/all_users");
            //var userData = JsonConvert.DeserializeObject<List<UserInfo>>(response);
            //UserDataListView.ItemsSource = userData;
        }
        public DataDisplayPage()
        {
            InitializeComponent();
            GetUserData();
            UserDataListView.RefreshCommand = new Command(() =>
            {
                GetUserData();
                UserDataListView.IsRefreshing = false;
            });
        }
    }
}