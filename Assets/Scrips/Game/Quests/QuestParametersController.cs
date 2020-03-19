using System.Collections.Generic;
using UnityEngine;

public class QuestParametersController : MonoBehaviour
{
    internal static List<bool> _listQuestionActivate = new List<bool>();

    private void Start()
    {
        InitializeList();
    }

    internal static void CmdUnableQuestion(int number)
    {
        _listQuestionActivate[number] = true;
    }

    internal static void InitializeList()
    {
        for (int i = 0; i < 3; i++)
        {
            _listQuestionActivate.Add(false);
        }
    }
}
