using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoPanel : MonoBehaviour {

    public Image[] skillSlots;
    public Sprite emptySlotSprite;

    private SkillUser playerSkills;
    private readonly int skillSlotCount = 11;
    private Skill currentSkill;
    private SkillUI skillUI;

    private void Awake() {
        playerSkills = FindObjectOfType<Player>().GetComponent<SkillUser>();
        skillUI = FindObjectOfType<SkillUI>();
    }
    public void SetSkill(Skill skill) {
        currentSkill = skill;
        Refresh();
    }
    private void Refresh() {
        if(playerSkills.acquiredSkills.Contains(currentSkill) && currentSkill is ActiveSkill) {
            for(int i = 0; i < skillSlotCount; i++) {
                skillSlots[i].gameObject.SetActive(true);
                if(i >= playerSkills.currentSkills.Count) {
                    skillSlots[i].sprite = emptySlotSprite;
                    skillSlots[i].color = Color.white;
                    continue;
                }
                if(playerSkills.currentSkills[i] && playerSkills.currentSkills[i].skillIcon) {
                    skillSlots[i].sprite = playerSkills.currentSkills[i].skillIcon;
                    skillSlots[i].color = ColorBySkillType.GetColorByType(playerSkills.currentSkills[i].attackType);
                }
                else {
                    skillSlots[i].sprite = emptySlotSprite;
                    skillSlots[i].color = Color.white;
                }
            }
        }
        else {
            for(int i = 0; i < skillSlotCount; i++) {
                skillSlots[i].gameObject.SetActive(false);
            }
        }
    }
    public void ReplaceSkillSlot(int index) {
        for(int i = 0; i < skillSlotCount; i++) {
            if(playerSkills.currentSkills.Count <= i) {
                continue;
            }
            if(playerSkills.currentSkills[i] == currentSkill) {
                playerSkills.currentSkills[i] = null;
                break;
            }
        }
        playerSkills.currentSkills[index] = currentSkill as ActiveSkill;
        skillUI.RefreshSkillSlots();
        Refresh();
    }
}
