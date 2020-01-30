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

    private string _connectionString = @"Data Source = SQL5041.site4now.net; 
        User Id = DB_A50AD1_broadwood_admin;
        Password = qwe123ZXC.;
        Initial Catalog = DB_A50AD1_broadwood;";

    void Start()
    {
        try
        {
            network = this.FindObjectByTag("Network");
            manager = network?.GetComponent<NetworkManager>();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error, something went wrong: { e.Message }");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var textObjects = gameObject.GetComponentsInChildren<Transform>();
        foreach (var obj in textObjects)
        {
            if (obj.name == "MatchName")
            {
                var matchName = obj.GetComponent<Text>().text;
                JoinToMatch(matchName);
                Debug.Log("User join to match!");
            }
        }
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

            using (SqlConnection dbConnection = new SqlConnection(_connectionString))
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
            Debug.Log($"Error, something went wrong: { ex.Message }");
            SceneManager.LoadScene("Menu");
        }
    }

    private GameObject FindObjectByTag(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag);
    }
}
