using UnityEngine;
using UnityEngine.UI;

public class OpenPanelOnClick : MonoBehaviour
{
    public GameObject panel;
    public AudioClip clickSound; // Reference to your click sound

    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        // Initially, disable the panel
        panel.SetActive(false);

        // Get the AudioSource component attached to this GameObject or add one if not present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the clip to play when clicking
        audioSource.clip = clickSound;
    }

    void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits this object
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                // If so, play the click sound
                audioSource.PlayOneShot(clickSound);

                // Open the panel
                panel.SetActive(true);
            }
            else
            {
                // If the click is not on this object, close the panel
                panel.SetActive(false);
            }
        }
    }
}
