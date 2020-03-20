using UnityEngine;
using UnityEngine.UI;

public class BaseHelper : MonoBehaviour
{
    public static GameObject FindObjectByTag(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag);
    }

    public static void ShowMessageError(string message, Text textObject)
    {
        textObject.text = message;
    }

    public static void ShowLobbyMessage(string tag, int score)
    {
        var message = FindObjectByTag(tag).transform.Find("Messages").gameObject;

        if (message == null)
        {
            return;
        }

        message.SetActive(true);

        var rezultLine = message.transform?.Find("GameMessage")?.Find("Rezult")?.GetComponent<Text>();

        if (rezultLine == null)
        {
            return;
        }

        rezultLine.text = $"Ваш результат равен {score}";
    }

    public static bool ConvertStringToBool(string line)
    {
        if (string.IsNullOrEmpty(line))
        {
            return false;
        }

        if (string.Equals(line, "true", System.StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }
}
