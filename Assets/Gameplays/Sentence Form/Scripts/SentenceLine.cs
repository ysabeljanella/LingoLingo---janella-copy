using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SentenceLine : MonoBehaviour, IDropHandler
{
    public int Id;
    public int SentenceId;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableWord draggableItem = dropped.GetComponent<DraggableWord>();
        if (draggableItem != null)
        {
            draggableItem.parentAfterDrag = transform;
            Id = draggableItem.Id;
            Debug.Log("Success");
        }
    }
}
