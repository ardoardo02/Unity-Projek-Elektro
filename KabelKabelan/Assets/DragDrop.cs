using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] string partType;
    
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Button portBtn;
    private PortCable portCable;

    public string PartType { get => partType; set => partType = value; }

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        portBtn = transform.GetComponentInChildren<Button>();
        portCable = transform.GetComponentInChildren<PortCable>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        // Debug.Log("OnBeginDrag");
        portBtn.interactable = false;
        portCable.CheckLine();
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag");
        // transform.position = Input.mousePosition;
        rectTransform.anchoredPosition += eventData.delta;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("OnPointerDown");
    }
}
