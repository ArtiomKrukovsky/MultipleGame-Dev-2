using System;
using System.Linq;
using Boo.Lang;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class JoinLobby : MonoBehaviour
{
    private GameObject network;
    private static NetworkManager manager;

    public void Start()
    {
        try
        {
            network = this.FindObjectByTag("Network");
            manager = network?.GetComponent<NetworkManager>();
            RefreshMatchies();
        }
        catch (Exception e)
        {
            Debug.Log($"Error, something went wrong: { e.Message }");
        }
    }


    public void JoinMatch(string matchName)
    {
        try
        {
            if (string.IsNullOrEmpty(matchName))
            {
                return;
            }

            foreach (var match in manager.matches)
            {
                if (match.name == matchName)
                {
                    SceneManager.LoadScene("GameScene");
                    manager.matchName = match.name;
                    manager.matchSize = (uint)match.currentSize;
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

    public static void RefreshMatchies()
    {
        if (manager == null)
        {
            Debug.Log($"Manager is null");
            return;
        }

        if (manager.matchMaker != null)
        {
            manager.StopMatchMaker();
        }

        manager.StartMatchMaker();

        if (manager.matches == null)
        {
            manager.matchMaker.ListMatches(0, 20, "", true, 0, 0, manager.OnMatchList);
        }
    }

    private GameObject FindObjectByTag(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag);
    }
}
