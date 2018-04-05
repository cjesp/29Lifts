using _29Lifts.Api.Public.models;
using _29Lifts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace _29Lifts.Converter
{
    public class RideTypeEnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var rideType = (LyftRideTypeEnum)value;

            switch (rideType)
            {
                case LyftRideTypeEnum.Lyft:
                    return "Lyft";
                case LyftRideTypeEnum.Plus:
                    return "Plus";
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
