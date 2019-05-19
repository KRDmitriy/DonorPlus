using System.Data.SqlClient;

namespace DonorPlusLib
{
    public static class Photo
    {
        /// <summary>
        /// Получение фотографии пользователя
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <param name="image">Фотография пользователя</param>
        /// <returns>Строка ошибки</returns>
        public static ResultObj Get(int id)
        {
            byte[] image = null;
            string errorMessage = "OK";

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT * FROM {"Photo"} WHERE ID='{id}'"
                };

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        image = (byte[])(reader.GetValue(2));
                    }
                    reader.Close();
                }
                else
                {
                    errorMessage = "Фотография не добавлена";
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

            return new ResultObj { ErrorMessage = errorMessage, Image = image };
        }

        /// <summary>
        /// Добавление фотографии пользователя
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <param name="image">Фотография пользователя</param>
        /// <returns>Строка ошибки</returns>
        public static string Push(int id, byte[] image)
        {
            string errorMessage = "OK";

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT * FROM {"Photo"} WHERE ID='{id}'"
                };

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Close();
                    command = new SqlCommand
                    {
                        Connection = connection,
                        CommandText = $@"UPDATE {"Photo"} SET photo = @image WHERE ID = {id}"
                    };
                    command.Parameters.AddWithValue("@image", image);

                    command.ExecuteNonQuery();
                }
                else
                {
                    reader.Close();
                    command = new SqlCommand
                    {
                        Connection = connection,
                        CommandText = $@"INSERT INTO {"Photo"} VALUES(@id, 0, @photo)"
                    };

                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@photo", image);

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
