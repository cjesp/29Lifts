using _29Lifts.Api.Rides.Models.SubModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Lifts.Api.Rides.Models
{
    [DataContract]
    public class Ride
    {
        [DataMember(Name = "ride_id")]
        public string RideId { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "origin")]
        public Origin Origin { get; set; }

        [DataMember(Name = "destination")]
        public Destination Destination { get; set; }

        [DataMember(Name = "passenger")]
        public Passenger Passenger { get; set; }

        [DataMember(Name = "primetime_confirmation_token")]
        public string PrimetimeConfirmationToken { get; set; }
    }
    
}
