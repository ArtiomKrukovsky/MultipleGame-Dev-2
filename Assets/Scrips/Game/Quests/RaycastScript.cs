using System;
using UnityEngine;
using UnityEngine.UI;

public class RaycastScript : MonoBehaviour
{
    public float rayDistance = 3f;

    void Update()
    {
        try
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                var cursor = GameObject.Find("Cursor");
                var ray = Camera.allCameras[0].ScreenPointToRay(cursor.transform.position);
                Debug.DrawRay(transform.position, ray.direction * rayDistance);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "CorrectAnswer")
                    {
                        Debug.Log("Correct answer");
                        this.DisableQuestObjects(hit);
                    }
                    else if (hit.transform.tag == "IncorrectAnswer")
                    {
                        Debug.Log("Incorrect answer");
                        this.DisableQuestObjects(hit);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error on raycast script with exception: {ex.Message}");
        }
        
    }

    private void DisableQuestObjects(RaycastHit hit)
    {
        GameObject.Find("QuestionPanel").SetActive(false);
        hit.transform.parent.gameObject.SetActive(false);

        var number = Convert.ToInt32(hit.transform.parent.name.Substring(8));
        GameObject.Find("Trigger" + number).GetComponent<BoxCollider>().enabled = false;
    }
}
