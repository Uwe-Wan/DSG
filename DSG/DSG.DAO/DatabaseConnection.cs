using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Data.Common;
using System.Data;

namespace DSG.DAO
{
    public class DatabaseConnection
    {
        public void DefineAndExecuteCommandNonQueryWithRowCheck(string sqlText, params SqlParameter[] sqlParameter)
        {
            int affectedRows = DefineAndExecuteCommandNonQuery(sqlText, sqlParameter);

            if (affectedRows < 1)
            {
                throw new Exception("No rows affected");
            }
        }

        public int DefineAndExecuteCommandNonQuery(string sqlText, params SqlParameter[] sqlParameter)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["DSGConnectionString"]))
            {
                SqlCommand command = new SqlCommand(sqlText, connection);

                if (sqlParameter != null)
                {
                    command.Parameters.AddRange(sqlParameter);
                }

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public DataTable GetDataTableFromConnection(string sqlText, params SqlParameter[] sqlParameters)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["DSGConnectionString"]))
            {
                SqlCommand command = new SqlCommand(sqlText, connection);

                if(sqlParameters != null)
                {
                    command.Parameters.AddRange(sqlParameters);
                }

                connection.Open();
                DbDataReader reader = command.ExecuteReader();
                DataTable dataTable = new DataTable();

                dataTable.Load(reader);
                return dataTable;
            }
        }

        public int? GetCountFromConnection(string sqlText)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["DSGConnectionString"]))
            {
                SqlCommand command = new SqlCommand(sqlText, connection);

                connection.Open();
                DbDataReader reader = command.ExecuteReader();

                int? count = null;

                if(reader.Read())
                {
                    count = reader.GetInt32(0);
                }

                return count;
            }
        }
    }
}
