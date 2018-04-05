using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _29Lifts.Api.Rides.Models;
using _29Lifts.Helper;
using Newtonsoft.Json;

namespace _29Lifts.Api.User
{
    public class LyftUserApi : ILyftUserApi
    {
        private static readonly string USER_URI = "https://api.lyft.com/v1/rides";

        public async Task<RideHistories> GetRideHistories(int limit = 10)
        {
            using (var client = await HttpClientHelper.GetUserHttpClient())
            {
                var response = await client.GetAsync(USER_URI + $"?start_time=2015-01-01T00:00:00Z&limit={limit}");
                var responseText = await response.Content.ReadAsStringAsync();
                var rideHistories = JsonConvert.DeserializeObject<RideHistories>(responseText);

                return rideHistories;
            }
        }
    }
}
