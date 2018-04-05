using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace _29Lifts.Controls
{
    public sealed partial class TimeNeedle : UserControl
    {

        public TimeNeedle()
        {
            this.InitializeComponent();
        }

        //private void NeedleTapped(object sender, TappedRoutedEventArgs e)
        //{
        //    CountAvailable = !CountAvailable;
        //    if (CountAvailable)
        //    {
        //        NoAvail.Begin();
        //    }
        //    else
        //    {
        //        ShowTime.Begin();
        //    }

        //}

        public bool CountAvailable
        {
            get { return (bool)GetValue(CountAvailableProperty); }
            set
            {
                if (value)
                {
                    ShowTime.Begin();
                }
                else
                {
                    NoAvail.Begin();
                }
                SetValue(CountAvailableProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CountAvailable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CountAvailableProperty =
            DependencyProperty.Register("CountAvailable", typeof(bool), typeof(TimeNeedle), new PropertyMetadata(0));



        public string MinuteCounter
        {
            get { return (string)GetValue(MinuteCounterProperty); }
            set { SetValue(MinuteCounterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinuteCounter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinuteCounterProperty =
            DependencyProperty.Register("MinuteCounter", typeof(string), typeof(TimeNeedle), new PropertyMetadata(0));


    }
}
