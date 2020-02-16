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
            network = BaseHelper.FindObjectByTag(BaseConstants.Network);
            manager = network?.GetComponent<NetworkManager>();
            RefreshMatchies();
        }
        catch (Exception e)
        {
            Debug.Log($"{BaseConstants.Messages.SomethingWentWrongMessage} { e.Message }");
        }
    }

    public static void RefreshMatchies()
    {
        if (manager == null)
        {
            Debug.Log(BaseConstants.Messages.ManagerNullMessage);
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
}
