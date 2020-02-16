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

    public void CreateServer()
    {
        try
        {
            foundErrorTextComponent = foundErrorTextComponent ?? BaseHelper.FindObjectByTag(BaseConstants.Messages.ErrorMessage).GetComponent<Text>();

            this.HideMessageError(foundErrorTextComponent);

            if (!serverName.text.Any())
            {
                BaseHelper.ShowMessageError("Please, enter name of server", foundErrorTextComponent);
                return;
            }

            GameObject network = BaseHelper.FindObjectByTag(BaseConstants.Network);
            var manager = network?.GetComponent<NetworkManager>();

            if (manager == null)
            {
                Debug.Log(BaseConstants.Messages.ManagerNullMessage);
                return;
            }

            manager.StopHost();

            if (manager.matchMaker == null)
            {
                manager.StartMatchMaker();
            }

            var mapName = PlayerPrefs.GetString(BaseConstants.Prefs.MapName);

            if (string.IsNullOrEmpty(mapName))
            {
                throw new Exception("Map in null");
            }

            using (SqlConnection dbConnection = new SqlConnection(DbHelper.ConnectionString))
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
                            BaseHelper.ShowMessageError("Server with this name is already exist", foundErrorTextComponent);
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

                    SceneManager.LoadScene(mapName);
                    manager.matchMaker.CreateMatch(serverName.text, manager.matchSize, true, "", "", "", 0, 0, manager.OnMatchCreate);
                }

                dbConnection.Close();
            }
        }
        catch(Exception e)
        {
            foundErrorTextComponent = foundErrorTextComponent ?? BaseHelper.FindObjectByTag(BaseConstants.Messages.ErrorMessage).GetComponent<Text>();
            BaseHelper.ShowMessageError($"{BaseConstants.Messages.SomethingWentWrongMessage}, try later :(", foundErrorTextComponent);
            Debug.Log($"{BaseConstants.Messages.SomethingWentWrongMessage} { e.Message }");
            SceneManager.LoadScene("Menu");
        }
    }

    private void HideMessageError(Text textObject)
    {
        textObject.text = "";
    }
}
