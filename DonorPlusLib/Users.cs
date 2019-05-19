using System.Collections.Generic;
using System.Data.SqlClient;

namespace DonorPlusLib
{
    public static class Users
    {
        /// <summary>
        /// Получение информации по ID
        /// </summary>
        /// <param name="id">ID искомого клиента</param>
        /// <param name="client">Класс клиента со всей информацией</param>
        /// <returns>Строка ошибки</returns>
        public static ResultObj GetInfoAboutUser(int id)
        {
            Client client = null;
            Client newClient = null;
            string errorMessage = "OK";

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT * FROM {Constants.BASENAME} WHERE ID='{id}'"
                };

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        newClient = new Client(reader.GetInt32(0),
                                 reader.GetValue(2).ToString(),
                                 reader.GetValue(3).ToString(),
                                 reader.GetValue(4).ToString(),
                                 reader.GetValue(5).ToString(),
                                 reader.GetValue(6).ToString(),
                                 null);
                        newClient.ChangeType(reader.GetInt32(1));
                    }
                    reader.Close();
                }
                else
                {
                    client = null;
                    errorMessage = "Пользователь с таким ID отсутствует";
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

            client = newClient;

            return new ResultObj { ErrorMessage = errorMessage, User = client };
        }

        /// <summary>
        /// Поиск пользователей по фамилии и имени
        /// </summary>
        /// <param name="searchString">Спрока поиска: фамилия и имя</param>
        /// <param name="client">Если нашли, то список пользователей</param>
        /// <returns>Строка ошибки</returns>
        public static List<Client> SearchForUser(string searchString)
        {
            List<Client> clients;
            List<Client> newClients = new List<Client>();
            clients = null;
            string errorMessage = "OK";
            string[] data = new string[100];
            int index = 0;
            foreach (string item in searchString.Split())
            {
                if (item != null)
                {
                    data[index++] = item;
                }
            }

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                connection.Open();

                SqlCommand command;
                switch (index)
                {
                    case 2:
                        command = new SqlCommand
                        {
                            Connection = connection,
                            CommandText = $@"SELECT * FROM {Constants.BASENAME}
                                             WHERE (SURNAME like N'{data[0]}' OR NAME = N'{data[1]}')
                                             OR (SURNAME like N'{data[1]}' OR NAME = N'{data[0]}')"
                        };
                        break;
                    default:
                        command = new SqlCommand
                        {
                            Connection = connection,
                            CommandText = $@"SELECT * FROM {Constants.BASENAME}
                                             WHERE SURNAME like N'{data[0]}' OR NAME = N'{data[0]}'"
                        };
                        break;
                }

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Client newClient = new Client(reader.GetInt32(0),
                                reader.GetValue(2).ToString(),
                                reader.GetValue(3).ToString(),
                                reader.GetValue(4).ToString(),
                                reader.GetValue(5).ToString(),
                                reader.GetValue(6).ToString(),
                                null);
                        newClient.ChangeType(reader.GetInt32(1));
                        newClients.Add(newClient);                        
                    }
                    reader.Close();
                    clients = newClients;
                }
                else
                {
                    clients = null;
                }
            }
            catch (SqlException ex)
            {
                clients = null;
                errorMessage = ex.Message;
            }
            catch (System.Exception ex)
            {
                clients = null;
                errorMessage = ex.Message;
            }
            finally
            {
                connection.Close();
            }
            return clients;
        }


        /// <summary>
        /// Получение всех пользователей
        /// </summary>
        /// <param name="clients">Список всех пользователей</param>
        /// <returns>Строка ошибки</returns>
        public static List<Client> GetAllUsers()
        {
            string errorMessage = "OK";
           List<Client> clients = new List<Client>();

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT * FROM {Constants.BASENAME}"
                };

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Client newClient = new Client(reader.GetInt32(0),
                                reader.GetValue(2).ToString(),
                                reader.GetValue(3).ToString(),
                                reader.GetValue(4).ToString(),
                                reader.GetValue(5).ToString(),
                                reader.GetValue(6).ToString(),
                                reader.GetValue(7).ToString());
                        newClient.ChangeType(reader.GetInt32(1));
                        clients.Add(newClient);
                    }
                    reader.Close();
                }
            }
            catch (SqlException ex)
            {
                clients = null;
                errorMessage = ex.Message;
            }
            catch (System.Exception ex)
            {
                clients = null;
                errorMessage = ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return clients;
        }

        public static ResultObj GetFromPhoneOrMail(string searchString)
        {
            Client client = null;
            string errorMessage = "OK";
            string[] data = new string[100];
            int index = 0;
            foreach (string item in searchString.Split())
            {
                if (item != null)
                {
                    data[index++] = item;
                }
            }

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT * FROM {Constants.BASENAME}
                                        WHERE PHONE = N'{data[0]}' OR EMAIL = N'{data[0]}'"
                };

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        client = new Client(reader.GetInt32(0),
                                reader.GetValue(2).ToString(),
                                reader.GetValue(3).ToString(),
                                reader.GetValue(4).ToString(),
                                reader.GetValue(5).ToString(),
                                reader.GetValue(6).ToString(),
                                reader.GetValue(7).ToString());
                        client.ChangeType(reader.GetInt32(1));
                    }
                    reader.Close();
                }
                else
                {
                    client = null;
                }
            }
            catch (SqlException ex)
            {
                client = null;
                errorMessage = ex.Message;
            }
            catch (System.Exception ex)
            {
                client = null;
                errorMessage = ex.Message;
            }
            finally
            {
                connection.Close();
            }
            return new ResultObj { User = client, ErrorMessage = errorMessage };
        }
    }
}
