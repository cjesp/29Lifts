using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace _29Lifts.ViewModels.MapPage
{
    public class MapLyftIcon
    {
        public Geopoint Location { get; set; }
        public ImageSource ImageSource { get; set; }
        public Point Anchor { get; set; } = new Point(0.5, 0.5);
    }
}
