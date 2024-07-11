using UnityEngine;
using UnityEngine.UI;

public class CanvasNavigation : MonoBehaviour
{
    public GameObject[] canvasPages; // Array of canvas pages
    public Button nextPageButton; // Button to navigate to the next page
    public Button previousPageButton; // Button to navigate to the previous page
    public GameObject gameObjectToShowOnLastPage; // Game object to show on the last page
    public string ModuleVariable;

    private int currentPageIndex = 0;

    void Start()
    {
        UpdatePageVisibility();
        // Add button click event listeners
        nextPageButton.onClick.AddListener(NextPage);
        previousPageButton.onClick.AddListener(PreviousPage);
    }

    void NextPage()
    {
        currentPageIndex++;
        UpdatePageVisibility();
    }

    void PreviousPage()
    {
        currentPageIndex--;
        UpdatePageVisibility();
    }

    void UpdatePageVisibility()
    {
        SoundHandler.instance.Click();
        // Hide all pages
        foreach (GameObject page in canvasPages)
        {
            page.SetActive(false);
        }

        // Show current page
        if (currentPageIndex >= 0 && currentPageIndex < canvasPages.Length)
        {
            canvasPages[currentPageIndex].SetActive(true);
        }

        // Enable/disable navigation buttons based on current page
        nextPageButton.interactable = currentPageIndex < canvasPages.Length - 1;
        previousPageButton.interactable = currentPageIndex > 0;

        
    }

    public void CloseNavigation()
    {
        PixelCrushers.DialogueSystem.Sequencer.Message("CloseGame");
    }
}
