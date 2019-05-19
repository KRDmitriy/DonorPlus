using System.Security.Cryptography;
using System.Text;

namespace DonorPlusLib
{
    public class MyOwnSecurity
    {
        /// <summary>
        /// Создание хэша
        /// </summary>
        /// <param name="md5Hash">Хэш</param>
        /// <param name="input">Исходная строка</param>
        /// <returns>Строка хэша</returns>
        private static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Получение готового хэша 
        /// </summary>
        /// <param name="from">Строка для хэширования</param>
        /// <returns>Строка хэша</returns>
        public static string Hash(string from)
        {
            return GetMd5Hash(Constants.MD5HASH, from);
        }
    }
}
