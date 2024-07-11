using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    public Camera mainCamera; // Reference to the main camera
    public GameObject soundHandler; // Reference to the SoundHandler GameObject

    private AudioSource backgroundMusic; // Reference to the audio source for the background music

    void Start()
    {
        Debug.Log("Start method called.");

        // Find and assign the main camera if it's not assigned in the inspector
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("No main camera found. Please assign a Camera to the mainCamera field.");
            }
        }

        // Check if soundHandler is assigned
        if (soundHandler != null)
        {
            Debug.Log("SoundHandler is assigned.");

            // Assign the audio source from the soundHandler GameObject
            backgroundMusic = soundHandler.GetComponent<AudioSource>();
            if (backgroundMusic == null)
            {
                Debug.LogError("No AudioSource found on the SoundHandler GameObject. Please add an AudioSource component to the SoundHandler GameObject.");
            }
            else
            {
                Debug.Log("AudioSource found and assigned.");
            }
        }
        else
        {
            Debug.LogError("No SoundHandler GameObject assigned. Please assign the SoundHandler GameObject to the soundHandler field.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // You can change KeyCode.Escape to any key you prefer
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Pause the game
            if (mainCamera != null)
            {
                mainCamera.enabled = false; // Disable the camera
            }
            if (backgroundMusic != null)
            {
                backgroundMusic.Pause(); // Pause the music
                Debug.Log("Music paused");
            }
            else
            {
                Debug.LogWarning("backgroundMusic reference is missing.");
            }
        }
        else
        {
            Time.timeScale = 1f; // Resume normal time scale
            if (mainCamera != null)
            {
                mainCamera.enabled = true; // Enable the camera
            }
            if (backgroundMusic != null)
            {
                backgroundMusic.UnPause(); // Resume the music
                Debug.Log("Music unpaused");
            }
            else
            {
                Debug.LogWarning("backgroundMusic reference is missing.");
            }
        }
    }
}
