﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WaitInPlace.Classes;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroFiveBit.Utils;

namespace WaitInPlace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BarcodePage : ContentPage
    {
        int yourNum;
        int waitTimeOrig2;
        string yourNumStr;
        int origNum;
        static Countdown countdown;
        

        protected async Task setEntryTime(int venue_uid)
        {
           EntryInfo nexit = new EntryInfo();
            nexit.user_id = Preferences.Get("customer_id", 0);
            nexit.venue_uid = venue_uid;
            DateTime now = DateTime.Now.ToLocalTime().ToUniversalTime();
            string currentTime = (string.Format("{0}", now));
            nexit.entry_time = currentTime.Substring(9, 9);
            var newExitJSONString = JsonConvert.SerializeObject(nexit);
            var content = new StringContent(newExitJSONString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://61vdohhos4.execute-api.us-west-1.amazonaws.com/dev/api/v2/entry_time_button");
            request.Method = HttpMethod.Put;
            request.Content = content;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
        }

        protected async Task setExitTime(int venue_uid)
        {
              ExitInfo newexit = new ExitInfo();
              newexit.usr_id = Preferences.Get("customer_id", 0);
              newexit.vnu_uid = venue_uid;
              DateTime now = DateTime.Now.ToLocalTime().ToUniversalTime();
              string currentTime = (string.Format("{0}", now));
              newexit.ext_time = currentTime.Substring(9, 9);
              var newExitJSONString = JsonConvert.SerializeObject(newexit);
              var content = new StringContent(newExitJSONString, Encoding.UTF8, "application/json");
              var request = new HttpRequestMessage();
              request.RequestUri = new Uri("https://61vdohhos4.execute-api.us-west-1.amazonaws.com/dev/api/v2/update_queue");
              request.Method = HttpMethod.Put;
              request.Content = content;
              var client = new HttpClient();
              HttpResponseMessage response = await client.SendAsync(request);
        }

        
        public BarcodePage(double waitTimeOrig, int placeInLine,string pagename)
        {
            InitializeComponent();
            yourNum = Preferences.Get("token_id", 0);
            origNum = placeInLine;
            waitTimeOrig2 = (int)waitTimeOrig;
            countdown = new Countdown();
            countdown.StartUpdating(300);
            cdLabel.SetBinding(Label.TextProperty,
                new Binding("RemainTime", BindingMode.Default, new CountdownConverter()));
            cdLabel.BindingContext = countdown;
             address1.Text=Preferences.Get("add", "");
            PageName.Text = pagename;
            
            Device.StartTimer(TimeSpan.FromMinutes(5), () =>
            {
                if (origNum < yourNum)
                {
                    origNum += 5;
                    return true;
                }
               
                return false; // True = Repeat again, False = Stop the timer
            });
        }
        private void bump_in(object sender, EventArgs e)
        {
            yourNum += 5;
            yourNumStr = yourNum.ToString();
            int venue = Preferences.Get("v_uid", 0 );
            Navigation.PushAsync(new yourNumberPage(waitTimeOrig2.ToString(), yourNum.ToString(), venue,address1.Text,PageName.Text));
        }

        private void exit_Clicked(object sender, EventArgs e)
        {
            int v_uid = Preferences.Get("v_uid", 1);
            setExitTime(v_uid);
            DisplayAlert("Exit Store", "You are exitting the store. Thanks for using WIP!.", "Continue");
            Navigation.PushAsync(new MainPage());
        }

        private void entry_Clicked(object sender, EventArgs e)
        {
           
            int v_uid = Preferences.Get("v_uid", 1);
            setEntryTime(v_uid);
            entry.BackgroundColor = Color.Transparent;
            entry.TextColor = Color.Transparent;
        }
    }
}