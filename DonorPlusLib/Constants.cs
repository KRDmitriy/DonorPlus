using System.Security.Cryptography;

namespace DonorPlusLib
{
    public static class Constants
    {
        public const string BASENAME = "";
        public const string SERVER = "";
        public const string DATABASE = "";
        public const string LOGIN = "";
        public const string PASSWORD = "";

        /// <summary>
        /// Создание нового MD5 хэша
        /// </summary>
        public static MD5 MD5HASH => MD5.Create();

        /// <summary>
        /// Строка подключения к Azure SQL Server
        /// </summary>
        public static string СonnectionString => $@"Data Source={SERVER}.database.windows.net;
                       Initial Catalog={DATABASE};
                       Integrated Security=True; 
                       User ID={LOGIN};
                       Password={PASSWORD}; 
                       Trusted_Connection=False; 
                       Encrypt=True;";
    }
}
