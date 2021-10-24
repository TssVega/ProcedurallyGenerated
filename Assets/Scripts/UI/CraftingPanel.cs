using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingPanel : MonoBehaviour {

    public GameObject[] craftingGrids;

    public Image[] tabs;

    public Color activeTabPanelColor;
    public Color inactiveTabPanelColor;

    public CraftingDatabase craftingDatabase;

    public ItemImages[] inputImages;
    public ItemImages[] outputImages;

    public TextMeshProUGUI[] unlockTexts;

    private Inventory inventory;

    private int currentTabIndex;
    private int currentPageIndex;

    private const int recipeCountPerPage = 4;
    private const int gridCount = 9;
    private const int tabCount = 9;

    private int[] pageCounts;

    private string unlockText;

    private LocalizationManager loc;

    private Stats playerStats;
    private Player player;

    // private int page;
    private void Awake() {
        player = FindObjectOfType<Player>();
        inventory = player.GetComponent<Inventory>();
        loc = LocalizationManager.localization;
        playerStats = player.GetComponent<Stats>();
    }
    private void OnEnable() {
        currentTabIndex = 0;
        currentPageIndex = 0;
        PressTab(currentTabIndex);
        CountPages();
        ClearGrids();
        ViewRecipes();
    }
    private void CountPages() {
        pageCounts = new int[tabCount];
        pageCounts[0] = (craftingDatabase.generalRecipes.Count - 1) / recipeCountPerPage + 1;
        pageCounts[1] = (craftingDatabase.strengthRecipes.Count - 1) / recipeCountPerPage + 1;
        pageCounts[2] = (craftingDatabase.agilityRecipes.Count - 1) / recipeCountPerPage + 1;
        pageCounts[3] = (craftingDatabase.dexterityRecipes.Count - 1) / recipeCountPerPage + 1;
        pageCounts[4] = (craftingDatabase.intelligenceRecipes.Count - 1) / recipeCountPerPage + 1;
        pageCounts[5] = (craftingDatabase.faithRecipes.Count - 1) / recipeCountPerPage + 1;
        pageCounts[6] = (craftingDatabase.wisdomRecipes.Count - 1) / recipeCountPerPage + 1;
        pageCounts[7] = (craftingDatabase.vitalityRecipes.Count - 1) / recipeCountPerPage + 1;
        pageCounts[8] = (craftingDatabase.charismaRecipes.Count - 1) / recipeCountPerPage + 1;
    }
    // Short get text from localization
    private string GT(string s) {
        return loc.GetText(s);
    }
    private void ViewRecipes() {
        int index;
        int iteration;
        int startingIndex;
        switch(currentTabIndex) {
            // General tab
            case 0:
                iteration = 0;
                ClearGrids();
                startingIndex = currentPageIndex * recipeCountPerPage;
                for(int i = startingIndex; i < startingIndex + recipeCountPerPage; i++) {
                    index = 0;
                    if(i <= craftingDatabase.generalRecipes.Count - 1) {
                        if(craftingDatabase.generalRecipes[i].input0 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.generalRecipes[i].input0.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.generalRecipes[i].input0.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.generalRecipes[i].input0.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.generalRecipes[i].input0.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.generalRecipes[i].input0.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.generalRecipes[i].input0.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.generalRecipes[i].input1 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.generalRecipes[i].input1.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.generalRecipes[i].input1.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.generalRecipes[i].input1.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.generalRecipes[i].input1.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.generalRecipes[i].input1.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.generalRecipes[i].input1.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.generalRecipes[i].input2 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.generalRecipes[i].input2.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.generalRecipes[i].input2.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.generalRecipes[i].input2.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.generalRecipes[i].input2.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.generalRecipes[i].input2.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.generalRecipes[i].input2.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.generalRecipes[i].input3 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.generalRecipes[i].input3.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.generalRecipes[i].input3.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.generalRecipes[i].input3.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.generalRecipes[i].input3.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.generalRecipes[i].input3.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.generalRecipes[i].input3.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.generalRecipes[i].input4 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.generalRecipes[i].input4.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.generalRecipes[i].input4.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.generalRecipes[i].input4.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.generalRecipes[i].input4.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.generalRecipes[i].input4.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.generalRecipes[i].input4.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.generalRecipes[i].input5 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.generalRecipes[i].input5.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.generalRecipes[i].input5.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.generalRecipes[i].input5.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.generalRecipes[i].input5.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.generalRecipes[i].input5.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.generalRecipes[i].input5.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.generalRecipes[i].input6 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.generalRecipes[i].input6.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.generalRecipes[i].input6.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.generalRecipes[i].input6.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.generalRecipes[i].input6.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.generalRecipes[i].input6.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.generalRecipes[i].input6.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.generalRecipes[i].input7 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.generalRecipes[i].input7.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.generalRecipes[i].input7.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.generalRecipes[i].input7.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.generalRecipes[i].input7.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.generalRecipes[i].input7.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.generalRecipes[i].input7.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.generalRecipes[i].input8 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.generalRecipes[i].input8.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.generalRecipes[i].input8.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.generalRecipes[i].input8.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.generalRecipes[i].input8.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.generalRecipes[i].input8.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.generalRecipes[i].input8.thirdColor;
                        }
                    }
                    if(i <= craftingDatabase.generalRecipes.Count - 1) {
                        outputImages[iteration].firstImage.sprite = craftingDatabase.generalRecipes[i].output.firstIcon;
                        outputImages[iteration].secondImage.sprite = craftingDatabase.generalRecipes[i].output.secondIcon;
                        outputImages[iteration].thirdImage.sprite = craftingDatabase.generalRecipes[i].output.thirdIcon;
                        outputImages[iteration].firstImage.color = craftingDatabase.generalRecipes[i].output.firstColor;
                        outputImages[iteration].secondImage.color = craftingDatabase.generalRecipes[i].output.secondColor;
                        outputImages[iteration].thirdImage.color = craftingDatabase.generalRecipes[i].output.thirdColor;
                    }
                    iteration++;
                }
                break;
            case 1:
                iteration = 0;
                ClearGrids();
                startingIndex = currentPageIndex * recipeCountPerPage;
                for(int i = startingIndex; i < startingIndex + recipeCountPerPage; i++) {
                    index = 0;                    
                    if(i <= craftingDatabase.strengthRecipes.Count - 1) {
                        int requirement = player.raceIndex == 4 ? craftingDatabase.strengthRecipes[i].strengthRequirement - 3 : craftingDatabase.strengthRecipes[i].strengthRequirement;
                        if(playerStats.strength < requirement) {
                            unlockText = $"{GT("need")} {requirement} {GT("strength")} {GT("toUnlock")}";
                            unlockTexts[iteration].text = unlockText;
                            unlockTexts[iteration].gameObject.SetActive(true);
                            iteration++;
                            continue;
                        }                        
                        if(craftingDatabase.strengthRecipes[i].input0 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.strengthRecipes[i].input0.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.strengthRecipes[i].input0.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.strengthRecipes[i].input0.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.strengthRecipes[i].input0.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.strengthRecipes[i].input0.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.strengthRecipes[i].input0.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.strengthRecipes[i].input1 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.strengthRecipes[i].input1.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.strengthRecipes[i].input1.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.strengthRecipes[i].input1.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.strengthRecipes[i].input1.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.strengthRecipes[i].input1.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.strengthRecipes[i].input1.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.strengthRecipes[i].input2 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.strengthRecipes[i].input2.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.strengthRecipes[i].input2.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.strengthRecipes[i].input2.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.strengthRecipes[i].input2.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.strengthRecipes[i].input2.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.strengthRecipes[i].input2.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.strengthRecipes[i].input3 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.strengthRecipes[i].input3.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.strengthRecipes[i].input3.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.strengthRecipes[i].input3.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.strengthRecipes[i].input3.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.strengthRecipes[i].input3.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.strengthRecipes[i].input3.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.strengthRecipes[i].input4 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.strengthRecipes[i].input4.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.strengthRecipes[i].input4.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.strengthRecipes[i].input4.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.strengthRecipes[i].input4.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.strengthRecipes[i].input4.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.strengthRecipes[i].input4.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.strengthRecipes[i].input5 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.strengthRecipes[i].input5.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.strengthRecipes[i].input5.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.strengthRecipes[i].input5.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.strengthRecipes[i].input5.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.strengthRecipes[i].input5.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.strengthRecipes[i].input5.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.strengthRecipes[i].input6 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.strengthRecipes[i].input6.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.strengthRecipes[i].input6.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.strengthRecipes[i].input6.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.strengthRecipes[i].input6.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.strengthRecipes[i].input6.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.strengthRecipes[i].input6.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.strengthRecipes[i].input7 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.strengthRecipes[i].input7.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.strengthRecipes[i].input7.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.strengthRecipes[i].input7.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.strengthRecipes[i].input7.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.strengthRecipes[i].input7.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.strengthRecipes[i].input7.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.strengthRecipes[i].input8 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.strengthRecipes[i].input8.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.strengthRecipes[i].input8.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.strengthRecipes[i].input8.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.strengthRecipes[i].input8.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.strengthRecipes[i].input8.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.strengthRecipes[i].input8.thirdColor;
                        }
                    }
                    if(i <= craftingDatabase.strengthRecipes.Count - 1) {
                        outputImages[iteration].firstImage.sprite = craftingDatabase.strengthRecipes[i].output.firstIcon;
                        outputImages[iteration].secondImage.sprite = craftingDatabase.strengthRecipes[i].output.secondIcon;
                        outputImages[iteration].thirdImage.sprite = craftingDatabase.strengthRecipes[i].output.thirdIcon;
                        outputImages[iteration].firstImage.color = craftingDatabase.strengthRecipes[i].output.firstColor;
                        outputImages[iteration].secondImage.color = craftingDatabase.strengthRecipes[i].output.secondColor;
                        outputImages[iteration].thirdImage.color = craftingDatabase.strengthRecipes[i].output.thirdColor;
                    }
                    iteration++;
                }
                break;
            case 2:
                iteration = 0;
                ClearGrids();
                startingIndex = currentPageIndex * recipeCountPerPage;
                for(int i = startingIndex; i < startingIndex + recipeCountPerPage; i++) {
                    index = 0;
                    if(i <= craftingDatabase.agilityRecipes.Count - 1) {
                        int requirement = player.raceIndex == 4 ? craftingDatabase.agilityRecipes[i].agilityRequirement - 3 : craftingDatabase.agilityRecipes[i].agilityRequirement;
                        if(playerStats.agility < requirement) {
                            unlockText = $"{GT("need")} {requirement} {GT("agility")} {GT("toUnlock")}";
                            unlockTexts[iteration].text = unlockText;
                            unlockTexts[iteration].gameObject.SetActive(true);
                            continue;
                        }
                        if(craftingDatabase.agilityRecipes[i].input0 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.agilityRecipes[i].input0.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.agilityRecipes[i].input0.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.agilityRecipes[i].input0.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.agilityRecipes[i].input0.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.agilityRecipes[i].input0.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.agilityRecipes[i].input0.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.agilityRecipes[i].input1 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.agilityRecipes[i].input1.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.agilityRecipes[i].input1.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.agilityRecipes[i].input1.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.agilityRecipes[i].input1.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.agilityRecipes[i].input1.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.agilityRecipes[i].input1.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.agilityRecipes[i].input2 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.agilityRecipes[i].input2.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.agilityRecipes[i].input2.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.agilityRecipes[i].input2.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.agilityRecipes[i].input2.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.agilityRecipes[i].input2.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.agilityRecipes[i].input2.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.agilityRecipes[i].input3 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.agilityRecipes[i].input3.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.agilityRecipes[i].input3.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.agilityRecipes[i].input3.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.agilityRecipes[i].input3.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.agilityRecipes[i].input3.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.agilityRecipes[i].input3.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.agilityRecipes[i].input4 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.agilityRecipes[i].input4.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.agilityRecipes[i].input4.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.agilityRecipes[i].input4.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.agilityRecipes[i].input4.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.agilityRecipes[i].input4.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.agilityRecipes[i].input4.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.agilityRecipes[i].input5 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.agilityRecipes[i].input5.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.agilityRecipes[i].input5.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.agilityRecipes[i].input5.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.agilityRecipes[i].input5.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.agilityRecipes[i].input5.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.agilityRecipes[i].input5.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.agilityRecipes[i].input6 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.agilityRecipes[i].input6.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.agilityRecipes[i].input6.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.agilityRecipes[i].input6.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.agilityRecipes[i].input6.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.agilityRecipes[i].input6.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.agilityRecipes[i].input6.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.agilityRecipes[i].input7 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.agilityRecipes[i].input7.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.agilityRecipes[i].input7.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.agilityRecipes[i].input7.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.agilityRecipes[i].input7.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.agilityRecipes[i].input7.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.agilityRecipes[i].input7.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.agilityRecipes[i].input8 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.agilityRecipes[i].input8.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.agilityRecipes[i].input8.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.agilityRecipes[i].input8.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.agilityRecipes[i].input8.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.agilityRecipes[i].input8.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.agilityRecipes[i].input8.thirdColor;
                        }
                    }
                    if(i <= craftingDatabase.agilityRecipes.Count - 1) {
                        outputImages[iteration].firstImage.sprite = craftingDatabase.agilityRecipes[i].output.firstIcon;
                        outputImages[iteration].secondImage.sprite = craftingDatabase.agilityRecipes[i].output.secondIcon;
                        outputImages[iteration].thirdImage.sprite = craftingDatabase.agilityRecipes[i].output.thirdIcon;
                        outputImages[iteration].firstImage.color = craftingDatabase.agilityRecipes[i].output.firstColor;
                        outputImages[iteration].secondImage.color = craftingDatabase.agilityRecipes[i].output.secondColor;
                        outputImages[iteration].thirdImage.color = craftingDatabase.agilityRecipes[i].output.thirdColor;
                    }
                    iteration++;
                }
                break;
            case 3:
                iteration = 0;
                ClearGrids();
                startingIndex = currentPageIndex * recipeCountPerPage;
                for(int i = startingIndex; i < startingIndex + recipeCountPerPage; i++) {
                    index = 0;
                    if(i <= craftingDatabase.dexterityRecipes.Count - 1) {
                        int requirement = player.raceIndex == 4 ? craftingDatabase.dexterityRecipes[i].dexterityRequirement - 3 : craftingDatabase.dexterityRecipes[i].dexterityRequirement;
                        if(playerStats.dexterity < requirement) {
                            unlockText = $"{GT("need")} {requirement} {GT("dexterity")} {GT("toUnlock")}";
                            unlockTexts[iteration].text = unlockText;
                            unlockTexts[iteration].gameObject.SetActive(true);
                            continue;
                        }
                        if(craftingDatabase.dexterityRecipes[i].input0 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.dexterityRecipes[i].input0.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.dexterityRecipes[i].input0.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.dexterityRecipes[i].input0.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.dexterityRecipes[i].input0.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.dexterityRecipes[i].input0.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.dexterityRecipes[i].input0.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.dexterityRecipes[i].input1 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.dexterityRecipes[i].input1.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.dexterityRecipes[i].input1.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.dexterityRecipes[i].input1.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.dexterityRecipes[i].input1.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.dexterityRecipes[i].input1.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.dexterityRecipes[i].input1.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.dexterityRecipes[i].input2 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.dexterityRecipes[i].input2.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.dexterityRecipes[i].input2.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.dexterityRecipes[i].input2.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.dexterityRecipes[i].input2.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.dexterityRecipes[i].input2.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.dexterityRecipes[i].input2.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.dexterityRecipes[i].input3 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.dexterityRecipes[i].input3.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.dexterityRecipes[i].input3.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.dexterityRecipes[i].input3.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.dexterityRecipes[i].input3.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.dexterityRecipes[i].input3.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.dexterityRecipes[i].input3.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.dexterityRecipes[i].input4 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.dexterityRecipes[i].input4.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.dexterityRecipes[i].input4.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.dexterityRecipes[i].input4.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.dexterityRecipes[i].input4.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.dexterityRecipes[i].input4.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.dexterityRecipes[i].input4.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.dexterityRecipes[i].input5 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.dexterityRecipes[i].input5.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.dexterityRecipes[i].input5.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.dexterityRecipes[i].input5.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.dexterityRecipes[i].input5.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.dexterityRecipes[i].input5.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.dexterityRecipes[i].input5.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.dexterityRecipes[i].input6 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.dexterityRecipes[i].input6.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.dexterityRecipes[i].input6.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.dexterityRecipes[i].input6.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.dexterityRecipes[i].input6.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.dexterityRecipes[i].input6.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.dexterityRecipes[i].input6.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.dexterityRecipes[i].input7 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.dexterityRecipes[i].input7.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.dexterityRecipes[i].input7.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.dexterityRecipes[i].input7.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.dexterityRecipes[i].input7.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.dexterityRecipes[i].input7.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.dexterityRecipes[i].input7.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.dexterityRecipes[i].input8 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.dexterityRecipes[i].input8.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.dexterityRecipes[i].input8.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.dexterityRecipes[i].input8.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.dexterityRecipes[i].input8.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.dexterityRecipes[i].input8.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.dexterityRecipes[i].input8.thirdColor;
                        }
                    }
                    if(i <= craftingDatabase.dexterityRecipes.Count - 1) {
                        outputImages[iteration].firstImage.sprite = craftingDatabase.dexterityRecipes[i].output.firstIcon;
                        outputImages[iteration].secondImage.sprite = craftingDatabase.dexterityRecipes[i].output.secondIcon;
                        outputImages[iteration].thirdImage.sprite = craftingDatabase.dexterityRecipes[i].output.thirdIcon;
                        outputImages[iteration].firstImage.color = craftingDatabase.dexterityRecipes[i].output.firstColor;
                        outputImages[iteration].secondImage.color = craftingDatabase.dexterityRecipes[i].output.secondColor;
                        outputImages[iteration].thirdImage.color = craftingDatabase.dexterityRecipes[i].output.thirdColor;
                    }
                    iteration++;
                }
                break;
            case 4:
                iteration = 0;
                ClearGrids();
                startingIndex = currentPageIndex * recipeCountPerPage;
                for(int i = startingIndex; i < startingIndex + recipeCountPerPage; i++) {
                    index = 0;
                    if(i <= craftingDatabase.intelligenceRecipes.Count - 1) {
                        int requirement = player.raceIndex == 4 ? craftingDatabase.intelligenceRecipes[i].intelligenceRequirement - 3 : craftingDatabase.intelligenceRecipes[i].intelligenceRequirement;
                        if(playerStats.intelligence < requirement) {
                            unlockText = $"{GT("need")} {requirement} {GT("intelligence")} {GT("toUnlock")}";
                            unlockTexts[iteration].text = unlockText;
                            unlockTexts[iteration].gameObject.SetActive(true);
                            continue;
                        }
                        if(craftingDatabase.intelligenceRecipes[i].input0 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.intelligenceRecipes[i].input0.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.intelligenceRecipes[i].input0.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.intelligenceRecipes[i].input0.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.intelligenceRecipes[i].input0.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.intelligenceRecipes[i].input0.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.intelligenceRecipes[i].input0.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.intelligenceRecipes[i].input1 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.intelligenceRecipes[i].input1.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.intelligenceRecipes[i].input1.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.intelligenceRecipes[i].input1.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.intelligenceRecipes[i].input1.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.intelligenceRecipes[i].input1.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.intelligenceRecipes[i].input1.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.intelligenceRecipes[i].input2 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.intelligenceRecipes[i].input2.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.intelligenceRecipes[i].input2.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.intelligenceRecipes[i].input2.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.intelligenceRecipes[i].input2.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.intelligenceRecipes[i].input2.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.intelligenceRecipes[i].input2.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.intelligenceRecipes[i].input3 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.intelligenceRecipes[i].input3.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.intelligenceRecipes[i].input3.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.intelligenceRecipes[i].input3.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.intelligenceRecipes[i].input3.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.intelligenceRecipes[i].input3.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.intelligenceRecipes[i].input3.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.intelligenceRecipes[i].input4 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.intelligenceRecipes[i].input4.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.intelligenceRecipes[i].input4.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.intelligenceRecipes[i].input4.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.intelligenceRecipes[i].input4.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.intelligenceRecipes[i].input4.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.intelligenceRecipes[i].input4.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.intelligenceRecipes[i].input5 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.intelligenceRecipes[i].input5.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.intelligenceRecipes[i].input5.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.intelligenceRecipes[i].input5.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.intelligenceRecipes[i].input5.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.intelligenceRecipes[i].input5.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.intelligenceRecipes[i].input5.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.intelligenceRecipes[i].input6 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.intelligenceRecipes[i].input6.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.intelligenceRecipes[i].input6.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.intelligenceRecipes[i].input6.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.intelligenceRecipes[i].input6.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.intelligenceRecipes[i].input6.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.intelligenceRecipes[i].input6.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.intelligenceRecipes[i].input7 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.intelligenceRecipes[i].input7.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.intelligenceRecipes[i].input7.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.intelligenceRecipes[i].input7.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.intelligenceRecipes[i].input7.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.intelligenceRecipes[i].input7.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.intelligenceRecipes[i].input7.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.intelligenceRecipes[i].input8 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.intelligenceRecipes[i].input8.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.intelligenceRecipes[i].input8.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.intelligenceRecipes[i].input8.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.intelligenceRecipes[i].input8.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.intelligenceRecipes[i].input8.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.intelligenceRecipes[i].input8.thirdColor;
                        }
                    }
                    if(i <= craftingDatabase.intelligenceRecipes.Count - 1) {
                        outputImages[iteration].firstImage.sprite = craftingDatabase.intelligenceRecipes[i].output.firstIcon;
                        outputImages[iteration].secondImage.sprite = craftingDatabase.intelligenceRecipes[i].output.secondIcon;
                        outputImages[iteration].thirdImage.sprite = craftingDatabase.intelligenceRecipes[i].output.thirdIcon;
                        outputImages[iteration].firstImage.color = craftingDatabase.intelligenceRecipes[i].output.firstColor;
                        outputImages[iteration].secondImage.color = craftingDatabase.intelligenceRecipes[i].output.secondColor;
                        outputImages[iteration].thirdImage.color = craftingDatabase.intelligenceRecipes[i].output.thirdColor;
                    }
                    iteration++;
                }
                break;
            case 5:
                iteration = 0;
                ClearGrids();
                startingIndex = currentPageIndex * recipeCountPerPage;
                for(int i = startingIndex; i < startingIndex + recipeCountPerPage; i++) {
                    index = 0;
                    if(i <= craftingDatabase.faithRecipes.Count - 1) {
                        int requirement = player.raceIndex == 4 ? craftingDatabase.faithRecipes[i].faithRequirement - 3 : craftingDatabase.faithRecipes[i].faithRequirement;
                        if(playerStats.faith < requirement) {
                            unlockText = $"{GT("need")} {requirement} {GT("faith")} {GT("toUnlock")}";
                            unlockTexts[iteration].text = unlockText;
                            unlockTexts[iteration].gameObject.SetActive(true);
                            continue;
                        }
                        if(craftingDatabase.faithRecipes[i].input0 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.faithRecipes[i].input0.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.faithRecipes[i].input0.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.faithRecipes[i].input0.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.faithRecipes[i].input0.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.faithRecipes[i].input0.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.faithRecipes[i].input0.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.faithRecipes[i].input1 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.faithRecipes[i].input1.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.faithRecipes[i].input1.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.faithRecipes[i].input1.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.faithRecipes[i].input1.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.faithRecipes[i].input1.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.faithRecipes[i].input1.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.faithRecipes[i].input2 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.faithRecipes[i].input2.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.faithRecipes[i].input2.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.faithRecipes[i].input2.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.faithRecipes[i].input2.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.faithRecipes[i].input2.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.faithRecipes[i].input2.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.faithRecipes[i].input3 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.faithRecipes[i].input3.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.faithRecipes[i].input3.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.faithRecipes[i].input3.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.faithRecipes[i].input3.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.faithRecipes[i].input3.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.faithRecipes[i].input3.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.faithRecipes[i].input4 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.faithRecipes[i].input4.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.faithRecipes[i].input4.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.faithRecipes[i].input4.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.faithRecipes[i].input4.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.faithRecipes[i].input4.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.faithRecipes[i].input4.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.faithRecipes[i].input5 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.faithRecipes[i].input5.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.faithRecipes[i].input5.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.faithRecipes[i].input5.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.faithRecipes[i].input5.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.faithRecipes[i].input5.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.faithRecipes[i].input5.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.faithRecipes[i].input6 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.faithRecipes[i].input6.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.faithRecipes[i].input6.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.faithRecipes[i].input6.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.faithRecipes[i].input6.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.faithRecipes[i].input6.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.faithRecipes[i].input6.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.faithRecipes[i].input7 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.faithRecipes[i].input7.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.faithRecipes[i].input7.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.faithRecipes[i].input7.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.faithRecipes[i].input7.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.faithRecipes[i].input7.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.faithRecipes[i].input7.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.faithRecipes[i].input8 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.faithRecipes[i].input8.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.faithRecipes[i].input8.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.faithRecipes[i].input8.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.faithRecipes[i].input8.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.faithRecipes[i].input8.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.faithRecipes[i].input8.thirdColor;
                        }
                    }
                    if(i <= craftingDatabase.faithRecipes.Count - 1) {
                        outputImages[iteration].firstImage.sprite = craftingDatabase.faithRecipes[i].output.firstIcon;
                        outputImages[iteration].secondImage.sprite = craftingDatabase.faithRecipes[i].output.secondIcon;
                        outputImages[iteration].thirdImage.sprite = craftingDatabase.faithRecipes[i].output.thirdIcon;
                        outputImages[iteration].firstImage.color = craftingDatabase.faithRecipes[i].output.firstColor;
                        outputImages[iteration].secondImage.color = craftingDatabase.faithRecipes[i].output.secondColor;
                        outputImages[iteration].thirdImage.color = craftingDatabase.faithRecipes[i].output.thirdColor;
                    }
                    iteration++;
                }
                break;
            case 6:
                iteration = 0;
                ClearGrids();
                startingIndex = currentPageIndex * recipeCountPerPage;
                for(int i = startingIndex; i < startingIndex + recipeCountPerPage; i++) {
                    index = 0;
                    if(i <= craftingDatabase.wisdomRecipes.Count - 1) {
                        int requirement = player.raceIndex == 4 ? craftingDatabase.wisdomRecipes[i].wisdomRequirement - 3 : craftingDatabase.wisdomRecipes[i].wisdomRequirement;
                        if(playerStats.wisdom < requirement) {
                            unlockText = $"{GT("need")} {requirement} {GT("wisdom")} {GT("toUnlock")}";
                            unlockTexts[iteration].text = unlockText;
                            unlockTexts[iteration].gameObject.SetActive(true);
                            continue;
                        }
                        if(craftingDatabase.wisdomRecipes[i].input0 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.wisdomRecipes[i].input0.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.wisdomRecipes[i].input0.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.wisdomRecipes[i].input0.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.wisdomRecipes[i].input0.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.wisdomRecipes[i].input0.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.wisdomRecipes[i].input0.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.wisdomRecipes[i].input1 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.wisdomRecipes[i].input1.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.wisdomRecipes[i].input1.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.wisdomRecipes[i].input1.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.wisdomRecipes[i].input1.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.wisdomRecipes[i].input1.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.wisdomRecipes[i].input1.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.wisdomRecipes[i].input2 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.wisdomRecipes[i].input2.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.wisdomRecipes[i].input2.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.wisdomRecipes[i].input2.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.wisdomRecipes[i].input2.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.wisdomRecipes[i].input2.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.wisdomRecipes[i].input2.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.wisdomRecipes[i].input3 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.wisdomRecipes[i].input3.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.wisdomRecipes[i].input3.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.wisdomRecipes[i].input3.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.wisdomRecipes[i].input3.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.wisdomRecipes[i].input3.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.wisdomRecipes[i].input3.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.wisdomRecipes[i].input4 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.wisdomRecipes[i].input4.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.wisdomRecipes[i].input4.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.wisdomRecipes[i].input4.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.wisdomRecipes[i].input4.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.wisdomRecipes[i].input4.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.wisdomRecipes[i].input4.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.wisdomRecipes[i].input5 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.wisdomRecipes[i].input5.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.wisdomRecipes[i].input5.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.wisdomRecipes[i].input5.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.wisdomRecipes[i].input5.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.wisdomRecipes[i].input5.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.wisdomRecipes[i].input5.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.wisdomRecipes[i].input6 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.wisdomRecipes[i].input6.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.wisdomRecipes[i].input6.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.wisdomRecipes[i].input6.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.wisdomRecipes[i].input6.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.wisdomRecipes[i].input6.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.wisdomRecipes[i].input6.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.wisdomRecipes[i].input7 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.wisdomRecipes[i].input7.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.wisdomRecipes[i].input7.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.wisdomRecipes[i].input7.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.wisdomRecipes[i].input7.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.wisdomRecipes[i].input7.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.wisdomRecipes[i].input7.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.wisdomRecipes[i].input8 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.wisdomRecipes[i].input8.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.wisdomRecipes[i].input8.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.wisdomRecipes[i].input8.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.wisdomRecipes[i].input8.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.wisdomRecipes[i].input8.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.wisdomRecipes[i].input8.thirdColor;
                        }
                    }
                    if(i <= craftingDatabase.wisdomRecipes.Count - 1) {
                        outputImages[iteration].firstImage.sprite = craftingDatabase.wisdomRecipes[i].output.firstIcon;
                        outputImages[iteration].secondImage.sprite = craftingDatabase.wisdomRecipes[i].output.secondIcon;
                        outputImages[iteration].thirdImage.sprite = craftingDatabase.wisdomRecipes[i].output.thirdIcon;
                        outputImages[iteration].firstImage.color = craftingDatabase.wisdomRecipes[i].output.firstColor;
                        outputImages[iteration].secondImage.color = craftingDatabase.wisdomRecipes[i].output.secondColor;
                        outputImages[iteration].thirdImage.color = craftingDatabase.wisdomRecipes[i].output.thirdColor;
                    }
                    iteration++;
                }
                break;
            case 7:
                iteration = 0;
                ClearGrids();
                startingIndex = currentPageIndex * recipeCountPerPage;
                for(int i = startingIndex; i < startingIndex + recipeCountPerPage; i++) {
                    index = 0;
                    if(i <= craftingDatabase.vitalityRecipes.Count - 1) {
                        int requirement = player.raceIndex == 4 ? craftingDatabase.vitalityRecipes[i].vitalityRequirement - 3 : craftingDatabase.vitalityRecipes[i].vitalityRequirement;
                        if(playerStats.vitality < requirement) {
                            unlockText = $"{GT("need")} {requirement} {GT("vitality")} {GT("toUnlock")}";
                            unlockTexts[iteration].text = unlockText;
                            unlockTexts[iteration].gameObject.SetActive(true);
                            continue;
                        }
                        if(craftingDatabase.vitalityRecipes[i].input0 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.vitalityRecipes[i].input0.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.vitalityRecipes[i].input0.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.vitalityRecipes[i].input0.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.vitalityRecipes[i].input0.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.vitalityRecipes[i].input0.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.vitalityRecipes[i].input0.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.vitalityRecipes[i].input1 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.vitalityRecipes[i].input1.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.vitalityRecipes[i].input1.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.vitalityRecipes[i].input1.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.vitalityRecipes[i].input1.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.vitalityRecipes[i].input1.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.vitalityRecipes[i].input1.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.vitalityRecipes[i].input2 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.vitalityRecipes[i].input2.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.vitalityRecipes[i].input2.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.vitalityRecipes[i].input2.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.vitalityRecipes[i].input2.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.vitalityRecipes[i].input2.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.vitalityRecipes[i].input2.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.vitalityRecipes[i].input3 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.vitalityRecipes[i].input3.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.vitalityRecipes[i].input3.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.vitalityRecipes[i].input3.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.vitalityRecipes[i].input3.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.vitalityRecipes[i].input3.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.vitalityRecipes[i].input3.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.vitalityRecipes[i].input4 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.vitalityRecipes[i].input4.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.vitalityRecipes[i].input4.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.vitalityRecipes[i].input4.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.vitalityRecipes[i].input4.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.vitalityRecipes[i].input4.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.vitalityRecipes[i].input4.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.vitalityRecipes[i].input5 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.vitalityRecipes[i].input5.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.vitalityRecipes[i].input5.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.vitalityRecipes[i].input5.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.vitalityRecipes[i].input5.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.vitalityRecipes[i].input5.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.vitalityRecipes[i].input5.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.vitalityRecipes[i].input6 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.vitalityRecipes[i].input6.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.vitalityRecipes[i].input6.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.vitalityRecipes[i].input6.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.vitalityRecipes[i].input6.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.vitalityRecipes[i].input6.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.vitalityRecipes[i].input6.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.vitalityRecipes[i].input7 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.vitalityRecipes[i].input7.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.vitalityRecipes[i].input7.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.vitalityRecipes[i].input7.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.vitalityRecipes[i].input7.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.vitalityRecipes[i].input7.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.vitalityRecipes[i].input7.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.vitalityRecipes[i].input8 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.vitalityRecipes[i].input8.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.vitalityRecipes[i].input8.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.vitalityRecipes[i].input8.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.vitalityRecipes[i].input8.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.vitalityRecipes[i].input8.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.vitalityRecipes[i].input8.thirdColor;
                        }
                    }
                    if(i <= craftingDatabase.vitalityRecipes.Count - 1) {
                        outputImages[iteration].firstImage.sprite = craftingDatabase.vitalityRecipes[i].output.firstIcon;
                        outputImages[iteration].secondImage.sprite = craftingDatabase.vitalityRecipes[i].output.secondIcon;
                        outputImages[iteration].thirdImage.sprite = craftingDatabase.vitalityRecipes[i].output.thirdIcon;
                        outputImages[iteration].firstImage.color = craftingDatabase.vitalityRecipes[i].output.firstColor;
                        outputImages[iteration].secondImage.color = craftingDatabase.vitalityRecipes[i].output.secondColor;
                        outputImages[iteration].thirdImage.color = craftingDatabase.vitalityRecipes[i].output.thirdColor;
                    }
                    iteration++;
                }
                break;
            case 8:
                iteration = 0;
                ClearGrids();
                startingIndex = currentPageIndex * recipeCountPerPage;
                for(int i = startingIndex; i < startingIndex + recipeCountPerPage; i++) {
                    index = 0;
                    if(i <= craftingDatabase.charismaRecipes.Count - 1) {
                        int requirement = player.raceIndex == 4 ? craftingDatabase.charismaRecipes[i].charismaRequirement - 3 : craftingDatabase.charismaRecipes[i].charismaRequirement;
                        if(playerStats.charisma < requirement) {
                            unlockText = $"{GT("need")} {requirement} {GT("charisma")} {GT("toUnlock")}";
                            unlockTexts[iteration].text = unlockText;
                            unlockTexts[iteration].gameObject.SetActive(true);
                            continue;
                        }
                        if(craftingDatabase.charismaRecipes[i].input0 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.charismaRecipes[i].input0.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.charismaRecipes[i].input0.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.charismaRecipes[i].input0.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.charismaRecipes[i].input0.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.charismaRecipes[i].input0.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.charismaRecipes[i].input0.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.charismaRecipes[i].input1 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.charismaRecipes[i].input1.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.charismaRecipes[i].input1.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.charismaRecipes[i].input1.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.charismaRecipes[i].input1.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.charismaRecipes[i].input1.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.charismaRecipes[i].input1.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.charismaRecipes[i].input2 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.charismaRecipes[i].input2.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.charismaRecipes[i].input2.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.charismaRecipes[i].input2.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.charismaRecipes[i].input2.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.charismaRecipes[i].input2.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.charismaRecipes[i].input2.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.charismaRecipes[i].input3 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.charismaRecipes[i].input3.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.charismaRecipes[i].input3.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.charismaRecipes[i].input3.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.charismaRecipes[i].input3.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.charismaRecipes[i].input3.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.charismaRecipes[i].input3.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.charismaRecipes[i].input4 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.charismaRecipes[i].input4.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.charismaRecipes[i].input4.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.charismaRecipes[i].input4.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.charismaRecipes[i].input4.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.charismaRecipes[i].input4.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.charismaRecipes[i].input4.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.charismaRecipes[i].input5 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.charismaRecipes[i].input5.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.charismaRecipes[i].input5.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.charismaRecipes[i].input5.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.charismaRecipes[i].input5.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.charismaRecipes[i].input5.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.charismaRecipes[i].input5.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.charismaRecipes[i].input6 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.charismaRecipes[i].input6.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.charismaRecipes[i].input6.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.charismaRecipes[i].input6.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.charismaRecipes[i].input6.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.charismaRecipes[i].input6.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.charismaRecipes[i].input6.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.charismaRecipes[i].input7 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.charismaRecipes[i].input7.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.charismaRecipes[i].input7.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.charismaRecipes[i].input7.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.charismaRecipes[i].input7.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.charismaRecipes[i].input7.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.charismaRecipes[i].input7.thirdColor;
                        }
                        index++;
                        if(craftingDatabase.charismaRecipes[i].input8 != null) {
                            inputImages[index + (iteration * gridCount)].firstImage.sprite = craftingDatabase.charismaRecipes[i].input8.firstIcon;
                            inputImages[index + (iteration * gridCount)].secondImage.sprite = craftingDatabase.charismaRecipes[i].input8.secondIcon;
                            inputImages[index + (iteration * gridCount)].thirdImage.sprite = craftingDatabase.charismaRecipes[i].input8.thirdIcon;
                            inputImages[index + (iteration * gridCount)].firstImage.color = craftingDatabase.charismaRecipes[i].input8.firstColor;
                            inputImages[index + (iteration * gridCount)].secondImage.color = craftingDatabase.charismaRecipes[i].input8.secondColor;
                            inputImages[index + (iteration * gridCount)].thirdImage.color = craftingDatabase.charismaRecipes[i].input8.thirdColor;
                        }
                    }
                    if(i <= craftingDatabase.charismaRecipes.Count - 1) {
                        outputImages[iteration].firstImage.sprite = craftingDatabase.charismaRecipes[i].output.firstIcon;
                        outputImages[iteration].secondImage.sprite = craftingDatabase.charismaRecipes[i].output.secondIcon;
                        outputImages[iteration].thirdImage.sprite = craftingDatabase.charismaRecipes[i].output.thirdIcon;
                        outputImages[iteration].firstImage.color = craftingDatabase.charismaRecipes[i].output.firstColor;
                        outputImages[iteration].secondImage.color = craftingDatabase.charismaRecipes[i].output.secondColor;
                        outputImages[iteration].thirdImage.color = craftingDatabase.charismaRecipes[i].output.thirdColor;
                    }
                    iteration++;
                }
                break;
        }
    }
    private void ClearGrids() {
        for(int i = 0; i < inputImages.Length; i++) {
            inputImages[i].firstImage.sprite = null;
            inputImages[i].secondImage.sprite = null;
            inputImages[i].thirdImage.sprite = null;
            inputImages[i].firstImage.color = Color.clear;
            inputImages[i].secondImage.color = Color.clear;
            inputImages[i].thirdImage.color = Color.clear;
        }
        for(int i = 0; i < outputImages.Length; i++) {
            outputImages[i].firstImage.sprite = null;
            outputImages[i].secondImage.sprite = null;
            outputImages[i].thirdImage.sprite = null;
            outputImages[i].firstImage.color = Color.clear;
            outputImages[i].secondImage.color = Color.clear;
            outputImages[i].thirdImage.color = Color.clear;
        }
        for(int i = 0; i < unlockTexts.Length; i++) {
            unlockTexts[i].gameObject.SetActive(false);
        }
    }
    public void ChangePage(bool forward) {
        currentPageIndex = forward ? ++currentPageIndex : --currentPageIndex;
        if(currentPageIndex < 0) {
            currentPageIndex = pageCounts[currentTabIndex] - 1;
        }
        else if(currentPageIndex >= pageCounts[currentTabIndex] - 1) {
            currentPageIndex = 0;
        }
        ClearGrids();
        ViewRecipes();
        AudioSystem.audioManager.PlaySound("inGameButton", 0f);
    }
    private void ChangeTab(int index) {
        for(int i = 0; i < tabs.Length; i++) {
            tabs[i].color = inactiveTabPanelColor;
        }
        tabs[index].color = activeTabPanelColor;
        AudioSystem.audioManager.PlaySound("inGameButton", 0f);
    }
    public void PressTab(int index) {
        currentTabIndex = index;
        currentPageIndex = 0;
        ChangeTab(index);
        ClearGrids();
        ViewRecipes();
    }
}
