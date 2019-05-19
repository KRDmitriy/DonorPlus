using System.Collections.Generic;

namespace DonorPlusLib
{
    public class ResultObj
    {
        public Client User { get; set; }

        public string ErrorMessage { get; set; }

        public byte[] Image { get; set; }

        public List<Message> Messages { get; set; }

        public List<int> Contacts { get; set; }
        public string BloodGroup { get; set; }
        public string RFactor { get; set; }
        public Request Request { get; set; }
        public List<Request> Requests { get; set; }
    }
}
