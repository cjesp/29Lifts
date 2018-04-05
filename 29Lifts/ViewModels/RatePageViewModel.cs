using _29Lifts.Navigation;
using _29Lifts.Services.SettingsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using _29Lifts.Api.Rides;
using Windows.UI.Popups;

namespace _29Lifts.ViewModels
{
    public class RatePageViewModel : ViewModelBase
    {
        private SettingsService _service;
        private ILyftRidesApi _api;
        Uri _ProfileImage = default(Uri);
        public string OtherTipAmount { get; set; }
        public Uri ProfileImage { get { return _ProfileImage; } set { Set(ref _ProfileImage, value); } }
        public TippingAmount AmountTipped { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; }
        public int Tips { get; set; }
        public string RideId { get; set; }
        decimal _Cost = default(decimal);
        public decimal Cost { get { return _Cost; } set { Set(ref _Cost, value); } }
        public string FeedbackText { get; set; }

        string _RateMeText = default(string);
        public string RateMeText { get { return _RateMeText; } set { Set(ref _RateMeText, value); } }

        string _DriverName = default(string);
        public string DriverName { get { return _DriverName; } set { Set(ref _DriverName, value); } }

        bool _ShowFeedback = default(bool);
        public bool ShowFeedback { get { return _ShowFeedback; } set { Set(ref _ShowFeedback, value); } }

        SolidColorBrush _OneForeground = default(SolidColorBrush);
        public SolidColorBrush OneForeground { get { return _OneForeground; } set { Set(ref _OneForeground, value); } }

        SolidColorBrush _TwoForeground = default(SolidColorBrush);
        public SolidColorBrush TwoForeground { get { return _TwoForeground; } set { Set(ref _TwoForeground, value); } }

        SolidColorBrush _ThreeForeground = default(SolidColorBrush);
        public SolidColorBrush ThreeForeground { get { return _ThreeForeground; } set { Set(ref _ThreeForeground, value); } }

        SolidColorBrush _FourForeground = default(SolidColorBrush);
        public SolidColorBrush FourForeground { get { return _FourForeground; } set { Set(ref _FourForeground, value); } }

        SolidColorBrush _FiveForeground = default(SolidColorBrush);
        public SolidColorBrush FiveForeground { get { return _FiveForeground; } set { Set(ref _FiveForeground, value); } }


        public RatePageViewModel()
        {
            Comments = string.Empty;
            Rating = 5;
            Tips = 0;
            RideId = string.Empty;
            _service = SettingsService.Instance;
            _service.ShowHamburgerButton = true;
            FeedbackText = string.Empty;
            ShowFeedback = false;
            _api = new LyftRidesApi();
            OneForeground = new SolidColorBrush(Colors.Gray);
            TwoForeground = new SolidColorBrush(Colors.Gray);
            ThreeForeground = new SolidColorBrush(Colors.Gray);
            FourForeground = new SolidColorBrush(Colors.Gray);
            FiveForeground = new SolidColorBrush(Colors.Gray);
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var navModel = parameter as RatePageNavigationModel;

            if (navModel != null)
            {
                RateMeText = $"Rate {navModel.Name} for this Lyft";
                RideId = navModel.RideId;
                Tips = navModel.Tip;
                ProfileImage = navModel.ProfileImage;
            }

            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            NavigationService.FrameFacade.BackRequested -= FrameFacade_BackRequested;
            await Task.CompletedTask;
        }

        private void FrameFacade_BackRequested(object sender, Template10.Common.HandledEventArgs e)
        {
            e.Handled = true;
        }

        public void StarTapped(object sender, TappedRoutedEventArgs e)
        {
            ShowFeedback = true;
            var border = sender as Border;
            if (border != null)
            {
                var tag = int.Parse(border.Tag as string);
                OneForeground = new SolidColorBrush(Color.FromArgb(100,255,0,191));
                TwoForeground = new SolidColorBrush(Colors.Gray);
                ThreeForeground = new SolidColorBrush(Colors.Gray);
                FourForeground = new SolidColorBrush(Colors.Gray);
                FiveForeground = new SolidColorBrush(Colors.Gray);
                switch (tag)
                {
                    case 1:
                        Rating = 1;
                        break;
                    case 2:
                        Rating = 2;
                        TwoForeground = new SolidColorBrush(Color.FromArgb(100, 255, 0, 191));
                        break;
                    case 3:
                        Rating = 3;
                        TwoForeground = new SolidColorBrush(Color.FromArgb(100, 255, 0, 191));
                        ThreeForeground = new SolidColorBrush(Color.FromArgb(100, 255, 0, 191));
                        break;
                    case 4:
                        Rating = 4;
                        TwoForeground = new SolidColorBrush(Color.FromArgb(100, 255, 0, 191));
                        ThreeForeground = new SolidColorBrush(Color.FromArgb(100, 255, 0, 191));
                        FourForeground = new SolidColorBrush(Color.FromArgb(100, 255, 0, 191));
                        break;
                    case 5:
                        Rating = 5;
                        TwoForeground = new SolidColorBrush(Color.FromArgb(100, 255, 0, 191));
                        ThreeForeground = new SolidColorBrush(Color.FromArgb(100, 255, 0, 191));
                        FourForeground = new SolidColorBrush(Color.FromArgb(100, 255, 0, 191));
                        FiveForeground = new SolidColorBrush(Color.FromArgb(100, 255, 0, 191));
                        break;
                    default:
                        break;
                }
            }
        }

        public async void SubmitTapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                var result = await _api.RateAndTipRide(RideId, Tips, Rating, FeedbackText);
            }
            catch (Exception k)
            {
                var dialog = new MessageDialog($"Error: {k.Message}");
                await dialog.ShowAsync();
            }

            NavigationService.Navigate(typeof(Views.MapPage));
        }

    }
}
