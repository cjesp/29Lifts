using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace _29Lifts.ViewModels.RideHistoryVM
{
    public class InactiveRide : RootRide
    {
        public Uri ProfileImageSrc { get; set; }
        public string DateAndTime { get; set; }
        public string Price { get; set; }
        public string Ride { get; set; }
        public string RideId { get; internal set; }
    }
}
