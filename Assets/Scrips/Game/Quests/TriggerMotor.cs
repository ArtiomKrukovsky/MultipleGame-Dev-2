using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMotor : MonoBehaviour
{
    public GameObject questionPanel;
    public GameObject masAnswers;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Question is visible");
            questionPanel.SetActive(true);
            if (gameObject.name == "Trigger1")
            {
                var answers = masAnswers.transform.Find("Question1");
                foreach (Transform answer in answers)
                {
                    answer.GetComponent<BoxCollider>().enabled = true;
                }
            }
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
