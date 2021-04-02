using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterCreation : MonoBehaviour {

    public Image skinImage;
    public Image hairImage;

    public CharacterAppearance appearance;

    private Color currentSkinColor;
    private Color currentHairColor;
    private Sprite currentHairStyle;

    private int currentSkinColorIndex = 0;
    private int currentHairColorIndex = 0;
    private int currentHairStyleIndex = 0;

    private Player player;

    private readonly int worldSize = 100;

    private void Awake() {
        player = FindObjectOfType<Player>();
        currentSkinColor = appearance.skinColors[currentSkinColorIndex];
        currentHairColor = appearance.hairColors[currentHairColorIndex];
        currentHairStyle = appearance.hairStyles[currentHairStyleIndex];
        skinImage.color = currentSkinColor;
        hairImage.color = currentHairColor;
        hairImage.sprite = currentHairStyle;
    }
    public void CompleteCharacterCreation() {
        SetNewGameData();
        player.SavePlayer();
        LoadSceneAsync("Levels");
    }
    private void LoadSceneAsync(string sceneName) {
        SceneManager.LoadSceneAsync(sceneName);
    }
    public void ChangeSkinColor(bool increment) {
        if(increment) {
            currentSkinColorIndex++;
            if(currentSkinColorIndex > appearance.skinColors.Length - 1) {
                currentSkinColorIndex = 0;
            }
            currentSkinColor = appearance.skinColors[currentSkinColorIndex];
        }
        else {
            currentSkinColorIndex--;
            if(currentSkinColorIndex < 0) {
                currentSkinColorIndex = appearance.skinColors.Length - 1;
            }
            currentSkinColor = appearance.skinColors[currentSkinColorIndex];
        }
        skinImage.color = currentSkinColor;
        player.skinColorIndex = currentSkinColorIndex;
    }
    public void ChangeHairColor(bool increment) {
        if(increment) {
            currentHairColorIndex++;
            if(currentHairColorIndex > appearance.hairColors.Length - 1) {
                currentHairColorIndex = 0;
            }
            currentHairColor = appearance.hairColors[currentHairColorIndex];
        }
        else {
            currentHairColorIndex--;
            if(currentHairColorIndex < 0) {
                currentHairColorIndex = appearance.hairColors.Length - 1;
            }
            currentHairColor = appearance.hairColors[currentHairColorIndex];
        }
        hairImage.color = currentHairColor;
        player.hairColorIndex = currentHairColorIndex;
    }
    public void ChangeHairStyle(bool increment) {
        if(increment) {
            currentHairStyleIndex++;
            if(currentHairStyleIndex > appearance.hairStyles.Length - 1) {
                currentHairStyleIndex = 0;
            }
            currentHairStyle = appearance.hairStyles[currentHairStyleIndex];
        }
        else {
            currentHairStyleIndex--;
            if(currentHairStyleIndex < 0) {
                currentHairStyleIndex = appearance.hairStyles.Length - 1;
            }
            currentHairStyle = appearance.hairStyles[currentHairStyleIndex];
        }
        hairImage.sprite = currentHairStyle;
        player.hairStyleIndex = currentHairStyleIndex;
    }
    private void SetNewGameData() {
        // World data
        WorldData world = new WorldData(new string[worldSize, worldSize], new int[] { 0, 0 });
        SaveSystem.SaveWorld(world, PersistentData.saveSlot);
        // Appearance
        player.skinColor.color = currentSkinColor;
        player.hairColor.color = currentHairColor;
        player.hairStyle.sprite = currentHairStyle;
        player.skinColorIndex = currentSkinColorIndex;
        player.hairColorIndex = currentHairColorIndex;
        player.hairStyleIndex = currentHairStyleIndex;
        // Inventory and equipment
        for(int i = 0; i < player.inventory.InventorySize; i++) {
            player.inventory.inventory[i] = null;
        }
        for(int i = 0; i < player.inventory.EquipmentSize; i++) {
            player.inventory.equipment[i] = null;
        }
        // Transform
        player.transform.position = new Vector3(5, 0, 0);
        player.transform.rotation = Quaternion.identity;
        // Status
        player.stats.health = 100f;
        player.stats.mana = 100f;
        player.stats.energy = 100f;
        player.stats.maxHealth = 100f;
        player.stats.maxMana = 100f;
        player.stats.maxEnergy = 100f;
        // Main
        player.stats.strength = 10;
        player.stats.agility = 10;
        player.stats.dexterity = 10;
        player.stats.intelligence = 10;
        player.stats.faith = 10;
        player.stats.wisdom = 10;
        player.stats.vitality = 10;
        player.stats.charisma = 10;
        // Damages
        player.stats.bashDamage = 10f;
        player.stats.pierceDamage = 10f;
        player.stats.slashDamage = 10f;
        player.stats.fireDamage = 10f;
        player.stats.iceDamage = 10f;
        player.stats.lightningDamage = 10f;
        player.stats.airDamage = 10f;
        player.stats.earthDamage = 10f;
        player.stats.lightDamage = 10f;
        player.stats.darkDamage = 10f;
        player.stats.poisonDamage = 10f;
        player.stats.bleedDamage = 10f;
        player.stats.curseDamage = 10f;
        // Defences
        player.stats.bashDefence = 10f;
        player.stats.pierceDefence = 10f;
        player.stats.slashDefence = 10f;
        player.stats.fireDefence = 10f;
        player.stats.iceDefence = 10f;
        player.stats.lightningDefence = 10f;
        player.stats.airDefence = 10f;
        player.stats.earthDefence = 10f;
        player.stats.lightDefence = 10f;
        player.stats.darkDefence = 10f;
        player.stats.poisonDefence = 10f;
        player.stats.bleedDefence = 10f;
        player.stats.curseDefence = 10f;
    }
}
