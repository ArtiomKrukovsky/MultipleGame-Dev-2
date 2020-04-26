using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public static void UpdateQuestScore()
    {
        string questScore = $"{RaycastScript._countOfAnswers}/{QuestParametersController.numberQuestions}";
        Text scoreText = BaseHelper.FindObjectByTag(BaseConstants.ScoreText).GetComponent<Text>();

        if (scoreText != null)
        {
            scoreText.text = questScore;
        }
    }
}
