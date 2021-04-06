using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillInfoPanel : MonoBehaviour {

    public Image[] skillSlots;
    public Sprite emptySlotSprite;
    public TextMeshProUGUI guideText;
    public TextMeshProUGUI acquireText;
    public GameObject learnButton;

    private SkillUser playerSkills;
    private readonly int skillSlotCount = 11;
    private Skill currentSkill;
    private SkillUI skillUI;
    private LocalizationManager localizationManager;
    private Stats playerStats;
    public SkillTreeManager skillTree;

    private void Awake() {
        playerStats = FindObjectOfType<Player>().GetComponent<Stats>();
        localizationManager = FindObjectOfType<LocalizationManager>();
        playerSkills = FindObjectOfType<Player>().GetComponent<SkillUser>();
        skillUI = FindObjectOfType<SkillUI>();
    }
    private void Start() {
        RefreshText();
    }
    public void SetSkill(Skill skill) {
        currentSkill = skill;
        Refresh();
    }
    public void LearnSkill() {
        if(playerStats.statPoints >= currentSkill.skillPointsNeeded) {
            playerStats.statPoints -= currentSkill.skillPointsNeeded;
            playerSkills.acquiredSkills.Add(currentSkill);
            Refresh();
            skillTree.UpdateStatPoints();
        }        
    }
    private void Refresh() {
        if(playerSkills.acquiredSkills.Contains(currentSkill)) {
            learnButton.SetActive(false);
        }
        else {
            learnButton.SetActive(true);
        }
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
    private void RefreshText() {
        guideText.text = localizationManager.GetText("skillInfoNotif");
        acquireText.text = localizationManager.GetText("learn");
    }
}
