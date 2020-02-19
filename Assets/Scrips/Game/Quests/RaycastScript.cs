using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastScript : MonoBehaviour
{
    public float rayDistance = 3f;
    void Update()
    {
        try
        {
            var cameras = Camera.allCameras;
            var ray = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(transform.position, ray.direction * rayDistance);
            RaycastHit hit;

            if (!Input.GetKeyDown(KeyCode.F))
            {
                return;
            }

            if (!Physics.Raycast(ray, out hit))
            {
                return;
            }

            if (hit.transform.tag == "CorrectAnswer")
            {
                Debug.Log("Correct answer");
                GameObject.Find("QuestionPanel").SetActive(false);
                hit.transform.parent.gameObject.SetActive(false);
            }
            else if (hit.transform.tag == "IncorrectAnswer")
            {
                Debug.Log("Incorrect answer");
                GameObject.Find("QuestionPanel").SetActive(false);
                hit.transform.parent.gameObject.SetActive(false);
            }

            var number = Convert.ToInt32(hit.transform.parent.name.Substring(8));
            GameObject.Find("Trigger" + number).GetComponent<BoxCollider>().enabled = false;
        }
        catch (Exception ex)
        {
            Debug.Log($"Error on raycast script with exception: {ex.Message}");
        }
        
    }
}
