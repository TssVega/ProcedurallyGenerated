using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

    private int team;
    private AreaSkill skill;
    private Stats attackerStats;
    private float timer;

    private float periodTimer;
    private int hitTimes = 0;

    public List<Collider2D> toHit;

    private void OnDisable() {
        toHit.Clear();
        hitTimes = 0;
    }
    public void SetTeam(int team) {
        this.team = team;
    }
    public void SetSkill(AreaSkill skill) {
        this.skill = skill;
        timer = skill.duration;
        periodTimer = 0f;
        hitTimes = 0;
    }
    public void SetAttackerStats(Stats attackerStats) {
        this.attackerStats = attackerStats;
    }
    /*
    private void FixedUpdate() {
        
    }*/
    private void Update() {
        if(periodTimer > 0f) {
            periodTimer -= Time.fixedDeltaTime;
        }
        if(periodTimer <= 0f && hitTimes < skill.hitTime && toHit.Count > 0) {
            Hit();
            hitTimes++;
            periodTimer = skill.duration / skill.hitTime;
        }
        if(timer > 0f) {
            timer -= Time.deltaTime;
            if(timer <= 0f) {
                toHit.Clear();
                gameObject.SetActive(false);
            }
        }
    }
    private void Hit() {
        for(int i = 0; i < toHit.Count; i++) {
            StatusEffects collisionStatus = toHit[i].GetComponent<StatusEffects>();
            skill.Activate(collisionStatus, attackerStats);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {        
        Stats collisionStats = collision.GetComponent<Stats>();
        // StatusEffects collisionStatus = collision.GetComponent<StatusEffects>();
        if(collisionStats && collisionStats.team != team) {
            toHit.Add(collision);
            // skill.Activate(collisionStatus, attackerStats);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if(toHit.Contains(collision)) {
            toHit.Remove(collision);
        }
    }
}
