using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUser : MonoBehaviour {

    //public List<Skill> skills;
    public SkillDatabase skillDatabase;
    public List<Skill> acquiredSkills;
    public List<ActiveSkill> currentSkills;
    private Stats stats;
    public List<float> skillCooldowns;
    public Transform projectileExitPos;
    private StatusEffects statusEffects;
    public Animator animator;
    private Player player;
    private Enemy enemy;
    private Inventory inventory;

    private void Awake() {
        player = GetComponent<Player>();
        enemy = GetComponent<Enemy>();
        skillCooldowns = new List<float>();
        if(player) {
            for(int i = 0; i < skillDatabase.skills.Count; i++) {
                skillCooldowns.Add(0f);
            }
        }
        if(enemy) {
            for(int i = 0; i < skillDatabase.enemySkills.Count; i++) {
                skillCooldowns.Add(0f);
            }
        }
        stats = GetComponent<Stats>();
        statusEffects = GetComponent<StatusEffects>();        
        inventory = GetComponent<Inventory>();
    }
    private void Update() {
        CountCooldowns();
        if(player) {
            if(Input.GetKey(KeyCode.Alpha1)) {
                UseSkill(currentSkills[0]);
            }
            if(Input.GetKey(KeyCode.Alpha2)) {
                UseSkill(currentSkills[1]);
            }
            if(Input.GetKey(KeyCode.Alpha3)) {
                UseSkill(currentSkills[2]);
            }
            if(Input.GetKey(KeyCode.Alpha4)) {
                UseSkill(currentSkills[3]);
            }
            if(Input.GetKey(KeyCode.Alpha5)) {
                UseSkill(currentSkills[4]);
            }
            if(Input.GetKey(KeyCode.Alpha6)) {
                UseSkill(currentSkills[5]);
            }
            if(Input.GetKey(KeyCode.Alpha7)) {
                UseSkill(currentSkills[6]);
            }
            if(Input.GetKey(KeyCode.Alpha8)) {
                UseSkill(currentSkills[7]);
            }
        }
    }
    public void UseSkill(ActiveSkill skill) {
        if(!stats.living) {
            return;
        }
        if(skill.skillIndex >= 0 && skillCooldowns[skill.skillIndex] > 0f) {
            return;
        }
        if(statusEffects.chanelling || statusEffects.stunned || statusEffects.immobilized) {
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
            if(proj.projectileData.arrowSkill) {
                Weapon w = inventory.equipment[0] as Weapon;
                if(w == null) {
                    statusEffects.GiveMana(skill.manaCost);
                    return;
                }
                if(w.weaponType != WeaponType.Bow) {
                    statusEffects.GiveMana(skill.manaCost);
                    return;
                }
            }
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
        else if(skill is SkillSequence) {
            SkillSequence seq = skill as SkillSequence;
            StartCoroutine(StartSkillSequence(seq));
        }
        else if(skill is AreaSkill) {
            AreaSkill area = skill as AreaSkill;
            StartCoroutine(StartAreaSkill(area));
        }
        else if(skill is BlockSkill) {
            BlockSkill block = skill as BlockSkill;
            StartCoroutine(StartBlockSkill(block));
        }
    }
    private IEnumerator ThrowProjectile(ProjectileSkill proj) {
        // Set status effects
        statusEffects.StartChanelling(proj.channelingTime);
        // Set bow string appearance on Loose skill usage
        if(proj.projectileData.arrowSkill) {
            player.releasedBowString.SetActive(true);
            for(int i = 0; i < player.tenseBowStrings.Length; i++) {
                player.tenseBowStrings[i].SetActive(false);
            }
        }
        if(proj.focusedSkill) {
            statusEffects.StartImmobilize(proj.channelingTime + proj.castTime);
        }
        // Set projectile game object
        skillCooldowns[proj.skillIndex] = proj.cooldown;
               
        // Set channelling particles
        List<GameObject> channelingParticles = new List<GameObject>();
        for(int i = 0; i < proj.channelingParticleNames.Length; i++) {
            channelingParticles.Add(ObjectPooler.objectPooler.GetPooledObject(proj.channelingParticleNames[i]));
            channelingParticles[i].transform.parent = transform;
            channelingParticles[i].transform.position = transform.position;
            channelingParticles[i].transform.rotation = Quaternion.identity;
            channelingParticles[i].SetActive(true);
            if(channelingParticles[i].GetComponent<ParticleSystem>()) {
                channelingParticles[i].GetComponent<ParticleSystem>().Play();
            }
        }
        PlayAnimation(proj.channelingAnimationName);
        yield return new WaitForSeconds(proj.channelingTime);
        // Set casting particles
        List<GameObject> castingParticles = new List<GameObject>();
        for(int i = 0; i < proj.castingParticleNames.Length; i++) {
            castingParticles.Add(ObjectPooler.objectPooler.GetPooledObject(proj.castingParticleNames[i]));
            castingParticles[i].transform.parent = transform;
            castingParticles[i].transform.position = transform.position;
            castingParticles[i].transform.rotation = Quaternion.identity;
            castingParticles[i].SetActive(true);
            if(castingParticles[i].GetComponent<ParticleSystem>()) {
                castingParticles[i].GetComponent<ParticleSystem>().Play();
            }
        }
        // Set projectile game object
        for(int i = 0; i < proj.projectileData.projectileCount; i++) {
                        
        }        
        if(proj.projectileData.homing) {
            // TODO: Find target to set projectile
            // projectile.GetComponent<Projectile>().SetProjectile();
        }
        GameObject[] projectiles = new GameObject[proj.projectileData.projectileCount];
        for(int i = 0; i < proj.projectileData.projectileCount; i++) {
            projectiles[i] = ObjectPooler.objectPooler.GetPooledObject("Projectile");
            // Set the vector
            Vector2 vect = (projectileExitPos.position - transform.position).normalized;
            // Get the angle to rotate
            float angle = -proj.projectileData.angleDifference * proj.projectileData.projectileCount / 2 +  i * proj.projectileData.angleDifference + (proj.projectileData.angleDifference / 2);
            // Rotate the vector
            // x2=cosβx1−sinβy1
            // y2=sinβx1+cosβy1
            vect = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle) * vect.x - Mathf.Sin(Mathf.Deg2Rad * angle) * vect.y, Mathf.Sin(Mathf.Deg2Rad * angle) * vect.x + Mathf.Cos(Mathf.Deg2Rad * angle) * vect.y);
            // Start the projectile
            projectiles[i].GetComponent<Projectile>().SetProjectile(proj, stats);
            projectiles[i].transform.position = projectileExitPos.position;
            projectiles[i].transform.rotation = Quaternion.identity;
            projectiles[i].GetComponent<Projectile>().StartProjectile(vect);
            projectiles[i].SetActive(true);
            PlayAnimation(proj.castingAnimationName);
            // Set projectile particles
            List<GameObject> particles = new List<GameObject>();
            for(int j = 0; j < proj.particleNames.Length; j++) {
                particles.Add(ObjectPooler.objectPooler.GetPooledObject(proj.particleNames[j]));
                particles[j].transform.parent = projectiles[i].transform;
                if(particles[j].GetComponent<TrailRenderer>()) {
                    particles[j].GetComponent<TrailRenderer>().emitting = false;
                }
                particles[j].transform.position = projectiles[i].transform.position;
                particles[j].transform.rotation = Quaternion.identity;
                particles[j].SetActive(true);
                if(particles[j].GetComponent<TrailRenderer>()) {
                    particles[j].GetComponent<TrailRenderer>().emitting = true;
                }
                if(particles[j].GetComponent<ParticleSystem>()) {
                    particles[j].GetComponent<ParticleSystem>().Play();
                }
            }
            // Wait for next projectile
            yield return new WaitForSeconds(proj.projectileData.timeDifference);
        }
        yield return new WaitForSeconds(proj.castTime);
        StopAnimation(proj);
    }
    private IEnumerator Buff(BuffSkill buff) {
        // Set status effects
        statusEffects.StartChanelling(buff.channelingTime);
        if(buff.focusedSkill) {
            statusEffects.StartImmobilize(buff.channelingTime + buff.castTime);
        }
        // Set buffer game object
        skillCooldowns[buff.skillIndex] = buff.cooldown;
        GameObject b = ObjectPooler.objectPooler.GetPooledObject("Buff");
        b.GetComponent<Buff>().SetBuff(buff.buffData);
        b.transform.position = transform.position;
        b.transform.rotation = Quaternion.identity;
        b.SetActive(true);
        // Set channelling particles
        List<GameObject> channelingParticles = new List<GameObject>();
        for(int i = 0; i < buff.channelingParticleNames.Length; i++) {
            channelingParticles.Add(ObjectPooler.objectPooler.GetPooledObject(buff.channelingParticleNames[i]));
            channelingParticles[i].transform.parent = transform;
            channelingParticles[i].transform.position = transform.position;
            channelingParticles[i].transform.rotation = Quaternion.identity;
            channelingParticles[i].SetActive(true);
            channelingParticles[i].GetComponent<ParticleSystem>().Play();
        }
        PlayAnimation(buff.channelingAnimationName);
        yield return new WaitForSeconds(buff.channelingTime);
        buff.Launch(statusEffects, stats);
        // Set casting particles
        List<GameObject> castingParticles = new List<GameObject>();
        for(int i = 0; i < buff.castingParticleNames.Length; i++) {
            castingParticles.Add(ObjectPooler.objectPooler.GetPooledObject(buff.castingParticleNames[i]));
            castingParticles[i].transform.parent = transform;
            castingParticles[i].transform.position = transform.position;
            castingParticles[i].transform.rotation = Quaternion.identity;
            castingParticles[i].SetActive(true);
            castingParticles[i].GetComponent<ParticleSystem>().Play();
        }
        PlayAnimation(buff.castingAnimationName);
        yield return new WaitForSeconds(buff.castTime);
        // Set particles
        List<GameObject> particles = new List<GameObject>();
        for(int i = 0; i < buff.particleNames.Length; i++) {
            particles.Add(ObjectPooler.objectPooler.GetPooledObject(buff.particleNames[i]));
            //particles[i] = ObjectPooler.objectPooler.GetPooledObject(proj.projectileData.particleNames[i]);
            particles[i].transform.parent = transform;
            particles[i].transform.position = transform.position;
            particles[i].transform.rotation = Quaternion.identity;
            particles[i].SetActive(true);
            particles[i].GetComponent<ParticleSystem>().Play();
        }        
        StopAnimation(buff);
    }
    private IEnumerator StartSkillSequence(SkillSequence seq) {
        skillCooldowns[seq.skillIndex] = seq.cooldown;
        for(int i = 0; i < seq.sequence.Count; i++) {
            UseSkill(seq.sequence[i].activeSkill);
            yield return new WaitForSeconds(seq.sequence[i].activeSkill.channelingTime + seq.sequence[i].activeSkill.castTime);
        }
    }
    private IEnumerator StartDash(DashSkill dash) {
        statusEffects.StartChanelling(dash.channelingTime);
        if(dash.focusedSkill) {
            statusEffects.StartImmobilize(dash.channelingTime + dash.castTime);
        }
        skillCooldowns[dash.skillIndex] = dash.cooldown;
        // Set channelling particles
        List<GameObject> channelingParticles = new List<GameObject>();
        for(int i = 0; i < dash.channelingParticleNames.Length; i++) {
            channelingParticles.Add(ObjectPooler.objectPooler.GetPooledObject(dash.channelingParticleNames[i]));
            channelingParticles[i].transform.parent = transform;
            channelingParticles[i].transform.position = transform.position;
            channelingParticles[i].transform.rotation = Quaternion.identity;
            channelingParticles[i].SetActive(true);
            channelingParticles[i].GetComponent<ParticleSystem>().Play();
        }
        PlayAnimation(dash.channelingAnimationName);
        yield return new WaitForSeconds(dash.channelingTime);
        List<GameObject> castingParticles = new List<GameObject>();
        for(int i = 0; i < dash.castingParticleNames.Length; i++) {
            castingParticles.Add(ObjectPooler.objectPooler.GetPooledObject(dash.castingParticleNames[i]));
            castingParticles[i].transform.parent = transform;
            castingParticles[i].transform.position = transform.position;
            castingParticles[i].transform.rotation = Quaternion.identity;
            castingParticles[i].SetActive(true);
            castingParticles[i].GetComponent<ParticleSystem>().Play();
        }
        PlayAnimation(dash.castingAnimationName);
        dash.Launch(statusEffects, stats);
        yield return new WaitForSeconds(dash.castTime);
        List<GameObject> particles = new List<GameObject>();
        for(int i = 0; i < dash.particleNames.Length; i++) {
            particles.Add(ObjectPooler.objectPooler.GetPooledObject(dash.particleNames[i]));
            //particles[i] = ObjectPooler.objectPooler.GetPooledObject(proj.projectileData.particleNames[i]);
            particles[i].transform.parent = transform;
            particles[i].transform.position = transform.position;
            particles[i].transform.rotation = Quaternion.identity;
            particles[i].SetActive(true);
            particles[i].GetComponent<ParticleSystem>().Play();
        }        
        StopAnimation(dash);
    }
    private IEnumerator StartAreaSkill(AreaSkill area) {
        statusEffects.StartChanelling(area.channelingTime);
        if(area.focusedSkill) {
            statusEffects.StartImmobilize(area.channelingTime + area.castTime);
        }
        skillCooldowns[area.skillIndex] = area.cooldown;
        // Set channelling particles
        List<GameObject> channelingParticles = new List<GameObject>();
        for(int i = 0; i < area.channelingParticleNames.Length; i++) {
            channelingParticles.Add(ObjectPooler.objectPooler.GetPooledObject(area.channelingParticleNames[i]));
            channelingParticles[i].transform.parent = transform;
            channelingParticles[i].transform.position = transform.position;
            channelingParticles[i].transform.rotation = Quaternion.identity;
            channelingParticles[i].SetActive(true);
            channelingParticles[i].GetComponent<ParticleSystem>().Play();
        }
        PlayAnimation(area.channelingAnimationName);
        yield return new WaitForSeconds(area.channelingTime);
        List<GameObject> castingParticles = new List<GameObject>();
        for(int i = 0; i < area.castingParticleNames.Length; i++) {
            castingParticles.Add(ObjectPooler.objectPooler.GetPooledObject(area.castingParticleNames[i]));
            castingParticles[i].transform.parent = transform;
            castingParticles[i].transform.position = transform.position;
            castingParticles[i].transform.rotation = Quaternion.identity;
            castingParticles[i].SetActive(true);
            castingParticles[i].GetComponent<ParticleSystem>().Play();
        }
        PlayAnimation(area.castingAnimationName);
        if(area.hitbox) {
            area.Launch(stats);
        }
        yield return new WaitForSeconds(area.castTime);
        List<GameObject> particles = new List<GameObject>();
        for(int i = 0; i < area.particleNames.Length; i++) {
            particles.Add(ObjectPooler.objectPooler.GetPooledObject(area.particleNames[i]));
            //particles[i] = ObjectPooler.objectPooler.GetPooledObject(proj.projectileData.particleNames[i]);
            particles[i].transform.parent = transform;
            particles[i].transform.position = transform.position;
            particles[i].transform.rotation = Quaternion.identity;
            particles[i].SetActive(true);
            particles[i].GetComponent<ParticleSystem>().Play();
        }                
        StopAnimation(area);
    }
    private IEnumerator StartBlockSkill(BlockSkill block) {
        statusEffects.StartChanelling(block.channelingTime);
        if(block.focusedSkill) {
            statusEffects.StartImmobilize(block.channelingTime + block.castTime);
        }
        skillCooldowns[block.skillIndex] = block.cooldown;
        // Set channelling particles
        List<GameObject> channelingParticles = new List<GameObject>();
        for(int i = 0; i < block.channelingParticleNames.Length; i++) {
            channelingParticles.Add(ObjectPooler.objectPooler.GetPooledObject(block.channelingParticleNames[i]));
            channelingParticles[i].transform.parent = transform;
            channelingParticles[i].transform.position = transform.position;
            channelingParticles[i].transform.rotation = Quaternion.identity;
            channelingParticles[i].SetActive(true);
            channelingParticles[i].GetComponent<ParticleSystem>().Play();
        }
        PlayAnimation(block.channelingAnimationName);
        yield return new WaitForSeconds(block.channelingTime);
        List<GameObject> castingParticles = new List<GameObject>();
        for(int i = 0; i < block.castingParticleNames.Length; i++) {
            castingParticles.Add(ObjectPooler.objectPooler.GetPooledObject(block.castingParticleNames[i]));
            castingParticles[i].transform.parent = transform;
            castingParticles[i].transform.position = transform.position;
            castingParticles[i].transform.rotation = Quaternion.identity;
            castingParticles[i].SetActive(true);
            castingParticles[i].GetComponent<ParticleSystem>().Play();
        }
        PlayAnimation(block.castingAnimationName);
        block.Launch(statusEffects);
        yield return new WaitForSeconds(block.castTime);
        List<GameObject> particles = new List<GameObject>();
        for(int i = 0; i < block.particleNames.Length; i++) {
            particles.Add(ObjectPooler.objectPooler.GetPooledObject(block.particleNames[i]));
            //particles[i] = ObjectPooler.objectPooler.GetPooledObject(proj.projectileData.particleNames[i]);
            particles[i].transform.parent = transform;
            particles[i].transform.position = transform.position;
            particles[i].transform.rotation = Quaternion.identity;
            particles[i].SetActive(true);
            particles[i].GetComponent<ParticleSystem>().Play();
        }
        StopAnimation(block);
    }
    private void PlayAnimation(string animationName) {
        if(!string.IsNullOrEmpty(animationName)) {
            animator.SetTrigger(animationName);
        }        
    }
    private void StopAnimation(ActiveSkill activeSkill) {
        if(!string.IsNullOrEmpty(activeSkill.idleAnimationName)) {
            animator.SetTrigger(activeSkill.idleAnimationName);
        }        
    }
    private void CountCooldowns() {
        for(int i = 0; i < skillCooldowns.Count; i++) {
            if(skillCooldowns[i] <= 0) {
                continue;                
            }
            if(skillCooldowns[i] > 0) {
                skillCooldowns[i] -= Time.deltaTime;
                if(skillDatabase.skills[i] is ProjectileSkill proj && skillCooldowns[i] <= 0) {
                    if(proj.projectileData.arrowSkill) {
                        for(int j = 0; j < player.tenseBowStrings.Length; j++) {
                            player.tenseBowStrings[j].SetActive(true);
                        }
                        player.releasedBowString.SetActive(false);
                    }                    
                }
            }            
        }        
    }
}
