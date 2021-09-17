using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour {

    private AIDestinationSetter destinationSetter;
    private AIPath aiPath;
    private Animator enemyAnimator;
    private SkillUser skillUser;
    private FieldOfView fov;
    private Stats stats;
    private StatusEffects status;

    private Rigidbody2D rb2D;

    public float roamTime = 3f;
    public float timeBetweenSkills = 0.5f;
    public float rotationTime = 1.5f;
    [HideInInspector] public float speed = 3f;

    private bool moving = false;
    private bool roaming = false;
    private bool rotating = false;
    public bool attacked = false;
    public bool hostility = false;          // Will it attack enemies on sight?
    public bool hostile = false;            // Current hostility status
    public bool willDefendItself = true;    // Will this entity defend itself if it gets attacked?

    public string walkAnimationName;
    public string idleAnimationName;

    public LevelGeneration level;

    private void Awake() {
        status = GetComponent<StatusEffects>();
        rb2D = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();
        fov = GetComponent<FieldOfView>();
        skillUser = GetComponent<SkillUser>();
        enemyAnimator = GetComponent<Animator>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();        
    }
    private void OnEnable() {
        moving = false;
        roaming = false;
        aiPath.canMove = true;
        speed = stats.runSpeed;
        aiPath.maxSpeed = speed;
        aiPath.destination = transform.position;
        destinationSetter.target = null;
        hostile = hostility;
        attacked = false;
        aiPath.enableRotation = true;
        rotating = false;
        StartCoroutine(Roam());
        StartCoroutine(RotateTowardsPlayer());
    }
    private void OnDisable() {
        StopAllCoroutines();        
    }
    private void Update() {
        if(!stats.living || rotating) {
            aiPath.canMove = false;
        }
        if(!rotating && !status.stunned && !status.immobilized && !status.chanelling) {
            aiPath.canMove = true;
            //transform.LookAt(destinationSetter.target, Vector3.forward);
            Vector3 lookingDirection = aiPath.desiredVelocity;
            if(lookingDirection.magnitude > 0.1f) {
                transform.eulerAngles = new Vector3(0, 0, Vector3.SignedAngle(Vector3.up, lookingDirection.normalized, Vector3.forward));
                rb2D.angularVelocity = 0;
            }
            
        }
        if(roaming && !aiPath.reachedDestination) {
            moving = true;
        }
        else if(destinationSetter.target && !aiPath.reachedDestination) {
            moving = true;
        }
        else {
            moving = false;
        }
        if(aiPath.canMove && moving) {
            if(!string.IsNullOrEmpty(walkAnimationName)) {
                enemyAnimator.SetTrigger(walkAnimationName);
            }
        }
        else {
            if(!string.IsNullOrEmpty(idleAnimationName)) {
                enemyAnimator.SetTrigger(idleAnimationName);
            }            
        }        
    }
    private IEnumerator RotateTowardsPlayer() {

        float progress = 0f;
        float speed = aiPath.rotationSpeed / 360f;

        while(progress < 1f && destinationSetter.target && !stats.status.stunned && !stats.status.chanelling && !stats.status.immobilized) {
            Vector3 targetRotation = new Vector3(
            0, 0, Vector3.SignedAngle(
                Vector3.up, destinationSetter.target.transform.position - transform.position, Vector3.forward));
            Quaternion lookOnLook = Quaternion.Euler(targetRotation);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, progress);
            rotating = true;
            progress += Time.deltaTime * speed;
            if(progress <= 1f) {
                yield return null;
            }
        }
        rotating = false;
        yield return new WaitForSeconds(rotationTime);
        StartCoroutine(RotateTowardsPlayer());
    }
    private IEnumerator Roam() {
        yield return new WaitForSeconds(Random.Range(roamTime, roamTime * 2));
        if(!destinationSetter.target) {
            roaming = true;
            if(level) {
                aiPath.destination = GetRoamingPosition();
            }            
        }
        else {
            roaming = false;
        }
        if(stats.living) {
            StartCoroutine(Roam());
        }        
    }
    public void SetLevel(LevelGeneration level) {
        this.level = level;
    }
    public void Enrage() {
        if(willDefendItself && hostile) {
            aiPath.enableRotation = false;
            StartCoroutine(UseRandomSkill());
        }
        else {
            Flee();
        }
    }
    private void Flee() {
        // Escape from the attacker
    }
    private IEnumerator UseRandomSkill() {
        yield return new WaitForSeconds(Random.Range(timeBetweenSkills, timeBetweenSkills * 2));
        if(stats.living) {
            List<int> availableIndices = new List<int>();
            
            for(int i = 0; i < skillUser.currentSkills.Length; i++) {
                ActiveSkill a = skillUser.currentSkills[i] as ActiveSkill;
                if(a && a.aiRange > fov.GetDistanceToEnemy() && skillUser.skillCooldowns[i] <= 0f) {
                    availableIndices.Add(i);
                }
            }
            if(availableIndices.Count > 0) {
                ActiveSkill activeSkill = skillUser.currentSkills[availableIndices[Random.Range(0, availableIndices.Count)]] as ActiveSkill;
                skillUser.UseSkill(activeSkill);
            }
            if(destinationSetter.target != null) {
                StartCoroutine(UseRandomSkill());
            }
        }              
    }
    private Vector3 GetRoamingPosition() {
        return level.GetRandomLocation();
    }
    private Vector3 GetRandomDirection() {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
