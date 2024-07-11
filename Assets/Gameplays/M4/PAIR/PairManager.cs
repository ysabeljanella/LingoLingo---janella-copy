using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PairManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static PairManager Instance;
    public PairCard card;
    public Transform cardContainer;
    public TextMeshProUGUI timer;
    public float gameDuration;
    public int module;
    public int challengeIndex;
    public int coinReward;
    public int currentScore;
    public int maxScore;

    public Transform gameObjectcontainer;
    public GameObject levelCompleteUI;
    void Start()
    {
        GameTimerScript.instance.FinishGameEvent += FinalizeQuiz;
        Instance = this;    
        GameTimerScript.instance.StartTimer(gameDuration);
        GameTimerScript.instance.timerText = timer;
        coinReward = 0;
        InitializePairCardGame();
    }
    public void InitializePairCardGame()
    {
        int leftCardIndex = 0;
        foreach (var pair in card.LeftImage)
        {           
            GameObject cardObject = Instantiate(card.CardButton, cardContainer);
            CardMatch cardMatch = cardObject.GetComponent<CardMatch>();
            cardMatch.cardIndex = leftCardIndex;
            cardMatch.image[0] = pair;
            cardMatch.image[1] = card.BackImage;
            leftCardIndex++;
        }

        int rightCardIndex = 0;
        foreach (var pair in card.RightImage)
        {
            GameObject cardObject = Instantiate(card.CardButton, cardContainer);
            CardMatch cardMatch = cardObject.GetComponent<CardMatch>();
            cardMatch.cardIndex = rightCardIndex;
            cardMatch.image[0] = pair;
            cardMatch.image[1] = card.BackImage;
            rightCardIndex++;
        }

        RandomizeOrder();
    }

    public void RandomizeOrder()
    {
        // Generate random order of indices
        List<int> indices = new List<int>();
        for (int i = 0; i < cardContainer.childCount; i++)
        {
            indices.Add(i);
        }

        // Shuffle indices using Fisher-Yates shuffle algorithm
        for (int i = 0; i < indices.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, indices.Count);
            int temp = indices[i];
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        // Rearrange child objects based on shuffled indices
        for (int i = 0; i < indices.Count; i++)
        {
            cardContainer.GetChild(i).SetSiblingIndex(indices[i]);
        }
    }

    public void FinalizeQuiz()
    {
        GameTimerScript.instance.StopTimer();
        GameObject result = Instantiate(levelCompleteUI, gameObjectcontainer);
        result.GetComponentInChildren<TextMeshProUGUI>().text = $"Your score {currentScore}/{maxScore}\n+{coinReward} coins";
        PlayerDataHandler.instance.SetModuleValue(module, challengeIndex, currentScore);
        PlayerDataHandler.instance.CoinReward(coinReward);
        StartCoroutine(CloseThisGame());
    }

    private IEnumerator CloseThisGame()
    {
        yield return new WaitForSeconds(3f);
        PixelCrushers.DialogueSystem.Sequencer.Message("CloseGame");
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameTimerScript.instance.FinishGameEvent -= FinalizeQuiz;
    }
}
