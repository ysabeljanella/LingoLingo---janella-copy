using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulesHandler : MonoBehaviour
{
    public static ModulesHandler Instance;
    public GameObject currentGame;
    private void Start()
    {
        Instance = this;
    }

    public GameObject ConsVows;
    public GameObject CalendarDates;
    public GameObject Module3FlashCard;

    [Header("Module 1")]
    public GameObject[] challenges1;
    [Header("Module 2")]
    public GameObject[] challenges2;
    [Header("Module 3")]
    public GameObject[] challenges3;
    [Header("Module 4")]
    public GameObject[] challenges4;
    [Header("Module 5")]
    public GameObject[] challenges5;
    [Header("Module 6")]
    public GameObject[] challenges6;

    public void ConsVowsShow()
    {
        currentGame = Instantiate(ConsVows, this.transform);
    }

    public void ShowCalendar()
    {
        currentGame = Instantiate(CalendarDates, this.transform);
    }

    public void ShowM3FlashCards()
    {
        currentGame = Instantiate(Module3FlashCard, this.transform);
    }

    public void Module1(int index)
    {
        currentGame = Instantiate(challenges1[index], this.transform);
    }

    public void Module2(int index)
    {
        currentGame = Instantiate(challenges2[index], this.transform);
    }

    public void Module3(int index)
    {
        currentGame = Instantiate(challenges3[index], this.transform);
    }

    public void Module4(int index)
    {
        currentGame = Instantiate(challenges4[index], this.transform);
    }

    public void Module5(int index)
    {
        currentGame = Instantiate(challenges5[index], this.transform);
    }

    public void Module6(int index)
    {
        currentGame = Instantiate(challenges6[index], this.transform);
    }

}
