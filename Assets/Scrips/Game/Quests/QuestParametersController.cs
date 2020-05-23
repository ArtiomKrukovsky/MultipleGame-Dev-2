using System.Collections.Generic;
using UnityEngine;

public class QuestParametersController : MonoBehaviour
{
    [SerializeField]
    public int _listQuestionCount = 20;

    public static int numberQuestions;

    internal static List<bool> _listQuestionActivate = new List<bool>();

    private void Start()
    {
        InitializeList();
        ScoreController.UpdateQuestScore();
    }

    internal static void CmdUnableQuestion(int number)
    {
        _listQuestionActivate[number] = true;
    }

    internal void InitializeList()
    {
        numberQuestions = _listQuestionCount;
        for (int i = 0; i < _listQuestionCount; i++)
        {
            _listQuestionActivate.Add(false);
        }
    }
}
