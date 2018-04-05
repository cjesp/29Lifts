using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Media;

namespace _29Lifts.ViewModels.RideHistoryVM
{
    public class ActiveRide : RootRide
    {
        public string RideId { get; internal set; }
        public string RideInProgress { get; set; }
        public string RideStatus { get; set; }
        public ImageSource RideTypeImageSrc { get; set; }
    }
}
