using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace WaitInPlace.Classes
{
    public class EntryInfo
    {
        public int user_id { get; set; }
        public int venue_uid { get; set; }
        //public int wait_time { get; set; }
        public string entry_time { get; set; }

    }
}
