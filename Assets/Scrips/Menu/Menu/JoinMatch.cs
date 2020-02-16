using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinMatch : MonoBehaviour, IPointerClickHandler
{
    private GameObject network;
    private static NetworkManager manager;

    private const float DoubleClickTime = .2f;
    private float lastClickTime;

    void Start()
    {
        try
        {
            network = BaseHelper.FindObjectByTag(BaseConstants.Network);
            manager = network?.GetComponent<NetworkManager>();
        }
        catch (Exception e)
        {
            Debug.LogError($"{BaseConstants.Messages.SomethingWentWrongMessage} { e.Message }");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var matchName = this.GetComponentInChildren<Text>().text;
        JoinToMatch(matchName);
        Debug.Log("User join to match!");
    }

    private void JoinToMatch(string matchName)
    {
        try
        {
            if (string.IsNullOrEmpty(matchName))
            {
                return;
            }

            string mapName = string.Empty;

            using (SqlConnection dbConnection = new SqlConnection(DbHelper.ConnectionString))
            {
                dbConnection.Open();

                string query = "SELECT SceneName FROM Servers WHERE ServerName = @serverName;";
                using (SqlCommand command = new SqlCommand(query, dbConnection))
                {
                    command.Parameters.Add("@serverName", SqlDbType.NVarChar).Value = matchName;

                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Debug.LogWarning("Server with this name not found!");
                            dbConnection.Close();
                            return;
                        }

                        while (reader.Read())
                        {
                            mapName = reader.GetString(0);
                        }
                    }
                }

                dbConnection.Close();
            }

            if (string.IsNullOrEmpty(mapName))
            {
                Debug.LogWarning("Map is Empty!");
                return;
            }

            foreach (var match in manager.matches)
            {
                if (match.name == matchName)
                {
                    SceneManager.LoadScene(mapName);
                    manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, manager.OnMatchJoined);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"{BaseConstants.Messages.SomethingWentWrongMessage} { ex.Message }");
            SceneManager.LoadScene("Menu");
        }
    }
}
