using _29Lifts.Api.Rides;
using _29Lifts.Api.Rides.Models;
using _29Lifts.Navigation;
using _29Lifts.Services.SettingsServices;
using _29Lifts.ViewModels.MapPage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.Devices.Geolocation;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace _29Lifts.ViewModels
{
    public class RidePageViewModel : ViewModelBase
    {
        private string RideId { get; set; }
        private Ride Ride { get; set; }
        public ILyftRidesApi _lyftRidesApi { get; set; }
        public DispatcherTimer RideDetailsTimer { get; set; }
        private LyftRideState RideState;
        private string _driverPhoneNumber;
        private int _ETAcount;
        public ObservableCollection<MapLyftIcon> Driver { get; set; }
        private SettingsService _settings;

        // events 
        public delegate void RideIsPendingEventHandler(Geopoint location, Geopoint destination);
        public event RideIsPendingEventHandler UpdateMap;

        public delegate void RideIsAcceptedEventHandler(Geopoint location, Geopoint rideLocation);
        public event RideIsAcceptedEventHandler RideIsAcceptedEvent;

        public delegate void RideHasArrivedEventHandler(Geopoint location, Geopoint rideLocation);
        public event RideHasArrivedEventHandler RideHasArrivedEvent;

        public delegate void PickedUpEventHandler(Geopoint location, Geopoint destination);
        public event PickedUpEventHandler PickedUpEvent;

        ImageSource _CarImageSrc = default(ImageSource);
        public ImageSource CarImageSrc { get { return _CarImageSrc; } set { Set(ref _CarImageSrc, value); } }

        Uri _ProfileImageSrc = default(Uri);
        public Uri ProfileImageSrc { get { return _ProfileImageSrc; } set { Set(ref _ProfileImageSrc, value); } }

        string _DestinationText = default(string);
        public string DestinationText { get { return _DestinationText; } set { Set(ref _DestinationText, value); } }

        string _PickupText = default(string);
        public string PickupText { get { return _PickupText; } set { Set(ref _PickupText, value); } }

        Geopoint _DestinationPoint = default(Geopoint);
        public Geopoint DestinationPoint { get { return _DestinationPoint; } set { Set(ref _DestinationPoint, value); } }

        Geopoint _LocationPoint = default(Geopoint);
        public Geopoint LocationPoint { get { return _LocationPoint; } set { Set(ref _LocationPoint, value); } }

        Geopoint _PickupPoint = default(Geopoint);
        public Geopoint PickupPoint { get { return _PickupPoint; } set { Set(ref _PickupPoint, value); } }

        Geopoint _CenterPoint = default(Geopoint);
        public Geopoint CenterPoint { get { return _CenterPoint; } set { Set(ref _CenterPoint, value); } }
        
        string _CarModel = default(string);
        public string CarModel { get { return _CarModel; } set { Set(ref _CarModel, value); } }
        
        string _LicensePlate = default(string);
        public string LicensePlate { get { return _LicensePlate; } set { Set(ref _LicensePlate, value); } }
        
        string _DriverName = default(string);
        public string DriverName { get { return _DriverName; } set { Set(ref _DriverName, value); } }
        
        string _DriverRating = default(string);
        public string DriverRating { get { return _DriverRating; } set { Set(ref _DriverRating, value); } }
        
        string _StatusText = default(string);
        public string StatusText { get { return _StatusText; } set { Set(ref _StatusText, value); } }

        Visibility _ShowProfilePic = default(Visibility);
        public Visibility ShowProfilePic { get { return _ShowProfilePic; } set { Set(ref _ShowProfilePic, value); } }

        bool _ShowCarDetails = default(bool);
        public bool ShowCarDetails { get { return _ShowCarDetails; } set { Set(ref _ShowCarDetails, value); } }

        bool _Spinner = default(bool);
        public bool Spinner { get { return _Spinner; } set { Set(ref _Spinner, value); } }

        Uri _ProfileImageUserSrc = default(Uri);
        public Uri ProfileImageUserSrc { get { return _ProfileImageUserSrc; } set { Set(ref _ProfileImageUserSrc, value); } }

        public RidePageViewModel()
        {
            _lyftRidesApi = new LyftRidesApi();
            PickupPoint = new Geopoint(new BasicGeoposition() { Latitude = 32.7555, Longitude = -97.3308 });
            LocationPoint = new Geopoint(new BasicGeoposition() { Latitude = 32.7555, Longitude = -97.3308 });
            DestinationPoint = new Geopoint(new BasicGeoposition() { Latitude = 32.7555, Longitude = -97.3308 });
            CenterPoint = new Geopoint(new BasicGeoposition() { Latitude = 32.7555, Longitude = -97.3308 });
            CarModel = "Car model";
            LicensePlate = "XXXXXXX";
            DriverName = "Driver name";
            DriverRating = "Rating";
            StatusText = "CONTACTING NEARBY DRIVERS";
            ShowProfilePic = Visibility.Collapsed;
            RideState = LyftRideState.Pending;
            _driverPhoneNumber = "";
            _ETAcount = 0;
            ShowCarDetails = false;
            Spinner = true;
            Driver = new ObservableCollection<MapLyftIcon>();
            _settings = SettingsService.Instance;
            ProfileImageSrc = new Uri("ms-appx://29Lifts/Assets/image.jpg");
            ProfileImageUserSrc = _settings.ProfileImageSrc;

            RideDetailsTimer = new DispatcherTimer();
            RideDetailsTimer.Interval = TimeSpan.FromMilliseconds(10000.0);
            RideDetailsTimer.Tick += RideDetailsTimerUpdate;
            RideDetailsTimer.Start();
        }

        private async void RideDetailsTimerUpdate(object sender, object e)
        {
            RideDetails details = null;

            try
            {
                details = await _lyftRidesApi.RideDetails(RideId);
            }
            catch (Exception k)
            {
                await ShowError(k, string.Empty);
                return;
            }

            Debug.WriteLine($"ride id: {details.RideId}");

            switch (details.Status)
            {
                case LyftRideState.Pending:
                    break;
                case LyftRideState.Accepted:
                    RideAccepted(details);
                    break;
                case LyftRideState.Arrived:
                    ArrivedState(details);
                    break;
                case LyftRideState.PickedUp:
                    Pickedup(details);
                    break;
                case LyftRideState.DroppedOff:
                    DroppedOff(details);
                    break;
                case LyftRideState.Canceled:
                    Canceled(details);
                    break;
                case LyftRideState.Error:
                    // todo
                    break;
                default:
                    break;
            }

        }

        private async Task ShowError(Exception k, string message)
        {
            var dialog = new MessageDialog($"{message} Error: {k.Message}");
            await dialog.ShowAsync();
        }

        private async void Canceled(RideDetails details)
        {
            RideDetailsTimer.Stop();
            Spinner = false;

            string message = "Your ride was canceled by Lyft";

            if (!string.IsNullOrEmpty(details.CanceledBy))
            {
                message = details.CanceledBy == "driver" ?
                    "Your ride was canceled by the driver." :
                    "Your ride was canceled due to no avilable drivers";
            }

            var dialog = new MessageDialog(
                message,
                "Ride canceled");

            await dialog.ShowAsync();

            _settings.ActiveRideId = string.Empty;
            NavigationService.Navigate(typeof(Views.MapPage));
            
        }

        private void DroppedOff(RideDetails details)
        {
            RideState = LyftRideState.DroppedOff;
            RideDetailsTimer.Stop();

            var parameter = new PaymentPageNavigationModel()
            {
                RideId = details.RideId,
                DriverName = details.Driver.FirstName,
                LineItems = details.LineItems,
                ProfileImageSrc = details.Driver.ImageUrl
            };

            _settings.ActiveRideId = string.Empty;
            NavigationService.Navigate(typeof(Views.PaymentPage), parameter);
        }

        private void Pickedup(RideDetails details)
        {
            RideState = LyftRideState.PickedUp;

            if (details.Location != null)
            {
                var destinationLocation = new Geopoint(new BasicGeoposition()
                {
                    Latitude = details.Destination.Latitude,
                    Longitude = details.Destination.Longitude,
                });

                var driverLocation = new Geopoint(new BasicGeoposition()
                {
                    Latitude = details.Location.Lat,
                    Longitude = details.Location.Lng
                });

                UpdateMap(driverLocation, destinationLocation);
            }

            if (details.Destination != null && details.Destination.EtaSeconds != null)
            {
                _ETAcount = (int)details.Destination.EtaSeconds / 60;
                StatusText = $"YOU'LL ARRIVE IN {_ETAcount} MIN";
            }
            else
            {
                StatusText = "NO VALID ETA AT THIS TIME";
            }
        }

        private void ArrivedState(RideDetails details)
        {
            StatusText = "YOUR DRIVER IS HERE";
            RideState = LyftRideState.Arrived;

            if (details.Location != null)
            {
                var pickupLocation = new Geopoint(new BasicGeoposition()
                {
                    Latitude = details.Origin.Latitude,
                    Longitude = details.Origin.Longitude - 0.0002,
                });

                var driverLocation = new Geopoint(new BasicGeoposition()
                {
                    Latitude = details.Location.Lat,
                    Longitude = details.Location.Lng
                });

                UpdateMap(pickupLocation, driverLocation);
            }

            if (details.Destination != null && details.Destination.EtaSeconds != null)
            {
                _ETAcount = (int)details.Destination.EtaSeconds / 60;
            }
        }

        private void RideAccepted(RideDetails details)
        {
            if (details.Origin != null && details.Origin.EtaSeconds != null)
            {
                StatusText = $"YOUR DRIVER WILL ARRIVE IN {(details.Origin.EtaSeconds + 30) / 60} MIN";
            }
            else
            {
                StatusText = "YOUR RIDE HAS BEEN ACCEPTED";
            }

            if (details.Location != null)
            {
                var pickupLocation = new Geopoint(new BasicGeoposition()
                {
                    Latitude = details.Origin.Latitude,
                    Longitude = details.Origin.Longitude,
                });

                var driverLocation = new Geopoint(new BasicGeoposition()
                {
                    Latitude = details.Location.Lat,
                    Longitude = details.Location.Lng
                });

                UpdateMap(pickupLocation, driverLocation);
            }

            if (RideState != LyftRideState.Accepted)
            {
                RideState = LyftRideState.Accepted;
                ShowCarDetails = true;
                Spinner = false;


                // driver related stuff
                if (details.Driver != null)
                {
                    if (!string.IsNullOrEmpty(details.Driver.FirstName))
                        DriverName = details.Driver.FirstName;

                    if (!string.IsNullOrEmpty(details.Driver.Rating))
                        DriverRating = $"{details.Driver.Rating}";
                    else
                        DriverRating = "No rating";

                    if (!string.IsNullOrEmpty(details.Driver.ImageUrl))
                    {
                        ProfileImageSrc = new Uri(details.Driver.ImageUrl);
                        ShowProfilePic = Visibility.Visible;
                    }

                    if (!string.IsNullOrEmpty(details.Driver.PhoneNumber))
                    {
                        _driverPhoneNumber = details.Driver.PhoneNumber;
                    }
                }

                if (details.Vehicle != null)
                {
                    var modelSet = false;
                    if (!string.IsNullOrEmpty(details.Vehicle.Model))
                    {
                        CarModel = details.Vehicle.Model;
                        modelSet = true;
                    }

                    if (!modelSet && !string.IsNullOrEmpty(details.Vehicle.Make))
                        CarModel = details.Vehicle.Make;

                    if (!string.IsNullOrEmpty(details.Vehicle.LicensePlate))
                        LicensePlate = details.Vehicle.LicensePlate;

                    if (!string.IsNullOrEmpty(details.Vehicle.ImageUrl))
                    {
                        CarImageSrc = new BitmapImage(new Uri(details.Vehicle.ImageUrl));
                    }
                }
            }

        }

        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var navParameter = parameter as RidePageNavigationModel;
            if (navParameter != null)
            {
                DestinationText = navParameter.DestinationText;
                PickupText = navParameter.PickupText;
                DestinationPoint = new Geopoint(navParameter.Destination);
                PickupPoint = new Geopoint(navParameter.PickupLocation);
                CenterPoint = new Geopoint(new BasicGeoposition()
                {
                    Latitude = navParameter.LocationPoint.Latitude - 0.0004,
                    Longitude = navParameter.LocationPoint.Longitude
                });
                LocationPoint = new Geopoint(navParameter.LocationPoint); 
                RideId = navParameter.Ride.RideId.ToString();
                Ride = navParameter.Ride;

            }

            var id = parameter as string;
            if (id != null)
            {
                RideDetails details = null;
                try
                {
                    details = await _lyftRidesApi.RideDetails(id);
                }
                catch (Exception e)
                {
                    var dialog = new MessageDialog($"Error: {e.Message}."
                        + "Try and restart the app and try again", "Something went wrong");
                    await dialog.ShowAsync();
                }

                if (details != null)
                {
                    DestinationText = details.Destination.Address;
                    PickupText = details.Origin.Address;
                    DestinationPoint = new Geopoint(new BasicGeoposition()
                    {
                        Latitude = details.Destination.Latitude,
                        Longitude = details.Destination.Longitude
                    });

                    PickupPoint = new Geopoint(new BasicGeoposition()
                    {
                        Latitude = details.Origin.Latitude,
                        Longitude = details.Origin.Longitude
                    });

                    LocationPoint = new Geopoint(new BasicGeoposition()
                    {
                        Latitude = details.Origin.Latitude,
                        Longitude = details.Origin.Longitude
                    });

                    CenterPoint = new Geopoint(new BasicGeoposition()
                    {
                        Latitude = details.Origin.Latitude - 0.0004,
                        Longitude = details.Origin.Longitude
                    });

                    RideId = details.RideId;
                }

            }

            UpdateMap(PickupPoint, DestinationPoint);
            await StartLocation();
            _settings.ShowHamburgerButton = false;
            ProfileImageUserSrc = _settings.ProfileImageSrc;

            await Task.CompletedTask;
        }


        private async Task StartLocation()
        {
            var access = await Geolocator.RequestAccessAsync();
            var dialog = new MessageDialog($"Error: This app requires your location to function properly");

            switch (access)
            {
                case GeolocationAccessStatus.Allowed:

                    var geolocator = new Geolocator() { DesiredAccuracy = PositionAccuracy.High, ReportInterval = 5000 };
                    geolocator.PositionChanged += PositionChanged;
                    break;

                case GeolocationAccessStatus.Unspecified:
                case GeolocationAccessStatus.Denied:
                default:
                    await dialog.ShowAsync();
                    return;
            }
        }

        private void PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Dispatcher.DispatchAsync(() =>
            {
                var updatedLocation = args.Position;
                LocationPoint = updatedLocation.Coordinate.Point;
            });
        }

        public void UserTapped(object sender, TappedRoutedEventArgs e)
        {
            Views.Shell.HamburgerMenu.IsOpen = true;
        }

        public async void ContactDriverTapped(object sender, TappedRoutedEventArgs e)
        {
            if (RideState == LyftRideState.Pending || RideState == LyftRideState.Canceled)
            {
                var message = RideState == LyftRideState.Pending ?
                    "No contact information is available at this time since your ride is still pending." :
                    "The ride has been canceled. No contact information available";

                var dialog = new MessageDialog(message, "No contact information available");
                await dialog.ShowAsync();
            }
            else
            {
                var uri = new Uri($"tel:{_driverPhoneNumber}");
                await Launcher.LaunchUriAsync(uri);
            }

        }

        public async void SendETA(object sender, TappedRoutedEventArgs e)
        {
            if (RideState == LyftRideState.Arrived || RideState == LyftRideState.PickedUp)
            {
                var uri = new Uri($"ms-chat:?Body=I'll arrive at {DestinationText} in {_ETAcount} min.");
                await Launcher.LaunchUriAsync(uri);
            }
            else
            {
                var message = "No ETA is available at this time. Once your driver has arrived ETA information will be available.";

                var dialog = new MessageDialog(message, "No ETA available");
                await dialog.ShowAsync();
            }

        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> pageState, bool suspending)
        {
            RideDetailsTimer.Stop();
            await Task.CompletedTask;
        }

        public async void CancelRideTapped(object sender, TappedRoutedEventArgs e)
        {
            var dialog = new MessageDialog(
                "Are you sure you want to cancel this ride?",
                "Cancel ride");

            dialog.Commands.Add(new UICommand("Yes") { Id = 0 });
            dialog.Commands.Add(new UICommand("No") { Id = 1 });
            dialog.DefaultCommandIndex = 1;
            dialog.CancelCommandIndex = 1;
            
            var result = await dialog.ShowAsync();
            
            // user has confirmed they want to cancel
            if ((int)result.Id == 0)
            {
                Cancellation cancellation = null;

                try
                {
                    cancellation = await _lyftRidesApi.CancelRide(RideId);
                }
                catch (Exception k)
                {
                    await ShowError(k, "Couldn't cancel your ride.");
                    return;
                }
                // this means we received a cancellation token (verify)
                if (cancellation != null)
                {
                    var dialogWithToken = new MessageDialog(
                        $"You'll be charged a cancellation fee ({cancellation.Amount:0C}). Do you still want to cancel your ride?",
                        "Cancellation fee");

                    dialog.Commands.Add(new UICommand("Yes") { Id = 0 });
                    dialog.Commands.Add(new UICommand("No") { Id = 1 });
                    dialog.DefaultCommandIndex = 1;
                    dialog.CancelCommandIndex = 1;

                    var resultWithToken = await dialog.ShowAsync();

                    // means the user accepted the cancellation fee
                    if ((int)resultWithToken.Id == 0)
                    {
                        var ignore = await _lyftRidesApi.CancelRide(RideId, cancellation.Token);
                    }
                }

                NavigationService.Navigate(typeof(Views.MapPage));

            }
        }
    }
}

