using GenericDatabaseQuery.DAL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GenericDatabaseQuery
{
    public partial class Connection
    {
        public MySqlConnection MyConnection;

        public Connection(string server, string database, string username, string password)
        {
            string connectionString = $"server={server}; database={database}; UID={username}; password={password}";
            MyConnection = new MySqlConnection(connectionString);
        }

        public bool IsOpen()
        {
            try
            {
                MyConnection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool IsClosed()
        {
            try
            {
                MyConnection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        
        public List<T> ExecuteSelectQuery<T>(string query) where T : new()
        {
            List<T> returnValue = new List<T>();

            if (IsOpen())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, MyConnection))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        T oneRecord = new T();                    
                        foreach (PropertyInfo propertyInfo in oneRecord.GetType().GetProperties())
                        {    
                            var databaseResult = dataReader[propertyInfo.Name];
                            propertyInfo.SetValue(oneRecord, databaseResult, null);                            
                        }
                        returnValue.Add(oneRecord);
                    }
                }
                IsClosed();
                return returnValue;
            }
            return null;                
        }

        public bool ExecuteQuery(string query)
        {
            if (IsOpen())
            {                
                MySqlCommand cmd = new MySqlCommand(query, MyConnection);

                if(query == "" || query == null)
                {
                    return false;
                }

                //Execute command
                cmd.ExecuteNonQuery();

                IsClosed();
                return true;
            }
            return false;
        }       
    }
}