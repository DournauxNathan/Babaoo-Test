using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");

        if (eventData.pointerDrag != null)
        {
            //Snap the current drag objet to the this anchoredPosition
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            //Set the current tile index to this cell index in hierarchy
            eventData.pointerDrag.GetComponent<Tile>().currentiD = transform.GetSiblingIndex();

            if (eventData.pointerDrag.GetComponent<Tile>().currentiD == eventData.pointerDrag.GetComponent<Tile>().targetID)
            {
                GameManager.instance.IsResolve(1);
            }
            else
            {
                GameManager.instance.IsResolve(-1);
            }
        }
    }

}
