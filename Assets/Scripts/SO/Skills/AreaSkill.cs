using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Area Skill")]
public class AreaSkill : ActiveSkill {

    public AttackType attackType;
    public float damageRate;
    public float duration;
    public float totalHits;
    public float movementSpeed;
    public Vector2 movementVector;
    public GameObject hitbox;
    
    public override void Activate(StatusEffects targetStatus, Stats attackerStats) {
        /*GameObject clone = ObjectPooler.objectPooler.GetPooledObject(hitbox.name);
        clone.transform.position = pos;
        clone.transform.rotation = rot;
        clone.SetActive(true);*/
        targetStatus.TakeDamage(DamageFromDamageType.GetDamage(attackType, attackerStats) * damageRate, attackType);
    }
    public override void Launch(Stats attackerStats) {
        GameObject hitboxObject = ObjectPooler.objectPooler.GetPooledObject(hitbox.name);
        hitboxObject.transform.position = attackerStats.transform.position;
        hitboxObject.transform.rotation = attackerStats.transform.rotation;
        Hitbox h = hitboxObject.GetComponent<Hitbox>();
        h.SetTeam(attackerStats.team);
        h.SetSkill(this);
        h.SetAttackerStats(attackerStats);
        hitboxObject.SetActive(true);
    }
    /*
    private IEnumerator Persistence() {
    
    }*/
}
