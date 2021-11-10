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

    public List<Transform> visibleTargets = new List<Transform>();

    private const float delay = 0.5f;

    private WaitForSeconds delayWait;

    private AIDestinationSetter destinationSetter;
    private Stats stats;
    private EnemyAI enemyAI;
    private float distanceToEnemy;

    public float GetDistanceToEnemy() {
        return distanceToEnemy;
    }
    public void SetDistanceToEnemy(float distance) {
        distanceToEnemy = distance;
    }
    private void Awake() {
        SetDistanceToEnemy(viewRadius);
        enemyAI = GetComponent<EnemyAI>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        stats = GetComponent<Stats>();
        delayWait = new WaitForSeconds(delay);
    }
    private void OnEnable() {
        StartCoroutine(FindTargetWithDelay());
    }
    private void OnDisable() {
        StopAllCoroutines();
    }
    private IEnumerator FindTargetWithDelay() {
        for(; ; ) {
            yield return delayWait;
            FindVisibleTargets();
            if(enemyAI) {
                ChaseClosestTarget();
            }            
        }
    }
    public Transform GetClosestTarget() {
        Transform closest = null;
        float leastDistance = viewRadius;
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
        return closest;
    }
    private void ChaseClosestTarget() {
        Transform closest = null;
        float leastDistance = viewRadius;
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
        if(enemyAI && enemyAI.hostile && closest) {
            SetDistanceToEnemy(leastDistance);
            destinationSetter.target = closest;
        }        
        if(destinationSetter && destinationSetter.target) {
            SetDistanceToEnemy(leastDistance);
            enemyAI.Enrage();
        }
        else if(destinationSetter) {
            SetDistanceToEnemy(viewRadius);
        }
    }
    // Finds targets inside field of view not blocked by walls
    private void FindVisibleTargets() {
        visibleTargets.Clear();
        //Adds targets in view radius to an array
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
        // For every targetsInViewRadius it checks if they are inside the field of view
        for(int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius[i].transform;
            if(target == transform) {
                continue;
            }
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            // Take noise making targets
            PlayerController pc = target.GetComponent<PlayerController>();
            if(pc && pc.makingNoise) {
                visibleTargets.Add(target);
                continue;
            }
            // Only take targets in the view cone
            if(Vector3.Angle(transform.up, dirToTarget) < viewAngle / 2f) {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                // If line drawn from object to target is not interrupted by wall, add target to list of visible targets
                if(!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) {                    
                    visibleTargets.Add(target);
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if(!angleIsGlobal) {
            angleInDegrees -= transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0f);
    }
}
