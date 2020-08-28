using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace WaitInPlace
{
    public class LocationInfo
    {

        public double current_lat { get; set; }
        public double current_long { get; set; }

        public int customer_id { get; set; }



    }
}
