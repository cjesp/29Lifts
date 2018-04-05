# 29Lifts
A Lyft clone for Windows Phone (UWP). This was a project for investigating the UWP platform but was never released to the store. It uses the [Template10](https://github.com/Windows-XAML/Template10) library.


# How to compile and run this project
* Setup a developer account at [Lyft](https://www.lyft.com/developers)
* Get a key for the UWP Map control at [Microsoft](https://docs.microsoft.com/en-us/windows/uwp/maps-and-location/authentication-key)

The Lyft secrets and tokens needs to be setup in the HttpClientHelper class. The map control is used in the following pages:
* MapPage 
* OrderPage 
* RidePage

Once this has been setup it should build and run. However, for completing a ride (as in the video below), you'll need to change state of the simulated ride, since we're not actually requesting a real Lyft ride. I've made a small WPF tool to change the state of the simulated ride, which can be found HERE. Once you press "Request lyft" while debugging in VS studio, the app will log the current ride id as well as token to the console. Copy/paste these to HERE and change the state (to simulate a real ride). Note: changing state is merely an api call to Lyft, so you could also use curl etc.


Check out this 2 min video showing a basic Lyft ride:
[![Video](https://img.youtube.com/vi/ZUbiNLOy4mc/maxresdefault.jpg)](https://youtu.be/ZUbiNLOy4mc)
