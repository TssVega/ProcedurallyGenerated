using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillInfoPanel : MonoBehaviour {

    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI damageTypeText;
    public Image damageTypeIcon;
    public TextMeshProUGUI damageType;
    public TextMeshProUGUI description;
    public TextMeshProUGUI skillPointsNeededText;

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

    public SkillDatabase skillDatabase;

    public Sprite[] damageIcons;

    private void Awake() {
        playerStats = FindObjectOfType<Player>().GetComponent<Stats>();
        localizationManager = FindObjectOfType<LocalizationManager>();
        playerSkills = FindObjectOfType<Player>().GetComponent<SkillUser>();
        skillUI = FindObjectOfType<SkillUI>();
    }
    public void SetSkill(Skill skill) {
        currentSkill = skill;
        Refresh();
        RefreshText();
        skillTree.UpdateStatPoints();
        skillTree.RefreshAcquiredStatus();
    }
    public void LearnSkill() {
        if(playerSkills.acquiredSkills.Contains(currentSkill)) {
            return;
        }        
        if(!CheckPrerequisites()) {
            return;
        }
        if(playerStats.statPoints >= currentSkill.skillPointsNeeded) {
            playerStats.statPoints -= currentSkill.skillPointsNeeded;
            playerSkills.acquiredSkills.Add(currentSkill);
            Refresh();
            RefreshText();
            skillTree.UpdateStatPoints();
            skillTree.RefreshAcquiredStatus();
            AudioSystem.audioManager.PlaySound("skillUnlocked", 0f);
        }
        else {
            AudioSystem.audioManager.PlaySound("inGameButton", 0f);
        }
    }
    private bool CheckPrerequisites() {
        bool allPrerequisitesMet = true;
        for(int i = 0; i < currentSkill.prerequisites.Length; i++) {
            if(currentSkill.prerequisites[i] < 0) {
                return true;
            }
            if(!playerSkills.acquiredSkills.Contains(skillDatabase.skills[currentSkill.prerequisites[i]])) {
                allPrerequisitesMet = false;
            }
        }
        return allPrerequisitesMet;
    }
    // Refresh all the skill slots
    private void Refresh() {
        // Activate the learn button if the skill is not yet learned
        if(playerSkills.acquiredSkills.Contains(currentSkill)) {
            learnButton.SetActive(false);
        }
        else if(!CheckPrerequisites()) {
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
                    //quantityTexts[i].text = "";
                    continue;
                }
                ActiveSkill a = playerSkills.currentSkills[i] as ActiveSkill;
                if(a && a.skillIcon) {
                    skillSlots[i].sprite = a.skillIcon;
                    skillSlots[i].color = ColorBySkillType.GetColorByType(a.attackType);
                    secondaryImages[i].sprite = null;
                    secondaryImages[i].color = Color.clear;
                    //quantityTexts[i].text = "";
                }
                else if(playerSkills.currentSkills[i] is Mushroom m) {
                    skillSlots[i].sprite = m.firstIcon;
                    skillSlots[i].color = m.firstColor;
                    secondaryImages[i].sprite = null;
                    secondaryImages[i].color = Color.clear;
                    //quantityTexts[i].text = skillUI.sk;
                }
                // Potions here
                else if(playerSkills.currentSkills[i] is Potion p) {
                    skillSlots[i].sprite = p.firstIcon;
                    skillSlots[i].color = p.firstColor;
                    secondaryImages[i].sprite = p.secondIcon;
                    secondaryImages[i].color = p.secondColor;
                    //quantityTexts[i].text = "";
                }
                else {
                    skillSlots[i].sprite = emptySlotSprite;
                    skillSlots[i].color = Color.white;
                    secondaryImages[i].sprite = null;
                    secondaryImages[i].color = Color.clear;
                    //quantityTexts[i].text = "";
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
        AudioSystem.audioManager.PlaySound("inGameButton", 0f);
    }
    private void RefreshText() {
        skillNameText.text = localizationManager.GetText(currentSkill.skillName);
        damageTypeText.text = localizationManager.GetText("damageType");
        if(currentSkill.attackType != AttackType.None) {
            damageTypeIcon.sprite = damageIcons[(int)currentSkill.attackType];
            damageTypeIcon.color = Color.white;
        }
        else {
            damageTypeIcon.sprite = null;
            damageTypeIcon.color = Color.clear;
        }
        damageType.text = localizationManager.GetText(currentSkill.attackType.ToString());
        description.text = localizationManager.GetText($"{currentSkill.skillName}Desc");
        if(playerSkills.acquiredSkills.Contains(currentSkill)) {
            skillPointsNeededText.text = "";
        }
        else {
            skillPointsNeededText.text = $"{localizationManager.GetText("skillPointsNeeded")}: {currentSkill.skillPointsNeeded}";
        }        
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
