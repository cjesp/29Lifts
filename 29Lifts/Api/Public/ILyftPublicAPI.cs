using _29Lifts.Api.Public.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Lifts.Api.Public
{
    public interface ILyftPublicAPI
    {
        Task<ETAEstimates> GetETAs(double lat, double lng, string ride_type = null);
        Task<RideTypes> GetRideTypes(double lat, double lng, string ride_type = null);
        Task<CostEstimates> GetCostEstimates(double start_lat, double start_lng, double end_lat, double end_lng, string ride_type = null);
        Task<NearbyDrivers> GetNearbyDrivers(double lat, double lng);
    }
}
