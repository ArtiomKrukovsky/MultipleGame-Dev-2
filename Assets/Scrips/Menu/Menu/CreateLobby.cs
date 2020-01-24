using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateLobby : MonoBehaviour
{
    public InputField serverName;

    private Text foundErrorTextComponent;

    public void CreateServer()
    {
        try
        {
            foundErrorTextComponent = foundErrorTextComponent ?? this.FindObjectByTag("Error message").GetComponent<Text>();

            this.HideMessageError(foundErrorTextComponent);

            if (!serverName.text.Any())
            {
                this.ShowMessageError("Please, enter name of server", foundErrorTextComponent);
                return;
            }

            GameObject network = this.FindObjectByTag("Network");
            var manager = network?.GetComponent<NetworkManager>();

            if (manager == null)
            {
                Debug.Log($"Manager is null");
                return;
            }

            if (manager.matchMaker == null)
            {
                manager.StartMatchMaker();
            }

            SceneManager.LoadScene("GameScene");
            manager.matchMaker.CreateMatch(serverName.text, manager.matchSize, true, "", "", "", 0, 0, manager.OnMatchCreate);
        }
        catch(Exception e)
        {
            foundErrorTextComponent = foundErrorTextComponent ?? this.FindObjectByTag("Error message").GetComponent<Text>();
            ShowMessageError("Oooppss, something went wrong, try later :(", foundErrorTextComponent);
            Debug.Log($"Error, something went wrong: { e.Message }");
            SceneManager.LoadScene("Menu");
        }
    }

    private GameObject FindObjectByTag(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag);
    }

    private void ShowMessageError(string message, Text textObject)
    {
        textObject.text = message;
    }

    private void HideMessageError(Text textObject)
    {
        textObject.text = "";
    }
}
