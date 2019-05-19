namespace DonorPlusLib
{
    public class Client
    {
        private byte[] photo;
        private readonly int id;
        private int type;
        private readonly string surname;
        private readonly string name;
        private readonly string secondname;
        private readonly string email;
        private readonly string phone;
        private string bloodGroup;
        private string rFactor;
        private readonly string password;

        public byte[] Photo => photo;
        public int Id => id;
        public int Type => type;
        public string Surname => surname;
        public string Name => name;
        public string SecondName => secondname;
        public string Email => email;
        public string Phone => phone;
        public string BloodGroup => bloodGroup;
        public string RFactor => rFactor;
        public string Password => password;

        public Client() { }

        public Client(int id, string surname, string name,
            string secondname, string email, string phone, string password)
        {
            this.id = id;
            this.surname = surname;
            this.name = name;
            this.secondname = secondname;
            this.email = email;
            this.phone = phone;
            bloodGroup = "";
            rFactor = "";
            this.password = password;
        }

        public Client(int id, string surname, string name,
            string secondname, string email, string phone, string password, byte[] image)
        {
            this.id = id;
            this.surname = surname;
            this.name = name;
            this.secondname = secondname;
            this.email = email;
            this.phone = phone;
            bloodGroup = "";
            rFactor = "";
            this.password = password;
            this.photo = image;
        }

        public void AddPhoto(byte[] image)
        {
            photo = image;
        }

        public void AddBloodGroup(string bloodGroup)
        {
            this.bloodGroup = bloodGroup;
        }

        public void AddRFactor(string rFactor)
        {
            this.rFactor = rFactor;
        }

        public void ChangeType(int newType)
        {
            type = newType;
        }

        public override string ToString()
        {
            return $"{Surname}\n{Name}";
        }
    }
}
