using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
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
       string playerName = this.GetPlayerName();
        if (!string.IsNullOrEmpty(playerName))
        {
            this.GetComponentInChildren<TextMesh>().text =  playerName;
            this.ChangePlayerObjectName(playerName);
        }
    }

    private void ChangePlayerObjectName(string name)
    {
        this.gameObject.name = name;
    }
}
