using System.Data.SqlClient;

namespace DonorPlusLib
{
    public static class BloodData
    {
        /// <summary>
        /// Получение данных крови клиента по его ID
        /// </summary>
        /// <param name="id">ID клиента</param>
        /// <returns>Данные крови</returns>
        public static ResultObj Get(int id)
        {
            string bloodGroup = "";
            string rFactor = "";
            string errorMessage = "OK";

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT * FROM BloodData WHERE ID='{id}'"
                };

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        bloodGroup = reader.GetString(1);
                        rFactor = reader.GetString(2);
                    }
                    reader.Close();
                }
                else
                {
                    errorMessage = "Данные не добавлены";
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

            return new ResultObj { ErrorMessage = errorMessage, BloodGroup = bloodGroup, RFactor = rFactor };
        }


        /// <summary>
        /// Добавление данных клиента
        /// </summary>
        /// <param name="id">ID клиента</param>
        /// <param name="bloodGroup">Группа крови клиента</param>
        /// <param name="rFactor">Резус-фактор крови клиента</param>
        /// <returns>Строка ошибки</returns>
        public static string Push(int id, string bloodGroup, string rFactor)
        {
            string errorMessage = "OK";

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT * FROM BloodData WHERE ID='{id}'"
                };

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Close();
                    command = new SqlCommand
                    {
                        Connection = connection,
                        CommandText = $@"UPDATE BloodData SET bloodGroup = @bg, rFactor = @rf WHERE ID = {id}"
                    };
                    command.Parameters.AddWithValue("@bg", bloodGroup);
                    command.Parameters.AddWithValue("@rf", rFactor);

                    command.ExecuteNonQuery();
                }
                else
                {
                    reader.Close();
                    command = new SqlCommand
                    {
                        Connection = connection,
                        CommandText = $@"INSERT INTO BloodData VALUES(@id, @bloodGroup, @rFactor)"
                    };

                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@bloodGroup", bloodGroup);
                    command.Parameters.AddWithValue("@rFactor", rFactor);

                    command.ExecuteNonQuery();
                }
                reader.Close();
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
