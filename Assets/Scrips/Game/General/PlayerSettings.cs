using UnityEngine;
using UnityEngine.Networking;

public class PlayerSettings : NetworkBehaviour
{
    [SyncVar]
    public string playerName = "player";

    private void Start()
    {
        try
        {
            SetPlayerName();
        }
        catch (System.Exception)
        {
            Debug.Log("Error, something went wrong");
        }
    }

    private string GetPlayerName()
    {
        return PlayerPrefs.GetString("PlayerName");
    }

    private void SetPlayerName()
    {
        if (isLocalPlayer)
        {
            string playerName = this.GetPlayerName();
            if (!string.IsNullOrEmpty(playerName))
            {
                CmdSyncNameOnServer(playerName);
            }
        }
    }

    [Command]
    public void CmdSyncNameOnServer(string name)
    {
        playerName = name;
    }

    private void Update()
    {
        this.GetComponentInChildren<TextMesh>().text = this.playerName;
    }
}
