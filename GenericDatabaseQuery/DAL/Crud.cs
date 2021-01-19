using EntityFramework.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GenericDatabaseQuery.DAL
{
    public partial class Crud : Connection
    {
        public Crud(string server, string database, string username, string password) : base(server, database, username, password) { }
     
       public bool Create<T>(T obj)
       {
            string tableName = obj.GetType().Name;
            string query = "INSERT INTO";

            string names = "(";
            string values = "(";

            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                names += propertyInfo.Name + ",";
                values += "'" + propertyInfo.GetValue(obj) + "',";
                // do stuff here
            }

            names = names.Substring(0, names.Length - 1);
            names += ")";

            values = values.Substring(0, values.Length - 1);
            values += ")";

            query += $" {tableName} {names} VALUES {values}";

            ExecuteQuery(query);
            return true;
       }

        // the read all method
        public List<T> SelectAll<T>() where T : new()
        {
            string tableName = typeof(T).Name;
            string query = $"SELECT * FROM {tableName}";
 
            return ExecuteSelectQuery<T>(query); 
        }

        // the read specific method
        public List<T> SelectWhere<T>(T obj) where T : new()
        {
            string tableName = typeof(T).Name;
            string query = $"SELECT * FROM {tableName} WHERE ";

            string searchValue = "";

            int counter = 0;

            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                string Id = propertyInfo.GetValue(obj).ToString();
                if ("0" != Id)
                {
                    if (counter == 0)
                    {
                        searchValue += $"{propertyInfo.Name}  = '{propertyInfo.GetValue(obj)}'";
                        counter++;
                    }
                    else
                    {
                        searchValue += $"AND {propertyInfo.Name}  = '{propertyInfo.GetValue(obj)}'";
                    }
                }         
            }
            query += $"{searchValue}";
            return ExecuteSelectQuery<T>(query);
        }       

        public string Update<T>(T obj)
        {
            string tableName = obj.GetType().Name;
            string query = $"UPDATE {tableName} SET ";

            int counter = 0;

            string updateValue = "";
            string whereValue = "";

            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(obj) != null || (propertyInfo.GetValue(obj) != null && propertyInfo.GetValue(obj).ToString() != ""))
                {
                    if (propertyInfo.Name.ToLower() == "id")
                    {
                        whereValue += $"{propertyInfo.Name } = '{propertyInfo.GetValue(obj)}'";
                    }
                    else
                    {
                        if (counter == 0)
                        {
                            updateValue += $"{propertyInfo.Name } = '{propertyInfo.GetValue(obj)}'";
                        }
                        else
                        {
                            updateValue += $" , {propertyInfo.Name } = '{propertyInfo.GetValue(obj)}'";
                        }

                        counter++;
                    } 
                }
            }
            query += $"{updateValue} WHERE {whereValue}";
            return query;
        }

        public string Delete<T>(T obj)
        {
            string tableName = obj.GetType().Name;
            string query = "DELETE FROM ";

            string searchValue = "";

            //this holds the current position, for adding the right syntax for the query
            int counter = 0;

            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(obj) != null || (propertyInfo.GetValue(obj) != null && propertyInfo.GetValue(obj).ToString() != ""))
                {
                    if (counter == 0)
                    {
                        searchValue += $"{propertyInfo.Name } = '{propertyInfo.GetValue(obj)}'";
                    }
                    else
                    {
                        searchValue += $" AND {propertyInfo.Name } = '{propertyInfo.GetValue(obj)}'";
                    }

                    counter++;
                }
            }

            query += $"{tableName} WHERE {searchValue}";
            return query;
        }
    }
}
