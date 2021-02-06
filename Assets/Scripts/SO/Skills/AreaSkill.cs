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
    
    public override void Activate(StatusEffects targetStatus, Stats target) {
        /*GameObject clone = ObjectPooler.objectPooler.GetPooledObject(hitbox.name);
        clone.transform.position = pos;
        clone.transform.rotation = rot;
        clone.SetActive(true);*/
    }
    /*
    private IEnumerator Persistence() {
    
    }*/
}
