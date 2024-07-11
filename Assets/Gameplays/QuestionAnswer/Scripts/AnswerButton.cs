using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    public int Id;

    public Color defaultColor; // Default color of the button
    public Color selectedColor; // Color to change to when selected
    public Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            // Change button color to selected color
            button.image.color = selectedColor;
        }
        else
        {
            // Revert button color to default color
            button.image.color = defaultColor;
        }
    }

}
