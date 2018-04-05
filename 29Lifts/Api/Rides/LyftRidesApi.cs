using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _29Lifts.Api.Rides.Models;
using _29Lifts.Helper;
using System.Net.Http;
using _29Lifts.Api.Public.models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace _29Lifts.Api.Rides
{
    public class LyftRidesApi : ILyftRidesApi
    {
        private static readonly string ORDER_RIDE_URI = "https://api.lyft.com/v1/rides";

        public async Task<bool> RateAndTipRide(string rideId, int tipAmount, int rating, string feedback, string currency = null)
        {
            using (var client = await HttpClientHelper.GetUserHttpClient())
            {
                var scopeJson = $"{{\"rating\": {rating}, \"tip\": {{ \"amount\" : {tipAmount}, \"currency\" : \"{currency}\" }}, "
                    + $"\"feedback\": \"{feedback}\"}}";
                var content = new StringContent(scopeJson, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(ORDER_RIDE_URI + $"/{rideId}/rating",content);
                var responseText = await response.Content.ReadAsStringAsync();
                return response.IsSuccessStatusCode ? true : false;
                
            }
        }

        public async Task<Cancellation> CancelRide(string rideId, string token = null)
        {
            if (string.IsNullOrEmpty(token))
            {
                using (var client = await HttpClientHelper.GetUserHttpClient())
                {
                    var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(ORDER_RIDE_URI + $"/{rideId}/cancel", content);
                    var responseText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var rideDetails = JsonConvert.DeserializeObject<Cancellation>(responseText);

                        return rideDetails;
                    }

                    return null;
                }
            }
            else
            {
                using (var client = await HttpClientHelper.GetUserHttpClient())
                {
                    var scopeJson = $"{{\"token\": \"{token}\"}}";
                    var content = new StringContent(scopeJson, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(ORDER_RIDE_URI + $"/{rideId}/cancel", content);
                    var responseText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    else
                    {
                        var rideDetails = JsonConvert.DeserializeObject<Cancellation>(responseText);

                        return rideDetails;
                    }
                    
                }
            }

        }
        
        public async Task<RideDetails> RideDetails(string rideId)
        {
            using (var client = await HttpClientHelper.GetUserHttpClient())
            {
                var response = await client.GetAsync(ORDER_RIDE_URI + $"/{rideId}");
                var responseText = await response.Content.ReadAsStringAsync();
                var rideDetails = JsonConvert.DeserializeObject<RideDetails>(responseText);

                return rideDetails;
            }
        }

        public async Task<Ride> StartRide(double start_lat, double start_lng, double end_lat, double end_lng, LyftRideTypeEnum ride_type, string prime_time_token = null)
        {
            using (var client = await HttpClientHelper.GetUserHttpClient())
            {
                var type = LyftTypeEnumToString(ride_type);

                var scopeJson = $"{{\"ride_type\": \"{type}\", \"origin\": {{ \"lat\" : {start_lat}, \"lng\" : {start_lng} }}, " 
                    + $"\"destination\": {{ \"lat\" : {end_lat}, \"lng\" : {end_lng} }} }}";
                var content = new StringContent(scopeJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(ORDER_RIDE_URI, content);
                var responseText = await response.Content.ReadAsStringAsync();
                var ride = JsonConvert.DeserializeObject<Ride>(responseText);


                Debug.WriteLine($"ride id: {ride.RideId}");


                return ride;
            }
        }

        private string LyftTypeEnumToString(LyftRideTypeEnum lyftRide)
        {
            switch (lyftRide)
            {
                case LyftRideTypeEnum.Lyft:
                    return "lyft";
                case LyftRideTypeEnum.Plus:
                    return "lyft_plus";
                case LyftRideTypeEnum.Line:
                    return "lyft_line";
                case LyftRideTypeEnum.Premier:
                    return "lyft_premier";
                case LyftRideTypeEnum.Unknown:
                default:
                    return string.Empty;
            }
        }
    }
}
