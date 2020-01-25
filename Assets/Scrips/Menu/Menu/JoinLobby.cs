using System;
using UnityEngine;
using UnityEngine.Networking;

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
