using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.Devices.Geolocation;

namespace _29Lifts.ViewModels.OrderPage
{
    public class DestinationViewModel : ViewModelBase
    {
        Geopoint _DestinationPoint = default(Geopoint);
        public Geopoint DestinationPoint { get { return _DestinationPoint; } set { Set(ref _DestinationPoint, value); } }

        public string Title { get; set; } = "Add destination";
        public string FormattedAddress { get; set; } = "Add destination";
    }
}
