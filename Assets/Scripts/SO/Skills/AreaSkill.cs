using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSkill : ActiveSkill {

    public string animationName;
    public AttackType attackType;
    public float damageRate;
    public float duration;
    public float totalHits;
    public float movemebtSpeed;
    public Vector2 movementVector;
    public GameObject hitbox;
    /*
    public override void Activate(Vector2 pos, Quaternion rot) {
        GameObject clone = ObjectPooler.objectPooler.GetPooledObject(hitbox.name);
        clone.transform.position = pos;
        clone.transform.rotation = rot;
        clone.SetActive(true);
    }*/
    /*
    private IEnumerator Persistence() {
    
    }*/
}
