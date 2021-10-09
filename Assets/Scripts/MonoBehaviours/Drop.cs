using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour {

    private Item item;

    private float takeItemTime = 0.5f;

    private bool taking = false;

    public void SetItem(Item item) {
        this.item = item;
        transform.localScale = Vector3.one * 0.5f;
        GetComponent<SpriteRenderer>().sprite = item.firstSprite;
        takeItemTime = 0.5f;
        taking = false;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            Inventory i = collision.GetComponent<Inventory>();
            if(i.CanAddToInventory() && !taking) {
                taking = true;
                StartCoroutine(TakeItem(i));
            }            
        }
    }
    private IEnumerator TakeItem(Inventory inventory) {        
        while(takeItemTime > 0f) {
            transform.localScale = new Vector3(takeItemTime, takeItemTime, 0.5f);
            takeItemTime -= Time.deltaTime;
            yield return null;
        }
        inventory.AddToInventory(item, true);
        item = null;
        gameObject.SetActive(false);
    }
}
