using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[CreateAssetMenu(menuName = "Skills/Dash")]
public class DashSkill : ActiveSkill {

    public DashData dashData;

    private const float dashMultiplier = 0.33f;

    public override void Launch(StatusEffects status, Stats stats) {
        status.StartDash(this);
        // Move(status.gameObject, stats);
    }
    private void Move(GameObject dasher, Stats dasherStats) {
        if(dasher) {
            //Vector2 vect = dasher.transform.up.normalized;
            //vect = new Vector2(Mathf.Cos(Mathf.Deg2Rad * dashData.angle) * vect.x - Mathf.Sin(Mathf.Deg2Rad * dashData.angle) * vect.y, Mathf.Sin(Mathf.Deg2Rad * dashData.angle) * vect.x + Mathf.Cos(Mathf.Deg2Rad * dashData.angle) * vect.y);
            dasher.GetComponent<Rigidbody2D>().velocity = dasher.transform.up.normalized * dasherStats.agility * dashData.dashMultiplier * dashMultiplier;
            // rb2d.AddForce(vect * dasherStats.agility * dashData.dashMultiplier * dashMultiplier);
        }
    }    
}
