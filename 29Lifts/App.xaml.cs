using Windows.UI.Xaml;
using System.Threading.Tasks;
using _29Lifts.Services.SettingsServices;
using Windows.ApplicationModel.Activation;
using Template10.Controls;
using Template10.Common;
using System;
using System.Linq;
using Windows.UI.Xaml.Data;
using Windows.UI.ViewManagement;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;

namespace _29Lifts
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    [Bindable]
    sealed partial class App : Template10.Common.BootStrapper
    {
        private SettingsService _settings;

        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);
            _settings = SettingsService.Instance;

            #region App settings

            RequestedTheme = _settings.AppTheme;
            CacheMaxDuration = _settings.CacheMaxDuration;
            ShowShellBackButton = _settings.UseShellBackButton;

            #endregion
        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            if (Window.Current.Content as ModalDialog == null)
            {
                // create a new frame 
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);

                // create modal root
                Window.Current.Content = new ModalDialog
                {
                    DisableBackButtonWhenModal = true,
                    Content = new Views.Shell(nav),
                    ModalContent = new Views.Busy(),
                };
            }

            Views.Shell.HamburgerMenu.HamburgerButtonVisibility = Visibility.Collapsed;

            await Task.CompletedTask;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // long-running startup tasks go here
            // if not logged in

            var result = ApplicationView.GetForCurrentView().TryEnterFullScreenMode();

            


            if (startKind == StartKind.Activate && args.Kind == ActivationKind.Protocol)
            {
                var activation = args as ProtocolActivatedEventArgs;
                var results = activation.Uri.Query;
                string code = string.Empty;

                foreach (var arg in results.Split('&'))
                {
                    string[] parts = arg.Replace("?", string.Empty).Split('=');
                    if (parts[0] == "code")
                    {
                        code = parts[1];
                    }
                }

                NavigationService.Navigate(typeof(Views.AuthenticatingPage), code);

            }
            else
            {
                var position = await StartLocation();
                _settings.LastLocation = position.Coordinate.Point.Position;

                if (_settings.UserToken == null)
                {
                    NavigationService.Navigate(typeof(Views.LoginPage));
                }
                else
                {
                    NavigationService.Navigate(typeof(Views.MapPage));
                }

                
            }



            await Task.CompletedTask;
        }

        private async Task<Geoposition> StartLocation()
        {
            var access = await Geolocator.RequestAccessAsync();
            var dialog = new MessageDialog($"Error: This app requires your location to function properly");

            switch (access)
            {
                case GeolocationAccessStatus.Allowed:

                    var geolocator = new Geolocator() { DesiredAccuracy = PositionAccuracy.High };
                    //geolocator.PositionChanged += PositionChanged;
                    //geolocator.StatusChanged += StatusChanged;
                    return await geolocator.GetGeopositionAsync();
                    

                case GeolocationAccessStatus.Unspecified:
                case GeolocationAccessStatus.Denied:
                default:
                    await dialog.ShowAsync();
                    return null;
            }
        }
    }
}

