using System;
using System.Data.SqlClient;

namespace DonorPlusLib
{
    public class Table
    {
        /// <summary>
        /// Очищение заданной таблицы без её удаления
        /// </summary>
        /// <param name="basename">Название таблицы</param>
        public static void Clean(string basename)
        {
            SqlConnection connection = Connect.MakeNewConnect;
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"TRUNCATE TABLE {basename}"
                };

                command.ExecuteNonQuery();

                Console.WriteLine($"{basename} cleaned");
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Удаление пользователя из таблицы
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <param name="basename">Название таблицы</param>
        public static void DeleteUser(int id, string basename)
        {
            SqlConnection connection = Connect.MakeNewConnect;
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"DELETE FROM {basename} WHERE id = {id}"
                };

                command.ExecuteNonQuery();

                Console.WriteLine($"{basename} cleaned");
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Показывает всю информацию таблицы
        /// </summary>
        /// <param name="basename">Название таблицы</param>
        public static void Show(string basename)
        {
            SqlConnection connection = Connect.MakeNewConnect;
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = $@"SELECT * FROM {basename}"
                };

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string rowInfo = $"{reader.ToString()}";
                        Console.WriteLine(rowInfo);
                    }
                }
                reader.Close();

                Console.WriteLine($"{basename} showed");
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}