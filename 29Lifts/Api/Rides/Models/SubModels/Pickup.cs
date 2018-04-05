using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Lifts.Api.Rides.Models.SubModels
{
    [DataContract]
    public class Pickup
    {
        [DataMember(Name = "lat")]
        public double Lat { get; set; }

        [DataMember(Name = "lng")]
        public double Lng { get; set; }

        [DataMember(Name = "address ")]
        public string Address { get; set; }

        [DataMember(Name = "time")]
        public DateTime Time { get; set; }
    }
}
