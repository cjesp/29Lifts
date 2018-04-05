using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace _29Lifts.ViewModels.OrderPage
{
    public class AddressSearchResultViewModel
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public Geopoint Point { get; set; }
    }
}
