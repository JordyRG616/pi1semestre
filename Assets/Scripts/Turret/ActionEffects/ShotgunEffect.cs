using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunEffect : ActionEffect
{
    [SerializeField] private float initialbulletSpeed;
    [SerializeField] private float initialProjectiles;

    protected override void SetData()
    {
        StatSet.Add(Stat.BulletSpeed ,initialbulletSpeed);
        SetBulletSpeed();
        StatSet.Add(Stat.Projectiles, initialProjectiles);
        SetProjectileCount();
    
        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetBulletSpeed();
        SetProjectileCount();
        
    }

    private void SetProjectileCount()
    {
        var newBurst = new ParticleSystem.Burst(0.0001f, StatSet[Stat.Projectiles]);
        shooter.emission.SetBurst(0, newBurst);

    }

    private void SetBulletSpeed()
    {
        var main = shooter.main;
        main.startSpeed = StatSet[Stat.BulletSpeed];
    }


    public override void ApplyEffect(HitManager hitManager)
    {        
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    public override string DescriptionText()
    {
        string description = "Shoots " + StatSet[Stat.Projectiles] + " bullets that deals " + StatSet[Stat.Damage] + " damage each.";
        return description;
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 3 || toLevel == 5) GainProjectile();
        else GainDamage();
    }

    private void GainDamage()
    {
        var damage = StatSet[Stat.Damage];
        damage *= 1.2f;
        SetStat(Stat.Damage, damage);
    }

    private void GainProjectile()
    {
        var projectiles = StatSet[Stat.Projectiles];
        projectiles += 1;
        SetStat(Stat.Projectiles, projectiles);
    }
}
