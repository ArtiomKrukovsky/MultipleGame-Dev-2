using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateLobby : MonoBehaviour
{
    public InputField serverName;

    private Text foundErrorTextComponent;

    string _connectionString = @"Data Source = SQL5041.site4now.net; 
        User Id = DB_A50AD1_broadwood_admin;
        Password = qwe123ZXC.;
        Initial Catalog = DB_A50AD1_broadwood;";

    public void CreateServer()
    {
        try
        {
            foundErrorTextComponent = foundErrorTextComponent ?? this.FindObjectByTag("Error message").GetComponent<Text>();

            this.HideMessageError(foundErrorTextComponent);

            if (!serverName.text.Any())
            {
                this.ShowMessageError("Please, enter name of server", foundErrorTextComponent);
                return;
            }

            GameObject network = this.FindObjectByTag("Network");
            var manager = network?.GetComponent<NetworkManager>();

            if (manager == null)
            {
                Debug.Log($"Manager is null");
                return;
            }

            manager.StopHost();

            if (manager.matchMaker == null)
            {
                manager.StartMatchMaker();
            }

            var mapName = PlayerPrefs.GetString("MapName");

            if (string.IsNullOrEmpty(mapName))
            {
                throw new Exception("Map in null");
            }

            using (SqlConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                string query = "SELECT * FROM Servers WHERE ServerName = @serverName;";
                using (SqlCommand command = new SqlCommand(query, dbConnection))
                {
                    command.Parameters.Add("@serverName", SqlDbType.NVarChar).Value = serverName.text;

                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            this.ShowMessageError("Server with this name is already exist", foundErrorTextComponent);
                            return;
                        }
                    }
                }

                string insertQuery = "Insert into Servers (Id, ServerName, SceneName)"
                               + " values (@id, @serverName, @sceneName) ";

                using (SqlCommand insertCommand = new SqlCommand(insertQuery, dbConnection))
                {
                    insertCommand.Parameters.Add("@id", SqlDbType.NVarChar).Value = Guid.NewGuid().ToString();
                    insertCommand.Parameters.Add("@serverName", SqlDbType.NVarChar).Value = serverName.text;
                    insertCommand.Parameters.Add("@sceneName", SqlDbType.NVarChar).Value = mapName;

                    insertCommand.ExecuteNonQuery();
                    Debug.Log("Success adding to database");

                    SceneManager.LoadScene(mapName);
                    manager.matchMaker.CreateMatch(serverName.text, manager.matchSize, true, "", "", "", 0, 0, manager.OnMatchCreate);
                }

                dbConnection.Close();
            }
        }
        catch(Exception e)
        {
            foundErrorTextComponent = foundErrorTextComponent ?? this.FindObjectByTag("Error message").GetComponent<Text>();
            ShowMessageError("Oooppss, something went wrong, try later :(", foundErrorTextComponent);
            Debug.Log($"Error, something went wrong: { e.Message }");
            SceneManager.LoadScene("Menu");
        }
    }

    private GameObject FindObjectByTag(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag);
    }

    private void ShowMessageError(string message, Text textObject)
    {
        textObject.text = message;
    }

    private void HideMessageError(Text textObject)
    {
        textObject.text = "";
    }
}
