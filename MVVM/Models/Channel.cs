using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamScheduler
{
    public class Channel
    {

        public string Name { get; set; }
        public string Url { get; set; }

        public string Description { get; set; }

        public Channel(string name, string url) {
            Name = name;
            Url = url;
        }
        public Channel(string name, string url, string description) {
            Name = name;
            Url = url;
            Description = description;
        }
    }
}
