using _29Lifts.Api.Public;
using _29Lifts.Api.Public.models;
using _29Lifts.Api.Rides;
using _29Lifts.Helper;
using _29Lifts.Navigation;
using _29Lifts.Services.SettingsServices;
using _29Lifts.ViewModels.OrderPage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace _29Lifts.ViewModels
{
    public class OrderPageViewModel : ViewModelBase
    {
        double _ZoomLevel = 19.0;
        public double ZoomLevel { get { return _ZoomLevel; } set { Set(ref _ZoomLevel, value); } }

        Geopoint _CenterPoint = default(Geopoint);
        public Geopoint CenterPoint { get { return _CenterPoint; } set { Set(ref _CenterPoint, value); } }

        Geopoint _LocationPoint = default(Geopoint);
        public Geopoint LocationPoint { get { return _LocationPoint; } set { Set(ref _LocationPoint, value); } }

        Geopoint _PickupPoint = default(Geopoint);
        public Geopoint PickupPoint { get { return _PickupPoint; } set { Set(ref _PickupPoint, value); } }

        DestinationViewModel _Destination = default(DestinationViewModel);
        public DestinationViewModel Destination { get { return _Destination; } set { Set(ref _Destination, value); } }

        string _DestinationText = default(string);
        public string DestinationText { get { return _DestinationText; } set { Set(ref _DestinationText, value); } }

        bool _ShowDestination = default(bool);
        public bool ShowDestination { get { return _ShowDestination; } set { Set(ref _ShowDestination, value); } }

        string _LocationText = default(string);
        public string LocationText { get { return _LocationText; } set { Set(ref _LocationText, value); } }

        string _MinuteCounter = default(string);
        public string MinuteCounter { get { return _MinuteCounter; } set { Set(ref _MinuteCounter, value); } }

        bool _ShowDetails = default(bool);
        public bool ShowDetails { get { return _ShowDetails; } set { Set(ref _ShowDetails, value); } }

        bool _ShowSearch = default(bool);
        public bool ShowSearch { get { return _ShowSearch; } set { Set(ref _ShowSearch, value); } }

        string _EstimatedPrice = default(string);
        public string EstimatedPrice { get { return _EstimatedPrice; } set { Set(ref _EstimatedPrice, value); } }

        string _PrimeTime = default(string);
        public string PrimeTime { get { return _PrimeTime; } set { Set(ref _PrimeTime, value); } }
        
        Uri _ProfilePictureSrc = default(Uri);
        public Uri ProfilePictureSrc { get { return _ProfilePictureSrc; } set { Set(ref _ProfilePictureSrc, value); } }

        public LyftRideTypeEnum SelectedRideType { get; set; }
        public bool CountAvailable { get; set; } = true;
        public ImageSource LyftTypeImageSrc { get; set; }
        public ILyftPublicAPI _lyftPublicApi { get; set; }
        public ILyftRidesApi _lyftUserApi { get; set; }
        //public ILyftPublicAPI _lyftApi { get; set; }
        public LyftModalDialogViewModel ModalViewModel { get; set; } = LyftModalDialogViewModel.EmptyInstanceByType(LyftRideTypeEnum.Unknown);
        private AutoSuggestBox SearchBox { get; set; }
        public ObservableCollection<AddressSearchResultViewModel> Suggestions { get; set; } = new ObservableCollection<AddressSearchResultViewModel>();
        public delegate void MapEventHandler(Geopoint location, Geopoint destination);
        public event MapEventHandler DestinationChosen;
        public event MapEventHandler ShowLocation;
        private DispatcherTimer _etaUpdateTimer;
        private SettingsService _settings = SettingsService.Instance;
        private bool IsPrimeTime;
        private string PrimeTimeRate;

        public OrderPageViewModel()
        {
            _lyftPublicApi = new LyftPublicApi();
            // Seattle, WA as default
            PickupPoint = new Geopoint(new BasicGeoposition() { Latitude = 32.7555, Longitude = -97.3308 });
            LocationPoint = new Geopoint(new BasicGeoposition() { Latitude = 32.7555, Longitude = -97.3308 });
            CenterPoint = new Geopoint(new BasicGeoposition() { Latitude = 32.7555, Longitude = -97.3308 });
            PrimeTime = string.Empty;
            EstimatedPrice = "Price Estimate";
            ShowDetails = false;
            ShowSearch = false;
            DestinationText = "Add destination";
            ShowDestination = false;
            DestinationChosen += DestinationChosenTriggered;
            //ShowLocation += UpdateMapForGPSLocation;
            PrimeTime = "Prime Time";
            IsPrimeTime = false;
            PrimeTimeRate = string.Empty;
            _lyftUserApi = new LyftRidesApi();
            ProfilePictureSrc = _settings.ProfileImageSrc;

            _etaUpdateTimer = new DispatcherTimer();
            _etaUpdateTimer.Interval = TimeSpan.FromMilliseconds(5000);
            _etaUpdateTimer.Tick += _etaUpdateTimer_Tick;

            //Suggestions.Add("john");
            //Suggestions.Add("johnny");
            //Suggestions.Add("jonni");
        }

        

        private async void _etaUpdateTimer_Tick(object sender, object e)
        {
            try
            {
                var etas = await _lyftPublicApi.GetETAs(PickupPoint.Position.Latitude, PickupPoint.Position.Longitude);

                var eta = etas.Estimates
                    .Where(x => x.RideType == SelectedRideType)
                    .SingleOrDefault();

                if (eta != null)
                {
                    MinuteCounter = $"{eta.EtaSeconds / 60}";
                }
            }
            catch (Exception k)
            {
                var dialog = new MessageDialog($"Error: {k.Message}");
                MinuteCounter = "n/a";
            }
        }

        private async void DestinationChosenTriggered(Geopoint location, Geopoint destination)
        {
            var costEstimates = await _lyftPublicApi.GetCostEstimates(PickupPoint.Position.Latitude, PickupPoint.Position.Longitude,
                Destination.DestinationPoint.Position.Latitude, Destination.DestinationPoint.Position.Longitude);

            var price = costEstimates.Estimates
                .Where(x => x.RideType == SelectedRideType)
                .SingleOrDefault();

            if (price == null)
            {
                return;
            }
            else
            {
                EstimatedPrice = $"{price.EstimatedCostCentsMin / 100:C0}-{price.EstimatedCostCentsMax / 100:C0}";

                PrimeTime = $"Prime Time: {price.PrimetimePercentage}";
                if (price.PrimetimePercentage != "0%")
                {
                    PrimeTimeRate = price.PrimetimePercentage;
                    IsPrimeTime = true;
                }
            }

        }

        public void DestinationRemoved()
        {
            ShowDestination = false;
            DestinationText = "Add destination";
            EstimatedPrice = "Price Estimate";
            PrimeTime = "Prime Time";
            IsPrimeTime = false;
        }

        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var navParameter = parameter as OrderPageNavigationModel;

            if (navParameter != null)
            {
                var adjustedCenterPoint = new Geopoint(new BasicGeoposition()
                {
                    Longitude = navParameter.CenterPoint.Longitude,
                    Latitude = navParameter.CenterPoint.Latitude - 0.0002,
                });
                CenterPoint = adjustedCenterPoint;
                //CenterPoint = new Geopoint(navParameter.CenterPoint);
                LocationPoint = new Geopoint(navParameter.LocationPoint);
                PickupPoint = new Geopoint(navParameter.CenterPoint);
                LocationText = navParameter.LocationText;
                MinuteCounter = navParameter.MinuteCounter;
                SelectedRideType = navParameter.RideType; 

                var prices = await _lyftPublicApi.GetRideTypes(LocationPoint.Position.Latitude, LocationPoint.Position.Longitude);

                ModalViewModel = LyftModalDialogViewModel.EmptyInstanceByType(navParameter.RideType, prices);
            }

            _etaUpdateTimer.Start();
            ProfilePictureSrc = _settings.ProfileImageSrc;

            await Task.CompletedTask;
        }

        public void ShowDetailsTapped(object sender, RoutedEventArgs e)
        {
            ShowDetails = true;
        }

        public void CloseDetailsTapped(object sender, RoutedEventArgs e)
        {
            ShowDetails = false;
        }

        public async void RequestLyftTapped(object sender, RoutedEventArgs e)
        {
            // if destination has been set
            if (ShowDestination)
            {
                if (_settings.UserToken == null)
                {
                    var dialog = new MessageDialog(
                        "You need to log in or register to request a Lyft ride.",
                        "Lyft authentication");

                    dialog.Commands.Add(new UICommand("Login/Register") { Id = 0 });
                    dialog.Commands.Add(new UICommand("Cancel") { Id = 1 });

                    dialog.DefaultCommandIndex = 0;
                    dialog.CancelCommandIndex = 1;

                    var result = await dialog.ShowAsync();

                    if ((int)result.Id == 0)
                    {
                        HttpClientHelper.RegisterLoginAtLyft();
                    }
                }

                else
                {
                    if (IsPrimeTime)
                    {
                        var dialog = new MessageDialog(
                        $"There's currently a great demand for Lyft rides. This ride will approximately be {PrimeTimeRate} more expensive than regular fares due to prime time. Do you accept?",
                        "Lyft Prime Time");

                        dialog.Commands.Add(new UICommand("Yes") { Id = 0 });
                        dialog.Commands.Add(new UICommand("No") { Id = 1 });

                        dialog.DefaultCommandIndex = 1;
                        dialog.CancelCommandIndex = 1;

                        var result = await dialog.ShowAsync();

                        if ((int)result.Id == 0)
                        {
                            //todo: fix primetime pricing
                        }
                    }

                    var ride = await _lyftUserApi.StartRide(PickupPoint.Position.Latitude, PickupPoint.Position.Longitude,
                        Destination.DestinationPoint.Position.Latitude, Destination.DestinationPoint.Position.Longitude, SelectedRideType);

                    _settings.ActiveRideId = ride.RideId;

                    var parameter = new RidePageNavigationModel()
                    {
                        Destination = Destination.DestinationPoint.Position,
                        DestinationText = DestinationText,
                        LocationPoint = LocationPoint.Position,
                        PickupLocation = PickupPoint.Position,
                        PickupText = LocationText,
                        Ride = ride
                    };

                    //_etaUpdateTimer.Stop();

                    NavigationService.Navigate(typeof(Views.RidePage), parameter);
                }
            }
            else
            {
                var dialog = new MessageDialog(
                       "You need to add a destination before you can request a ride.",
                       "Please add destination");

                var result = await dialog.ShowAsync();
            }



        }

        public void DestinationTapped(object sender, RoutedEventArgs e)
        {
            ShowSearch = true;
            if (SearchBox != null)
            {
                SearchBox.Focus(FocusState.Keyboard);
            }
        }

        public async void GpsTapped(object sender, RoutedEventArgs e)
        {
            await StartLocation();
        }

        private async Task StartLocation()
        {
            var access = await Geolocator.RequestAccessAsync();
            var dialog = new MessageDialog($"Error: This app requires to know your location to function");

            switch (access)
            {
                case GeolocationAccessStatus.Allowed:

                    var geolocator = new Geolocator() { DesiredAccuracy = PositionAccuracy.High };
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
            });
        }

        private void UpdateLocation(Geoposition position)
        {
            ShowLocation(position.Coordinate.Point, position.Coordinate.Point);
        }

        public void UserTapped(object sender, RoutedEventArgs e)
        {
            Views.Shell.HamburgerMenu.IsOpen = true;
        }

        #region SearchBox

        public void CloseSearchBoxTapped(object sender, RoutedEventArgs e)
        {
            ShowSearch = false;
        }

        public void SearchBoxLoaded(object sender, RoutedEventArgs e)
        {
            if (ShowSearch == true)
            {
                var box = sender as AutoSuggestBox;
                SearchBox = box;
                box.Focus(FocusState.Keyboard);
            }
        }

        public void SearchTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing, 
            // otherwise assume the value got filled in by TextMemberPath 
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset
                //sender.ItemsSource = dataset;
                var searchTerm = sender.Text;

                if (string.IsNullOrEmpty(searchTerm))
                {
                    // meaning we cleared the searchbox
                    DestinationRemoved();
                    return;
                }

                Dispatcher.DispatchAsync(async () =>
                {
                    try
                    {
                        var mapLocation = await MapLocationFinder.FindLocationsAsync(searchTerm, LocationPoint, 10);
                        //http://spatial.virtualearth.net/REST/v1/data/accessId/dataSourceName/entityTypeName(entityId)?$format=json&isStaging=isStaging&key=AuWKpDAtdfdkO_If4n1l1JWb-TMNZG3YUZAkpJJuz_bv86IEuxjUkLsq3TVhxZ3e

                        if (mapLocation.Status == MapLocationFinderStatus.Success)
                        {
                            Suggestions.Clear();
                            var results = mapLocation.Locations;
                            foreach (var result in results)
                            {
                                if (string.IsNullOrEmpty(result.Address.StreetNumber) || string.IsNullOrEmpty(result.Address.StreetNumber))
                                {
                                    Suggestions.Add(new AddressSearchResultViewModel()
                                    {
                                        Title = result.Address.FormattedAddress,
                                        Point = result.Point
                                    });
                                }
                                else
                                {
                                    Suggestions.Add(new AddressSearchResultViewModel()
                                    {
                                        Title = $"{result.Address.StreetNumber} {result.Address.Street}",
                                        SubTitle = $"{result.Address.Town}, {result.Address.Region}",
                                        Point = result.Point
                                    });
                                }
                            }
                       
                        }
                    }
                    catch (ArgumentException)
                    {
                        
                    }
                });
            }
        }

        public void SearchSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // Set sender.Text. You can use args.SelectedItem to build your text string.
            var selected = args.SelectedItem as AddressSearchResultViewModel;
            if (selected != null)
            {
                sender.Text = selected.Title;
                Destination = new DestinationViewModel()
                {
                    DestinationPoint = selected.Point,
                    Title = selected.Title,
                    FormattedAddress = (selected.Title)
                };

                DestinationText = Destination.Title;
                ShowDestination = true;
                ShowSearch = false;
                DestinationChosen(LocationPoint, Destination.DestinationPoint);
            }
        }

        public void SearchQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
            }
            else
            {
                // Use args.QueryText to determine what to do.
            }
        }

        #endregion

    }

}
