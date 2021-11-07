using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Databases/CraftingDatabase")]
public class CraftingDatabase : ScriptableObject {

    public List<CraftingRecipe> allRecipes;
    public List<CraftingRecipe> generalRecipes;
    public List<CraftingRecipe> strengthRecipes;
    public List<CraftingRecipe> agilityRecipes;
    public List<CraftingRecipe> dexterityRecipes;
    public List<CraftingRecipe> intelligenceRecipes;
    public List<CraftingRecipe> faithRecipes;
    public List<CraftingRecipe> wisdomRecipes;
    public List<CraftingRecipe> vitalityRecipes;
    public List<CraftingRecipe> charismaRecipes;

    public void SortAllRecipes() {
        generalRecipes = new List<CraftingRecipe>();
        strengthRecipes = new List<CraftingRecipe>();
        agilityRecipes = new List<CraftingRecipe>();
        dexterityRecipes = new List<CraftingRecipe>();
        intelligenceRecipes = new List<CraftingRecipe>();
        faithRecipes = new List<CraftingRecipe>();
        wisdomRecipes = new List<CraftingRecipe>();
        vitalityRecipes = new List<CraftingRecipe>();
        charismaRecipes = new List<CraftingRecipe>();
        generalRecipes = allRecipes
            .Where(x => x.strengthRequirement == 0)
            .Where(x => x.agilityRequirement == 0)
            .Where(x => x.dexterityRequirement == 0)
            .Where(x => x.intelligenceRequirement == 0)
            .Where(x => x.faithRequirement == 0)
            .Where(x => x.wisdomRequirement == 0)
            .Where(x => x.vitalityRequirement == 0)
            .Where(x => x.charismaRequirement == 0)
            .ToList();

        var strList = allRecipes.Where(x => x.strengthRequirement > 0);
        strengthRecipes = strList.OrderBy(x => x.strengthRequirement).ToList();

        var agiList = allRecipes.Where(x => x.agilityRequirement > 0);
        agilityRecipes = agiList.OrderBy(x => x.agilityRequirement).ToList();

        var dexList = allRecipes.Where(x => x.dexterityRequirement > 0);
        dexterityRecipes = dexList.OrderBy(x => x.dexterityRequirement).ToList();

        var intList = allRecipes.Where(x => x.intelligenceRequirement > 0);
        intelligenceRecipes = intList.OrderBy(x => x.intelligenceRequirement).ToList();

        var faiList = allRecipes.Where(x => x.faithRequirement > 0);
        faithRecipes = faiList.OrderBy(x => x.faithRequirement).ToList();

        var wisList = allRecipes.Where(x => x.wisdomRequirement > 0);
        wisdomRecipes = wisList.OrderBy(x => x.wisdomRequirement).ToList();

        var vitalList = allRecipes.Where(x => x.vitalityRequirement > 0);
        vitalityRecipes = vitalList.OrderBy(x => x.vitalityRequirement).ToList();

        var chaList = allRecipes.Where(x => x.charismaRequirement > 0);
        charismaRecipes = chaList.OrderBy(x => x.charismaRequirement).ToList();
    }
}
