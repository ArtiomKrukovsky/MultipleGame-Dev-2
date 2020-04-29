using System;
using System.Linq;
using UnityEngine;

public class RaycastScript : MonoBehaviour
{
    public float rayDistance = 4f;

    internal static int _countOfAnswers = 0;

    internal static int _score = 0;

    void Update()
    {
        try
        {
            if ((IsEndGame() ?? false) && QuestParametersController._listQuestionActivate.Count == _countOfAnswers)
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
                        IncreaseTeamScore();
                        IncreaseTotalScoreAndUpdateQuestText();
                    }
                    else if (hit.transform.tag == "IncorrectAnswer")
                    {
                        Debug.Log("Incorrect answer");
                        this.DisableQuestObjects(hit);
                        IncreaseTotalScoreAndUpdateQuestText();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error on raycast script with exception: {ex.Message}");
        }
        
    }

    public static void IncreaseTotalScoreAndUpdateQuestText()
    {
        _countOfAnswers++;
        ScoreController.UpdateQuestScore();
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
        _score++;
    }

    private static bool? IsEndGame()
    {
        return QuestParametersController._listQuestionActivate?.All(x=> x == true);
    }
}
