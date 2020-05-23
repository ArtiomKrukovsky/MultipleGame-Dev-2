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
            FinishTheQuest();

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
                        _score++;

                        IncreaseTotalScoreAndUpdateQuestText();
                        RepaintQuestionMarker(GetQuestionNumber(hit));
                    }
                    else if (hit.transform.tag == "IncorrectAnswer")
                    {
                        Debug.Log("Incorrect answer");
                        this.DisableQuestObjects(hit);

                        IncreaseTotalScoreAndUpdateQuestText();
                        RepaintQuestionMarker(GetQuestionNumber(hit));
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

    public static void FinishTheQuest()
    {
        if ((IsEndGame() ?? false) && QuestParametersController._listQuestionActivate.Count == _countOfAnswers)
        {
            RedirectMenuScripts.LeaveServer(true, _score);
        }
    }

    public static void RepaintQuestionMarker(int number)
    {
        string markerName = BaseConstants.Marker + number;
        GameObject marker = GameObject.Find($"Markers/{markerName}");

        if (marker != null)
        {
            marker.GetComponent<Renderer>().material.color = Color.green;
        }
    }

    private void DisableQuestObjects(RaycastHit hit)
    {
        GameObject.Find("QuestionPanel").SetActive(false);
        hit.transform.parent.gameObject.SetActive(false);

        var number = GetQuestionNumber(hit);
        GameObject.Find("Trigger" + number).GetComponent<BoxCollider>().enabled = false;
    }

    private int GetQuestionNumber(RaycastHit hit)
    {
        return Convert.ToInt32(hit.transform.parent.name.Substring(8));
    }

    private static bool? IsEndGame()
    {
        return QuestParametersController._listQuestionActivate?.All(x=> x == true);
    }
}
