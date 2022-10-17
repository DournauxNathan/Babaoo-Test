using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        //Get Components
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        boxCollider = GetComponent<BoxCollider2D>();

        //Disable collider
        boxCollider.enabled = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager.instance.isSwappable(rectTransform))
        {
            //Debug.Log("BeginDrag");

            //Disable collider
            boxCollider.enabled = true;

            //Set the image's alpha
            canvasGroup.alpha = .6f;
            //Disable collision with raycats
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag");
        if (GameManager.instance.isSwappable(rectTransform))
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("EndDrag");
        //Set the image's alpha back to 1
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("PointerDown");
    }
}
