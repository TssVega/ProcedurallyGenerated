using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Dash")]
public class DashSkill : ActiveSkill {

    public DashData dashData;

    public override void Launch(StatusEffects status) {
        Move(status.GetComponent<Rigidbody2D>(), status.GetComponent<Stats>());
    }
    private void Move(Rigidbody2D dasher, Stats dasherStats) {
        Rigidbody2D rb2d = dasher.GetComponent<Rigidbody2D>();
        if(rb2d) {
            Debug.Log("Dashing by " + dasher.transform.up.normalized * dasherStats.agility * dashData.dashMultiplier);
            rb2d.velocity = dasher.transform.up.normalized * dasherStats.agility * dashData.dashMultiplier;
        }
    }
}
