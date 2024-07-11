using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public static SoundHandler instance;
    [SerializeField] private SliderManager volumeSlider;
    private const string volumePrefsKey = "VolumeLevel";
    public AudioSource audioSource;
    public AudioSource clickSource;
    public AudioClip[] clip;

    // Enum to define different game states
    public enum GameState
    {
        Default,
        Conversation,
        Quiz
    }

    // Variable to store the current game state
    public GameState state;

    void Start()
    {
        instance = this;

        if (PlayerPrefs.HasKey(volumePrefsKey))
        {
            float savedVolume = PlayerPrefs.GetFloat(volumePrefsKey);
            volumeSlider.mainSlider.value = savedVolume;
            SetVolume(savedVolume);
        }
        else
        {
            // Default volume level
            volumeSlider.mainSlider.value = 1f;
            SetVolume(1f);
        }

        // Subscribe to slider's value changed event directly in the inspector
        volumeSlider.mainSlider.onValueChanged.AddListener(AdjustVolume);
    }

    void AdjustVolume(float value)
    {
        SetVolume(value);

        // Save the volume level
        PlayerPrefs.SetFloat(volumePrefsKey, value);
        PlayerPrefs.Save();
    }

    void SetVolume(float value)
    {
        // Ensure value is between 0 and 1
        float clampedValue = Mathf.Clamp01(value);

        // Adjust AudioListener volume
        AudioListener.volume = clampedValue;

        Debug.Log("Volume adjusted to: " + clampedValue);
    }

    public void Click()
    {
        clickSource.Play();
    }

    // Method to set the game state
    public void SetGameState(GameState gameState)
    {
        state = gameState;
        PlayAudioForState();
    }

    public void SetGameStateDefault()
    {
        state = GameState.Default;
        PlayAudioForState();
    }

    public void SetGameStateConvo()
    {
        state = GameState.Conversation;
        PlayAudioForState();
    }

    public void SetGameStateQuiz()
    {
        state = GameState.Quiz;
        PlayAudioForState();
    }
    // Method to play audio based on the current game state
    private void PlayAudioForState()
    {
        // Ensure that the audio source and clips are set
        if (audioSource != null && clip != null)
        {
            // Check if the index for the current state is within the array bounds
            if ((int)state >= 0 && (int)state < clip.Length && clip[(int)state] != null)
            {
                audioSource.clip = clip[(int)state];
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("Audio clip for the current state is not set!");
            }
        }
        else
        {
            Debug.LogWarning("Audio source or clip is not assigned!");
        }
    }
}
