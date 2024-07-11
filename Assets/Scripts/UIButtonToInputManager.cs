using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonToInputManager : MonoBehaviour
{
    [SerializeField]
    private Button jumpButton; // <- assign this in inspector

    private void Awake()
    {
        jumpButton.onClick.AddListener(() => DoJump());
    }

    private void DoJump()
    {
        // Jump logic here
    }
}
