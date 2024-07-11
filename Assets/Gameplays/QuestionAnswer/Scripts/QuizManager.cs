 using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{

    public static QuizManager instance;
    public QuestionScriptableObject[] questionScriptableObjects;
    public TextMeshProUGUI timer;
    public Text currentQuestion;

    public Transform gameObjectcontainer;
    public GameObject levelCompleteUI;

    public Transform AnswerParent;
    public int currentScore;
    public int maxScore;
    public int currentQuestionIndex = 0;
    public float gameDuration;
    public int module;
    public int challengeIndex;
    private int coinReward;

    public Button submitButton;
    public List<int> selectedAnswers = new List<int>();

    private void Start()
    {//
        GameTimerScript.instance.FinishGameEvent += FinalizeQuiz;
        instance = this;      
        maxScore = questionScriptableObjects.Length;
        Debug.Log(questionScriptableObjects.Length);
        LoadQuestion(currentQuestionIndex);
        GameTimerScript.instance.StartTimer(gameDuration);
        GameTimerScript.instance.timerText = timer;
        coinReward = 0;

        submitButton.interactable = false;

    }
    private void OnDestroy()
    {
        GameTimerScript.instance.FinishGameEvent -= FinalizeQuiz;
    }

    public void LoadQuestion(int Index)
    {
        SoundHandler.instance.clickSource.Play();
        currentQuestionIndex = Index;
        currentQuestion.text = questionScriptableObjects[Index].question;
        
        selectedAnswers.Clear();
        foreach (Transform child in AnswerParent.transform)
        {
            Destroy(child.gameObject);
        }


        int answerId = 0;
        foreach (string answer in questionScriptableObjects[Index].answers)
        {
            GameObject questionItem = Instantiate(questionScriptableObjects[Index].answerButtonPrefab, AnswerParent);
            TextMeshProUGUI buttonText = questionItem.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = answer;

            questionItem.GetComponentInChildren<AnswerButton>().Id = answerId;

            questionItem.GetComponent<Button>().onClick.AddListener(() => ToggleAnswer(questionItem.GetComponent<AnswerButton>()));
            answerId++;
        }

        RandomizeOrder();
    }
    public void ToggleAnswer(AnswerButton answerButton)
    {
        // Toggle selection of answers
        if (selectedAnswers.Contains(answerButton.Id))
        {
            selectedAnswers.Remove(answerButton.Id);
            submitButton.interactable = false;
            answerButton.SetSelected(false);
        }
        else
        {
            // Check if max selections reached
            
            if (selectedAnswers.Count < 2)
            {
                selectedAnswers.Add(answerButton.Id);
                answerButton.SetSelected(true);

                if(selectedAnswers.Count == 2)
                {
                    submitButton.interactable = true;
                }                         
            }
            else
            {
                Debug.Log("Maximum selections reached");
            }
        }
    }
    public void SubmitAnswer()
    {
        // Check if selected answers are correct
        bool isCorrect = true;
        for (int i = 0; i < selectedAnswers.Count; i++)
        {
            if (selectedAnswers[i] != i)
            {
                isCorrect = false;
                break;
            }
        }

        // Update score based on correctness
        if (isCorrect)
        {
            currentScore++;
            coinReward += 10;
        }

        currentQuestionIndex++; // Increment the index here

        if (currentQuestionIndex < questionScriptableObjects.Length) // Check if index is within bounds
        {
            LoadQuestion(currentQuestionIndex); // Load the next question
        }
        else
        {
            FinalizeQuiz(); // Finalize the quiz if all questions have been answered
        }
    }
    public void RandomizeOrder()
    {
        // Generate random order of indices
        List<int> indices = new List<int>();
        for (int i = 0; i < AnswerParent.childCount; i++)
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
            AnswerParent.GetChild(i).SetSiblingIndex(indices[i]);
        }
    }


    public void AnswerHandler(int Id)
    {
        if (Id == 0)
        {
            currentScore++;
            coinReward += 10;
        }

        currentQuestionIndex++; // Increment the index here

        if (currentQuestionIndex < questionScriptableObjects.Length) // Check if index is within bounds
        {
            LoadQuestion(currentQuestionIndex); // Load the next question
        }
        else
        {
            FinalizeQuiz(); // Finalize the quiz if all questions have been answered
        }
    }

    private IEnumerator CloseThisGame()
    {
        yield return new WaitForSeconds(3f);
        PixelCrushers.DialogueSystem.Sequencer.Message("CloseGame");
        Destroy(gameObject);
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
}
