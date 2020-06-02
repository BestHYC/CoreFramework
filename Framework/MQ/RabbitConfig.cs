using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    public class RabbitConfig
    {
        public String Host { get; set; }
        public Int32 Port { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public String VHost { get; set; }
    }
}
