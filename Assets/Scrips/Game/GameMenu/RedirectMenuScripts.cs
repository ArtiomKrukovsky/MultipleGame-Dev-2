using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class RedirectMenuScripts : MonoBehaviour
{
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

            if (networkManager.matches == null)
            {
                Debug.Log("Matches is null");
                return;
            }

            var serverNameToDelete = "";

            foreach (var match in networkManager.matches)
            {
                if (match.networkId == matchInfo.networkId)
                {
                    serverNameToDelete = match.name;
                }
            }

            if (!string.IsNullOrEmpty(serverNameToDelete))
            {
               PlayerPrefs.SetString("ServerToDisconnect", serverNameToDelete);
            }

            networkManager.StopHost();
        }
        catch (System.Exception)
        {
            networkManager.StopHost();
            SceneManager.LoadScene("Menu");
        }
    }
}
