using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour {

    private SkillUser playerSkills;
    public Image[] skillButtons;
    public Image[] secondImages;
    private Image[] skillMasks;
    private readonly int maxActiveSkills = 11;
    private bool[] pressing;

    private float potionCooldown = 0f;

    private Inventory inventory;

    private void Awake() {
        inventory = FindObjectOfType<Inventory>();
        pressing = new bool[maxActiveSkills];
        playerSkills = FindObjectOfType<Player>().GetComponent<SkillUser>();
        skillMasks = new Image[skillButtons.Length];
        for(int i = 0; i < skillButtons.Length; i++) {
            skillMasks[i] = skillButtons[i].transform.GetChild(0).GetComponent<Image>();
        }
    }
    private void Start() {
        RefreshSkillSlots();
    }
    private void Update() {
        UseSkill();
        Countdown();
        SkillCooldownMask();
    }
    private void Countdown() {
        if(potionCooldown > 0f) {
            potionCooldown -= Time.deltaTime;
        }
    }
    public void RefreshSkillSlots() {
        for(int i = 0; i < maxActiveSkills; i++) {
            if(i >= playerSkills.currentSkills.Length) {
                skillButtons[i].sprite = null;
                skillButtons[i].color = Color.clear;
                skillMasks[i].fillAmount = 0f;
                secondImages[i].sprite = null;
                secondImages[i].color = Color.clear;
                continue;
            }
            if(playerSkills.currentSkills[i] != null) {
                if(playerSkills.currentSkills[i] is ActiveSkill a) {
                    skillButtons[i].sprite = a.skillIcon;
                    skillButtons[i].color = ColorBySkillType.GetColorByType(a.attackType);
                    skillMasks[i].fillAmount = playerSkills.skillCooldowns[a.skillIndex]
                        / a.cooldown;
                    secondImages[i].sprite = null;
                    secondImages[i].color = Color.clear;
                }
                // Else set the icons of the potion or mushroom
                if(playerSkills.currentSkills[i] is Mushroom m) {
                    skillButtons[i].sprite = m.firstIcon;
                    skillButtons[i].color = m.firstColor;
                    skillMasks[i].fillAmount = potionCooldown / 1f;
                    secondImages[i].sprite = null;
                    secondImages[i].color = Color.clear;
                }
            }
            else {
                skillButtons[i].sprite = null;
                skillButtons[i].color = Color.clear;
                skillMasks[i].fillAmount = 0f;
                secondImages[i].sprite = null;
                secondImages[i].color = Color.clear;
            }
        }
    }    
    public void UseSkill() {
        for(int i = 0; i < maxActiveSkills; i++) {
            if(pressing[i] && playerSkills.currentSkills[i] != null) {
                if(playerSkills.currentSkills[i] is ActiveSkill) {
                    playerSkills.UseSkill(playerSkills.currentSkills[i]);
                }
                else if(potionCooldown <= 0f) {                    
                    if(inventory.CanConsumeItem(playerSkills.currentSkills[i])) {
                        potionCooldown = 1f;
                        inventory.ConsumeItem(playerSkills.currentSkills[i]);
                    }                    
                }
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
            if(i >= playerSkills.currentSkills.Length) {
                skillButtons[i].sprite = null;
                skillButtons[i].color = Color.clear;
                skillMasks[i].fillAmount = 0f;
                secondImages[i].sprite = null;
                secondImages[i].color = Color.clear;
                continue;
            }
            if(playerSkills.currentSkills[i] != null) {
                if(playerSkills.currentSkills[i] is ActiveSkill a) {
                    skillButtons[i].sprite = a.skillIcon;
                    skillButtons[i].color = ColorBySkillType.GetColorByType(a.attackType);
                    skillMasks[i].fillAmount = playerSkills.skillCooldowns[a.skillIndex]
                        / a.cooldown;
                    secondImages[i].sprite = null;
                    secondImages[i].color = Color.clear;
                }
                // Else set the icons of the potion or mushroom
                if(playerSkills.currentSkills[i] is Mushroom m) {
                    skillButtons[i].sprite = m.firstIcon;
                    skillButtons[i].color = m.firstColor;
                    skillMasks[i].fillAmount = potionCooldown / 1f;
                    secondImages[i].sprite = null;
                    secondImages[i].color = Color.clear;
                }
            }
            else {
                skillButtons[i].sprite = null;
                skillButtons[i].color = Color.clear;
                skillMasks[i].fillAmount = 0f;
                secondImages[i].sprite = null;
                secondImages[i].color = Color.clear;
            }
        }
    }
}
