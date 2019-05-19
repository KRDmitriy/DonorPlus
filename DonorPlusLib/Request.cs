namespace DonorPlusLib
{
    public class Request
    {
        public int ID { get; set; }
        public int AuthorID { get; set; }
        public string BloodGroup { get; set; }
        public string RFactor { get; set; }
        public string ExtraBloodData { get; set; }
        public string Descripton { get; set; }
        public string Region { get; set; }
        public bool Solved { get; set; }

        public override string ToString()
        {
            return $"{BloodGroup} {RFactor} {Region} {ExtraBloodData} {Descripton}";
        }
    }
}
