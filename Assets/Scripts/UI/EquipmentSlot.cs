﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler {

    public int slotIndex;

    private Inventory inventory;
    private SelectedItem selectedItem;
    private Canvas canvas;

    private void OnEnable() {
        inventory = FindObjectOfType<Inventory>();
        canvas = FindObjectOfType<Canvas>();
        selectedItem = FindObjectOfType<UICanvas>().selectedItem;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if(inventory.equipment[slotIndex]) {
            inventory.SetVisibilityOfEquipmentSlot(false, slotIndex);
            Item item = inventory.equipment[slotIndex];
            selectedItem.SetImages(item.firstIcon, item.secondIcon, item.thirdIcon, item.firstColor, item.secondColor, item.thirdColor);
            selectedItem.rectTransform.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }        
    }

    public void OnDrag(PointerEventData eventData) {
        selectedItem.rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnDrop(PointerEventData eventData) {
        EquipmentSlot droppedSlot = GetComponent<EquipmentSlot>();
        ItemSlot fromSlot = eventData.pointerDrag.GetComponent<ItemSlot>();
        if(inventory.inventory[fromSlot.slotIndex] != null) {
            //inventory.EquipItem(inventory.inventory[fromSlot.slotIndex]);
            inventory.EquipItemInSlot(fromSlot.slotIndex);
        }
        selectedItem.Release();
        inventory.SetVisibilityOfEquipmentSlot(true, slotIndex);
    }

    public void OnEndDrag(PointerEventData eventData) {
        selectedItem.Release();
        inventory.SetVisibilityOfEquipmentSlot(true, slotIndex);
    }
}