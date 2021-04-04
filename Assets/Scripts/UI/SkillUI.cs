using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour {

    private SkillUser playerSkills;
    public Image[] skillButtons;
    private Image[] skillMasks;
    private int maxActiveSkills = 11;
    private bool[] pressing;

    private void Awake() {
        pressing = new bool[maxActiveSkills];
        playerSkills = FindObjectOfType<Player>().GetComponent<SkillUser>();
        skillMasks = new Image[skillButtons.Length];
        for(int i = 0; i < skillButtons.Length; i++) {
            skillMasks[i] = skillButtons[i].transform.GetChild(0).GetComponent<Image>();
        }
    }
    private void Start() {        
        for(int i = 0; i < maxActiveSkills; i++) {
            if(i >= playerSkills.currentSkills.Count) {
                skillButtons[i].sprite = null;
                skillButtons[i].color = Color.clear;
                skillMasks[i].fillAmount = 0f;
                continue;
            }
            if(playerSkills.currentSkills[i]) {
                skillButtons[i].sprite = playerSkills.currentSkills[i].skillIcon;
                skillButtons[i].color = ColorBySkillType.GetColorByType(playerSkills.currentSkills[i].attackType);
                skillMasks[i].fillAmount = playerSkills.skillCooldowns[playerSkills.currentSkills[i].skillIndex]
                    / playerSkills.currentSkills[i].cooldown;
            }
            else {
                skillButtons[i].sprite = null;
                skillButtons[i].color = Color.clear;
                skillMasks[i].fillAmount = 0f;
            }
        }
    }
    private void Update() {
        UseSkill();
        SkillCooldownMask();
    }
    public void UseSkill() {
        for(int i = 0; i < maxActiveSkills; i++) {
            if(pressing[i]) {
                playerSkills.UseSkill(playerSkills.currentSkills[i]);
            }
        }
    }
    public void ButtonDown(int index) {
        pressing[index] = true;
    }
    public void ButtonUp(int index) {
        pressing[index] = false;
    }
    private void SkillCooldownMask() {
        for(int i = 0; i < maxActiveSkills; i++) {
            if(i >= playerSkills.currentSkills.Count) {
                skillButtons[i].sprite = null;
                skillButtons[i].color = Color.clear;
                skillMasks[i].fillAmount = 0f;
                continue;
            }
            if(playerSkills.currentSkills[i]) {
                skillButtons[i].sprite = playerSkills.currentSkills[i].skillIcon;
                skillButtons[i].color = ColorBySkillType.GetColorByType(playerSkills.currentSkills[i].attackType);
                skillMasks[i].fillAmount = playerSkills.skillCooldowns[playerSkills.currentSkills[i].skillIndex]
                    / playerSkills.currentSkills[i].cooldown;
            }
            else {
                skillButtons[i].sprite = null;
                skillButtons[i].color = Color.clear;
                skillMasks[i].fillAmount = 0f;
            }
        }
    }
}
