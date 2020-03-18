using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TriggerMotor : NetworkBehaviour
{
    public GameObject questionPanel;
    public GameObject masAnswers;

    internal static List<bool> _listQuestionActivate = new List<bool>();

    public override void OnStartServer()
    {
        InitializeList();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player1")
        {
            return;
        } 
        int number = Convert.ToInt32(gameObject.name.Substring(7));
        if (number == 0 || _listQuestionActivate[number - 1])
        {
            return;
        }
        other.GetComponent<RaycastScript>().enabled = true;
        CmdUnableQuestion(number - 1);
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

    private void CmdUnableQuestion(int number)
    {
        _listQuestionActivate[number] = true;
    }

    private void InitializeList()
    {
        for (int i = 0; i < 1; i++)
        {
            _listQuestionActivate.Add(false);
        }
    }
}
