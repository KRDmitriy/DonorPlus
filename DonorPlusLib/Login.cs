using System.Data.SqlClient;

namespace DonorPlusLib
{
    public class Login
    {
        /// <summary>
        /// Вход клиента в приложение
        /// </summary>
        /// <param name="emailOrPhone">Адрес электронной почты или мобильного телефона клиента</param>
        /// <param name="password">Пароль клиента</param>
        /// <param name="client">Класс клиента со всеми его данными</param>
        /// <returns>Строка ошибки</returns>
        public static ResultObj CheckLogin(string emailOrPhone, string password)
        {
            Client client;
            Client newClient = null;
            string errorMessage = "OK";

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT * FROM {Constants.BASENAME} WHERE EMAIL='{emailOrPhone}' OR PHONE='{emailOrPhone}'"
                };

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.GetValue(7).ToString() != MyOwnSecurity.Hash(password))
                        {
                            errorMessage = "Неверный пароль";
                        }
                        else
                        {
                            newClient = new Client(reader.GetInt32(0),
                                reader.GetValue(2).ToString(),
                                reader.GetValue(3).ToString(),
                                reader.GetValue(4).ToString(),
                                reader.GetValue(5).ToString(),
                                reader.GetValue(6).ToString(),
                                reader.GetValue(7).ToString());
                            newClient.ChangeType(reader.GetInt32(1));
                        }
                    }
                    reader.Close();
                }
                else
                {
                    client = null;
                    errorMessage = "Данная почта не зарегистрирована";
                }
            }
            catch (SqlException ex)
            {
                newClient = null;
                errorMessage = ex.Message;
            }
            catch (System.Exception ex)
            {
                newClient = null;
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
        /// Проверка существования аккаунта с соответствующим телефоном
        /// </summary>
        /// <param name="phone">Номер мобильного телефона</param>
        /// <param name="errorMessage">Строка ошибки. Если ошибки нет, возвращает 'OK'</param>
        /// <returns>Возвращает boolean наличия телефона в БД</returns>
        public static bool CheckPhone(string phone, out string errorMessage)
        {
            errorMessage = "OK";
            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT * FROM {Constants.BASENAME} WHERE PHONE='{phone}'"
                };

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    return true;
                }

                errorMessage = "Нет аккаунта с данным телефоном";
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
