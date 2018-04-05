using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _29Lifts.Api.Public.models;
using _29Lifts.Helper;
using Newtonsoft.Json;

namespace _29Lifts.Api.Public
{
    public class LyftPublicApi : ILyftPublicAPI
    {
        private static readonly string ETA_URI = "https://api.lyft.com/v1/eta?";
        private static readonly string RIDE_TYPE_URI = "https://api.lyft.com/v1/ridetypes?";
        private static readonly string COST_URI = "https://api.lyft.com/v1/cost?";
        private static readonly string DRIVERS_URI = "https://api.lyft.com/v1/drivers?";

        public async Task<CostEstimates> GetCostEstimates(double start_lat, double start_lng, double end_lat, double end_lng, string ride_type = null)
        {
            using (var client = await HttpClientHelper.GetPublicHttpClient())
            {
                var requestUri = new Uri($"{COST_URI}start_lat={start_lat}&start_lng={start_lng}&end_lat={end_lat}&end_lng={end_lng}");
                var response = await client.GetAsync(requestUri);
                var responseText = await response.Content.ReadAsStringAsync();
                var costEstimates = JsonConvert.DeserializeObject<CostEstimates>(responseText);
                return costEstimates;
            }
        }

        public async Task<ETAEstimates> GetETAs(double lat, double lng, string ride_type = null)
        {
            using (var client = await HttpClientHelper.GetPublicHttpClient())
            {
                var requestUri = new Uri($"{ETA_URI}lat={lat}&lng={lng}");
                var response = await client.GetAsync(requestUri);
                var responseText = await response.Content.ReadAsStringAsync();
                var etas = JsonConvert.DeserializeObject<ETAEstimates>(responseText);
                return etas;
            }
        }

        public async Task<RideTypes> GetRideTypes(double lat, double lng, string ride_type = null)
        {
            using (var client = await HttpClientHelper.GetPublicHttpClient())
            {
                var requestUri = new Uri($"{RIDE_TYPE_URI}lat={lat}&lng={lng}");
                var response = await client.GetAsync(requestUri);
                var responseText = await response.Content.ReadAsStringAsync();
                var rides = JsonConvert.DeserializeObject<RideTypes>(responseText);
                return rides;
            }
        }

        public async Task<NearbyDrivers> GetNearbyDrivers(double lat, double lng)
        {
            using (var client = await HttpClientHelper.GetPublicHttpClient())
            {
                var requestUri = new Uri($"{DRIVERS_URI}lat={lat}&lng={lng}");
                var response = await client.GetAsync(requestUri);
                var responseText = await response.Content.ReadAsStringAsync();
                var nearbyDrivers = JsonConvert.DeserializeObject<NearbyDrivers>(responseText);
                return nearbyDrivers;
            }
        }
    }
}
