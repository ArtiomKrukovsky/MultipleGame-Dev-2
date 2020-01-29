using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class RedirectMenuScripts : MonoBehaviour
{
    string _connectionString = @"Data Source = SQL5041.site4now.net; 
        User Id = DB_A50AD1_broadwood_admin;
        Password = qwe123ZXC.;
        Initial Catalog = DB_A50AD1_broadwood;";

    private void Start()
    {
        var networkManager = NetworkManager.singleton;

        networkManager.StartMatchMaker();
        networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, networkManager.OnMatchList);
    }

    public void OpenSettings()
    {
    }

    public void RedirectToLobby()
    {
        this.LeaveServer();
    }

    public void QuitGame()
    {
        this.LeaveServer();
        Application.Quit();
    }

    private async void LeaveServer()
    {
        var networkManager = NetworkManager.singleton;

        try
        { 
            MatchInfo matchInfo = networkManager.matchInfo;
            networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
            SceneManager.LoadScene("Menu");

            var serverNameToDelete = "";

            if (networkManager.matches == null)
            {
                Debug.Log("Matches is null");
                return;
            }

            foreach (var match in networkManager.matches)
            {
                if (match.networkId == matchInfo.networkId && match.currentSize <= 1)
                {
                    serverNameToDelete = match.name;
                }
            }

            if (!string.IsNullOrEmpty(serverNameToDelete))
            {
                using (SqlConnection dbConnection = new SqlConnection(_connectionString))
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

            networkManager.StopHost();
        }
        catch (System.Exception)
        {
            networkManager.StopHost();
            SceneManager.LoadScene("Menu");
        }
    }

    private GameObject FindObjectByTag(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag);
    }
}
