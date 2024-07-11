using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MatchItem : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerEnterHandler, IPointerUpHandler
{
    static MatchItem hoverItem;
    public GameObject linePrefab;
    public string itemName;
    public string answer;
    private GameObject line;
    private MatchItem connectedItem;
    public bool Left;
    public bool Occupied;
    public void OnDrag(PointerEventData eventData)
    {
        UpdateLine(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Left) return;
        if(connectedItem != null)
        {
            connectedItem.Occupied = false;
        }
        
        Destroy(line);
        connectedItem = null;
        line = Instantiate(linePrefab, transform.position, Quaternion.identity, transform.parent.parent);
        SoundHandler.instance.clickSource.Play();
        UpdateLine(eventData.position);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverItem = this;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (connectedItem == null && hoverItem != null && hoverItem != this && !hoverItem.Left && !hoverItem.Occupied)
        {
            SoundHandler.instance.clickSource.Play();
            connectedItem = hoverItem;
            hoverItem.Occupied = true;
            answer = connectedItem.itemName;
            UpdateLine(hoverItem.transform.position);
        }
        else
        {
            answer = null;
            Destroy(line);
        }
    }

    void UpdateLine(Vector3 position)
    {
        if (line != null)
        {
            // update direction
            Vector3 direction = position - transform.position;
            line.transform.right = direction;
            // update scale
            line.transform.localScale = new Vector3(direction.magnitude, 1, 1);
        }
    }
}
