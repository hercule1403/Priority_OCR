using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GlobalDMS.Data
{
    public class DatabaseHelper
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

        public static SqlConnection GetOpenConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public static List<T> ExecuteStoredProcedure<T>(string procedureName, SqlParameter[] parameters, Func<SqlDataReader, T> mapFunction)
        {
            using (SqlConnection connection = GetOpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<T> result = new List<T>();
                        while (reader.Read())
                        {
                            T item = mapFunction(reader);
                            result.Add(item);
                        }
                        return result;
                    }
                }
            }
        }

        public static void ExecuteStoredProcedure(string procedureName, SqlParameter[] parameters)
        {
            using (SqlConnection connection = GetOpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}