using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PortalUI : MonoBehaviour {

    private int[] bonusIndices;

    public TextMeshProUGUI chooseOneText;
    public TextMeshProUGUI[] bonusTexts;
    public TextMeshProUGUI[] chooseTexts;

    private Portal portal;

    private void Start() {
        chooseOneText.text = LocalizationManager.localization.GetText("chooseOne");
        for(int i = 0; i < chooseTexts.Length; i++) {
            chooseTexts[i].text = LocalizationManager.localization.GetText("choose");
        }
    }
    public void SetBonuses(List<int> indices, Portal portal) {
        this.portal = portal;
        bonusIndices = indices.ToArray();
        for(int i = 0; i < bonusTexts.Length; i++) {
            bonusTexts[i].text = LocalizationManager.localization.GetText($"npc{bonusIndices[i]}");
        }
    }
    public void GetBonus(int index) {
        portal.GetBonus(index);
    }
}
