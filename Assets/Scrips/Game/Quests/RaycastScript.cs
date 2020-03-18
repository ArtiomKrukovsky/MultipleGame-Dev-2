using System;
using System.Linq;
using UnityEngine;

public class RaycastScript : MonoBehaviour
{
    public float rayDistance = 3f;

    private int _countOfAnswers = 0;

    private int _score = 0;

    void Update()
    {
        try
        {
            if (IsEndGame() ?? false && TriggerMotor._listQuestionActivate.Count == _countOfAnswers)
            {
                RedirectMenuScripts.LeaveServer(true, _score);
            }

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
                        _countOfAnswers++;
                    }
                    else if (hit.transform.tag == "IncorrectAnswer")
                    {
                        Debug.Log("Incorrect answer");
                        this.DisableQuestObjects(hit);
                        _countOfAnswers++;
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

    private void IncreaseTeamScore()
    {
        this._score++;
    }

    private static bool? IsEndGame()
    {
        return TriggerMotor._listQuestionActivate?.All(x=> x == true);
    }
}
