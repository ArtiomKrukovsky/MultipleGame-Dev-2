using System;
using System.Data;
using System.Data.Common;
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

                serverNameToDelete = serverNameToDelete.Replace("[", "");
                serverNameToDelete = serverNameToDelete.Replace("]", "");
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

    public static string GetQuestionFromDB(string mapName, string questNumber)
    {
        if (string.IsNullOrEmpty(mapName) || string.IsNullOrEmpty(questNumber))
        {
            Debug.Log("Quest is null");
            return BaseConstants.Messages.QuestNullMessage;
        }

        using (SqlConnection dbConnection = new SqlConnection(ConnectionString))
        {
            dbConnection.Open();

            string selectQuestQuery = "SELECT Question FROM Questions WHERE Map = @mapName AND QuestionNumber = @questNumber;";

            SqlCommand command = new SqlCommand(selectQuestQuery, dbConnection);

            command.Parameters.Add("@mapName", SqlDbType.NVarChar).Value = mapName;
            command.Parameters.Add("@questNumber", SqlDbType.NVarChar).Value = questNumber;

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    Debug.Log("Question is not exist in DB");
                    dbConnection.Close();
                    return BaseConstants.Messages.QuestNullMessage;
                }

                while (reader.Read())
                {
                    string question = reader.GetString(0);

                    if (!string.IsNullOrEmpty(question))
                    {
                        dbConnection.Close();
                        return question;
                    }
                }
            }

            Debug.Log("Question is not exist in DB");
            dbConnection.Close();
            return BaseConstants.Messages.QuestNullMessage;
        }
    }

    public static void SetRatingToBD(string mapName, string teamName, int? score)
    {
        if (string.IsNullOrEmpty(mapName) || string.IsNullOrEmpty(teamName) || score == null)
        {
            Debug.Log("Paramets to set rating is null");
            return;
        }

        using (SqlConnection dbConnection = new SqlConnection(ConnectionString))
        {
            dbConnection.Open();

            string query = "INSERT INTO Rating (Id, Team, Map, Score)" + " values (@id, @teamName, @mapName, @score) ";

            using (SqlCommand command = new SqlCommand(query, dbConnection))
            {
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = Guid.NewGuid().ToString();
                command.Parameters.Add("@mapName", SqlDbType.NVarChar).Value = mapName;
                command.Parameters.Add("@teamName", SqlDbType.NVarChar).Value = teamName;
                command.Parameters.Add("@score", SqlDbType.Int).Value = score;

                command.ExecuteNonQuery();
                Debug.Log("Success added rating to DB");
            }

            dbConnection.Close();
        }
    }

    public static void AddQuestionToDB(string question, string mapName, string questNumber)
    {
        if (string.IsNullOrEmpty(mapName) || string.IsNullOrEmpty(questNumber))
        {
            Debug.Log("Quest is null");
        }

        using (SqlConnection dbConnection = new SqlConnection(ConnectionString))
        {
            dbConnection.Open();

            string addQuestionQuery = "INSERT INTO Questions (Id, QuestionNumber, Map, Question) VALUES (@id, @questNumber, @mapName, @question) ";

            using (SqlCommand command = new SqlCommand(addQuestionQuery, dbConnection))
            {
                command.Parameters.Add("@mapName", SqlDbType.NVarChar).Value = mapName;
                command.Parameters.Add("@question", SqlDbType.NVarChar).Value = question;
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = Guid.NewGuid().ToString();
                command.Parameters.Add("@questNumber", SqlDbType.NVarChar).Value = questNumber;
                command.ExecuteNonQuery();

                Debug.Log("Success add question to database");
            }

            dbConnection.Close();
        }
    }
}
