using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        SceneManager.LoadScene("Menu");

        singleton.StopHost();
    }

    public override void OnStopServer()
    {
        var serverToDisconnect = PlayerPrefs.GetString("ServerToDisconnect");

        if (!string.IsNullOrEmpty(serverToDisconnect))
        {
            DbHelper.DeleteServerFromDB(serverToDisconnect);
        }

        base.OnStopServer();
    }

    public override void OnStopClient()
    {
        PlayerPrefs.DeleteKey("ServerToDisconnect");
        base.OnStopClient();
    }
}
