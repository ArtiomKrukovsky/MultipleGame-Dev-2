using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    public void AppExit()
    {
        Application.Quit();
    }

    public void CloseMessageLobbyPopup()
    {
        var message = BaseHelper.FindObjectByTag("LobbyMessage").transform.Find("Messages").gameObject;

        if (message == null)
        {
            return;
        }

        message.SetActive(false);

        var rezultLine = message.transform?.Find("GameMessage")?.Find("Rezult")?.GetComponent<Text>();

        if (rezultLine == null)
        {
            return;
        }

        rezultLine.text = $"Ваш результат равен 0";
    }
}
