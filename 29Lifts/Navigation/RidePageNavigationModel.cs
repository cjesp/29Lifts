using _29Lifts.Api.Rides.Models;
using _29Lifts.ViewModels.OrderPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace _29Lifts.Navigation
{
    public class RidePageNavigationModel
    {
        public string DestinationText { get; set; }
        public string PickupText { get; set; }
        public BasicGeoposition Destination { get; set; }
        public BasicGeoposition PickupLocation { get; set; }
        public BasicGeoposition LocationPoint { get; set; }
        public Ride Ride { get; set; }
    }
}
