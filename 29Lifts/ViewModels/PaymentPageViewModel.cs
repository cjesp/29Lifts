using _29Lifts.Api.Rides.Models;
using _29Lifts.Services.SettingsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Template10.Services.NavigationService;
using _29Lifts.Navigation;

namespace _29Lifts.ViewModels
{
    public enum TippingAmount
    {
        NoTip,
        OneDollar,
        TwoDollars,
        FiveDollars,
        OtherAmount
    }

    public class PaymentPageViewModel : ViewModelBase
    {
        private SettingsService _service;
        Uri _ProfileImage = default(Uri);
        public string OtherTipAmount { get; set; }
        public Uri ProfileImage { get { return _ProfileImage; } set { Set(ref _ProfileImage, value); } }
        public TippingAmount AmountTipped { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; }
        public decimal Tips { get; set; }
        public string RideId { get; set; }
        decimal _Cost = default(decimal);
        public decimal Cost { get { return _Cost; } set { Set(ref _Cost, value); } }

        string _RateMeText = default(string);
        public string RateMeText { get { return _RateMeText; } set { Set(ref _RateMeText, value); } }

        string _DriverName = default(string);
        public string DriverName { get { return _DriverName; } set { Set(ref _DriverName, value); } }

        string _TotalCost = default(string);
        public string TotalCost { get { return _TotalCost; } set { Set(ref _TotalCost, value); } }

        bool _ShowOtherTip = default(bool);
        public bool ShowOtherTip { get { return _ShowOtherTip; } set { Set(ref _ShowOtherTip, value); } }
        
        string _TripPrice = default(string);
        public string TripPrice { get { return _TripPrice; } set { Set(ref _TripPrice, value); } }
        
        string _PrimeTimePrice = default(string);
        public string PrimeTimePrice { get { return _PrimeTimePrice; } set { Set(ref _PrimeTimePrice, value); } }

        string _TipsPrice = default(string);
        public string TipsPrice { get { return _TipsPrice; } set { Set(ref _TipsPrice, value); } }
        
        string _ServiceFeePrice = default(string);
        public string ServiceFeePrice { get { return _ServiceFeePrice; } set { Set(ref _ServiceFeePrice, value); } }

        string _Credit = default(string);
        public string Credit { get { return _Credit; } set { Set(ref _Credit, value); } }

        // Tipping colors
        SolidColorBrush _NoTipBackground = default(SolidColorBrush);
        public SolidColorBrush NoTipBackground { get { return _NoTipBackground; } set { Set(ref _NoTipBackground, value); } }

        SolidColorBrush _NoTipForeground = default(SolidColorBrush);
        public SolidColorBrush NoTipForeground { get { return _NoTipForeground; } set { Set(ref _NoTipForeground, value); } }

        SolidColorBrush _DollarOneBackground = default(SolidColorBrush);
        public SolidColorBrush DollarOneBackground { get { return _DollarOneBackground; } set { Set(ref _DollarOneBackground, value); } }

        SolidColorBrush _DollarOneForeground = default(SolidColorBrush);
        public SolidColorBrush DollarOneForeground { get { return _DollarOneForeground; } set { Set(ref _DollarOneForeground, value); } }

        SolidColorBrush _DollarTwoBackground = default(SolidColorBrush);
        public SolidColorBrush DollarTwoBackground { get { return _DollarTwoBackground; } set { Set(ref _DollarTwoBackground, value); } }

        SolidColorBrush _DollarTwoForeground = default(SolidColorBrush);
        public SolidColorBrush DollarTwoForeground { get { return _DollarTwoForeground; } set { Set(ref _DollarTwoForeground, value); } }

        SolidColorBrush _DollarFiveBackground = default(SolidColorBrush);
        public SolidColorBrush DollarFiveBackground { get { return _DollarFiveBackground; } set { Set(ref _DollarFiveBackground, value); } }

        SolidColorBrush _DollarFiveForeground = default(SolidColorBrush);
        public SolidColorBrush DollarFiveForeground { get { return _DollarFiveForeground; } set { Set(ref _DollarFiveForeground, value); } }

        SolidColorBrush _OtherAmountBackground = default(SolidColorBrush);
        public SolidColorBrush OtherAmountBackground { get { return _OtherAmountBackground; } set { Set(ref _OtherAmountBackground, value); } }

        SolidColorBrush _OtherAmountForeground = default(SolidColorBrush);
        public SolidColorBrush OtherAmountForeground { get { return _OtherAmountForeground; } set { Set(ref _OtherAmountForeground, value); } }

        bool _ShowPrices = default(bool);
        public bool ShowPrices { get { return _ShowPrices; } set { Set(ref _ShowPrices, value); } }
        
        public PaymentPageViewModel()
        {
            Comments = string.Empty;
            Rating = 5;
            Tips = 0.0m;
            RideId = string.Empty;
            ShowOtherTip = false;
            OtherTipAmount = string.Empty;
            AmountTipped = TippingAmount.NoTip;
            TripPrice = "$0.00";
            PrimeTimePrice = "$0.00";
            ServiceFeePrice = "$0.00";
            TipsPrice = "$0.00";
            Credit = "$0.00";
            NoTipBackground = new SolidColorBrush(Color.FromArgb(100, 53, 35, 132));
            NoTipForeground = new SolidColorBrush(Colors.White);
            DollarOneBackground = new SolidColorBrush(Colors.White);
            DollarOneForeground = new SolidColorBrush(Colors.Black);
            DollarTwoBackground = new SolidColorBrush(Colors.White);
            DollarTwoForeground = new SolidColorBrush(Colors.Black);
            DollarFiveBackground = new SolidColorBrush(Colors.White);
            DollarFiveForeground = new SolidColorBrush(Colors.Black);
            OtherAmountBackground = new SolidColorBrush(Colors.White);
            OtherAmountForeground = new SolidColorBrush(Colors.Black);
            _service = SettingsService.Instance;
            _service.ShowHamburgerButton = true;
            ShowPrices = false;
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var details = parameter as PaymentPageNavigationModel;

            // if arriving from Ride page
            if (details != null)
            {
                DriverName = details.DriverName;
                ProfileImage = new Uri(details.ProfileImageSrc);
                RateMeText = $"Rate {DriverName} for this Lyft";
                RideId = details.RideId;

                if (details.LineItems != null)
                {
                    decimal sum = 0.0m;
                    foreach (var lineItem in details.LineItems)
                    {
                        if (lineItem.Type == "Ride")
                        {
                            TripPrice = $"{(decimal)lineItem.Amount / 100.0m:C}";
                        }
                        if (lineItem.Type == "Service fee")
                        {
                            ServiceFeePrice = $"{(decimal)lineItem.Amount / 100.0m:C}";
                        }
                        if (lineItem.Type == "Prime Time")
                        {
                            PrimeTimePrice = $"{(decimal)lineItem.Amount / 100.0m:C}";
                        }
                        if (lineItem.Type == "Credit")
                        {
                            Credit = $"{(decimal)lineItem.Amount / 100.0m:C}";
                        }
                        if (lineItem.Type == "Tip")
                        {
                            continue;
                        }

                        sum += (decimal) lineItem.Amount / 100.0m;
                    }

                    Cost = sum;
                    TotalCost = $"{Cost:C}";

                }

                // don't go back if we're coming from the ride view
                NavigationService.FrameFacade.BackRequested += FrameFacade_BackRequested;

                return;
            }

            // if arriving from elsewhere
            string id = string.Empty;
            try
            {
                id = (string)parameter;
            }
            catch (Exception e)
            {
                var dialog = new MessageDialog($"Error: {e.Message}");
                throw;
            }

            await Task.CompletedTask;    
        }

        private void FrameFacade_BackRequested(object sender, HandledEventArgs e)
        {
            e.Handled = true;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            NavigationService.FrameFacade.BackRequested -= FrameFacade_BackRequested;
            await Task.CompletedTask;
        }

        public void NextTapped(object sender, TappedRoutedEventArgs e)
        {
            var tips = (int)(Tips * 100m);
            var parameter = new RatePageNavigationModel()
            {
                Name = DriverName,
                RideId = RideId,
                Tip = tips,
                ProfileImage = ProfileImage,
            };
            NavigationService.Navigate(typeof(Views.RatePage), parameter);
            
        }

        public void ClosePricesTapped(object sender, TappedRoutedEventArgs e)
        {
            ShowPrices = false;
        }

        public void PriceTapped(object sender, TappedRoutedEventArgs e)
        {
            ShowPrices = true;
        }

        public void TipTapped(object sender, TappedRoutedEventArgs e)
        {
            var elem = sender as Border;
            TippingAmount amount;
            if (elem != null)
            {
                amount = ConvertTagToAmount((string)elem.Tag);
                // set colors to default
                NoTipBackground = new SolidColorBrush(Colors.White);
                NoTipForeground = new SolidColorBrush(Colors.Black);
                DollarOneBackground = new SolidColorBrush(Colors.White);
                DollarOneForeground = new SolidColorBrush(Colors.Black);
                DollarTwoBackground = new SolidColorBrush(Colors.White);
                DollarTwoForeground = new SolidColorBrush(Colors.Black);
                DollarFiveBackground = new SolidColorBrush(Colors.White);
                DollarFiveForeground = new SolidColorBrush(Colors.Black);
                OtherAmountBackground = new SolidColorBrush(Colors.White);
                OtherAmountForeground = new SolidColorBrush(Colors.Black);
                ShowOtherTip = false;

                switch (amount)
                {
                    case TippingAmount.NoTip:
                        NoTipBackground = new SolidColorBrush(Color.FromArgb(100, 53, 35, 132));
                        NoTipForeground = new SolidColorBrush(Colors.White);
                        AmountTipped = TippingAmount.NoTip;
                        UpdateTotalCost();
                        break;
                    case TippingAmount.OneDollar:
                        DollarOneBackground = new SolidColorBrush(Color.FromArgb(100, 53, 35, 132));
                        DollarOneForeground = new SolidColorBrush(Colors.White);
                        AmountTipped = TippingAmount.OneDollar;
                        UpdateTotalCost();
                        break;
                    case TippingAmount.TwoDollars:
                        DollarTwoBackground = new SolidColorBrush(Color.FromArgb(100, 53, 35, 132));
                        DollarTwoForeground = new SolidColorBrush(Colors.White);
                        AmountTipped = TippingAmount.TwoDollars;
                        UpdateTotalCost();
                        break;
                    case TippingAmount.FiveDollars:
                        DollarFiveBackground = new SolidColorBrush(Color.FromArgb(100, 53, 35, 132));
                        DollarFiveForeground = new SolidColorBrush(Colors.White);
                        AmountTipped = TippingAmount.FiveDollars;
                        UpdateTotalCost();
                        break;
                    case TippingAmount.OtherAmount:
                        OtherAmountBackground = new SolidColorBrush(Color.FromArgb(100, 53, 35, 132));
                        OtherAmountForeground = new SolidColorBrush(Colors.White);
                        ShowOtherTip = true;
                        AmountTipped = TippingAmount.OtherAmount;
                        break;
                    default:
                        break;
                }
            }
        }

        public void OtherTipAmountChanged(object sender, TextChangedEventArgs e)
        {
            var txtBox = sender as TextBox;
            if (txtBox != null)
            {
                var msg = txtBox.Text;
                decimal tip;
                if (decimal.TryParse(msg, out tip))
                {
                    TotalCost = $"{Cost + tip:C}";
                    TipsPrice = $"{tip:C}";
                    Tips = tip;
                }
            }
        }

        private void UpdateTotalCost()
        {
            switch (AmountTipped)
            {
                case TippingAmount.NoTip:
                    TotalCost = $"{Cost:C}";
                    TipsPrice = $"{0.00m:C}";
                    Tips = 0.0m;
                    break;
                case TippingAmount.OneDollar:
                    TotalCost = $"{Cost + 1.00m:C}";
                    TipsPrice = $"{1.00m:C}";
                    Tips = 1.00m;
                    break;
                case TippingAmount.TwoDollars:
                    TotalCost = $"{Cost + 2.00m:C}";
                    TipsPrice = $"{2.00m:C}";
                    Tips = 2.00m;
                    break;
                case TippingAmount.FiveDollars:
                    TotalCost = $"{Cost + 5.00m:C}";
                    TipsPrice = $"{5.00m:C}";
                    Tips = 5.00m;
                    break;
                case TippingAmount.OtherAmount:
                    decimal tip;
                    decimal.TryParse(OtherTipAmount, out tip);
                    TotalCost = $"{Cost + tip:C}";
                    TipsPrice = $"{tip:C}";
                    Tips = tip;
                    break;
                default:
                    break;
            }
        }

        private TippingAmount ConvertTagToAmount(string tag)
        {
            switch (tag)
            {
                case "NoTip":
                    return TippingAmount.NoTip;
                case "DollarOne":
                    return TippingAmount.OneDollar;
                case "DollarTwo":
                    return TippingAmount.TwoDollars;
                case "DollarFive":
                    return TippingAmount.FiveDollars;
                case "OtherAmount":
                    return TippingAmount.OtherAmount;
                default:
                    return TippingAmount.NoTip;
            }
        }
    }
}
