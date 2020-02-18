using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TriggerMotor : MonoBehaviour
{
    public GameObject questionPanel;
    public GameObject masAnswers;

    //make field sync
    private List<bool> _questionActivate = new List<bool>();

    private void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            _questionActivate.Add(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            questionPanel.SetActive(true);
            for (int i = 0; i < _questionActivate.Count; i++)
            {
                if (gameObject.name == "Trigger" + (i + 1) && !_questionActivate[i])
                {
                    _questionActivate[i] = true;
                    var question = GameObject.FindGameObjectWithTag("QuestionText");
                    question.GetComponent<Text>().text = "question 1 from db";

                    var answers = masAnswers.transform.Find("Question" + (i + 1));
                    foreach (Transform answer in answers)
                    {
                        answer.GetComponent<BoxCollider>().enabled = true;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            questionPanel.SetActive(false);
            for (int i = 0; i < _questionActivate.Count; i++)
            {
                if (gameObject.name == "Trigger" + (i + 1))
                {
                    var answers = masAnswers.transform.Find("Question" + (i + 1));
                    foreach (Transform answer in answers)
                    {
                        answer.gameObject.SetActive(false);
                    }
                }
            }
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
