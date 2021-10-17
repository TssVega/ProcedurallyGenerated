using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Databases/CraftingDatabase")]
public class CraftingDatabase : ScriptableObject {

    public List<CraftingRecipe> generalRecipes;
    public List<CraftingRecipe> strengthRecipes;
    public List<CraftingRecipe> agilityRecipes;
    public List<CraftingRecipe> dexterityRecipes;
    public List<CraftingRecipe> intelligenceRecipes;
    public List<CraftingRecipe> faithRecipes;
    public List<CraftingRecipe> wisdomRecipes;
    public List<CraftingRecipe> vitalityRecipes;
    public List<CraftingRecipe> charismaRecipes;
}
