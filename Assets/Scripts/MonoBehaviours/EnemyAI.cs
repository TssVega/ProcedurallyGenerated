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

    private List<int> activeSkillIndices;

    private void Awake() {
        skillUser = GetComponent<SkillUser>();
        enemyAnimator = GetComponent<Animator>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        activeSkillIndices = new List<int>();
        speed = aiPath.maxSpeed;
    }
    private void OnEnable() {
        startingPosition = transform.position;
        GetActiveSkills();
        StartCoroutine(Roam());
    }
    private void OnDisable() {
        StopAllCoroutines();        
    }
    private void Update() {
        enemyAnimator.SetFloat("Speed", aiPath.reachedDestination || !aiPath.canMove ? 0f : 1f);
    }
    private void GetActiveSkills() {
        for(int i = 0; i < skillUser.acquiredSkills.Count; i++) {
            if(skillUser.acquiredSkills[i] is ActiveSkill) {
                activeSkillIndices.Add(i);
            }
        }
    }
    private IEnumerator Roam() {
        yield return new WaitForSeconds(Random.Range(roamTime, roamTime * 2));
        if(!destinationSetter.target) {
            aiPath.destination = GetRoamingPosition();
        }        
        StartCoroutine(Roam());
    }
    public void Enrage() {
        StartCoroutine(UseRandomSkill());
    }
    private IEnumerator UseRandomSkill() {
        yield return new WaitForSeconds(timeBetweenSkills);
        ActiveSkill activeSkill = skillUser.acquiredSkills[activeSkillIndices[Random.Range(0, activeSkillIndices.Count)]] as ActiveSkill;
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
