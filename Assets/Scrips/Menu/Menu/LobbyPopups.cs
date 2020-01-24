using UnityEngine;
using UnityEngine.UI;

public class LobbyPopups : MonoBehaviour
{
    public GameObject createLobbyPopup;

    private Text foundErrorTextComponent;

    public void ViewCreateLobbyPopup()
    {
        if (createLobbyPopup == null)
        {
            return;
        }

        if (createLobbyPopup.activeSelf == false)
        {
            createLobbyPopup.SetActive(true);
        }
        else
        {
            createLobbyPopup.SetActive(false);
        }
    }

    public void CancelCreateLobbyPopup()
    {
        try
        {
            if (createLobbyPopup != null)
            {
                foundErrorTextComponent = foundErrorTextComponent ?? this.FindObjectByTag("Error message").GetComponent<Text>();
                this.HideMessageError(foundErrorTextComponent);
                createLobbyPopup.SetActive(false);
            }
        }
        catch
        {
            foundErrorTextComponent = foundErrorTextComponent ?? this.FindObjectByTag("Error message").GetComponent<Text>();
            this.ShowMessageError("Oooppss, something went wrong, try later :(", foundErrorTextComponent);
            Debug.Log("Error, something went wrong");
            return;
        }
    }

    private void ShowMessageError(string message, Text textObject)
    {
        textObject.text = message;
    }

    private GameObject FindObjectByTag(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag);
    }

    private void HideMessageError(Text textObject)
    {
        textObject.text = "";
    }
}
