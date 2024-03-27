using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace YP3
{
    internal class DatabaseManager
    {
        MySqlConnection connection = new MySqlConnection("server = localhost; port = 3306; username = root; password = 111; database = YP3");

        // Метод, который открывает соединение, если оно закрыто
        public void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        // Метод, который закрывает соединение, если оно открыто
        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }
        // Метод для выполнения запросов типа INSERT, UPDATE, DELETE
        public void ExecuteNonQuery(string query)
        {
            try
            {
                OpenConnection();
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Ошибка при выполнении запроса: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        // Метод для выполнения запросов типа SELECT и заполнения DataTable
        public DataTable ExecuteQuery(string query)
        {
            DataTable table = new DataTable();
            try
            {
                OpenConnection();
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(table);
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Ошибка при выполнении запроса: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return table;
        }
        // Метод для проверки логина и пароля
        public bool CheckLogin(string username, string password)
        {
            bool isValid = false;

            try
            {
                OpenConnection();
                string query = "SELECT COUNT(*) FROM Employees WHERE employee_login = @username AND employee_password = SHA2(@password, 256)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                int count = Convert.ToInt32(command.ExecuteScalar());
                isValid = count > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Ошибка при проверке логина и пароля: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return isValid;
        }

        public object ExecuteScalar(string query, params MySqlParameter[] parameters)
        {
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddRange(parameters);
            OpenConnection();
            object result = command.ExecuteScalar();
            CloseConnection();
            return result;
        }

        public void ExecuteAdapter(string query, DataTable table)
        {
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            adapter.Fill(table);
        }
        public MySqlConnection GetConnection()
        {
            return connection;
        }

    }
}
