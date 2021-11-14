using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGate : MonoBehaviour, IInteractable {

    private bool interacted = false;

    private Player player;

    private WaitForSeconds waitASec;

    private ParticleSystem partSys;

    private NotificationText notificationText;

    public Sprite interactSprite;

    public Sprite UISprite {
        get;
        set;
    }
    public bool Seller {
        get;
        set;
    }

    private void Awake() {
        notificationText = FindObjectOfType<NotificationText>();
        waitASec = new WaitForSeconds(1f);
        Seller = false;
        UISprite = interactSprite;
        player = FindObjectOfType<Player>();
        partSys = GetComponent<ParticleSystem>();
    }
    private void OnEnable() {
        interacted = false;
        partSys.Play();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            collision.GetComponent<Player>().SetInteraction(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            collision.GetComponent<Player>().ClearInteraction(this);
        }
    }
    public void Interact() {
        if(interacted) {
            return;
        }
        interacted = true;
        GameObject boss = player.CallBoss();
        boss.transform.position = transform.position;
        boss.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 359f));
        boss.SetActive(true);
        notificationText.SetText(boss.GetComponent<Enemy>().bossName, Color.red);
        StartCoroutine(DisableAfterDelay());
    }
    private IEnumerator DisableAfterDelay() {
        partSys.Stop();
        yield return waitASec;
        gameObject.SetActive(false);
    }
}
