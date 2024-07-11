using Michsky.MUIP;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameProgressScript : MonoBehaviour
{
    [SerializeField] private ProgressBar[] ModuleProgress;
    public int[] percentage;

    [Header("Module1")]
    public TextMeshProUGUI[] text1;

    [Header("Module2")]
    public TextMeshProUGUI[] text2;

    [Header("Module3")]
    public TextMeshProUGUI[] text3;

    [Header("Module4")]
    public TextMeshProUGUI[] text4;

    [Header("Module5")]
    public TextMeshProUGUI[] text5;

    [Header("Module6")]
    public TextMeshProUGUI[] text6;
    private void Start()
    {
        PlayerDataProgress();
    }

    public void CloseProgress()
    {
        PixelCrushers.DialogueSystem.Sequencer.Message("CloseGame");
    }
    private void HandleZeroPercent()
    {
        foreach (ProgressBar progressBar in ModuleProgress)
        {
            int index = System.Array.IndexOf(ModuleProgress, progressBar);
            if (index >= 0 && index < percentage.Length && percentage[index] == 0)
            {
                progressBar.invert = true;
            }
            else
            {
                progressBar.invert = false;
            }
        }
    }
    public void PlayerDataProgress()
    {
        int currentIndex = 0;
        foreach (PlayerDataHandler.Module module in PlayerDataHandler.instance.modules)
        {
            int sum = module.challenges[0] + module.challenges[1] + module.challenges[2];

            switch (currentIndex)
            {
                case 0:
                    text1[0].text = "Game 1: " + module.challenges[0].ToString();
                    text1[1].text = "Game 2: " + module.challenges[1].ToString();
                    text1[2].text = "Game 3: " + module.challenges[2].ToString();
                    break;
                case 1:
                    text2[0].text = "Game 1: " + module.challenges[0].ToString();
                    text2[1].text = "Game 2: " + module.challenges[1].ToString();
                    text2[2].text = "Game 3: " + module.challenges[2].ToString();
                    break;
                case 2:
                    text3[0].text = "Game 1: " + module.challenges[0].ToString();
                    text3[1].text = "Game 2: " + module.challenges[1].ToString();
                    text3[2].text = "Game 3: " + module.challenges[2].ToString();
                    break;
                case 3:
                    text4[0].text = "Game 1: " + module.challenges[0].ToString();
                    text4[1].text = "Game 2: " + module.challenges[1].ToString();
                    text4[2].text = "Game 3: " + module.challenges[2].ToString();
                    break;
                case 4:
                    text5[0].text = "Game 1: " + module.challenges[0].ToString();
                    text5[1].text = "Game 2: " + module.challenges[1].ToString();
                    text5[2].text = "Game 3: " + module.challenges[2].ToString();
                    break;
                case 5:
                    text6[0].text = "Game 1: " + module.challenges[0].ToString();
                    text6[1].text = "Game 2: " + module.challenges[1].ToString();
                    text6[2].text = "Game 3: " + module.challenges[2].ToString();
                    break;
            }

            int maxScore = PlayerDataHandler.instance.ChallengeMaxScore[currentIndex];
            if(sum <= 1)
            {
                percentage[currentIndex] = 0;
            }
            else
            {
                percentage[currentIndex] = Mathf.RoundToInt((float)sum / maxScore * 100);
            }           
            currentIndex++;
        }

        InitializeProgressBars();
    }
    private void InitializeProgressBars()
    {
        foreach (ProgressBar progressBar in ModuleProgress)
        {
            progressBar.onValueChanged.AddListener(delegate { OnValueChanged(progressBar); });
        }

        HandleZeroPercent();
    }

    private void OnValueChanged(ProgressBar progressBar)
    {
        int index = System.Array.IndexOf(ModuleProgress, progressBar);
        if (index >= 0 && index < percentage.Length)
        {
            float value = progressBar.currentPercent;
            if (value >= percentage[index])
            {
                progressBar.isOn = false;
                progressBar.currentPercent = percentage[index];
                progressBar.onValueChanged.RemoveAllListeners();
            }
            Debug.Log("Current value: " + value.ToString());
        }
    }

    private void OnEnable()
    {
        foreach (ProgressBar progressBar in ModuleProgress)
        {
            progressBar.isOn = true;
            progressBar.currentPercent = 0f; // Reset progress to 0 when script is enabled

        }
        PlayerDataProgress();
    }

    private void OnDisable()
    {
        foreach (ProgressBar progressBar in ModuleProgress)
        {
            progressBar.isOn = false;
            progressBar.currentPercent = 0f; // Reset progress to 0 when script is disabled
        }
    }
}
