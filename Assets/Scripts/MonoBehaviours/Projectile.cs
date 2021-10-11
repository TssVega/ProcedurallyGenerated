using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Projectile : MonoBehaviour {

    private ProjectileSkill projectileSkill;
    private ProjectileData projectileData;
    private Rigidbody2D rb2d;
    private Vector3 projectileVector;
    private float countdown;
    private Stats attackerStats;
    private Transform target;
    private Transform targetToSet;
    private CircleCollider2D circleCollider2D;
    private Light2D light2D;

    private const float setTargetDelay = 0.75f;

    private bool isActive = true;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        light2D = GetComponent<Light2D>();
    }
    private void OnEnable() {
        if(targetToSet) {
            StartCoroutine(SetTargetDelayed(targetToSet));
        }
        isActive = true;
        light2D.enabled = true;
    }
    private void Update() {
        countdown -= Time.deltaTime;
        if(countdown <= 0f) {
            StartCoroutine(EndProjectile());
        }
    }
    private void FixedUpdate() {
        if(!isActive) {
            rb2d.angularVelocity = 0f;
            rb2d.velocity = Vector2.zero;
            return;
        }
        if(projectileData.homing && target) {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb2d.angularVelocity = -rotateAmount * projectileData.rotationSpeed;
            rb2d.velocity = transform.up * projectileData.projectileSpeed;
        }
        else {
            gameObject.transform.position += projectileVector * projectileData.projectileSpeed * Time.fixedDeltaTime;
        }        
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(!isActive) {
            return;
        }
        if(collision.CompareTag("Wall")) {
            StartCoroutine(EndProjectile());
        }
        else if(collision.GetComponent<Stats>() && projectileData.team != collision.GetComponent<Stats>().team) {
            StatusEffects targetStatus = collision.GetComponent<StatusEffects>();
            // Crevalonian arrow
            if(attackerStats.player && attackerStats.player.raceIndex == 2) {
                const float crevalonianArrowPush = 0.1f;
                targetStatus.StartAirPush(crevalonianArrowPush, transform);
            }
            projectileSkill.Activate(targetStatus, attackerStats);
            if(!projectileData.penetrates) {
                StartCoroutine(EndProjectile());
            }
        }        
    }
    private IEnumerator EndProjectile() {
        isActive = false;
        for(int i = 0; i < transform.childCount; i++) {
            if(transform.GetChild(i).GetComponent<ParticleSystem>()) {
                transform.GetChild(i).GetComponent<ParticleSystem>().Stop();
            }            
        }
        light2D.enabled = false;
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < transform.childCount; i++) {
            if(transform.GetChild(i).GetComponent<ParticleSystem>()) {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        transform.DetachChildren();
        target = null;
        gameObject.SetActive(false);
    }
    public void SetProjectile(ProjectileSkill skill, Stats attackerStats) {
        projectileSkill = skill;
        projectileData = skill.projectileData;        
        this.attackerStats = attackerStats;
        circleCollider2D.radius = projectileData.radius;
        light2D.color = skill.projectileData.lightColor;
        light2D.intensity = skill.projectileData.lightIntensity;
        light2D.pointLightOuterRadius = skill.projectileData.lightRadius;
    }
    public void StartProjectile(Vector2 movementVector, Transform target) {
        countdown = projectileData.lifetime;
        projectileVector = movementVector;
        targetToSet = target;
    }
    public IEnumerator SetTargetDelayed(Transform target) {
        yield return new WaitForSeconds(setTargetDelay);
        this.target = target;
    }
}
