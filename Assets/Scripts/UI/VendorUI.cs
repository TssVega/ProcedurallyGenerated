using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VendorUI : MonoBehaviour {

    private Inventory inventory;

    private Item[] items;

    private const int shopSize = 8;

    public TextMeshProUGUI[] costTexts;

    public ItemImages[] itemImages;
    public Image[] coinImages;

    public Sprite goldCoin;
    public Sprite silverCoin;

    private void Awake() {
        inventory = FindObjectOfType<Player>().GetComponent<Inventory>();
        items = new Item[shopSize];
    }
    public void SetItems(Item[] items) {
        for(int i = 0; i < items.Length; i++) {
            this.items[i] = items[i];
            bool costsGold = this.items[i].goldCost > 0;
            coinImages[i].sprite = costsGold ? goldCoin : silverCoin;
            costTexts[i].text = costsGold ? this.items[i].goldCost.ToString() : this.items[i].silverCost.ToString();
            itemImages[i].firstImage.sprite = this.items[i].firstIcon;
            itemImages[i].secondImage.sprite = this.items[i].secondIcon;
            itemImages[i].thirdImage.sprite = this.items[i].thirdIcon;
            itemImages[i].firstImage.color = this.items[i].firstColor;
            itemImages[i].secondImage.color = this.items[i].secondColor;
            itemImages[i].thirdImage.color = this.items[i].thirdColor;
        }
    }
    public void OpenItemInfo() {
    
    }
    public void BuyItem(int i) {
        if(!inventory.CanAddToInventory()) {
            return;
        }
        bool costsGold = this.items[i].goldCost > 0;
        (int goldCount, int goldIndex) = inventory.GetGoldCountAndIndex();
        (int silverCount, int silverIndex) = inventory.GetSilverCountAndIndex();
        if(costsGold && goldCount >= this.items[i].goldCost) {
            inventory.TakeGold(this.items[i].goldCost);
            inventory.AddToInventory(this.items[i], false);
            AudioSystem.audioManager.PlaySound("buy", 0f);
        }
        else if(silverCount >= this.items[i].silverCost) {
            inventory.TakeSilver(this.items[i].silverCost);
            inventory.AddToInventory(this.items[i], false);
            AudioSystem.audioManager.PlaySound("buy", 0f);
        }
        else {
            AudioSystem.audioManager.PlaySound("inGameButton", 0f);
        }
    }
}
