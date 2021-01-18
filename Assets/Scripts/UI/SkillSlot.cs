using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSlot : MonoBehaviour {

    private int slotIndex;
    private TextMeshProUGUI slotIndexText;

    private void Awake() {
        slotIndexText = GetComponent<TextMeshProUGUI>();
    }

    public void SetSlotIndex(int index) {
        slotIndex = index;
        slotIndexText.text = slotIndex.ToString();
    }
}
