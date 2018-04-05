using _29Lifts.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace _29Lifts.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RidePage : Page
    {
        private RidePageViewModel _vm;

        public RidePage()
        {
            this.InitializeComponent();
            _vm = DataContext as RidePageViewModel;
            _vm.UpdateMap += UpdateMap;
            _vm.RideIsAcceptedEvent += RideIsAcceptedEvent;
            _vm.RideHasArrivedEvent += RideHasArrivedEvent;
            _vm.PickedUpEvent += PickedUpEvent;
        }

        private void PickedUpEvent(Geopoint location, Geopoint destination)
        {
            throw new NotImplementedException();
        }

        private void RideHasArrivedEvent(Geopoint location, Geopoint rideLocation)
        {
            throw new NotImplementedException();
        }

        private void RideIsAcceptedEvent(Geopoint location, Geopoint rideLocation)
        {
            throw new NotImplementedException();
        }

        private async void UpdateMap(Geopoint location, Geopoint destination)
        {
            var map = TheMap;

            var locations = new List<BasicGeoposition>();
            if (location.Position.Latitude == destination.Position.Latitude &&
                location.Position.Longitude == destination.Position.Longitude)
            {
                // hack to circumvent cases where position are the same
                locations.Add(RemoveAltitude(location));
                locations.Add(new BasicGeoposition()
                {
                    Latitude = location.Position.Latitude - 0.000000001,
                    Longitude = location.Position.Longitude - 0.0002
                });
            }
            else
            {
                locations.Add(RemoveAltitude(location));
                locations.Add(RemoveAltitude(destination));
            }

            var bounds = GeoboundingBox.TryCompute(locations);
            bool result = false;

            if (map.LoadingStatus == MapLoadingStatus.Loading || map.LoadingStatus == MapLoadingStatus.DataUnavailable)
            {
                while (map.LoadingStatus == MapLoadingStatus.Loading || map.LoadingStatus == MapLoadingStatus.DataUnavailable)
                {
                    await Task.Delay(50);
                }
                result = await map.TrySetViewBoundsAsync(bounds, new Thickness(10.0, 30.0, 10.0, (map.ActualHeight / 2.0) * 1.05), MapAnimationKind.Bow);

            }
            else
            {
                result = await map.TrySetViewBoundsAsync(bounds, new Thickness(10.0, 30.0, 10.0, (map.ActualHeight / 2.0) * 1.05), MapAnimationKind.Bow);
            }

        }

        private BasicGeoposition RemoveAltitude(Geopoint location)
        {
            return new BasicGeoposition()
            {
                Latitude = location.Position.Latitude,
                Longitude = location.Position.Longitude,
            };
        }
    }
}
