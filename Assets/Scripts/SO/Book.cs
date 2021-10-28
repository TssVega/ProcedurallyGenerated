using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Book")]
public class Book : Item, IUsable {

    public BookType bookType;
    public SkillDatabase skillDatabase;

    public void Consume(StatusEffects status) {
        switch(bookType) {
            case BookType.Strength:
                status.stats.strength++;
                AudioSystem.audioManager.PlaySound("mainStatGained", 0f);
                break;
            case BookType.Agility:
                status.stats.agility++;
                AudioSystem.audioManager.PlaySound("mainStatGained", 0f);
                break;
            case BookType.Dexterity:
                status.stats.dexterity++;
                AudioSystem.audioManager.PlaySound("mainStatGained", 0f);
                break;
            case BookType.Intelligence:
                status.stats.intelligence++;
                AudioSystem.audioManager.PlaySound("mainStatGained", 0f);
                break;
            case BookType.Faith:
                status.stats.faith++;
                AudioSystem.audioManager.PlaySound("mainStatGained", 0f);
                break;
            case BookType.Wisdom:
                status.stats.wisdom++;
                AudioSystem.audioManager.PlaySound("mainStatGained", 0f);
                break;
            case BookType.Vitality:
                status.stats.vitality++;
                AudioSystem.audioManager.PlaySound("mainStatGained", 0f);
                break;
            case BookType.Charisma:
                status.stats.charisma++;
                AudioSystem.audioManager.PlaySound("mainStatGained", 0f);
                break;
            case BookType.Flame:
                status.stats.fireDamage += 2f;
                break;
            case BookType.Fire:
                status.stats.fireDefence += 2f;
                break;
            case BookType.Aqua:
                status.stats.iceDamage += 2f;
                break;
            case BookType.Sea:
                status.stats.iceDefence += 2f;
                break;
            case BookType.Wind:
                status.stats.airDamage += 2f;
                break;
            case BookType.Air:
                status.stats.airDefence += 2f;
                break;
            case BookType.Druidry:
                status.stats.earthDamage += 2f;
                break;
            case BookType.Earth:
                status.stats.earthDefence += 2f;
                break;
            case BookType.Shock:
                status.stats.lightningDamage += 2f;
                break;
            case BookType.Thunder:
                status.stats.lightningDefence += 2f;
                break;
            case BookType.Sheen:
                status.stats.lightDamage += 2f;
                break;
            case BookType.Light:
                status.stats.lightDefence += 2f;
                break;
            case BookType.Dark:
                status.stats.darkDamage += 2f;
                break;
            case BookType.Void:
                status.stats.darkDefence += 2f;
                break;
            case BookType.Cunning:
                status.stats.poisonDamage += 2f;
                break;
            case BookType.Venom:
                status.stats.poisonDefence += 2f;
                break;
            case BookType.Brutality:
                status.stats.bleedDamage += 2f;
                break;
            case BookType.Blood:
                status.stats.bleedDefence += 2f;
                break;
            case BookType.Spirit:
                status.stats.curseDamage += 2f;
                break;
            case BookType.Souls:
                status.stats.curseDefence += 2f;
                break;
            case BookType.Grounding:
                status.stats.shockThreshold += 10;
                break;
            case BookType.Night:
                status.stats.lightingThreshold += 10;
                break;
            case BookType.Nature:
                status.stats.earthingThreshold += 10;
                break;
            case BookType.Fireproofing:
                status.stats.burningThreshold += 10;
                break;
            case BookType.Warmth:
                status.stats.frostbiteThreshold += 10;
                break;
            case BookType.Snake:
                status.stats.poisonThreshold += 10;
                break;
            case BookType.Vigor:
                status.stats.bleedThreshold += 10;
                break;
            case BookType.Bless:
                status.stats.curseThreshold += 10;
                break;
            case BookType.Luck:
                status.stats.luck++;
                break;
            case BookType.Skill:
                status.stats.statPoints++;
                break;
            case BookType.Thrust:
                status.stats.pierceDamage += 2;
                break;
            case BookType.Toughness:
                status.stats.pierceDefence += 2;
                break;
            case BookType.Bash:
                status.stats.bashDamage += 2;
                break;
            case BookType.Elasticity:
                status.stats.bashDefence += 2;
                break;
            case BookType.Slash:
                status.stats.slashDamage += 2;
                break;
            case BookType.Muscle:
                status.stats.slashDefence += 2;
                break;
            case BookType.Pyromancy:
                UnlockNewSkill(AttackType.Fire, status);
                break;
            case BookType.Hydromancy:
                UnlockNewSkill(AttackType.Ice, status);
                break;
            case BookType.Aerothurgy:
                UnlockNewSkill(AttackType.Air, status);
                break;
            case BookType.Geomancy:
                UnlockNewSkill(AttackType.Earth, status);
                break;
            case BookType.Electromancy:
                UnlockNewSkill(AttackType.Lightning, status);
                break;
            case BookType.Photomancy:
                UnlockNewSkill(AttackType.Light, status);
                break;
            case BookType.Necromancy:
                UnlockNewSkill(AttackType.Dark, status);
                break;
            default:
                break;
        }
        AudioSystem.audioManager.PlaySound("bookRead", 0f);
    }
    private void UnlockNewSkill(AttackType attackType, StatusEffects status) {
        for(int i = 0; i < skillDatabase.skills.Count; i++) {
            if(skillDatabase.skills[i].attackType == attackType) {
                status.skillUser.acquiredSkills.Add(skillDatabase.skills[i]);
                if(status.player.npcBonuses[26]) {
                    int randomIndex = Random.Range(0, 9);
                    switch(randomIndex) {
                        case 0:
                            status.stats.strength++;
                            break;
                        case 1:
                            status.stats.agility++;
                            break;
                        case 2:
                            status.stats.dexterity++;
                            break;
                        case 3:
                            status.stats.intelligence++;
                            break;
                        case 4:
                            status.stats.faith++;
                            break;
                        case 5:
                            status.stats.wisdom++;
                            break;
                        case 6:
                            status.stats.vitality++;
                            break;
                        case 7:
                            status.stats.charisma++;
                            break;
                    }
                    status.player.inventory.CheckCrafting();
                    status.player.UpdateMaxHealth();
                    AudioSystem.audioManager.PlaySound("mainStatGained", 0f);
                }
                AudioSystem.audioManager.PlaySound("skillUnlocked", 0f);
                return;
            }
        }
    }
}

public enum BookType {
    Strength, Agility, Dexterity, Intelligence, Faith, Wisdom, Vitality, Charisma, Flame, Fire, Aqua, Sea,
    Wind, Air, Druidry, Earth, Shock, Thunder, Sheen, Light, Dark, Void, Cunning, Venom, Brutality, Blood, Spirit,
    Souls, Grounding, Night, Nature, Fireproofing, Warmth, Snake, Vigor, Bless, Luck, Skill, Thrust, Toughness, Bash, Elasticity,
    Slash, Muscle, Pyromancy, Hydromancy, Aerothurgy, Geomancy, Electromancy, Photomancy, Necromancy
}
