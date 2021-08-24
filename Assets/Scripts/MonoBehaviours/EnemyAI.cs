using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour {

    private AIDestinationSetter destinationSetter;
    private AIPath aiPath;
    private Animator enemyAnimator;
    private SkillUser skillUser;

    private Vector3 startingPosition;

    public float roamTime = 3f;
    public float roamRange = 5f;
    public float timeBetweenSkills = 0.5f;
    [HideInInspector] public float speed = 3f;

    private bool moving = false;
    private bool roaming = false;

    private void Awake() {
        skillUser = GetComponent<SkillUser>();
        enemyAnimator = GetComponent<Animator>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        speed = aiPath.maxSpeed;
    }
    private void OnEnable() {
        moving = false;
        roaming = false;
        startingPosition = transform.position;
        StartCoroutine(Roam());
    }
    private void OnDisable() {
        StopAllCoroutines();        
    }
    private void Update() {
        if(roaming && !aiPath.reachedDestination) {
            moving = true;
        }
        else if(!roaming && destinationSetter.target && !aiPath.reachedDestination) {
            moving = true;
        }
        else {
            moving = false;
        }
        enemyAnimator.SetFloat("Speed", aiPath.canMove && moving ? 1f : 0f);
    }
    private IEnumerator Roam() {
        yield return new WaitForSeconds(Random.Range(roamTime, roamTime * 2));
        if(!destinationSetter.target) {
            roaming = true;
            aiPath.destination = GetRoamingPosition();
        }
        else {
            roaming = false;
        }
        StartCoroutine(Roam());
    }
    public void Enrage() {
        StartCoroutine(UseRandomSkill());
    }
    private IEnumerator UseRandomSkill() {        
        yield return new WaitForSeconds(Random.Range(timeBetweenSkills, timeBetweenSkills * 2));
        ActiveSkill activeSkill = skillUser.currentSkills[Random.Range(0, skillUser.currentSkills.Count)];
        skillUser.UseSkill(activeSkill);
        if(destinationSetter.target != null) {
            StartCoroutine(UseRandomSkill());
        }        
    }
    private Vector3 GetRoamingPosition() {
        return startingPosition + GetRandomDirection() * Random.Range(roamRange, roamRange * 2);
    }
    private Vector3 GetRandomDirection() {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
