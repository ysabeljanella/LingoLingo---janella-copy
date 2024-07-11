using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageNavigation : MonoBehaviour
{
    public Image imageComponent; // The Image UI component to display the images
    public Sprite[] images; // Array of images to display
    public AudioClip[] audioClips; // Array of audio clips to play for each page
    public Button nextPageButton; // Button to navigate to the next page
    public Button previousPageButton; // Button to navigate to the previous page

    private int currentPageIndex = 0;
    private AudioSource audioSource; // The AudioSource component to play the audio

    void Start()
    {
        // Ensure we have an AudioSource component on the GameObject
        audioSource = gameObject.AddComponent<AudioSource>();

        // Initial setup to display the first image and set button states
        UpdatePageVisibility();

        // Add button click event listeners
        nextPageButton.onClick.AddListener(NextPage);
        previousPageButton.onClick.AddListener(PreviousPage);
    }

    void NextPage()
    {
        if (currentPageIndex < images.Length - 1)
        {
            currentPageIndex++;
            UpdatePageVisibility();
        }
    }

    void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdatePageVisibility();
        }
    }

    void UpdatePageVisibility()
    {
        // Show current page image
        if (currentPageIndex >= 0 && currentPageIndex < images.Length)
        {
            imageComponent.sprite = images[currentPageIndex];
            PlayCurrentAudio(); // Play the audio for the current page
        }

        // Enable/disable navigation buttons based on the current page index
        nextPageButton.interactable = currentPageIndex < images.Length - 1;
        previousPageButton.interactable = currentPageIndex > 0;
    }

    void PlayCurrentAudio()
    {
        // Ensure the current page index is within the range of the audio clips array
        if (currentPageIndex >= 0 && currentPageIndex < audioClips.Length)
        {
            audioSource.clip = audioClips[currentPageIndex];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No audio clip assigned for this page.");
        }
    }

    public void CloseNavigation()
    {
        // Example method call to close the navigation, this depends on your specific use case
        PixelCrushers.DialogueSystem.Sequencer.Message("CloseGame");
    }
}
