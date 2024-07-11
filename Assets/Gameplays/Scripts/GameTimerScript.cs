using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimerScript : MonoBehaviour
{
    public static GameTimerScript instance;

    public TextMeshProUGUI timerText; // Reference to the Text component where you want to display the timer
    private float timerDuration = 10f;
    private float startTime;
    public bool timerRunning;

    public event Action StartGameEvent;
    public event Action FinishGameEvent;

    void Start()
    {
        instance = this;        
    }

    void Update()
    {
        if (timerRunning)
        {
            float timeElapsed = Time.time - startTime;
            DisplayTime(timerDuration - timeElapsed);
            if (timeElapsed >= timerDuration)
            {
                TimerRunOut();
            }
        }
    }

    public void StartTimer(float duration)
    {
        timerDuration = duration;
        startTime = Time.time;
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerRunOut()
    {
        // Perform actions when the timer runs out
        Debug.Log("Timer has run out!");
        FinishGameEvent?.Invoke();
    }

    public void QuitApp()
    {
        PlayerDataHandler.instance.SaveModuleChallengesToJson();
        Application.Quit();
    }
}
