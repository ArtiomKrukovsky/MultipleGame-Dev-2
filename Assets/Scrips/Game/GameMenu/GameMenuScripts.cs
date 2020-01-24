using System;
using UnityEngine;

public class GameMenuScripts : MonoBehaviour
{
    private GameObject gameMenuPopup;

    void Update()
    {
        try
        {
            if (Input.GetKeyUp("escape"))
            {
                gameMenuPopup = gameMenuPopup ?? this.FindObjectByTag("GameMenu").transform.GetChild(0).gameObject;

                if (gameMenuPopup == null)
                {
                    return;
                }

                if (gameMenuPopup.activeSelf == false)
                {
                    gameObject.GetComponent<PlayerMotor>().enabled = false;
                    gameMenuPopup.SetActive(true);
                }
                else
                {
                    gameObject.GetComponent<PlayerMotor>().enabled = true;
                    gameMenuPopup.SetActive(false);
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log($"Error, something went wrong :{e}");
            return;
        }
    }

    private GameObject FindObjectByTag(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag);
    }
}
