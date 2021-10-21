using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler {

    public int slotIndex;

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
        parentRect = rectTransform.parent.parent.parent.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData) {        
        if(inventory.GetItem(slotIndex)) {
            inventory.SetVisibilityOfCraftingSlot(false, slotIndex);
            Item item = inventory.GetItem(slotIndex);
            selectedItem.SetImages(item.firstIcon, item.secondIcon, item.thirdIcon, item.firstColor, item.secondColor, item.thirdColor);
            selectedItem.rectTransform.anchoredPosition = parentRect.localPosition + rectTransform.localPosition;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData) {        
        selectedItem.rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;        
    }

    public void OnDrop(PointerEventData eventData) {
        CraftingSlot fromCraftingSlot = eventData.pointerDrag.GetComponent<CraftingSlot>();
        ItemSlot fromSlot = eventData.pointerDrag.GetComponent<ItemSlot>();
        CraftingOutput craftingOutput = eventData.pointerDrag.GetComponent<CraftingOutput>();
        if(fromSlot && inventory.inventory[fromSlot.slotIndex]) {
            if(fromSlot) {
                inventory.SetItem(inventory.inventory[fromSlot.slotIndex], slotIndex);
                inventory.TakeItemFromSlot(fromSlot.slotIndex);
            }
        }
        else if(fromCraftingSlot) {
            if(this != fromCraftingSlot) {
                if(inventory.craftingSlots[slotIndex] == null) {
                    inventory.SetItem(inventory.craftingSlots[fromCraftingSlot.slotIndex], slotIndex);
                    inventory.RemoveItem(fromCraftingSlot.slotIndex);
                }
                else {
                    Item i = inventory.craftingSlots[slotIndex];
                    inventory.SetItem(inventory.craftingSlots[fromCraftingSlot.slotIndex], slotIndex);
                    inventory.SetItem(i, fromCraftingSlot.slotIndex);
                }
            }
        }
        else if(craftingOutput) {
            if(inventory.CanAddToInventory()) {
                inventory.SetItem(inventory.craft, slotIndex);
                inventory.craft = null;                
                inventory.UpdateCraftingResult();
            }
        }
        selectedItem.Release();
        inventory.SetVisibilityOfCraftingSlot(true, slotIndex);        
    }

    public void OnEndDrag(PointerEventData eventData) {        
        canvasGroup.blocksRaycasts = true;
        selectedItem.Release();
        inventory.SetVisibilityOfCraftingSlot(true, slotIndex);        
    }
}
