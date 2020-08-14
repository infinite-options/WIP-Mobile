using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace WaitInPlace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void To_Venue_Page(object sender, EventArgs e)
        {
            Preferences.Set("ETA", Eta.Text);
            Navigation.PushAsync(new VenuePage());
        }
        private void main_page1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}