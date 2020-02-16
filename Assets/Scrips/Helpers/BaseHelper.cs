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
}
