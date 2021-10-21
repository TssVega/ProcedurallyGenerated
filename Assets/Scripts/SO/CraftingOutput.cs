using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingOutput : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

    private Inventory inventory;
    private SelectedItem selectedItem;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private RectTransform parentRect;

    private void OnEnable() {
        inventory = FindObjectOfType<Inventory>();
        canvas = FindObjectOfType<Canvas>();
        selectedItem = FindObjectOfType<UICanvas>().selectedItem;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        parentRect = rectTransform.parent.parent.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if(inventory.craft) {            
            Item item = inventory.craft;
            selectedItem.SetImages(item.firstIcon, item.secondIcon, item.thirdIcon, item.firstColor, item.secondColor, item.thirdColor);
            selectedItem.rectTransform.anchoredPosition = parentRect.localPosition + rectTransform.localPosition;
            canvasGroup.blocksRaycasts = false;
            inventory.ConsumeCraftingMaterials();
            inventory.SetVisibilityOfCraft(false);
        }
    }

    public void OnDrag(PointerEventData eventData) {
        selectedItem.rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.blocksRaycasts = true;
        selectedItem.Release();
        inventory.SetVisibilityOfCraft(true);
    }
}
