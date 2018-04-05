using _29Lifts.Api.Rides.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace _29Lifts.ViewModels
{
    public class ReceiptPageViewModel : ViewModelBase
    {
        string _Title = default(string);
        public string Title { get { return _Title; } set { Set(ref _Title, value); } }

        string _ThanksText = default(string);
        public string ThanksText { get { return _ThanksText; } set { Set(ref _ThanksText, value); } }

        Uri _ProfileImageSrc = default(Uri);
        public Uri ProfileImageSrc { get { return _ProfileImageSrc; } set { Set(ref _ProfileImageSrc, value); } }

        string _PickupTime = default(string);
        public string PickupTime { get { return _PickupTime; } set { Set(ref _PickupTime, value); } }
        
        string _PickupAddress = default(string);
        public string PickupAddress { get { return _PickupAddress; } set { Set(ref _PickupAddress, value); } }

        string _DropoffTime = default(string);
        public string DropoffTime { get { return _DropoffTime; } set { Set(ref _DropoffTime, value); } }

        string _DropoffAddress = default(string);
        public string DropoffAddress { get { return _DropoffAddress; } set { Set(ref _DropoffAddress, value); } }

        string _LyftFareText = default(string);
        public string LyftFareText { get { return _LyftFareText; } set { Set(ref _LyftFareText, value); } }

        string _LyftFarePrice = default(string);
        public string LyftFarePrice { get { return _LyftFarePrice; } set { Set(ref _LyftFarePrice, value); } }

        string _ServicefeePrice = default(string);
        public string ServicefeePrice { get { return _ServicefeePrice; } set { Set(ref _ServicefeePrice, value); } }

        string _PrimeTimeText = default(string);
        public string PrimeTimeText { get { return _PrimeTimeText; } set { Set(ref _PrimeTimeText, value); } }

        string _PrimeTimePrice = default(string);
        public string PrimeTimePrice { get { return _PrimeTimePrice; } set { Set(ref _PrimeTimePrice, value); } }

        string _TipPrice = default(string);
        public string TipPrice { get { return _TipPrice; } set { Set(ref _TipPrice, value); } }

        string _Subtotal = default(string);
        public string Subtotal { get { return _Subtotal; } set { Set(ref _Subtotal, value); } }

        string _CreditPrice = default(string);
        public string CreditPrice { get { return _CreditPrice; } set { Set(ref _CreditPrice, value); } }

        string _TotalPrice = default(string);
        public string TotalPrice { get { return _TotalPrice; } set { Set(ref _TotalPrice, value); } }

        string _CancellationFee = default(string);
        public string CancellationFee { get { return _CancellationFee; } set { Set(ref _CancellationFee, value); } }

        bool _IsDroppedOff = default(bool);
        public bool IsDroppedOff { get { return _IsDroppedOff; } set { Set(ref _IsDroppedOff, value); } }

        string _Dropoff = default(string);
        public string Dropoff { get { return _Dropoff; } set { Set(ref _Dropoff, value); } }



        public ReceiptPageViewModel()
        {
            IsDroppedOff = true;
            Dropoff = "Dropoff";
            CreditPrice = $"{0.0m:C}";
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var ride = parameter as RideHistory;

            if (ride != null)
            {

                if (ride.Status == "canceled")
                {
                    IsDroppedOff = false;
                    Dropoff = "Cancelled";
                    Title = ride.RequestedAt.ToString("MMM d yyyy - h:mm tt");
                    ProfileImageSrc = new Uri(ride.Driver?.ImageUrl ?? "ms-appx://29Quizlet/Assets/avatar.png");
                    TotalPrice = $"{(decimal)ride.Price.Amount / 100.0m:C}";
                    ThanksText = $"This ride was cancelled";
                    PickupTime = $"{ride.Pickup?.Time.ToString("h:mm tt")}";
                    PickupAddress = $"{ride.Origin?.Address}";
                }
                else
                {
                    Title = ride.RequestedAt.ToString("MMM d yyyy - h:mm tt");
                    ProfileImageSrc = new Uri(ride.Driver?.ImageUrl ?? "ms-appx://29Quizlet/Assets/avatar.png");
                    TotalPrice = $"{(decimal)ride.Price.Amount / 100.0m:C}";
                    ThanksText = $"Thanks for riding with {ride.Driver?.FirstName}" ?? string.Empty;
                    PickupTime = $"{ride.Pickup?.Time.ToString("h:mm tt")}";
                    PickupAddress = $"{ride.Origin?.Address}";
                    DropoffTime = $"{ride.Dropoff?.Time.ToString("h:mm tt")}";
                    DropoffAddress = $"{ride.Dropoff?.Address}";
                    LyftFareText = $"Lyft fare";
                    CreditPrice = $"{0.0:C}";

                    if (!ride.PrimetimePercentage?.Equals("0%") ?? false)
                        PrimeTimeText = $"Prime time ({ride.PrimetimePercentage})";
                    else
                        PrimeTimeText = @"Prime Time";
                }

                decimal sum = 0.0m;
                foreach (var lineItem in ride.LineItems)
                {
                    if (lineItem.Type == "Ride")
                    {
                        LyftFarePrice = $"{(decimal)lineItem.Amount / 100.0m:C}";
                    }
                    if (lineItem.Type == "Service fee")
                    {
                        ServicefeePrice = $"{(decimal)lineItem.Amount / 100.0m:C}";
                    }
                    if (lineItem.Type == "Prime Time")
                    {
                        PrimeTimePrice = $"{(decimal)lineItem.Amount / 100.0m:C}";
                    }
                    if (lineItem.Type == "Credit")
                    {
                        CreditPrice = $"{(decimal)lineItem.Amount / 100.0m:C}";
                        continue; // we don't want to add this to the subtotal
                    }
                    if (lineItem.Type == "Tip")
                    {
                        TipPrice = $"{(decimal)lineItem.Amount / 100.0m:C}";
                    }
                    if (lineItem.Type == "Cancel Fee")
                    {
                        CancellationFee = $"{(decimal)lineItem.Amount / 100.0m:C}";
                    }

                    sum += (decimal)lineItem.Amount / 100.0m;
                }

                Subtotal = $"{sum:C}";
            }


            await Task.CompletedTask;
        }
    }
}
