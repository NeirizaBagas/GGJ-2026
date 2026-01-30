using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null && transform.childCount < 1)
        {
            Debug.Log("Item dropped into slot");
            RectTransform droppedItem = eventData.pointerDrag.GetComponent<RectTransform>();
            droppedItem.SetParent(this.transform, false);
            droppedItem.anchoredPosition = Vector2.zero;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
