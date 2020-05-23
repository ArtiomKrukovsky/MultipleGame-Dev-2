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

    public void ReturnToGame()
    {
        var gameMenuPopup = BaseHelper.FindObjectByTag("GameMenu").transform.GetChild(0).gameObject;

        Cursor.visible = false;
        var player = BaseHelper.FindObjectByTag("Player1").transform.gameObject;
        player.GetComponent<PlayerMotor>().enabled = true;
        gameMenuPopup.SetActive(false);
    }

    public void RedirectToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void RedirectToSupport()
    {
        SceneManager.LoadScene("Support");
    }

    public void RedirectToLobby()
    {
        LeaveServer();
    }

    public void QuitGame()
    {
        LeaveServer();
        Application.Quit();
    }

    internal static void LeaveServer(bool IsEndOfTheGame = false, int score = 0)
    {
        var networkManager = NetworkManager.singleton;

        try
        { 
            MatchInfo matchInfo = networkManager.matchInfo;
            networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
            Cursor.visible = true;
            SceneManager.LoadScene("Menu");

            if (networkManager.matches == null)
            {
                Debug.Log("Matches is null");
                return;
            }

            var serverName = "";

            foreach (var match in networkManager.matches)
            {
                if (match.networkId == matchInfo.networkId)
                {
                    serverName = match.name;
                }
            }

            if (IsEndOfTheGame)
            {
                DbHelper.SetRatingToBD(SceneManager.GetActiveScene().name, serverName, score);
                DbHelper.DeleteServerFromDB(serverName);
                SetParametersForLobbyMessage(score);
            }

            RaycastScript._countOfAnswers = 0;
            RaycastScript._score = 0;

            QuestParametersController._listQuestionActivate.Clear();

            if (!string.IsNullOrEmpty(serverName))
            {
               PlayerPrefs.SetString("ServerToDisconnect", serverName);
            }

            networkManager.StopHost();
        }
        catch (System.Exception)
        {
            networkManager.StopHost();
            SceneManager.LoadScene("Menu");
        }
    }

    private static void SetParametersForLobbyMessage(int score)
    {
        PlayerPrefs.SetString("IsEndOfTheGame", "true");
        PlayerPrefs.SetInt("PlayerScore", score);
    }
}
