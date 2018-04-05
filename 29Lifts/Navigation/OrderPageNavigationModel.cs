using _29Lifts.Api.Public.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace _29Lifts.Navigation
{
    public class OrderPageNavigationModel
    {
        public BasicGeoposition CenterPoint { get; set; }
        public BasicGeoposition LocationPoint { get; set; }
        public string LocationText { get; set; }
        public string MinuteCounter { get; set; }
        public LyftRideTypeEnum RideType { get; set; }
    }
}
