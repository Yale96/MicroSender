using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoSend.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Application { get; set; }
        public string TimeStamp { get; set; }
        public string LogLevel { get; set; }
        public string Location { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            return "Message id: " + Id + ", application: " + Application + ", Time: " + TimeStamp 
                + ", Log level: " + ", Location: " + Location + ", Content: " + Content;
        }
    }
}