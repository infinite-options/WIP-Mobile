using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace WaitInPlace
{
    public class UserInfo
    {
        public int customer_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
       // public string address { get; set; }
        public string current_lat { get; set; }
        public string current_long { get; set; }
    }
}
