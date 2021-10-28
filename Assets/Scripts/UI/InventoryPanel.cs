using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour {

    private Inventory inventory;
    private Player player;
    private Stats playerStats;

    private bool imagesSet = false;
    private bool firstTime = true;

    public Image characterImage;

    public TextMeshProUGUI inventoryText;
    public TextMeshProUGUI characterText;
    public TextMeshProUGUI craftingText;
    public TextMeshProUGUI skillsText;
    public TextMeshProUGUI mapText;

    [Header("Main Stats")]
    public TextMeshProUGUI mainStatsText;
    public TextMeshProUGUI thresholdsText;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI agilityText;
    public TextMeshProUGUI dexterityText;
    public TextMeshProUGUI intelligenceText;
    public TextMeshProUGUI faithText;
    public TextMeshProUGUI wisdomText;
    public TextMeshProUGUI vitalityText;
    public TextMeshProUGUI charismaText;
    [Header("Main Stat Values")]
    public TextMeshProUGUI strengthValueText;
    public TextMeshProUGUI agilityValueText;
    public TextMeshProUGUI dexterityValueText;
    public TextMeshProUGUI intelligenceValueText;
    public TextMeshProUGUI faithValueText;
    public TextMeshProUGUI wisdomValueText;
    public TextMeshProUGUI vitalityValueText;
    public TextMeshProUGUI charismaValueText;
    [Header("Thresholds")]
    public TextMeshProUGUI burningThresholdText;
    public TextMeshProUGUI rootThresholdText;
    public TextMeshProUGUI frostbiteThresholdText;
    public TextMeshProUGUI shockThresholdText;
    public TextMeshProUGUI litThresholdText;
    public TextMeshProUGUI poisoningThresholdText;
    public TextMeshProUGUI bleedingThresholdText;
    public TextMeshProUGUI curseThresholdText;
    [Header("Threshold Values")]
    public TextMeshProUGUI burningThresholdValueText;
    public TextMeshProUGUI rootThresholdValueText;
    public TextMeshProUGUI frostbiteThresholdValueText;
    public TextMeshProUGUI shockThresholdValueText;
    public TextMeshProUGUI litThresholdValueText;
    public TextMeshProUGUI poisoningThresholdValueText;
    public TextMeshProUGUI bleedingThresholdValueText;
    public TextMeshProUGUI curseThresholdValueText;
    [Header("Damage Stats")]
    public TextMeshProUGUI fireText;
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI airText;
    public TextMeshProUGUI earthText;
    public TextMeshProUGUI lightningText;
    public TextMeshProUGUI lightText;
    public TextMeshProUGUI darkText;
    public TextMeshProUGUI bashText;
    public TextMeshProUGUI pierceText;
    public TextMeshProUGUI slashText;
    public TextMeshProUGUI bleedText;
    public TextMeshProUGUI poisonText;
    public TextMeshProUGUI curseText;
    [Header("Race")]
    public TextMeshProUGUI raceText;
    public TextMeshProUGUI raceValueText;
    public TextMeshProUGUI raceDescText;
    [Header("Crafting")]
    public ItemImages[] inputImages;

    public ItemImages outputImages;

    /*
public int burningThreshold;
public int earthingThreshold;
public int frostbiteThreshold;
public int shockThreshold;
public int lightingThreshold;
public int poisonThreshold;
public int bleedThreshold;
public int curseThreshold;

    */
    [Header("Other")]
    public GameObject skillBook;
    public GameObject map;

    private LocalizationManager loc;

    private void Awake() {
        player = FindObjectOfType<Player>();
        playerStats = player.GetComponent<Stats>();
        inventory = player.GetComponent<Inventory>();
        inventory.SetInventory();
        loc = LocalizationManager.localization;
    }    
    private void OnEnable() {
        characterImage.color = player.appearance.races[player.raceIndex].skinColor;
        if(firstTime) {
            firstTime = false;
            return;
        }
        if(!imagesSet) {
            imagesSet = true;
            inventory.SetInventoryImages();
        }
        CheckButtons();
        inventory.UpdateStats();
        UpdateTexts(playerStats);
    }
    public void PutItem(Item item, int index) {
        inputImages[index].firstImage.sprite = item.firstIcon;
        inputImages[index].secondImage.sprite = item.secondIcon;
        inputImages[index].thirdImage.sprite = item.thirdIcon;
        inputImages[index].firstImage.color = item.firstColor;
        inputImages[index].secondImage.color = item.secondColor;
        inputImages[index].thirdImage.color = item.thirdColor;
    }
    public void ClearItem(int index) {
        inputImages[index].firstImage.sprite = null;
        inputImages[index].secondImage.sprite = null;
        inputImages[index].thirdImage.sprite = null;
        inputImages[index].firstImage.color = Color.clear;
        inputImages[index].secondImage.color = Color.clear;
        inputImages[index].thirdImage.color = Color.clear;
    }
    public void SetCraft(Item item) {
        outputImages.firstImage.sprite = item.firstIcon;
        outputImages.secondImage.sprite = item.secondIcon;
        outputImages.thirdImage.sprite = item.thirdIcon;
        outputImages.firstImage.color = item.firstColor;
        outputImages.secondImage.color = item.secondColor;
        outputImages.thirdImage.color = item.thirdColor;
    }
    public void ClearCraft() {
        outputImages.firstImage.sprite = null;
        outputImages.secondImage.sprite = null;
        outputImages.thirdImage.sprite = null;
        outputImages.firstImage.color = Color.clear;
        outputImages.secondImage.color = Color.clear;
        outputImages.thirdImage.color = Color.clear;
    }
    public void UpdateTexts(Stats stats) {
        mainStatsText.text = loc.GetText("mainStats");
        thresholdsText.text = loc.GetText("thresholds");
        strengthText.text = loc.GetText("strength");
        agilityText.text = loc.GetText("agility");
        dexterityText.text = loc.GetText("dexterity");
        intelligenceText.text = loc.GetText("intelligence");
        faithText.text = loc.GetText("faith");
        wisdomText.text = loc.GetText("wisdom");
        vitalityText.text = loc.GetText("vitality");
        charismaText.text = loc.GetText("charisma");
        strengthValueText.text = stats.strength.ToString();
        agilityValueText.text = stats.agility.ToString();
        dexterityValueText.text = stats.dexterity.ToString();
        intelligenceValueText.text = stats.intelligence.ToString();
        faithValueText.text = stats.faith.ToString();
        wisdomValueText.text = stats.wisdom.ToString();
        vitalityValueText.text = stats.vitality.ToString();
        charismaValueText.text = stats.charisma.ToString();
        burningThresholdText.text = loc.GetText("burningThreshold");
        rootThresholdText.text = loc.GetText("rootThreshold");
        frostbiteThresholdText.text = loc.GetText("frostbiteThreshold");
        shockThresholdText.text = loc.GetText("shockThreshold");
        litThresholdText.text = loc.GetText("litThreshold");
        poisoningThresholdText.text = loc.GetText("poisonThreshold");
        bleedingThresholdText.text = loc.GetText("bleedThreshold");
        curseThresholdText.text = loc.GetText("curseThreshold");
        burningThresholdValueText.text = stats.burningThreshold.ToString();
        rootThresholdValueText.text = stats.earthingThreshold.ToString();
        frostbiteThresholdValueText.text = stats.frostbiteThreshold.ToString();
        shockThresholdValueText.text = stats.shockThreshold.ToString();
        litThresholdValueText.text = stats.lightingThreshold.ToString();
        poisoningThresholdValueText.text = stats.poisonThreshold.ToString();
        bleedingThresholdValueText.text = stats.bleedThreshold.ToString();
        curseThresholdValueText.text = stats.curseThreshold.ToString();
        fireText.text = loc.GetText("Fire");
        waterText.text = loc.GetText("Ice");
        airText.text = loc.GetText("Air");
        earthText.text = loc.GetText("Earth");
        lightningText.text = loc.GetText("Lightning");
        lightText.text = loc.GetText("Light");
        darkText.text = loc.GetText("Dark");
        bashText.text = loc.GetText("Bash");
        pierceText.text = loc.GetText("Pierce");
        slashText.text = loc.GetText("Slash");
        bleedText.text = loc.GetText("Bleed");
        poisonText.text = loc.GetText("Poison");
        curseText.text = loc.GetText("Curse");
        raceText.text = loc.GetText("race");
        raceValueText.text = $"{player.appearance.races[player.raceIndex].raceName}:";
        raceDescText.text = loc.GetText($"{player.appearance.races[player.raceIndex].raceName}Desc");
    }
    public void CheckButtons() {
        inventoryText.text = LocalizationManager.localization.GetText("inventory");
        characterText.text = LocalizationManager.localization.GetText("character");
        skillBook.SetActive(player.skillBookUnlocked);
        skillsText.gameObject.SetActive(player.skillBookUnlocked);
        skillsText.text = LocalizationManager.localization.GetText("skills");
        map.SetActive(player.mapUnlocked);
        mapText.gameObject.SetActive(player.mapUnlocked);
        mapText.text = LocalizationManager.localization.GetText("map");
        craftingText.text = LocalizationManager.localization.GetText("crafting");
    }
    public void SaveButton() {
        player.SavePlayer();
    }
}
