using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject instructionsPanel1; // The first panel to show
    public GameObject instructionsPanel2; // The second panel to show
    public AudioClip clickSound; // Reference to the click sound

    private AudioSource audioSource; // Reference to the AudioSource component
    private int clickCount = 0; // Track the number of clicks

    void Start()
    {
        // Ensure only the first panel is active on start
        instructionsPanel1.SetActive(true);
        instructionsPanel2.SetActive(false);

        // Get or add AudioSource component for playing sounds
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Increment the click count
            clickCount++;

            // Play click sound
            audioSource.PlayOneShot(clickSound);

            // If it's the first click, show the second panel
            if (clickCount == 1)
            {
                instructionsPanel1.SetActive(false);
                instructionsPanel2.SetActive(true);
            }
            // If it's the third click, hide the second panel
            else if (clickCount == 3)
            {
                instructionsPanel2.SetActive(false);
            }
        }
    }
}
