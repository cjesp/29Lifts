using _29Lifts.Helper;
using _29Lifts.Services.SettingsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace _29Lifts.ViewModels
{
    public class AuthenticatingPageViewModel : ViewModelBase
    {
        string _AuthenticatingText = default(string);
        public string AuthenticatingText { get { return _AuthenticatingText; } set { Set(ref _AuthenticatingText, value); } }

        bool _ShowSpinner = default(bool);
        public bool ShowSpinner { get { return _ShowSpinner; } set { Set(ref _ShowSpinner, value); } }

        private SettingsService _settings = SettingsService.Instance;

        public AuthenticatingPageViewModel()
        {
            AuthenticatingText = "Authenticating...";
            ShowSpinner = true;
        }

        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var code = parameter as string;

            if (code != null)
            {
                // we're in business and we're authenticating
                var authenticated = await HttpClientHelper.AuthenticateWithCode(code);

                if (authenticated)
                {
                    AuthenticatingText = "Success: you're authenticated";
                    //ShowSpinner = false;

                    Dispatcher.Dispatch(() =>
                    {
                        _settings.IsFullScreen = false;
                        NavigationService.Navigate(typeof(Views.MapPage));
                    }, 3000);
                }
                else
                {
                    AuthenticatingText = "An error occured";
                    //ShowSpinner = false;
                }

            }
        }
    }
}
