using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUser : MonoBehaviour {

    //public List<Skill> skills;
    public SkillDatabase skillDatabase;
    public List<ActiveSkill> currentSkills;
    private Stats stats;
    public List<float> skillCooldowns;
    public Transform projectileExitPos;
    private StatusEffects statusEffects;

    private void Awake() {
        skillCooldowns = new List<float>();
        for(int i = 0; i < skillDatabase.skills.Count; i++) {
            skillCooldowns.Add(0f);
        }
        stats = GetComponent<Stats>();
        statusEffects = GetComponent<StatusEffects>();
    }
    private void Update() {
        CountCooldowns();
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            UseSkill(currentSkills[0]);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            UseSkill(currentSkills[1]);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)) {
            UseSkill(currentSkills[2]);
        }
    }
    public void UseSkill(ActiveSkill skill) {        
        if(skillCooldowns[skill.skillIndex] > 0f) {
            return;
        }
        if(!statusEffects.CanUseMana(skill.manaCost)) {
            return;
        }
        else {
            statusEffects.UseMana(skill.manaCost);
        }
        if(skill is ProjectileSkill) {
            ProjectileSkill proj = skill as ProjectileSkill;
            StartCoroutine(ThrowProjectile(proj));
        }
        else if(skill is BuffSkill) {
            BuffSkill buff = skill as BuffSkill;
            StartCoroutine(Buff(buff));
        }
        else if(skill is DashSkill) {
            DashSkill dash = skill as DashSkill;
            StartCoroutine(StartDash(dash));
        }
    }
    private IEnumerator ThrowProjectile(ProjectileSkill proj) {
        // Set status effects
        statusEffects.StartChanelling(proj.channelingTime);
        // Set projectile game object
        skillCooldowns[proj.skillIndex] = proj.cooldown;
        GameObject projectile = ObjectPooler.objectPooler.GetPooledObject("Projectile");
        // Set channelling particles
        List<GameObject> channelingParticles = new List<GameObject>();
        for(int i = 0; i < proj.projectileData.channelingParticleNames.Length; i++) {
            channelingParticles.Add(ObjectPooler.objectPooler.GetPooledObject(proj.projectileData.channelingParticleNames[i]));
            channelingParticles[i].transform.parent = projectile.transform;
            channelingParticles[i].transform.position = transform.position;
            channelingParticles[i].transform.rotation = Quaternion.identity;
            channelingParticles[i].SetActive(true);
            channelingParticles[i].GetComponent<ParticleSystem>().Play();
        }
        yield return new WaitForSeconds(proj.channelingTime);
        // Set casting particles
        List<GameObject> castingParticles = new List<GameObject>();
        for(int i = 0; i < proj.projectileData.castingParticleNames.Length; i++) {
            castingParticles.Add(ObjectPooler.objectPooler.GetPooledObject(proj.projectileData.castingParticleNames[i]));
            castingParticles[i].transform.parent = projectile.transform;
            castingParticles[i].transform.position = transform.position;
            castingParticles[i].transform.rotation = Quaternion.identity;
            castingParticles[i].SetActive(true);
            castingParticles[i].GetComponent<ParticleSystem>().Play();
        }
        // Set projectile game object        
        projectile.GetComponent<Projectile>().SetProjectile(proj.projectileData);
        projectile.transform.position = projectileExitPos.position;
        projectile.transform.rotation = Quaternion.identity;        
        projectile.SetActive(true);
        projectile.GetComponent<Projectile>().StartProjectile((projectileExitPos.position - transform.position).normalized);
        yield return new WaitForSeconds(proj.castTime);
        // Set projectile particles
        List<GameObject> particles = new List<GameObject>();
        for(int i = 0; i < proj.projectileData.particleNames.Length; i++) {
            particles.Add(ObjectPooler.objectPooler.GetPooledObject(proj.projectileData.particleNames[i]));
            particles[i].transform.parent = projectile.transform;
            particles[i].transform.position = projectile.transform.position;
            particles[i].transform.rotation = Quaternion.identity;
            particles[i].SetActive(true);
            particles[i].GetComponent<ParticleSystem>().Play();
        }               
    }
    private IEnumerator Buff(BuffSkill buff) {
        // Set status effects
        statusEffects.StartChanelling(buff.channelingTime);
        // Set buffer game object
        skillCooldowns[buff.skillIndex] = buff.cooldown;
        GameObject b = ObjectPooler.objectPooler.GetPooledObject("Buff");
        b.GetComponent<Buff>().SetBuff(buff.buffData);
        b.transform.position = transform.position;
        b.transform.rotation = Quaternion.identity;
        b.SetActive(true);
        // Set channelling particles
        List<GameObject> channelingParticles = new List<GameObject>();
        for(int i = 0; i < buff.buffData.channelingParticleNames.Length; i++) {
            channelingParticles.Add(ObjectPooler.objectPooler.GetPooledObject(buff.buffData.channelingParticleNames[i]));
            channelingParticles[i].transform.parent = b.transform;
            channelingParticles[i].transform.position = transform.position;
            channelingParticles[i].transform.rotation = Quaternion.identity;
            channelingParticles[i].SetActive(true);
            channelingParticles[i].GetComponent<ParticleSystem>().Play();
        }
        yield return new WaitForSeconds(buff.channelingTime);
        // Set casting particles
        List<GameObject> castingParticles = new List<GameObject>();
        for(int i = 0; i < buff.buffData.castingParticleNames.Length; i++) {
            castingParticles.Add(ObjectPooler.objectPooler.GetPooledObject(buff.buffData.castingParticleNames[i]));
            castingParticles[i].transform.parent = b.transform;
            castingParticles[i].transform.position = transform.position;
            castingParticles[i].transform.rotation = Quaternion.identity;
            castingParticles[i].SetActive(true);
            castingParticles[i].GetComponent<ParticleSystem>().Play();
        }
        yield return new WaitForSeconds(buff.castTime);
        // Set particles
        List<GameObject> particles = new List<GameObject>();
        for(int i = 0; i < buff.buffData.particleNames.Length; i++) {
            particles.Add(ObjectPooler.objectPooler.GetPooledObject(buff.buffData.particleNames[i]));
            //particles[i] = ObjectPooler.objectPooler.GetPooledObject(proj.projectileData.particleNames[i]);
            particles[i].transform.parent = transform;
            particles[i].transform.position = transform.position;
            particles[i].transform.rotation = Quaternion.identity;
            particles[i].SetActive(true);
            particles[i].GetComponent<ParticleSystem>().Play();
        }
    }
    private IEnumerator StartDash(DashSkill dash) {
        statusEffects.StartChanelling(dash.channelingTime);
        skillCooldowns[dash.skillIndex] = dash.cooldown;
        // Set channelling particles
        List<GameObject> channelingParticles = new List<GameObject>();
        for(int i = 0; i < dash.dashData.channelingParticleNames.Length; i++) {
            channelingParticles.Add(ObjectPooler.objectPooler.GetPooledObject(dash.dashData.channelingParticleNames[i]));
            channelingParticles[i].transform.parent = transform;
            channelingParticles[i].transform.position = transform.position;
            channelingParticles[i].transform.rotation = Quaternion.identity;
            channelingParticles[i].SetActive(true);
            channelingParticles[i].GetComponent<ParticleSystem>().Play();
        }
        yield return new WaitForSeconds(dash.channelingTime);
        List<GameObject> castingParticles = new List<GameObject>();
        for(int i = 0; i < dash.dashData.castingParticleNames.Length; i++) {
            castingParticles.Add(ObjectPooler.objectPooler.GetPooledObject(dash.dashData.castingParticleNames[i]));
            castingParticles[i].transform.parent = transform;
            castingParticles[i].transform.position = transform.position;
            castingParticles[i].transform.rotation = Quaternion.identity;
            castingParticles[i].SetActive(true);
            castingParticles[i].GetComponent<ParticleSystem>().Play();
        }
        yield return new WaitForSeconds(dash.castTime);
        List<GameObject> particles = new List<GameObject>();
        for(int i = 0; i < dash.dashData.particleNames.Length; i++) {
            particles.Add(ObjectPooler.objectPooler.GetPooledObject(dash.dashData.particleNames[i]));
            //particles[i] = ObjectPooler.objectPooler.GetPooledObject(proj.projectileData.particleNames[i]);
            particles[i].transform.parent = transform;
            particles[i].transform.position = transform.position;
            particles[i].transform.rotation = Quaternion.identity;
            particles[i].SetActive(true);
            particles[i].GetComponent<ParticleSystem>().Play();
        }
        dash.Launch(statusEffects);
    }
    private void CountCooldowns() {
        for(int i = 0; i < skillCooldowns.Count; i++) {
            if(skillCooldowns[i] > 0) {
                skillCooldowns[i] -= Time.deltaTime;
            }
        }
    }
}
