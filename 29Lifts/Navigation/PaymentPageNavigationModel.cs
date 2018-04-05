using _29Lifts.Api.Rides.Models.SubModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Lifts.Navigation
{
    public class PaymentPageNavigationModel
    {
        public string RideId { get; set; }
        public string DriverName { get; set; }
        public string ProfileImageSrc { get; set; }
        public IList<LineItem> LineItems { get; set; }
    }
}
