using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

    private int team;
    private AreaSkill skill;
    private Stats attackerStats;
    private readonly float time = 0.1f;
    private float timer;

    public void SetTeam(int team) {
        this.team = team;
    }
    public void SetSkill(AreaSkill skill) {
        this.skill = skill;
    }
    public void SetAttackerStats(Stats attackerStats) {
        this.attackerStats = attackerStats;
    }
    private void OnEnable() {
        timer = time;
    }
    private void Update() {
        if(timer > 0f) {
            timer -= Time.deltaTime;
            if(timer <= 0f) {
                gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        Stats collisionStats = collision.GetComponent<Stats>();
        StatusEffects collisionStatus = collision.GetComponent<StatusEffects>();
        if(collisionStats && collisionStats.team != team) {
            skill.Activate(collisionStatus, attackerStats);
        }
    }
}
