using _29Lifts.Api.Public.models;
using _29Lifts.Api.Rides.Models;
using System;
using Template10.Common;
using Template10.Utils;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;

namespace _29Lifts.Services.SettingsServices
{
    public class SettingsService
    {
        public static SettingsService Instance { get; } = new SettingsService();
        Template10.Services.SettingsService.ISettingsHelper _helper;
        private SettingsService()
        {
            _helper = new Template10.Services.SettingsService.SettingsHelper();
        }

        public bool UseShellBackButton
        {
            get { return _helper.Read<bool>(nameof(UseShellBackButton), true); }
            set
            {
                _helper.Write(nameof(UseShellBackButton), value);
                BootStrapper.Current.NavigationService.Dispatcher.Dispatch(() =>
                {
                    BootStrapper.Current.ShowShellBackButton = value;
                    BootStrapper.Current.UpdateShellBackButton();
                    BootStrapper.Current.NavigationService.Refresh();
                });
            }
        }

        public ApplicationTheme AppTheme
        {
            get
            {
                var theme = ApplicationTheme.Light;
                var value = _helper.Read<string>(nameof(AppTheme), theme.ToString());
                return Enum.TryParse<ApplicationTheme>(value, out theme) ? theme : ApplicationTheme.Dark;
            }
            set
            {
                _helper.Write(nameof(AppTheme), value.ToString());
                (Window.Current.Content as FrameworkElement).RequestedTheme = value.ToElementTheme();
                Views.Shell.HamburgerMenu.RefreshStyles(value);
            }
        }

        public TimeSpan CacheMaxDuration
        {
            get { return _helper.Read<TimeSpan>(nameof(CacheMaxDuration), TimeSpan.FromDays(2)); }
            set
            {
                _helper.Write(nameof(CacheMaxDuration), value);
                BootStrapper.Current.CacheMaxDuration = value;
            }
        }

        public bool ShowHamburgerButton
        {
            get { return _helper.Read<bool>(nameof(ShowHamburgerButton), true); }
            set
            {
                _helper.Write(nameof(ShowHamburgerButton), value);
                Views.Shell.HamburgerMenu.HamburgerButtonVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public bool IsFullScreen
        {
            get { return _helper.Read<bool>(nameof(IsFullScreen), false); }
            set
            {
                _helper.Write(nameof(IsFullScreen), value);
                Views.Shell.HamburgerMenu.IsFullScreen = value;
            }
        }

        public AccessTokenPublic PublicToken
        {
            get { return _helper.Read<AccessTokenPublic>(nameof(PublicToken), null); }
            set
            {
                _helper.Write(nameof(PublicToken), value);
            }
        }

        public DateTime PublicTokenExpiration
        {
            get { return _helper.Read<DateTime>(nameof(PublicTokenExpiration), new DateTime(1985, 7, 12)); }
            set
            {
                _helper.Write(nameof(PublicTokenExpiration), value);
            }
        }

        public AccessTokenAuth UserToken
        {
            get { return _helper.Read<AccessTokenAuth>(nameof(UserToken), null); }
            set
            {
                _helper.Write(nameof(UserToken), value);
            }
        }

        public DateTime UserTokenExpiration
        {
            get { return _helper.Read<DateTime>(nameof(UserTokenExpiration), new DateTime(1985, 7, 12)); }
            set
            {
                _helper.Write(nameof(UserTokenExpiration), value);
            }
        }

        public string ActiveRideId
        {
            get { return _helper.Read<string>(nameof(ActiveRideId), string.Empty); }
            set
            {
                _helper.Write(nameof(ActiveRideId), value);
            }
        }

        public BasicGeoposition LastLocation
        {
            get { return _helper.Read<BasicGeoposition>(nameof(LastLocation), new BasicGeoposition() { Latitude = 32.754921, Longitude = -97.330334 }); }
            set
            {
                _helper.Write(nameof(LastLocation), value);
            }
        }

        public Uri ProfileImageSrc
        {
            get { return _helper.Read<Uri>(nameof(ProfileImageSrc), new Uri("ms-appx://29Quizlet/Assets/avatar.png")); }
            set
            {
                _helper.Write(nameof(ProfileImageSrc), value);
            }
        }
    }
}

