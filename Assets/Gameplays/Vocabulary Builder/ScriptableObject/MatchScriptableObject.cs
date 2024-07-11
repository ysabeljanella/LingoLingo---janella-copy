using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MatchWordItems", menuName = "ScriptableObjects/MatchItems", order = 1)]
public class MatchScriptableObject : ScriptableObject
{
  
    public GameObject leftGameObject;
    public GameObject rightGameObject;

    public List<string> Lefts;
    public List<string> Rights;
    public Sprite[] ImageRight;
    public List<AudioClip> audioClips;
    
}
