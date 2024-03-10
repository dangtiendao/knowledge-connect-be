using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnector
{
    public class DatabaseConnectorBase
    {
        private readonly string connectionString;

        public DatabaseConnectorBase(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected void ExecuteCommand(Action<MySqlConnection, MySqlCommand> action)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    try
                    {
                        connection.Open();
                        action(connection, command);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        throw;
                    }
                }
            }
        }


    }
}
