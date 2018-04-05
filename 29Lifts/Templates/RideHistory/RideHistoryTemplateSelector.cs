using _29Lifts.ViewModels.RideHistoryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace _29Lifts.Templates.RideHistory
{
    public class RideHistoryTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ActiveRide { get; set; }
        public DataTemplate InactiveRide { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var ride = item as ActiveRide;
            if (ride != null)
                return this.ActiveRide;
            else
                return this.InactiveRide;
            
        }
    }
}
