using _29Lifts.Api.Rides.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Lifts.Api.User
{
    public interface ILyftUserApi
    {
        Task<RideHistories> GetRideHistories(int limit = 10);
    }
}
