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

    private SyncListBool _questionActivate = new SyncListBool();

    private void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            _questionActivate.Add(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }
        questionPanel.SetActive(true);
        int number = Convert.ToInt32(gameObject.name.Substring(7));
        if (number == 0 || _questionActivate[number - 1])
        {
            return;
        }
        _questionActivate[number - 1] = true;
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
        if (other.tag != "Player")
        {
            return;
        }
        questionPanel.SetActive(false);
        int number = Convert.ToInt32(gameObject.name.Substring(7));
        var answers = masAnswers.transform.Find("Question" + number);
        foreach (Transform answer in answers)
        {
            answer.gameObject.SetActive(false);
        }

        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}
