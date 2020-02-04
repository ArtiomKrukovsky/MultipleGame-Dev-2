using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UnityEngine;

public static class DbHelper
{
    public const string ConnectionString = "Data Source=SQL5050.site4now.net;" +
                                            "Initial Catalog = DB_A54D04_broadwood1;" +
                                            "User Id = DB_A54D04_broadwood1_admin;" +
                                            "Password=qwe123ZXC.;";

    public static void DeleteServerFromDB(string serverNameToDelete)
    {
        if (!string.IsNullOrEmpty(serverNameToDelete))
        {
            using (SqlConnection dbConnection = new SqlConnection(ConnectionString))
            {
                dbConnection.Open();

                string deleteServerQuery = "DELETE FROM Servers WHERE ServerName = @nameToDelete;";

                using (SqlCommand command = new SqlCommand(deleteServerQuery, dbConnection))
                {
                    command.Parameters.Add("@nameToDelete", SqlDbType.NVarChar).Value = serverNameToDelete;
                    command.ExecuteNonQuery();

                    Debug.Log("Success deleting from database");
                }

                dbConnection.Close();
            }
        }
    }
}
