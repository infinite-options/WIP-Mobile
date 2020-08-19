using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace WaitInPlace
{
    public class TicketInfo
    {
        public int t_user_id { get; set; }
        public int t_uid { get; set; }
        public string t_entry_time { get; set; }
    }
}
