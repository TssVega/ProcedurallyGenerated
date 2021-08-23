using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FieldOfView : MonoBehaviour {

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    private const float delay = 0.5f;

    private AIDestinationSetter destinationSetter;
    private Stats stats;
    private EnemyAI enemyAI;

    private void Awake() {
        enemyAI = GetComponent<EnemyAI>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        stats = GetComponent<Stats>();
    }

    void Start() {
        StartCoroutine(FindTargetWithDelay());
    }

    IEnumerator FindTargetWithDelay() {
        while(true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
            ChaseClosestTarget();
        }
    }

    private void ChaseClosestTarget() {
        Transform closest = null;
        float leastDistance = 9999f;
        for(int i = 0; i < visibleTargets.Count; i++) {
            Stats stats = visibleTargets[i].GetComponent<Stats>();
            if(stats && (stats.team == this.stats.team || this.stats == stats)) {
                continue;
            }
            if(visibleTargets[i] == transform) {
                continue;
            }
            if(Vector3.Distance(transform.position, visibleTargets[i].position) < leastDistance) {
                leastDistance = Vector3.Distance(transform.position, visibleTargets[i].position);
                closest = visibleTargets[i];
            }
        }
        destinationSetter.target = closest;
        if(destinationSetter.target) {
            enemyAI.Enrage(); 
        }
    }
    //Finds targets inside field of view not blocked by walls
    void FindVisibleTargets() {
        visibleTargets.Clear();
        //Adds targets in view radius to an array
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
        //For every targetsInViewRadius it checks if they are inside the field of view
        for(int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.up, dirToTarget) < viewAngle / 2) {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                //If line draw from object to target is not interrupted by wall, add target to list of visible 
                //targets
                if(!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) {
                    if(target == transform) {
                        continue;
                    }
                    visibleTargets.Add(target);
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if(!angleIsGlobal) {
            angleInDegrees -= transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }
}
