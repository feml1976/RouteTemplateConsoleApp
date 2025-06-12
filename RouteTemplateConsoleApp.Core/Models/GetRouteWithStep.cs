using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteTemplateConsoleApp.Core.Models
{
    public class GetRouteWithStep
    {
        public double Id { get; set; }
        public double RouteId { get; set; }
        public string Name { get; set; }
        public int NumberOfLegs { get; set; }
        public string DeparturePlace { get; set; }
        public string ArrivalPlace { get; set; }
        public string Code { get; set; }
        public DateTime TimeStamp { get; set; }
        public string UserName { get; set; }
        public double Metros { get; set; }
        public double Segundos { get; set; }
        public string Steps { get; set; }
        public int State { get; set; } = 1;
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
