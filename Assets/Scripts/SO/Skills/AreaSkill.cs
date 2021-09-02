using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Area Skill")]
public class AreaSkill : ActiveSkill {

    public float damageRate;
    public float duration;
    public GameObject hitbox;
    public bool warn = false;
    [Tooltip("Negative values pull and positive values push back")]
    [Range(-10, 10)]
    public float push = 0;
    
    public override void Activate(StatusEffects targetStatus, Stats attackerStats) {
        /*GameObject clone = ObjectPooler.objectPooler.GetPooledObject(hitbox.name);
        clone.transform.position = pos;
        clone.transform.rotation = rot;
        clone.SetActive(true);*/
        targetStatus.TakeDamage(DamageFromDamageType.GetDamage(attackType, attackerStats)
            * damageRate, attackType, this, attackerStats.status);
        targetStatus.StartAirPush(push, attackerStats.transform);
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
