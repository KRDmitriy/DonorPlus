using System.Collections.Generic;
using System.Data.SqlClient;

namespace DonorPlusLib
{
    public class Requests
    {
        /// <summary>
        /// Получает все текущие запросы из базы
        /// </summary>
        /// <returns>Список запросов</returns>
        public static ResultObj GetAll()
        {
            List<Request> requests = new List<Request>();
            string errorMessage = "OK";

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT * FROM Requests"
                };

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        requests.Add(new Request()
                        {
                            ID = reader.GetInt32(0),
                            AuthorID = reader.GetInt32(1),
                            BloodGroup = reader.GetString(2),
                            RFactor = reader.GetString(3),
                            ExtraBloodData = reader.GetString(4),
                            Descripton = reader.GetString(5),
                            Region = reader.GetString(6),
                            Solved = reader.GetInt32(7) == 1 ? true : false
                        });
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

            return new ResultObj { ErrorMessage = errorMessage, Requests = requests };
        }

        /// <summary>
        /// Обновляет данные определенного запроса
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <returns>Результат обновления</returns>
        public static string Push(Request request)
        {
            string errorMessage = "OK";

            if (request.ExtraBloodData == null)
                request.ExtraBloodData = "";
            if (request.Descripton == null)
                request.Descripton = "";

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT * FROM Requests WHERE ID='{request.ID}'"
                };

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Close();
                    command = new SqlCommand
                    {
                        Connection = connection,
                        CommandText = $@"UPDATE Requests SET solved = 1 WHERE ID = {request.ID}"
                    };

                    command.ExecuteNonQuery();
                }
                else
                {
                    reader.Close();
                    command = new SqlCommand
                    {
                        Connection = connection,
                        CommandText = $@"INSERT INTO Requests VALUES(@authorId, @bloodGroup, @rFactor, @extraBloodData,
                                            @descriptionText, @region, @solved)"
                    };

                    command.Parameters.AddWithValue("@authorId", request.AuthorID);
                    command.Parameters.AddWithValue("@bloodGroup", request.BloodGroup);
                    command.Parameters.AddWithValue("@rFactor", request.RFactor);
                    command.Parameters.AddWithValue("@extraBloodData", request.ExtraBloodData);
                    command.Parameters.AddWithValue("@descriptionText", request.Descripton);
                    command.Parameters.AddWithValue("@region", request.Region);
                    command.Parameters.AddWithValue("@solved", 0);

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
