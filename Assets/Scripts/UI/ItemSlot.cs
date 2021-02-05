using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler {

    public int slotIndex;

    private Inventory inventory;
    private SelectedItem selectedItem;
    private Canvas canvas;
    private RectTransform itemSlotGrid;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void OnEnable() {
        inventory = FindObjectOfType<Inventory>();
        canvas = FindObjectOfType<Canvas>();
        selectedItem = FindObjectOfType<UICanvas>().selectedItem;
        itemSlotGrid = FindObjectOfType<GridLayoutGroup>().GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if(inventory.inventory[slotIndex]) {
            inventory.SetVisibilityOfInventorySlot(false, slotIndex);
            Item item = inventory.inventory[slotIndex];
            selectedItem.SetImages(item.firstIcon, item.secondIcon, item.thirdIcon, item.firstColor, item.secondColor, item.thirdColor);
            selectedItem.rectTransform.anchoredPosition = itemSlotGrid.localPosition + rectTransform.localPosition;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData) {
        selectedItem.rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnDrop(PointerEventData eventData) {
        Debug.Log("On drop called");
        ItemSlot droppedSlot = GetComponent<ItemSlot>();
        EquipmentSlot fromEquipmentSlot = eventData.pointerDrag.GetComponent<EquipmentSlot>();
        ItemSlot fromSlot = eventData.pointerDrag.GetComponent<ItemSlot>();
        if(fromSlot && inventory.inventory[fromSlot.slotIndex]) {
            if(fromSlot && droppedSlot && fromSlot != droppedSlot) {
                inventory.MoveItem(fromSlot.slotIndex, droppedSlot.slotIndex);
            }
        }
        else if(fromEquipmentSlot) {
            if(fromEquipmentSlot && droppedSlot && fromEquipmentSlot != droppedSlot) {
                inventory.UnequipItem(fromEquipmentSlot.slotIndex, droppedSlot.slotIndex);
            }
        }
        selectedItem.Release();
        inventory.SetVisibilityOfInventorySlot(true, slotIndex);
    }

    public void OnEndDrag(PointerEventData eventData) {
        Debug.Log("On end drag called");
        canvasGroup.blocksRaycasts = true;
        selectedItem.Release();
        inventory.SetVisibilityOfInventorySlot(true, slotIndex);
    }
}
