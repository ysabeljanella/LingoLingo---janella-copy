using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "ScriptableObjects/Question", order = 1)]
public class QuestionScriptableObject : ScriptableObject
{
    public GameObject answerButtonPrefab;
    public string question;

    public string[] answers;
}
