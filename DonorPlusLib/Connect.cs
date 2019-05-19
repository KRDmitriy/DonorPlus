using System.Data.SqlClient;

namespace DonorPlusLib
{
    internal class Connect
    {
        /// <summary>
        /// Создание SQLConnection
        /// </summary>
        /// <param name="connectionStr">Строка подключения</param>
        /// <returns>Новое SqlConnection</returns>
        public static SqlConnection MakeNewConnect => new SqlConnection(Constants.СonnectionString);
    }
}