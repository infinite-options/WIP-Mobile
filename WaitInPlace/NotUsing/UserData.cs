using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace WaitInPlace
{
    public class UserData
    {
            public int customer_id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string address { get; set; }
            public string eta { get; set; }
            public string transport { get; set; }

    }
}
