using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using _29Lifts.Models;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Services.Maps;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Input;
using _29Lifts.Api.Public;
using _29Lifts.Api.Public.models;
using System.Collections.ObjectModel;
using _29Lifts.ViewModels.MapPage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using _29Lifts.Navigation;
using _29Lifts.Services.SettingsServices;
using Windows.UI.ViewManagement;

namespace _29Lifts.ViewModels
{
    // maps key
    // AuWKpDAtdfdkO_If4n1l1JWb-TMNZG3YUZAkpJJuz_bv86IEuxjUkLsq3TVhxZ3e

    public class MapPageViewModel : ViewModelBase
    {
        private ILyftPublicAPI _lyftPublicApi;
        private SettingsService _settings;
        public ObservableCollection<MapLyftIcon> MapIcons { get; set; } = new ObservableCollection<MapLyftIcon>();
        private DispatcherTimer _mapChangedTimer;
        private DispatcherTimer _mapUpdateDriversETAs;
        public ImageSource LyftImageSrc { get; set; } = new BitmapImage(new Uri("ms-appx://29Quizlet/Assets/Lyft.png"));
        public ImageSource LyftPlusImageSrc { get; set; } = new BitmapImage(new Uri("ms-appx://29Quizlet/Assets/LyftPlus.png"));
        public ImageSource LyftLineImageSrc { get; set; } = new BitmapImage(new Uri("ms-appx://29Quizlet/Assets/LyftLine.png"));
        public ImageSource LyftPremierImageSrc { get; set; } = new BitmapImage(new Uri("ms-appx://29Quizlet/Assets/LyftPremier.png"));
        private List<LyftRideTypeEnum> RideTypesAvailable { get; set; } = new List<LyftRideTypeEnum>();


        ImageSource _SelectedRideImageSrc = default(ImageSource);
        public ImageSource SelectedRideImageSrc { get { return _SelectedRideImageSrc; } set { Set(ref _SelectedRideImageSrc, value); } }

        public MapPageViewModel()
        {
            _settings = SettingsService.Instance;
            BasicGeoposition cityPosition = new BasicGeoposition() { Latitude = 47.604, Longitude = -122.329 };
            CenterPoint = new Geopoint(_settings.LastLocation);
            LocationPoint = new Geopoint(_settings.LastLocation);
            _settings.ShowHamburgerButton = false;
            
            // init settings
            MinuteCounter = "10";
            MinuteCounterLyft = "11";
            MinuteCounterPlus = "14";
            LyftBackground = new SolidColorBrush(Colors.WhiteSmoke);
            LyftPlusBackground = new SolidColorBrush(Colors.White);
            LyftLineBackground = new SolidColorBrush(Colors.White);
            LyftPremierBackground = new SolidColorBrush(Colors.White);
            LocationText = "";
            SelectedRideImageSrc = LyftImageSrc;
            ProfilePictureSrc = _settings.ProfileImageSrc;

            ZoomLevel = 17.0d;

            _mapChangedTimer = new DispatcherTimer();
            _mapChangedTimer.Tick += MapChangedTimer_Tick;
            _mapChangedTimer.Interval = TimeSpan.FromMilliseconds(500.0);

            _mapUpdateDriversETAs = new DispatcherTimer();
            _mapUpdateDriversETAs.Tick += _mapUpdateDriversETAs_Tick;
            _mapUpdateDriversETAs.Interval = TimeSpan.FromMilliseconds(15000.0);

            _lyftPublicApi = new LyftPublicApi();

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {

            }
        }

        private async void _mapUpdateDriversETAs_Tick(object sender, object e)
        {
            await UpdateNearbyDrivers();
            await UpdateETAs();
            await UpdateRideTypesAvailable();
        }

        private async Task UpdateRideTypesAvailable()
        {
            var rides = await _lyftPublicApi.GetRideTypes(CenterPoint.Position.Latitude, CenterPoint.Position.Longitude);

            if (rides.Rides == null)
                return;

            RideTypesAvailable.Clear();

            foreach (var rideType in rides.Rides)
            {
                RideTypesAvailable.Add(rideType.Ride_Type);
            }

            if (!RideTypesAvailable.Any(x => x == LyftRideTypeEnum.Line))
                SetLyftRideAsUnavilable(LyftRideTypeEnum.Line);
            if (!RideTypesAvailable.Any(x => x == LyftRideTypeEnum.Lyft))
                SetLyftRideAsUnavilable(LyftRideTypeEnum.Lyft);
            if (!RideTypesAvailable.Any(x => x == LyftRideTypeEnum.Plus))
                SetLyftRideAsUnavilable(LyftRideTypeEnum.Plus);
            if (!RideTypesAvailable.Any(x => x == LyftRideTypeEnum.Premier))
                SetLyftRideAsUnavilable(LyftRideTypeEnum.Premier);
        }

        private void SetLyftRideAsUnavilable(LyftRideTypeEnum rideType)
        {
            switch (rideType)
            {
                case LyftRideTypeEnum.Unknown:
                    break;
                case LyftRideTypeEnum.Lyft:
                    LyftBackground = new SolidColorBrush(Colors.LightPink);
                    MinuteCounterLyft = "n/a";
                    break;
                case LyftRideTypeEnum.Plus:
                    LyftPlusBackground = new SolidColorBrush(Colors.LightPink);
                    MinuteCounterPlus = "n/a";
                    break;
                case LyftRideTypeEnum.Line:
                    LyftLineBackground = new SolidColorBrush(Colors.LightPink);
                    MinuteCounterLine = "n/a";
                    break;
                case LyftRideTypeEnum.Premier:
                    LyftPremierBackground = new SolidColorBrush(Colors.LightPink);
                    MinuteCounterPremier = "n/a";
                    break;
                default:
                    break;
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            _settings.ShowHamburgerButton = false;
            if (suspensionState.Any())
            {
                var centerPointLat = (double)suspensionState["CenterPoint.Lat"];
                var centerPointLng = (double)suspensionState["CenterPoint.Lng"];

                CenterPoint = new Geopoint(new BasicGeoposition()
                {
                    Latitude = centerPointLat,
                    Longitude = centerPointLng
                });

                var locationPointLat = (double)suspensionState["LocationPoint.Lat"];
                var locationPointLng = (double)suspensionState["LocationPoint.Lng"];

                LocationPoint = new Geopoint(new BasicGeoposition()
                {
                    Latitude = locationPointLat,
                    Longitude = locationPointLng
                });

                LocationText = (string)suspensionState[nameof(LocationText)];
                var str = (string)suspensionState[nameof(LyftRideType)];
                LyftRideTypeEnum rideType;
                var success = Enum.TryParse<LyftRideTypeEnum>(str, out rideType);
                if (success)
                    LyftRideType = rideType;
            }

            // if we came back from orderpage we dont look for a location  
            if (!suspensionState.Any())
                GpsTapped(null, null);
            
            _mapUpdateDriversETAs.Start();

            ProfilePictureSrc = _settings.ProfileImageSrc;

            await Task.CompletedTask;
        }


        public void TheMap_CenterChanged(MapControl sender, object args)
        {
            LocationText = "Updating location...";
            _mapChangedTimer.Stop();
            _mapChangedTimer.Start();
        }

        private void MapChangedTimer_Tick(object sender, object e)
        {
            UpdateAddress();
            _mapChangedTimer.Stop();
        }

        string _LocationText = default(string);
        public string LocationText { get { return _LocationText; } set { Set(ref _LocationText, value); } }

        SolidColorBrush _LyftPlusBackground = default(SolidColorBrush);
        public SolidColorBrush LyftPlusBackground { get { return _LyftPlusBackground; } set { Set(ref _LyftPlusBackground, value); } }

        SolidColorBrush _LyftBackground = default(SolidColorBrush);
        public SolidColorBrush LyftBackground { get { return _LyftBackground; } set { Set(ref _LyftBackground, value); } }
        
        SolidColorBrush _LyftLineBackground = default(SolidColorBrush);
        public SolidColorBrush LyftLineBackground { get { return _LyftLineBackground; } set { Set(ref _LyftLineBackground, value); } }
        
        SolidColorBrush _LyftPremierBackground = default(SolidColorBrush);
        public SolidColorBrush LyftPremierBackground { get { return _LyftPremierBackground; } set { Set(ref _LyftPremierBackground, value); } }

        bool _LyftCarSelectorOpen = false;
        public bool LyftCarSelectorOpen { get { return _LyftCarSelectorOpen; } set { Set(ref _LyftCarSelectorOpen, value); } }

        bool _ShowTypeOfRide = true;
        public bool ShowTypeOfRide { get { return _ShowTypeOfRide; } set { Set(ref _ShowTypeOfRide, value); } }

        LyftRideTypeEnum _LyftRideType = LyftRideTypeEnum.Lyft;
        public LyftRideTypeEnum LyftRideType { get { return _LyftRideType; } set { Set(ref _LyftRideType, value); } }

        string _MinuteCounter = string.Empty;
        public string MinuteCounter { get { return _MinuteCounter; } set { Set(ref _MinuteCounter, value); } }
        
        string _MinuteCounterLyft = default(string);
        public string MinuteCounterLyft { get { return _MinuteCounterLyft; } set { Set(ref _MinuteCounterLyft, value); } }
        
        string _MinuteCounterPlus = default(string);
        public string MinuteCounterPlus { get { return _MinuteCounterPlus; } set { Set(ref _MinuteCounterPlus, value); } }
        
        string _MinuteCounterLine = default(string);
        public string MinuteCounterLine { get { return _MinuteCounterLine; } set { Set(ref _MinuteCounterLine, value); } }

        string _MinuteCounterPremier = default(string);
        public string MinuteCounterPremier { get { return _MinuteCounterPremier; } set { Set(ref _MinuteCounterPremier, value); } }

        bool _CountAvailable = false;
        public bool CountAvailable { get { return _CountAvailable; } set { Set(ref _CountAvailable, value); } }

        Geopoint _CenterPoint = default(Geopoint);
        public Geopoint CenterPoint { get { return _CenterPoint; } set { Set(ref _CenterPoint, value); } }

        Geopoint _LocationPoint = default(Geopoint);
        public Geopoint LocationPoint { get { return _LocationPoint; } set { Set(ref _LocationPoint, value); } }

        double _ZoomLevel = default(double);
        public double ZoomLevel { get { return _ZoomLevel; } set { Set(ref _ZoomLevel, value); } }

        Uri _profilePictureSrc = default(Uri);
        public Uri ProfilePictureSrc
        {
            get { return _profilePictureSrc; }
            set
            {
                _profilePictureSrc = value;
                RaisePropertyChanged("ProfilePictureSrc");
            }
        }

        private async Task StartLocation()
        {
            var access = await Geolocator.RequestAccessAsync();
            var dialog = new MessageDialog($"Error: This app requires your location to function properly");

            switch (access)
            {
                case GeolocationAccessStatus.Allowed:

                    var geolocator = new Geolocator() { DesiredAccuracy = PositionAccuracy.High };
                    //geolocator.PositionChanged += PositionChanged;
                    geolocator.StatusChanged += StatusChanged;
                    var position = await geolocator.GetGeopositionAsync();
                    UpdateLocation(position);


                    break;

                case GeolocationAccessStatus.Unspecified:
                case GeolocationAccessStatus.Denied:
                default:
                    await dialog.ShowAsync();
                    return;
            }
        }

        private async void StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            await Dispatcher.DispatchAsync(async () =>
            {
                switch (args.Status)
                {
                    case PositionStatus.Ready:
                    case PositionStatus.Initializing:
                        break;
                    case PositionStatus.NoData:
                        await new MessageDialog("Error: could not fetch location").ShowAsync();
                        return;
                    case PositionStatus.Disabled:
                        await new MessageDialog("Error: Please enable location services to use this app").ShowAsync();
                        return;
                    case PositionStatus.NotInitialized:
                        await new MessageDialog("Error: location was not initialized").ShowAsync();
                        return;
                    case PositionStatus.NotAvailable:
                        await new MessageDialog("Error: location services is not available on this device/OS").ShowAsync();
                        return;
                    default:
                        await new MessageDialog("Error: Could not find your current location").ShowAsync();
                        return;

                }
            });
        }

        private async void PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            await Dispatcher.DispatchAsync(() =>
            {
                UpdateLocation(args.Position);
                ZoomLevel = 17.0d;
            });
        }

        private void UpdateLocation(Geoposition position)
        {
            LocationPoint = position.Coordinate.Point;
            UpdateAddress();
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            suspensionState["CenterPoint.lat"] = CenterPoint.Position.Latitude;
            suspensionState["CenterPoint.lng"] = CenterPoint.Position.Longitude;
            suspensionState["LocationPoint.lat"] = LocationPoint.Position.Latitude;
            suspensionState["LocationPoint.lng"] = LocationPoint.Position.Longitude;
            suspensionState[nameof(LocationText)] = LocationText;
            suspensionState[nameof(LyftRideType)] = LyftRideType.ToString();
            
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }

        public void UserTapped(object sender, RoutedEventArgs e)
        {
            Views.Shell.HamburgerMenu.IsOpen = true;
        }

        public async void GpsTapped(object sender, RoutedEventArgs e)
        {
            LocationText = "Trying to locate your position...";
            await StartLocation();
            CenterPoint = LocationPoint;
            CountAvailable = true;
            MinuteCounter = "11";
            await ResetDriversNearby();
            await UpdateETAs();
            await UpdateRideTypesAvailable();

        }




        private void FlipCarSelectionMenu()
        {
            LyftCarSelectorOpen = !LyftCarSelectorOpen;
            ShowTypeOfRide = !ShowTypeOfRide;
        }

        public async void LyftPlusCarTapped(object sender, RoutedEventArgs e)
        {
            LyftPremierBackground = new SolidColorBrush(Colors.White);
            LyftPlusBackground = new SolidColorBrush(Colors.WhiteSmoke);
            LyftLineBackground = new SolidColorBrush(Colors.White);
            LyftBackground = new SolidColorBrush(Colors.White);

            MinuteCounter = MinuteCounterPlus;

            LyftRideType = LyftRideTypeEnum.Plus;
            SelectedRideImageSrc = LyftPlusImageSrc;

            FlipCarSelectionMenu();
            await ResetDriversNearby();

        }

        public async void LyftLineCarTapped(object sender, RoutedEventArgs e)
        {
            LyftPremierBackground = new SolidColorBrush(Colors.White);
            LyftPlusBackground = new SolidColorBrush(Colors.White);
            LyftLineBackground = new SolidColorBrush(Colors.WhiteSmoke);
            LyftBackground = new SolidColorBrush(Colors.White);

            MinuteCounter = MinuteCounterLyft;

            LyftRideType = LyftRideTypeEnum.Lyft;
            SelectedRideImageSrc = LyftImageSrc;

            FlipCarSelectionMenu();
            await ResetDriversNearby();
        }

        public async void LyftPremierCarTapped(object sender, RoutedEventArgs e)
        {
            LyftPremierBackground = new SolidColorBrush(Colors.WhiteSmoke);
            LyftPlusBackground = new SolidColorBrush(Colors.White);
            LyftLineBackground = new SolidColorBrush(Colors.White);
            LyftBackground = new SolidColorBrush(Colors.White);

            MinuteCounter = MinuteCounterLyft;

            LyftRideType = LyftRideTypeEnum.Lyft;
            SelectedRideImageSrc = LyftImageSrc;

            FlipCarSelectionMenu();
            await ResetDriversNearby();
        }

        public async void LyftCarTapped(object sender, RoutedEventArgs e)
        {
            LyftPremierBackground = new SolidColorBrush(Colors.White);
            LyftPlusBackground = new SolidColorBrush(Colors.White);
            LyftLineBackground = new SolidColorBrush(Colors.White);
            LyftBackground = new SolidColorBrush(Colors.WhiteSmoke);

            MinuteCounter = MinuteCounterLyft;

            LyftRideType = LyftRideTypeEnum.Lyft;
            SelectedRideImageSrc = LyftImageSrc;

            FlipCarSelectionMenu();
            await ResetDriversNearby();
        }

        private async Task SetSelectedCarType(LyftRideTypeEnum type)
        {
            LyftPremierBackground = new SolidColorBrush(Colors.White);
            LyftPlusBackground = new SolidColorBrush(Colors.White);
            LyftLineBackground = new SolidColorBrush(Colors.White);
            LyftBackground = new SolidColorBrush(Colors.White);

            switch (type)
            {
                case LyftRideTypeEnum.Unknown:
                    break;
                case LyftRideTypeEnum.Lyft:
                    LyftBackground = new SolidColorBrush(Colors.WhiteSmoke);
                    break;
                case LyftRideTypeEnum.Plus:
                    LyftPlusBackground = new SolidColorBrush(Colors.WhiteSmoke);
                    break;
                case LyftRideTypeEnum.Line:
                    LyftLineBackground = new SolidColorBrush(Colors.WhiteSmoke);
                    break;
                case LyftRideTypeEnum.Premier:
                    LyftPremierBackground = new SolidColorBrush(Colors.WhiteSmoke);
                    break;
                default:
                    break;
            }

            await UpdateRideTypesAvailable();

        }

        private async Task ResetDriversNearby()
        {
            _mapUpdateDriversETAs.Stop();
            await UpdateNearbyDrivers();
            _mapUpdateDriversETAs.Start();
        }

        private async void UpdateAddress()
        {
            try
            {
                var mapLocation = await MapLocationFinder.FindLocationsAtAsync(CenterPoint);
                if (mapLocation.Status == MapLocationFinderStatus.Success)
                {
                    var first = mapLocation.Locations.FirstOrDefault();
                    if (first != null && !string.IsNullOrEmpty(first.Address.StreetNumber) && !string.IsNullOrEmpty(first.Address.Street))
                    {
                        LocationText = $"{first.Address.StreetNumber} {first.Address.Street}";
                    }
                    else
                    {
                        LocationText = "Couldn't find an address for this location";
                    }
                }
            }
            catch (Exception e)
            {
                var dialog = new MessageDialog($"Error occured: {e.Message}");
                await dialog.ShowAsync();
                return;
            }

            await UpdateETAs();
        }

        private async Task UpdateETAs()
        {
            ETAEstimates etas = null;
            try
            {
                etas = await _lyftPublicApi.GetETAs(CenterPoint.Position.Latitude, CenterPoint.Position.Longitude);
            }
            catch (Exception e)
            {
                var dialog = new MessageDialog($"Error occured: {e.Message}");
                await dialog.ShowAsync();
                return;
            }

            if (etas.Estimates == null)
            {
                CountAvailable = false;
                MinuteCounterLyft = "n/a";
                MinuteCounterPlus = "n/a";
                MinuteCounterPremier = "n/a";
                MinuteCounterLine = "n/a";
                return;
            }

            //CountAvailable = true;

            foreach (var eta in etas.Estimates)
            {
                if (eta.EtaSeconds == null)
                {
                    switch (eta.RideType)
                    {
                        case LyftRideTypeEnum.Unknown:
                            break;
                        case LyftRideTypeEnum.Lyft:
                            MinuteCounterLyft = "n/a";
                            break;
                        case LyftRideTypeEnum.Plus:
                            MinuteCounterPlus = "n/a";
                            break;
                        case LyftRideTypeEnum.Line:
                            MinuteCounterLine = "n/a";
                            break;
                        case LyftRideTypeEnum.Premier:
                            MinuteCounterPremier = "n/a/";
                            break;
                        default:
                            break;
                    }

                    if (eta.RideType == LyftRideType)
                    {
                        CountAvailable = false;
                    }
                    continue;
                }

                var time = eta.EtaSeconds / 60;

                if (eta.RideType == LyftRideTypeEnum.Lyft)
                {
                    MinuteCounterLyft = $"{time}";
                    
                    if (LyftRideType == LyftRideTypeEnum.Lyft)
                    {
                        MinuteCounter = MinuteCounterLyft;
                        CountAvailable = true;
                    }
                    
                }
                if (eta.RideType == LyftRideTypeEnum.Plus)
                {
                    MinuteCounterPlus = $"{time}";

                    if (LyftRideType == LyftRideTypeEnum.Plus)
                    {
                        MinuteCounter = MinuteCounterPlus;
                        CountAvailable = true;
                    }
                }
                if (eta.RideType == LyftRideTypeEnum.Line)
                {
                    MinuteCounterPlus = $"{time}";

                    if (LyftRideType == LyftRideTypeEnum.Line)
                    {
                        MinuteCounter = MinuteCounterLine;
                        CountAvailable = true;
                    }
                }
                if (eta.RideType == LyftRideTypeEnum.Premier)
                {
                    MinuteCounterPlus = $"{time}";

                    if (LyftRideType == LyftRideTypeEnum.Premier)
                    {
                        MinuteCounter = MinuteCounterPremier;
                        CountAvailable = true;
                    }
                }
            }
            
        }

        private async Task UpdateNearbyDrivers()
        {
            NearbyDrivers nearbyDrivers = null;
            try
            {
                nearbyDrivers = await _lyftPublicApi.GetNearbyDrivers(CenterPoint.Position.Latitude, CenterPoint.Position.Longitude);
            }
            catch (Exception e)
            {
                var dialog = new MessageDialog($"Error occured: {e.Message}");
                await dialog.ShowAsync();
                return;
            }

            if (nearbyDrivers.Drivers == null)
                return;

            var drivers = nearbyDrivers.Drivers
                .Where(x => x.RideType == LyftRideType)
                .SingleOrDefault();

            if (drivers == null)
                return;

            List<MapLyftIcon> icons = new List<MapLyftIcon>();
            foreach (var driver in drivers.Drivers)
            {
                var location = driver.Locations.FirstOrDefault();
                var mapIcon = new MapLyftIcon()
                {
                    Location = new Geopoint(new BasicGeoposition()
                    {
                        Latitude = location.lat,
                        Longitude = location.lng
                    }),
                    ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/LyftCarMap.png"))
                };

                icons.Add(mapIcon);
            }

            MapIcons.Clear();
            foreach (var icon in icons)
            {
                MapIcons.Add(icon);
            }
        }

        public void CarTypeSelectorTapped(object sender, RoutedEventArgs e)
        {
            FlipCarSelectionMenu();
        }

        public async void SetPickup(object sender, RoutedEventArgs e)
        {
            if (CenterPoint == null || LocationPoint == null)
            {
                var msgDialog = new MessageDialog("Please wait till your adress has been locked!");
                await msgDialog.ShowAsync();
                return;
            }

            var navParameter = new OrderPageNavigationModel()
            {
                CenterPoint = CenterPoint.Position,
                LocationPoint = LocationPoint.Position,
                LocationText = LocationText,
                MinuteCounter = MinuteCounter,
                RideType = LyftRideType,
            };
            NavigationService.Navigate(typeof(Views.OrderPage), navParameter);
        }

    }
}

