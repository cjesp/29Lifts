using _29Lifts.Api.Public.models;
using _29Lifts.Api.Rides.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Lifts.Api.Rides
{
    public interface ILyftRidesApi
    {
        Task<Ride> StartRide(double start_lat, double start_lng, double end_lat, double end_lng, LyftRideTypeEnum ride_type, string prime_time_token = null);
        Task<RideDetails> RideDetails(string rideId);
        Task<Cancellation> CancelRide(string rideId, string token = null);
        Task<bool> RateAndTipRide(string rideId, int tipAmount, int rating, string feedback, string currency = "USD");
    }
}
