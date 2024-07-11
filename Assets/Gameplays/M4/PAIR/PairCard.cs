using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "PairCard", menuName = "ScriptableObjects/Pair", order = 1)]
public class PairCard : ScriptableObject
{
    public GameObject CardButton;
    public Sprite BackImage;

    public Sprite[] LeftImage;
    public Sprite[] RightImage;


}
