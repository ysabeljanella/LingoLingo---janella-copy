using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SentenceFormGame : MonoBehaviour
{
    public static SentenceFormGame instance;
    public GameObject levelCompleteUI;
    public Transform gameObjectcontainer;
    public SentenceFormScriptableObject[] scriptableObject;
    public Transform sentenceContainer;
    public Transform wordsContainer;
    private List<SentenceLine> sentenceLine;
    public TextMeshProUGUI timer;
    public int currentScore;
    public int maxScore;
    public float gameDuration;
    public int module;
    public int challengeIndex;
    private int currentChallengeIndex = 0;
    private int coinReward;
    public TextMeshProUGUI english;
    private void Start()
    {
        GameTimerScript.instance.FinishGameEvent += FinalizeForm;
        sentenceLine = new List<SentenceLine>();
        instance = this;
        coinReward = 0;
        GameTimerScript.instance.StartTimer(gameDuration);
        GameTimerScript.instance.timerText = timer;
        LoadChallenge();
    }

    public void FinalizeForm()
    {
        GameTimerScript.instance.StopTimer();
        GameObject result = Instantiate(levelCompleteUI, gameObjectcontainer);
        result.GetComponentInChildren<TextMeshProUGUI>().text = $"Your score: {currentScore}/{maxScore}\n+{coinReward} coins";
        PlayerDataHandler.instance.SetModuleValue(module, challengeIndex, currentScore);
        PlayerDataHandler.instance.CoinReward(coinReward);

        StartCoroutine(CloseThisGame());
    }

    public void LoadChallenge()
    {
        if (currentChallengeIndex < scriptableObject.Length) // Check if there are more challenges
        {
            SentenceFormScriptableObject currentScriptableObject = scriptableObject[currentChallengeIndex];
            english.text = currentScriptableObject.sentenceEnglish.ToString();
            int sentenceIndex = 1;
            int audioIndex = 0;
            foreach (string word in currentScriptableObject.words)
            {
                GameObject sentence = Instantiate(currentScriptableObject.SentenceLine, sentenceContainer);
                SentenceLine line = sentence.GetComponent<SentenceLine>();

                sentence.name = sentenceIndex.ToString();

                line.SentenceId = sentenceIndex;

                GameObject wordObject = Instantiate(currentScriptableObject.Word, wordsContainer);

                if(currentScriptableObject.audio.Length > 0)
                {
                    wordObject.GetComponent<AudioSource>().clip = currentScriptableObject.audio[audioIndex];
                    audioIndex++;
                }
                
                wordObject.GetComponentInChildren<TextMeshProUGUI>().text = word;
                wordObject.GetComponent<DraggableWord>().Id = sentenceIndex;

                sentenceLine.Add(line);
                sentenceIndex++;
            }
            Debug.Log("Randomizing word order...");
            RandomizeWordOrder();
            Debug.Log("Word order randomized.");
        }
        else
        {
            FinalizeForm(); // Finalize if there are no more challenges
        }
    }
    public void RandomizeWordOrder()
    {
        // Generate random order of indices
        List<int> indices = new List<int>();
        for (int i = 0; i < wordsContainer.childCount; i++)
        {
            indices.Add(i);
        }

        // Shuffle indices using Fisher-Yates shuffle algorithm
        for (int i = 0; i < indices.Count; i++)
        {
            int randomIndex = Random.Range(i, indices.Count);
            int temp = indices[i];
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        // Rearrange child objects based on shuffled indices
        for (int i = 0; i < indices.Count; i++)
        {
            wordsContainer.GetChild(i).SetSiblingIndex(indices[i]);
        }
    }
    public void AnswerHandler()
    {
        maxScore++; // Increment max score for each challenge
        bool isCorrectOrder = true;

        foreach (SentenceLine obj in sentenceLine)
        {
            if (obj.Id != obj.SentenceId)
            {
                isCorrectOrder = false;
                break;
            }
        }

        if (isCorrectOrder)
        {
            currentScore++; // Increment current score if order is correct
            coinReward += 20;
        }

        currentChallengeIndex++; // Move to the next challenge

        ClearPreviousChallenge(); // Clear previous challenge data
        LoadChallenge(); // Load the next challenge
    }

    private void ClearPreviousChallenge()
    {
        foreach (Transform child in sentenceContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in wordsContainer)
        {
            Destroy(child.gameObject);
        }

        sentenceLine.Clear();
    }
    private void OnDestroy()
    {
        GameTimerScript.instance.FinishGameEvent -= FinalizeForm;
    }

    private IEnumerator CloseThisGame()
    {
        yield return new WaitForSeconds(3f);
        PixelCrushers.DialogueSystem.Sequencer.Message("CloseGame");
        Destroy(gameObject);
    }
}
