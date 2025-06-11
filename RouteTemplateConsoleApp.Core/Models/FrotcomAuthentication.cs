using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteTemplateConsoleApp.Core.Models
{
    public class FrotcomAuthentication
    {
        public string token { get; set; }
        public string provider { get; set; }
        public List<object> announcements { get; set; }
    }
}
