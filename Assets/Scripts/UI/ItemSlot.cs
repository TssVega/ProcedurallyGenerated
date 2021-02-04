using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler {

    public int slotIndex;
    private Player player;
    private Inventory _inventory;
    private InventoryPanel parent;
    private SelectedItem selectedItem;
    private bool visible = true;
    private Image image;
    private TextMeshProUGUI quantityText;

    private void OnEnable() {
        selectedItem = FindObjectOfType<UICanvas>().selectedItem;
        quantityText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();        
    }

    public void OnBeginDrag(PointerEventData eventData) {
        //selectedItem.
    }

    public void OnDrag(PointerEventData eventData) {
        throw new System.NotImplementedException();
    }

    public void OnDrop(PointerEventData eventData) {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData) {
        throw new System.NotImplementedException();
    }
}
