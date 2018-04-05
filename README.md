# 29Lifts
A Lyft clone for Windows Phone (UWP). This was a project for investigating the UWP platform but was never released to the store. It uses the [Template10](https://github.com/Windows-XAML/Template10) library. Tested with the Win 10 15063 SDK.


# How to compile and run this project
* Setup a developer account at [Lyft](https://www.lyft.com/developers)
* Get a key for the UWP Map control at [Microsoft](https://docs.microsoft.com/en-us/windows/uwp/maps-and-location/authentication-key)

The Lyft secrets and tokens needs to be setup in the HttpClientHelper class. The map control is used in the following pages:
* MapPage 
* OrderPage 
* RidePage

e.g. change "XXXXXXX" in MapServiceToken:
```XAML
<maps:MapControl x:Name="TheMap" Center="{x:Bind ViewModel.CenterPoint, Mode=OneWay}" ZoomLevel="18" 
                            MapServiceToken="XXXXXXX">
            <maps:MapItemsControl ItemsSource="{x:Bind ViewModel.Driver}">
                <maps:MapItemsControl.ItemTemplate>
                    <DataTemplate x:DataType="localvm:MapLyftIcon">
                        <Image Source="{x:Bind ImageSource}" maps:MapControl.Location="{x:Bind Location}" maps:MapControl.NormalizedAnchorPoint="{x:Bind Anchor}"/>
                    </DataTemplate>
                </maps:MapItemsControl.ItemTemplate>
            </maps:MapItemsControl>
            <TextBlock FontSize="14" HorizontalAlignment="Left" Foreground="Black" VerticalAlignment="Center" 
                        FontFamily="Segoe MDL2 Assets" Text="&#xE81D;" maps:MapControl.NormalizedAnchorPoint="0.5,0.5" maps:MapControl.Location="{x:Bind ViewModel.LocationPoint, Mode=OneWay}"/>
            <Image Source="ms-appx://29Lifts/Assets/pinPickup.png" maps:MapControl.Location="{x:Bind ViewModel.PickupPoint, Mode=OneWay}"
                   maps:MapControl.NormalizedAnchorPoint="0.5 1.0"/>
            <Image Source="ms-appx://29Lifts/Assets/pin.png" maps:MapControl.Location="{x:Bind ViewModel.DestinationPoint, Mode=OneWay}"
                   maps:MapControl.NormalizedAnchorPoint="0.5 1.0"/>
        </maps:MapControl>
```

Once this has been setup it should build and run. However, for completing a ride (as in the video below), you'll need to change state of the simulated ride, since we're not actually requesting a real Lyft ride. I've made a small WPF tool to change the state of the simulated ride, which can be found [here](https://github.com/cjesp/29LiftsSupportApp). Once you press "Request lyft" while debugging in VS studio, the app will log the current ride id as well as token to the console. Copy/paste these to the tool app and change the state (to simulate a real ride). Note: changing state is merely an api call to Lyft, so you could also use curl etc.


Check out this 2 min video showing a basic Lyft ride:
[![Video](https://img.youtube.com/vi/ZUbiNLOy4mc/maxresdefault.jpg)](https://youtu.be/ZUbiNLOy4mc)
