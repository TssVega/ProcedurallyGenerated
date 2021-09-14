﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillInfoPanel : MonoBehaviour {

    public Image[] skillSlots;
    public Image[] secondaryImages;
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

    public TextMeshProUGUI[] quantityTexts;

    private void Awake() {
        playerStats = FindObjectOfType<Player>().GetComponent<Stats>();
        localizationManager = FindObjectOfType<LocalizationManager>();
        playerSkills = FindObjectOfType<Player>().GetComponent<SkillUser>();
        skillUI = FindObjectOfType<SkillUI>();
    }
    private void OnEnable() {
        //RefreshText();
    }
    public void SetSkill(Skill skill) {
        currentSkill = skill;
        Refresh();
        RefreshText();
    }
    public void LearnSkill() {
        if(playerSkills.acquiredSkills.Contains(currentSkill)) {
            return;
        }
        if(playerStats.statPoints >= currentSkill.skillPointsNeeded) {
            playerStats.statPoints -= currentSkill.skillPointsNeeded;
            playerSkills.acquiredSkills.Add(currentSkill);
            Refresh();
            RefreshText();
            skillTree.UpdateStatPoints();
            skillTree.RefreshAcquiredStatus();
        }        
    }
    // Refresh all the skill slots
    private void Refresh() {
        // Activate the learn button if the skill is not yet learned
        if(playerSkills.acquiredSkills.Contains(currentSkill)) {
            learnButton.SetActive(false);
        }
        else {
            learnButton.SetActive(true);
        }
        // Set skill slots sprites
        if(playerSkills.acquiredSkills.Contains(currentSkill) && currentSkill is ActiveSkill) {
            for(int i = 0; i < skillSlotCount; i++) {
                skillSlots[i].gameObject.SetActive(true);
                if(i >= playerSkills.currentSkills.Length) {
                    skillSlots[i].sprite = emptySlotSprite;
                    skillSlots[i].color = Color.white;
                    secondaryImages[i].sprite = null;
                    secondaryImages[i].color = Color.clear;
                    quantityTexts[i].text = "";
                    continue;
                }
                ActiveSkill a = playerSkills.currentSkills[i] as ActiveSkill;
                if(a && a.skillIcon) {
                    skillSlots[i].sprite = a.skillIcon;
                    skillSlots[i].color = ColorBySkillType.GetColorByType(a.attackType);
                    secondaryImages[i].sprite = null;
                    secondaryImages[i].color = Color.clear;
                    quantityTexts[i].text = "";
                }
                else if(playerSkills.currentSkills[i] is Mushroom m) {
                    skillSlots[i].sprite = m.firstIcon;
                    skillSlots[i].color = m.firstColor;
                    secondaryImages[i].sprite = null;
                    secondaryImages[i].color = Color.white;
                }
                /* Potions here
                else if(playerSkills.currentSkills[i] is Mushroom m) {
                    skillSlots[i].sprite = m.firstIcon;
                    skillSlots[i].color = m.firstColor;
                    secondaryImages[i].sprite = null;
                    secondaryImages[i].color = Color.white;
                }*/
                else {
                    skillSlots[i].sprite = emptySlotSprite;
                    skillSlots[i].color = Color.white;
                    secondaryImages[i].sprite = null;
                    secondaryImages[i].color = Color.clear;
                    quantityTexts[i].text = "";
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
            if(playerSkills.currentSkills.Length <= i) {
                continue;
            }
            ActiveSkill a = playerSkills.currentSkills[i] as ActiveSkill;
            if(a == currentSkill) {
                playerSkills.currentSkills[i] = null;
                break;
            }
        }
        playerSkills.currentSkills[index] = currentSkill as ActiveSkill;
        skillUI.RefreshSkillSlots();
        Refresh();
    }
    private void RefreshText() {        
        acquireText.text = localizationManager.GetText("learn");
        if(playerSkills.acquiredSkills.Contains(currentSkill) && currentSkill is ActiveSkill) {
            guideText.text = localizationManager.GetText("skillInfoNotif");
        }
        else if(playerSkills.acquiredSkills.Contains(currentSkill) && currentSkill is PassiveSkill) {
            guideText.text = localizationManager.GetText("passiveNotif");
        }
        else {
            guideText.text = "";
        }
    }
}
