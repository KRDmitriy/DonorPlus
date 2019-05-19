using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DonorPlusLib
{
    public static class MessageLog
    {
        /// <summary>
        /// Добавляет новое сообщение в журнал
        /// </summary>
        /// <param name="authorId">ID автора сообщения</param>
        /// <param name="clientId">ID получателя сообщения</param>
        /// <param name="message">Текст сообщения</param>
        /// <returns>Строка ошибки</returns>
        public static string Add(int authorId, int clientId, string message)
        {
            string errorMessage = "OK";

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"INSERT INTO {"MessageLog"}
                        VALUES (@AuthorId, @ClientId, @ServerTime, @MessageText)"
                };
                command.Parameters.AddWithValue("@AuthorId", authorId);
                command.Parameters.AddWithValue("@ClientId", clientId);
                command.Parameters.AddWithValue("@ServerTime", DateTime.Now);
                command.Parameters.AddWithValue("@MessageText", message);

                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                errorMessage = ex.Message;
            }
            catch (Exception ex)
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
        /// Получение списка сообщений
        /// </summary>
        /// <param name="ownId">ID клиента</param>
        /// <param name="interlocutorId">ID собеседника</param>
        /// <param name="ownMessages">Список сообщений только клиента</param>
        /// <param name="interlocutorMessages">Список сообщений только собеседника</param>
        /// <returns>Строка ошибки</returns>
        public static string GetMessages(int ownId, int interlocutorId, out List<Message> ownMessages,
                                        out List<Message> interlocutorMessages)
        {
            string errorMessage = "OK";
            ownMessages = new List<Message>();
            interlocutorMessages = new List<Message>();

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT * FROM {"MessageLog"}
                                    WHERE (AuthorId='{ownId}' AND ClientId='{interlocutorId}')
                                    ORDER BY ServerTime"
                };

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ownMessages.Add(new Message(reader.GetValue(3).ToString(),
                            (DateTime)reader.GetValue(2)));
                    }
                    reader.Close();
                }
                else
                {
                    errorMessage = "Нет сообщений";
                }

                command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT * FROM {"MessageLog"}
                                    WHERE AuthorId='{interlocutorId}' AND ClientId='{ownId}'
                                    ORDER BY ServerTime"
                };

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        interlocutorMessages.Add(new Message(reader.GetValue(3).ToString(),
                            (DateTime)reader.GetValue(2)));
                    }
                    reader.Close();
                    errorMessage = "OK";
                }
                else
                {
                    errorMessage = "Нет сообщений";
                }
            }
            catch (SqlException ex)
            {
                errorMessage = ex.Message;
            }
            catch (Exception ex)
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
        /// Получение списка сообщений
        /// </summary>
        /// <param name="ownId">ID клиента</param>
        /// <param name="interlocutorId">ID собеседника</param>
        /// <param name="messages">Список всех сообщений</param>
        /// <returns>Строка ошибки</returns>
        public static ResultObj GetMessages(int ownId, int interlocutorId)
        {
            string errorMessage = "OK";
            List<Message> messages = new List<Message>();

            SqlConnection connection = Connect.MakeNewConnect;

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT * FROM {"MessageLog"}
                                    WHERE (AuthorId='{ownId}' AND ClientId='{interlocutorId}')
                                    OR (AuthorId='{interlocutorId}' AND ClientId='{ownId}')
                                    ORDER BY ServerTime"
                };

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        messages.Add(new Message(int.Parse(reader.GetValue(0).ToString()),
                            reader.GetValue(3).ToString(), reader.GetDateTime(2)));
                    }
                    reader.Close();
                }
                else
                {
                    messages = null;
                    errorMessage = "Нет сообщений";
                }
            }
            catch (SqlException ex)
            {
                messages = null;
                errorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                messages = null;
                errorMessage = ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return new ResultObj { ErrorMessage = errorMessage, Messages = messages };
        }
    }
}
