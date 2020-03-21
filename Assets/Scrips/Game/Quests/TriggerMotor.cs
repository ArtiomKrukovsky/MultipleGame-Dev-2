using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TriggerMotor : NetworkBehaviour
{
    public GameObject questionPanel;
    public GameObject masAnswers;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player1")
        {
            return;
        } 
        int number = Convert.ToInt32(gameObject.name.Substring(7));
        if (number == 0 || QuestParametersController._listQuestionActivate[number - 1])
        {
            return;
        }
        other.GetComponent<RaycastScript>().enabled = true;
        QuestParametersController.CmdUnableQuestion(number - 1);
        questionPanel.SetActive(true);
        var question = GameObject.FindGameObjectWithTag("QuestionText");
        question.GetComponent<Text>().text = DbHelper.GetQuestionFromDB(SceneManager.GetActiveScene().name, number.ToString());

        var answers = masAnswers.transform.Find("Question" + number);
        foreach (Transform answer in answers)
        {
            answer.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player1")
        {
            return;
        }
        RaycastScript._countOfAnswers++;
        questionPanel.SetActive(false);
        other.GetComponent<RaycastScript>().enabled = false;
        int number = Convert.ToInt32(gameObject.name.Substring(7));
        var answers = masAnswers.transform.Find("Question" + number);
        foreach (Transform answer in answers)
        {
            answer.gameObject.SetActive(false);
        }

        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}
