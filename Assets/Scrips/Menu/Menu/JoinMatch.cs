using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinMatch : MonoBehaviour
{
    public Text matchName;

    private GameObject network;
    private static NetworkManager manager;

    private const float DoubleClickTime = .2f;
    private float lastClickTime;

    void Start()
    {
        try
        {
            network = this.FindObjectByTag("Network");
            manager = network?.GetComponent<NetworkManager>();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error, something went wrong: { e.Message }");
        }
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            try
            {
                float timeSinceLastClick = Time.time - lastClickTime;

                if (timeSinceLastClick <= DoubleClickTime)
                {
                    JoinToMatch(matchName.text);
                    Debug.Log("User join to match!");
                }

                lastClickTime = Time.time;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error, something went wrong: { ex.Message }");
            }
        }
    }

    private void JoinToMatch(string matchName)
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

    private GameObject FindObjectByTag(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag);
    }
}
