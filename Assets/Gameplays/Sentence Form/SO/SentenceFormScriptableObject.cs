using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sentence", menuName = "ScriptableObjects/Sentence", order = 1)]
public class SentenceFormScriptableObject : ScriptableObject
{
    public GameObject Word;
    public GameObject SentenceLine;
    public string sentenceEnglish;
    public string[] words;
    public AudioClip[] audio;
}
