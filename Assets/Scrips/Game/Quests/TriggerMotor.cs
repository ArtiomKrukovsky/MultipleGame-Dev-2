using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerMotor : MonoBehaviour
{
    public GameObject questionPanel;
    public GameObject masAnswers;

    [SerializeField]
    private List<bool> _questionActivate = new List<bool>();

    private void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            _questionActivate.Add(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Question is visible");
            questionPanel.SetActive(true);
            for (int i = 0; i < _questionActivate.Count; i++)
            {
                if (gameObject.name == "Trigger" + (i + 1) && !_questionActivate[i + 1])
                {
                    _questionActivate[0] = true;
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
            Debug.Log("Question is invisible");
            questionPanel.SetActive(false);
            if (gameObject.name == "Trigger1")
            {
                var answers = masAnswers.transform.Find("Question1");
                foreach (Transform answer in answers)
                {
                    answer.gameObject.SetActive(false);
                }
            }
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
