using _29Lifts.Api.Public;
using _29Lifts.Api.Public.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace _29Lifts.ViewModels.OrderPage
{
    public class LyftModalDialogViewModel : INotifyPropertyChanged
    {
        //public ImageSource Image { get; set; }
        //public string Title { get; set; }
        //public string Description { get; set; }
        //public string Minimum { get; set; }
        //public string BaseFare { get; set; }
        //public string PerMile { get; set; }
        //public string PerMinute { get; set; }


        ImageSource _Image = default(ImageSource);
        public ImageSource Image { get { return _Image; } set { Set(ref _Image, value); } }

        string _Title = default(string);
        public string Title { get { return _Title; } set { Set(ref _Title, value); } }

        string _Description = default(string);
        public string Description { get { return _Description; } set { Set(ref _Description, value); } }

        string _Minimum = default(string);
        public string Minimum { get { return _Minimum; } set { Set(ref _Minimum, value); } }

        string _BaseFare = default(string);
        public string BaseFare { get { return _BaseFare; } set { Set(ref _BaseFare, value); } }

        string _PerMile = default(string);
        public string PerMile { get { return _PerMile; } set { Set(ref _PerMile, value); } }

        string _PerMinute = default(string);
        public string PerMinute { get { return _PerMinute; } set { Set(ref _PerMinute, value); } }


        string _ShortTitle = default(string);
        public string ShortTitle { get { return _ShortTitle; } set { Set(ref _ShortTitle, value); } }

        private static LyftModalDialogViewModel LyftInstance(RideType ride)
        {

            return new LyftModalDialogViewModel()
            {
                Image = new BitmapImage(new Uri("ms-appx://29Quizlet/Assets/Lyft.png")),
                Title = "Lyft",
                ShortTitle = "Fast ride, 4 seats",
                Description = "A personal ride for when you need to get to your destination fast. Fits up to 4 people.",
                Minimum = $"{ride.PricingDetails.CostMinimum/100.0:C}",
                BaseFare = $"{ride.PricingDetails.BaseCharge / 100.0:C}",
                PerMile = $"{ride.PricingDetails.CostPerMile / 100.0:C}",
                PerMinute = $"{ride.PricingDetails.CostPerMinute / 100.0:C}",
            };
        }

        private static LyftModalDialogViewModel PlusInstance(RideType ride)
        {

            return new LyftModalDialogViewModel()
            {
                Image = new BitmapImage(new Uri("ms-appx://29Quizlet/Assets/LyftPlus.png")),
                Title = "Plus",
                ShortTitle = "Supersized ride, 6 seats",
                Description = "A supersized ride when you need more space. Fits up to 6 people.",
                Minimum = $"{ride.PricingDetails.CostMinimum / 100.0:C}",
                BaseFare = $"{ride.PricingDetails.BaseCharge / 100.0:C}",
                PerMile = $"{ride.PricingDetails.CostPerMile / 100.0:C}",
                PerMinute = $"{ride.PricingDetails.CostPerMinute / 100.0:C}",
            };
        }

        private static LyftModalDialogViewModel DefaultInstance()
        {


            return new LyftModalDialogViewModel()
            {
                Image = new BitmapImage(new Uri("ms-appx://29Quizlet/Assets/Lyft.png")),
                Title = "Lyft",
                Description = "A personal ride for when you need to get to your destination fast. Fits up to 4 people.",
                Minimum = "$3.80",
                BaseFare = "$0.00",
                PerMile = "$0.90",
                PerMinute = "$0.12"
            };
        }

        public static LyftModalDialogViewModel EmptyInstanceByType(LyftRideTypeEnum type, RideTypes rides = null)
        {
            switch (type)
            {
                case LyftRideTypeEnum.Lyft:
                    var rideLyft = rides.Rides.Where(x => x.Ride_Type == LyftRideTypeEnum.Lyft).SingleOrDefault();
                    return LyftInstance(rideLyft);
                case LyftRideTypeEnum.Plus:
                    var ridePlus = rides.Rides.Where(x => x.Ride_Type == LyftRideTypeEnum.Plus).SingleOrDefault();
                    return PlusInstance(ridePlus);
                case LyftRideTypeEnum.Unknown:
                case LyftRideTypeEnum.Line:
                case LyftRideTypeEnum.Premier:
                default:
                    return DefaultInstance();
            }
        }


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

    }
}
