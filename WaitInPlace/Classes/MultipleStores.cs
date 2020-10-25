using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace WaitInPlace
{
  public  class MultipleStores
    {
        internal int j;

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
        public Color color { get; internal set; }
        public object image_line { get; internal set; }
        public Color backcolor { get; internal set; }
        public string apx_entry { get; internal set; }
    }
}
