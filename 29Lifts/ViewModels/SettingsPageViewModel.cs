using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.SettingsService;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace _29Lifts.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();
        private Services.SettingsServices.SettingsService _settings;

        public SettingsPageViewModel()
        {
            _settings = Services.SettingsServices.SettingsService.Instance;
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            _settings.ShowHamburgerButton = true;
            return Task.CompletedTask;
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> pageState, bool suspending)
        {
            _settings.ShowHamburgerButton = false;
            return Task.CompletedTask;
        }
    }

    public class SettingsPartViewModel : ViewModelBase
    {
        Services.SettingsServices.SettingsService _settings;

        Uri _ProfileImageSrc = default(Uri);
        public Uri ProfileImageSrc { get { return _ProfileImageSrc; } set { Set(ref _ProfileImageSrc, value); } }

        public SettingsPartViewModel()
        {
            _settings = Services.SettingsServices.SettingsService.Instance;
            ProfileImageSrc = _settings.ProfileImageSrc;
        }

        public string LogInOutText
        {
            get
            {
                if (_settings.UserToken == null)
                    return "Log in";
                else
                    return "Log out";
            }

            set {  }
        }


        DelegateCommand _LogInOut;
        public DelegateCommand LogInOut
           => _LogInOut ?? (_LogInOut = new DelegateCommand(() =>
           {
               if (_settings.UserToken != null)
               {
                   _settings.UserToken = null;
               }
               else
               {
                   var guid = Guid.NewGuid().ToString();
                   var uri = new Uri($"https://api.lyft.com/oauth/authorize?response_type=code&client_id=85X7D3CKRnHD&scope=public rides.read offline rides.request profile&state={guid}");
                   Task.Run(() => Launcher.LaunchUriAsync(uri));
               }
           }, () => true));

        DelegateCommand _SelectImageCommand;
        public DelegateCommand SelectImageCommand
           => _SelectImageCommand ?? (_SelectImageCommand = new DelegateCommand(async () =>
           {
               var filePicker = new FileOpenPicker();
               filePicker.ViewMode = PickerViewMode.Thumbnail;
               filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
               filePicker.FileTypeFilter.Add(".png");
               filePicker.FileTypeFilter.Add(".jpg");
               filePicker.FileTypeFilter.Add(".jpeg");
               var file = await filePicker.PickSingleFileAsync();
               if (file != null)
               {
                   
                   var localFolder = ApplicationData.Current.LocalFolder;
                   var newCopy = await file.CopyAsync(localFolder, "ProfileImage", NameCollisionOption.ReplaceExisting);
                   _settings.ProfileImageSrc = new Uri(newCopy.Path);
                   ProfileImageSrc = _settings.ProfileImageSrc;
               }
           }, () => true));


        DelegateCommand _SetDefaultImageCommand;
        public DelegateCommand SetDefaultImageCommand
           => _SetDefaultImageCommand ?? (_SetDefaultImageCommand = new DelegateCommand(() =>
           {
               _settings.ProfileImageSrc = new Uri("ms-appx://29Quizlet/Assets/avatar.png");
               ProfileImageSrc = _settings.ProfileImageSrc;
           }, () => true));

        public async void SelectImage(object sender, RoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.FileTypeFilter.Clear();
            filePicker.FileTypeFilter.Add(".png");
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.FileTypeFilter.Add(".bmp");
            var file = await filePicker.PickSingleFileAsync();
            if (file != null)
            {
                //todo: fix image loading

            }
        }


           
        }

    public class AboutPartViewModel : ViewModelBase
    {
        public Uri Logo => Windows.ApplicationModel.Package.Current.Logo;

        public string DisplayName => Windows.ApplicationModel.Package.Current.DisplayName;

        public string Publisher => $"Copyright \u00A9 2016, {Windows.ApplicationModel.Package.Current.PublisherDisplayName}";

        public Uri LyftLogo => new Uri("ms-appx://29Quizlet/Assets/lyft_100pxLogo.png");

        public string Version
        {
            get
            {
                var v = Windows.ApplicationModel.Package.Current.Id.Version;
                return $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
            }
        }

        public Uri RateMe => new Uri("http://aka.ms/template10");
    }
}

