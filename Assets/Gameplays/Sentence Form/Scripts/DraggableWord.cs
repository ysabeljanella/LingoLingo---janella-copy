using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableWord : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int Id;
    public Image image;
    public Transform parentAfterDrag;
    public Transform defaultParent;
    public Transform wordsContainer;
    public Transform sentenceContainer;
    private SentenceLine parentSentenceLine;
    private void Start()
    {
        defaultParent = transform.parent;
        wordsContainer = GameObject.Find("Words Container").transform;
        sentenceContainer = GameObject.Find("Sentence Container").transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (transform.parent.GetComponent<SentenceLine>() != null)
        {
            // If yes, store the reference to the SentenceLine
            parentSentenceLine = transform.parent.GetComponent<SentenceLine>();
            parentSentenceLine.Id = 0;
        }
        Debug.Log("Begin drag");
        transform.SetParent(wordsContainer); // Set parent to words container initially
        transform.SetAsLastSibling();
        image.raycastTarget = false;
        SoundHandler.instance.clickSource.Play();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Check if the item is dropped over a sentence line in the sentence container
        bool droppedOnSentenceLine = eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<SentenceLine>() != null;

        // If dropped on a valid drop target (sentence line), set its parent accordingly
        if (droppedOnSentenceLine)
        {
            transform.SetParent(eventData.pointerEnter.transform); // Set parent to the sentence line
        }
        else
        {
            // Snap back to its original parent if dropped on nothing
            transform.SetParent(defaultParent); // Set parent back to the default parent (words container)
            transform.localPosition = Vector3.zero; // Optionally, reset the position to snap it exactly to the original position
        }

        image.raycastTarget = true;
        SoundHandler.instance.clickSource.Play();
    }
}
