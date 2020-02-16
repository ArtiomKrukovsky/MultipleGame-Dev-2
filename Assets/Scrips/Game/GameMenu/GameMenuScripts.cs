using System;
using UnityEngine;

public class GameMenuScripts : MonoBehaviour
{
    private GameObject gameMenuPopup;

    void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        try
        {
            if (Input.GetKeyUp("escape"))
            {
                gameMenuPopup = gameMenuPopup ?? BaseHelper.FindObjectByTag("GameMenu").transform.GetChild(0).gameObject;

                if (gameMenuPopup == null)
                {
                    return;
                }

                if (gameMenuPopup.activeSelf == false)
                {
                    Cursor.visible = true;
                    gameObject.GetComponent<PlayerMotor>().enabled = false;
                    gameMenuPopup.SetActive(true);
                }
                else
                {
                    Cursor.visible = false;
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
}
