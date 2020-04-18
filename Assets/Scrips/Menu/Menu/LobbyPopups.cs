using UnityEngine;
using UnityEngine.UI;

public class LobbyPopups : MonoBehaviour
{
    public static GameObject createLobbyPopup;

    private Text foundErrorTextComponent;

    public static void ViewCreateLobbyPopup(GameObject _createLobbyPopup)
    {
        createLobbyPopup = _createLobbyPopup;

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
            PlayerPrefs.DeleteKey(BaseConstants.Prefs.MapName);
            createLobbyPopup.SetActive(false);
        }
    }

    public void CancelCreateLobbyPopup()
    {
        try
        {
            if (createLobbyPopup != null)
            {
                foundErrorTextComponent = foundErrorTextComponent ?? BaseHelper.FindObjectByTag(BaseConstants.Messages.ErrorMessage).GetComponent<Text>();
                this.HideMessageError(foundErrorTextComponent);
                createLobbyPopup.SetActive(false);
                PlayerPrefs.DeleteKey(BaseConstants.Prefs.MapName);
            }
        }
        catch
        {
            foundErrorTextComponent = foundErrorTextComponent ?? BaseHelper.FindObjectByTag(BaseConstants.Messages.ErrorMessage).GetComponent<Text>();
            BaseHelper.ShowMessageError($"{BaseConstants.Messages.SomethingWentWrongMessage}, попробуйте позже :(", foundErrorTextComponent);
            Debug.Log(BaseConstants.Messages.SomethingWentWrongMessage);
            return;
        }
    }

    private void HideMessageError(Text textObject)
    {
        textObject.text = "";
    }
}
