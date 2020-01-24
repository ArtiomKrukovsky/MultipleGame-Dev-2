using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class RedirectMenuScripts : MonoBehaviour
{
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

    private void LeaveServer()
    {
        var networkManager = NetworkManager.singleton;

        try
        { 
            MatchInfo matchInfo = networkManager.matchInfo;
            networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
            networkManager.StopHost();
            SceneManager.LoadScene("Menu");
        }
        catch (System.Exception)
        {
            networkManager.StopHost();
            SceneManager.LoadScene("Menu");
        }
    }
}
