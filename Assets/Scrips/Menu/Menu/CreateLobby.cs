﻿using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateLobby : MonoBehaviour
{
    public InputField serverName;

    private Text foundErrorTextComponent;

    public async void CreateServer()
    {
        try
        {
            foundErrorTextComponent = foundErrorTextComponent ?? BaseHelper.FindObjectByTag(BaseConstants.Messages.ErrorMessage).GetComponent<Text>();

            this.HideMessageError(foundErrorTextComponent);

            if (!serverName.text.Any())
            {
                BaseHelper.ShowMessageError("Пожалуйтса введите имя сервера", foundErrorTextComponent);
                SceneLoading.SceneLoadingLogo();
                return;
            }

            GameObject network = BaseHelper.FindObjectByTag(BaseConstants.Network);
            var manager = network?.GetComponent<NetworkManager>();

            if (manager == null)
            {
                Debug.Log(BaseConstants.Messages.ManagerNullMessage);
                SceneLoading.SceneLoadingLogo();
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
                DbDataReader reader = null;
                await Task.Factory.StartNew(() =>
                {
                    dbConnection.Open();

                    string query = "SELECT * FROM Servers WHERE ServerName = @serverName;";
                    SqlCommand command = new SqlCommand(query, dbConnection);

                    command.Parameters.Add("@serverName", SqlDbType.NVarChar).Value = serverName.text;
                    reader = command.ExecuteReader();
                });

                using (reader)
                {
                    if (reader.HasRows)
                    {
                        BaseHelper.ShowMessageError("Сервер с таким именем уже существует", foundErrorTextComponent);
                        SceneLoading.SceneLoadingLogo();
                        return;
                    }
                }

                await Task.Factory.StartNew(() =>
                {
                    string insertQuery = "Insert into Servers (Id, ServerName, SceneName)"
                               + " values (@id, @serverName, @sceneName) ";

                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, dbConnection))
                    {
                        insertCommand.Parameters.Add("@id", SqlDbType.NVarChar).Value = Guid.NewGuid().ToString();
                        insertCommand.Parameters.Add("@serverName", SqlDbType.NVarChar).Value = serverName.text;
                        insertCommand.Parameters.Add("@sceneName", SqlDbType.NVarChar).Value = mapName;

                        insertCommand.ExecuteNonQuery();
                    }
                });

                SceneManager.LoadScene(mapName);
                manager.matchMaker.CreateMatch(serverName.text, manager.matchSize, true, "", "", "", 0, 0, manager.OnMatchCreate);

                dbConnection.Close();
            }
        }
        catch(Exception e)
        {
            foundErrorTextComponent = foundErrorTextComponent ?? BaseHelper.FindObjectByTag(BaseConstants.Messages.ErrorMessage).GetComponent<Text>();
            BaseHelper.ShowMessageError($"{BaseConstants.Messages.SomethingWentWrongMessage}, попробуйте позже :(", foundErrorTextComponent);
            Debug.Log($"{BaseConstants.Messages.SomethingWentWrongMessage} { e.Message }");
            SceneLoading.SceneLoadingLogo();
            SceneManager.LoadScene("Menu");
        }
    }

    private void HideMessageError(Text textObject)
    {
        textObject.text = "";
    }
}
