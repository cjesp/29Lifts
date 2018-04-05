using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Lifts.Api.Rides.Models
{
    [DataContract]
    public class ErrorDetail
    {
        public string cancel_confirmation { get; set; }
    }

    [DataContract]
    public class Cancellation
    {
        [DataMember(Name = "error")]
        public string Error { get; set; }

        [DataMember(Name = "error_detail")]
        public List<ErrorDetail> ErrorDetail { get; set; }

        [DataMember(Name = "amount")]
        public int? Amount { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "token_duration")]
        public int TokenDuration { get; set; }
    }
}
