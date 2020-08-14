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
        public string entry_time { get; set; } 
        public string exit_time { get; set; } 
        public int venue_id { get; set; } 
        public string street { get; set; } 
        public string city { get; set; } 
        public string state { get; set; } 
        public string zip { get; set; } 
        public string lattitude { get; set; } 
        public string longitude { get; set; } 
        public string wait_time { get; set; } 
        public int queue_size { get; set; } 
    }
}
