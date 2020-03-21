using System;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    void Start()
    {
        this.ProccesLobbyMessage();
    }

    private void ProccesLobbyMessage()
    {
        try
        {
            var isEndOfTheGame = PlayerPrefs.GetString("IsEndOfTheGame");
            var score = PlayerPrefs.GetInt("PlayerScore");

            if (BaseHelper.ConvertStringToBool(isEndOfTheGame))
            {
                BaseHelper.ShowLobbyMessage("LobbyMessage", score);
                ResetParametersForLobbyMessage();
            }
        }
        catch (Exception)
        {
            Debug.Log("Can't isEndOfTheGame convert to bool");
            ResetParametersForLobbyMessage();
        }
    }

    private void ResetParametersForLobbyMessage()
    {
        PlayerPrefs.DeleteKey("IsEndOfTheGame");
        PlayerPrefs.DeleteKey("PlayerScore");
    }
}
