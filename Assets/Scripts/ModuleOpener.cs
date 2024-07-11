using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModuleOpener : MonoBehaviour
{
    public GameObject modulePanel;
    public Text panelText;

    public AudioSource clickSound; // Reference to the AudioSource component
    public AudioClip clickClip;    // AudioClip for the clicking sound

    void Start()
    {
        // Ensure you have an AudioSource component on the same GameObject
        // as this script or set it in the Inspector.
        if (clickSound == null)
        {
            clickSound = gameObject.AddComponent<AudioSource>();
        }

        // Load the click sound AudioClip
        clickClip = Resources.Load<AudioClip>("clilck(1)");

        // Assign the loaded AudioClip to the AudioSource
        if (clickClip != null)
        {
            clickSound.clip = clickClip;
        }
    }

    void PlayClickSound()
    {
        // Play the click sound if available
        if (clickSound != null && clickClip != null)
        {
            clickSound.PlayOneShot(clickClip);
        }
    }

    void OnMouseDown()
    {
        // Play the click sound
        PlayClickSound();

        // Show the module panel
        modulePanel.SetActive(true);
    }

    public void OnYesClick()
    {
        // Play the click sound
        PlayClickSound();

        // Load the Module1Scene
        SceneManager.LoadScene("Consonants");
    }
}

