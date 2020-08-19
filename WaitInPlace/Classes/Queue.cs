using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace WaitInPlace
{
  public class Queue
    {
        public int queue_len { get; set; }
        public string wait_time { get; set; }
    }
}
