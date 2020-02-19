using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastScript : MonoBehaviour
{
    public float rayDistance = 3f;
    void Update()
    {
        var cameras = Camera.allCameras;
        var ray = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(transform.position, ray.direction * rayDistance);
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "CorrectAnswer")
                {
                    Debug.Log("Correct answer");
                    GameObject.Find("QuestionPanel").SetActive(false);
                    //GameObject.Find("Question1").SetActive(false);
                    hit.transform.parent.gameObject.SetActive(false);
                }
                else if (hit.transform.tag == "IncorrectAnswer")
                {
                    Debug.Log("Incorrect answer");
                    GameObject.Find("QuestionPanel").SetActive(false);
                    //GameObject.Find("Question1").SetActive(false);
                    hit.transform.parent.gameObject.SetActive(false);
                }

                GameObject.Find("Trigger1").GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
