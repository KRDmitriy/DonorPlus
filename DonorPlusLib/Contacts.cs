using System.Collections.Generic;
using System.Data.SqlClient;

namespace DonorPlusLib
{
    public static class Contacts
    {
        /// <summary>
        /// Добавление новой связи между клиентами
        /// </summary>
        /// <param name="firstId">ID 1го клиента</param>
        /// <param name="secondId">ID 2го клиента</param>
        /// <returns>Строка ошибки</returns>
        public static string Add(int firstId, int secondId)
        {
            string errorMessage = "OK";

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                if (!CheckContact(firstId, secondId))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand
                    {
                        Connection = connection,
                        CommandText = $@"INSERT INTO Contacts VALUES (@FirstId, @SecondId)"
                    };

                    command.Parameters.AddWithValue("@FirstId", firstId);
                    command.Parameters.AddWithValue("@SecondId", secondId);

                    command.ExecuteNonQuery();

                    command = new SqlCommand
                    {
                        Connection = connection,
                        CommandText = $@"INSERT INTO Contacts VALUES (@SecondId, @FirstId)"
                    };

                    command.Parameters.AddWithValue("@FirstId", firstId);
                    command.Parameters.AddWithValue("@SecondId", secondId);

                    command.ExecuteNonQuery();
                }
                else
                {
                    errorMessage = "Есть контакт!";
                }
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
        /// Список всех связей клиента
        /// </summary>
        /// <param name="Id">ID клиента</param>
        /// <returns>Список тех, с кем общается клиент</returns>
        public static ResultObj GetContacts(int Id)
        {
            List<int> contacts = new List<int>();
            string errorMessage = "OK";

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT * FROM Contacts WHERE FirstId='{Id}'"
                };

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        contacts.Add(int.Parse(reader.GetValue(1).ToString()));
                    }
                    reader.Close();
                }
                else
                {
                    contacts = null;
                    errorMessage = "Здесь появятся Ваши контакты";
                }
            }
            catch (SqlException ex)
            {
                contacts = null;
                errorMessage = ex.Message;
            }
            catch (System.Exception ex)
            {
                contacts = null;
                errorMessage = ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return new ResultObj { ErrorMessage = errorMessage, Contacts = contacts };
        }

        /// <summary>
        /// Проверка существования контакта
        /// </summary>
        /// <param name="Id">ID 1го клиента</param>
        /// <param name="Id2">ID 2го клиента</param>
        /// <returns>Наличие контакта</returns>
        public static bool CheckContact(int Id, int Id2)
        {
            string errorMessage = "OK";

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT * FROM Contacts WHERE FirstId='{Id}' AND SecondId = '{Id2}'"
                };

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    return true;
                }
                return false;
            }
            catch (SqlException ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            catch (System.Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
