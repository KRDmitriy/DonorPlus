using System.Data.SqlClient;

namespace DonorPlusLib
{
    public class Registration
    {
        /// <summary>
        /// Регистрация нового клиента
        /// </summary>
        /// <param name="client">Данные клиента</param>
        /// <returns>Строка ошибки</returns>
        public static string Add(Client client)
        {
            string errorMessage = "OK";

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"INSERT INTO {Constants.BASENAME} VALUES (@type, @surname, 
                                        @name, @secondname, @email, @phone, @password)"
                };

                command.Parameters.AddWithValue("@type", client.Type);
                command.Parameters.AddWithValue("@surname", client.Surname);
                command.Parameters.AddWithValue("@name", client.Name);
                command.Parameters.AddWithValue("@secondname", client.SecondName);
                command.Parameters.AddWithValue("@email", client.Email);
                command.Parameters.AddWithValue("@phone", client.Phone);
                command.Parameters.AddWithValue("@password", MyOwnSecurity.Hash(client.Password));

                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                errorMessage = ex.Message;
            }
            catch (System.Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                connection.Close();
            }
            return errorMessage;
        }


        /// <summary>
        /// Изменение данных клиента
        /// </summary>
        /// <param name="client">Данные клиента</param>
        /// <returns>Строка ошибки</returns>
        public static string Change(Client client)
        {
            string errorMessage = "OK";

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                Photo.Push(client.Id, client.Photo);
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"UPDATE {Constants.BASENAME} SET surname = N'{client.Surname}', 
                        name = N'{client.Name}', secondname = N'{client.SecondName}',
                        email = N'{client.Email}', phone = N'{client.Phone}', password = N'{MyOwnSecurity.Hash(client.Password).ToString()}'
                        WHERE Id = {client.Id}"
                };

                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                errorMessage = ex.Message;
            }
            catch (System.Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                connection.Close();
            }
            return errorMessage;
        }
    }
}
