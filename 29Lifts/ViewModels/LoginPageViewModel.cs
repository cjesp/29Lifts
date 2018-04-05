using _29Lifts.Api.Rides.Models.SubModels;
using _29Lifts.Helper;
using _29Lifts.Navigation;
using _29Lifts.Services.SettingsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace _29Lifts.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public string LoginText { get; set; } = "Login here";

        public void SkipTapped()
        {
            var settings = SettingsService.Instance;
            settings.IsFullScreen = false;
            NavigationService.Navigate(typeof(Views.MapPage));
        }

        public void LoginTapped()
        {
            var guid = Guid.NewGuid().ToString();
            var uri = new Uri($"https://api.lyft.com/oauth/authorize?response_type=code&client_id=85X7D3CKRnHD&scope=public rides.read offline rides.request profile&state={guid}");
            Task.Run(() => Launcher.LaunchUriAsync(uri));
        }

        public void PaymentClicked()
        {
            var parameter = new PaymentPageNavigationModel()
            {
                DriverName = "Claus",
                ProfileImageSrc = "ms-appx://29Quizlet/Assets/avatar.png",
                RideId = "123456789",
                LineItems = new List<LineItem>()
                {
                    new LineItem()
                    {
                        Type = "Trip",
                        Currency = "USD",
                        Amount = 1500,
                    },
                    new LineItem()
                    {
                        Type = "Service fee",
                        Currency = "USD",
                        Amount = 135,
                    },
                    new LineItem()
                    {
                        Type = "Tip",
                        Currency = "USD",
                        Amount = 100,
                    },
                    new LineItem()
                    {
                        Type = "Prime Time",
                        Currency = "USD",
                        Amount = 500,
                    },
                }
            };

            NavigationService.Navigate(typeof(Views.PaymentPage), parameter);
        }

    }
}
