using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerMotor : MonoBehaviour
{
    public GameObject questionPanel;
    public GameObject masAnswers;
    private void OnTriggerEnter(Collider other)
    {
        //можно вести переменную(кол-во игроков) чтобы квест был виден только 1 игроку
        if (other.tag == "Player")
        {
            Debug.Log("Question is visible");
            questionPanel.SetActive(true);
            if (gameObject.name == "Trigger1")
            {
                var question = GameObject.FindGameObjectWithTag("QuestionText");
                question.GetComponent<Text>().text = "question 1 from db";

                var answers = masAnswers.transform.Find("Question1");
                foreach (Transform answer in answers)
                {
                    answer.GetComponent<BoxCollider>().enabled = true;
                }
            }
            //gameObject.GetComponent<BoxCollider>().enabled = false;
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
