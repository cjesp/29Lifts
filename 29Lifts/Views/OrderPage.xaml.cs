using _29Lifts.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class OrderPage : Page
    {
        private OrderPageViewModel _vm; 

        public OrderPage()
        {
            this.InitializeComponent();
            _vm = DataContext as OrderPageViewModel;
            _vm.DestinationChosen += DestinationChosen;
            _vm.ShowLocation += DestinationChosen;
        }

        private async void DestinationChosen(Geopoint location, Geopoint destination)
        {
            var map = TheMap;

            var locations = new List<BasicGeoposition>();
            locations.Add(location.Position);
            locations.Add(destination.Position);
            var bounds = GeoboundingBox.TryCompute(locations);
            await map.TrySetViewBoundsAsync(bounds, new Thickness(10.0, 30.0, 10.0, (map.ActualHeight / 2.0) * 1.05), MapAnimationKind.Bow);
            
        }
    }
}
