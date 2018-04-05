using _29Lifts.Api.User;
using _29Lifts.Services.SettingsServices;
using _29Lifts.ViewModels.RideHistoryVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using _29Lifts.Api.Rides.Models.SubModels;
using _29Lifts.Api.Rides.Models;
using Windows.UI.Popups;
using _29Lifts.Exceptions;

namespace _29Lifts.ViewModels
{
    public class RideHistoryPageViewModel : ViewModelBase
    {
        private SettingsService _settings;
        private ILyftUserApi _api;
        public ObservableCollection<RootRide> Rides { get; set; }
        public IList<RideHistory> RideHistories { get; set; }

    public RideHistoryPageViewModel()
        {
            _settings = SettingsService.Instance;
            _api = new LyftUserApi();
            Rides = new ObservableCollection<RootRide>();
            RideHistories = new List<RideHistory>();
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            _settings.ShowHamburgerButton = true;
            RideHistories histories = null;

            try
            {
                histories = await _api.GetRideHistories();

            }
            catch (NotLoggedInException)
            {
                var dialog = new MessageDialog("You need to login to access this page.");
                await dialog.ShowAsync();
                NavigationService.Navigate(typeof(Views.SettingsPage), 0);
                return;
            }
            catch (Exception e)
            {
                var dialog = new MessageDialog($"Error: {e.Message}");
                await dialog.ShowAsync();

                NavigationService.Navigate(typeof(Views.MapPage));
                return;
            }

            if (histories == null)
            {
                if (_settings.UserToken == null)
                {
                    var dialog = new MessageDialog("An error occured. Are you logged in?");
                    await dialog.ShowAsync();
                    NavigationService.Navigate(typeof(Views.MapPage));
                    return;
                }
                else
                {
                    var dialog = new MessageDialog("An error occured on Lyfts part. Try again later.");
                    await dialog.ShowAsync();
                    NavigationService.Navigate(typeof(Views.MapPage));
                    return;
                }
            }

            if (histories != null && histories.RideHistory.Any())
            {
                foreach (var ride in histories.RideHistory)
                {
                    RideHistories.Add(ride);

                    if (ride.Status == "droppedOff")
                    {
                        var rideTime = ride.Dropoff.Time - ride.Pickup.Time;

                        var elem = new InactiveRide()
                        {
                            DateAndTime = ride.RequestedAt.ToString("MMM d - h:mm tt"),
                            Price = $"{ride.Price.Amount / 100:C}",
                            ProfileImageSrc = new Uri(ride.Driver.ImageUrl),
                            Ride = $"{ride.RideType.ToString()} \u2022 {rideTime.Minutes}m {rideTime.Seconds}s",
                            RideId = ride.RideId,
                        };
                        Rides.Add(elem);
                    }
                    else if (ride.Status == "canceled")
                    {
                        var elem = new InactiveRide()
                        {
                            DateAndTime = ride.RequestedAt.ToString("MMM d - h:mm tt"),
                            Price = $"{ride.Price?.Amount ?? 0:C}",
                            ProfileImageSrc = new Uri(ride.Driver?.ImageUrl ?? "ms-appx://29Quizlet/Assets/avatar.png"),
                            Ride = $"{ride.RideType.ToString()} \u2022 cancelled",
                            RideId = ride.RideId,
                        };

                        Rides.Add(elem);
                    }
                    else
                    {
                        string rideType = "ms-appx://29Quizlet/Assets/Lyft.png";

                        switch (ride.RideType)
                        {
                            case Api.Public.models.LyftRideTypeEnum.Unknown:
                                break;
                            case Api.Public.models.LyftRideTypeEnum.Lyft:
                                break;
                            case Api.Public.models.LyftRideTypeEnum.Plus:
                                rideType = "ms-appx://29Quizlet/Assets/LyftPlus.png";
                                break;
                            case Api.Public.models.LyftRideTypeEnum.Line:
                                rideType = "ms-appx://29Quizlet/Assets/LyftLine.png";
                                break;
                            case Api.Public.models.LyftRideTypeEnum.Premier:
                                rideType = "ms-appx://29Quizlet/Assets/LyftPremier.png";
                                break;
                            default:
                                break;
                        }

                        var elem = new ActiveRide()
                        {
                            RideInProgress = "This ride is in progress.",
                            RideStatus = $"Ride is {ride.Status}, click to continue...",
                            RideTypeImageSrc = new BitmapImage(new Uri(rideType)),
                            RideId = ride.RideId,
                        };

                        Rides.Add(elem);
                    }
                }
            }
            
            await Task.CompletedTask;
        }

        public async void SelectedRide(object sender, ItemClickEventArgs e)
        {
            var elem = e.ClickedItem as RootRide;
            if (elem != null)
            {
                var activeRide = elem as ActiveRide;
                if (activeRide != null)
                {
#if DEBUG
                    NavigationService.Navigate(typeof(Views.RidePage), _settings.ActiveRideId);
#endif
#if !DEBUG
                    NavigationService.Navigate(typeof(Views.RidePage), activeRide.RideId);
#endif
                }
                else
                {
                    var inactiveRide = (InactiveRide)elem;
                    var ride = RideHistories.Where(x => x.RideId == inactiveRide.RideId)
                        .SingleOrDefault();

                    if (ride != null)
                    {
                        NavigationService.Navigate(typeof(Views.ReceiptPage), ride);
                    }
                    else
                    {
                        // todo: error dialog
                        var dialog = new MessageDialog("Error: Ride id was not defined.");
                        await dialog.ShowAsync();
                        NavigationService.Navigate(typeof(Views.MapPage));
                    }
                }
            } 
        }
    }
}
