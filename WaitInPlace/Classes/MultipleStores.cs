using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;


namespace WaitInPlace
{
  public  class MultipleStores
    {
        public int venue_uid { get; set; } 
        public int venue_id { get; set; } 
        public string street { get; set; } 
        public string city { get; set; } 
        public string state { get; set; } 
        public string zip { get; set; } 
        public string latitude { get; set; } 
        public string longitude { get; set; }

        public int queue_size { get; set; }
        public string wait_time { get; set; }

        public string address { get; set; }
        public string Distance { get; set; }
        public string travel_time { get; internal set; }
        public string walk_time { get; internal set; }
        public string car_time { get; internal set; }
        public string transit_time { get; internal set; }
    }
}
