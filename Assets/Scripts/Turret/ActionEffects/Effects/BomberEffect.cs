using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class BomberEffect : ActionEffect
{
    [SerializeField] [FMODUnity.EventRef] private string explosionSFX;
    [SerializeField] private float initialProjectiles;
    [SerializeField] private float initalBulletSize;
    [SerializeField] private ParticleSystem subShooter;
    [SerializeField] private ActionEffect fragEffect;
    

    public override Stat specializedStat => Stat.Projectiles;

    public override Stat secondaryStat => Stat.Size;

    public override void Initiate()
    {
        base.Initiate();

        fragEffect.Initiate();
    }

    public override void SetData()
    {
        StatSet.Add(Stat.Projectiles, initialProjectiles);
        SetProjectiles();
        StatSet.Add(Stat.Size, initalBulletSize);
        SetBulletSize();
        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetProjectiles();
        SetBulletSize();
    }

    private void SetProjectiles()
    {
        var emission = subShooter.emission;
        ParticleSystem.Burst burst = new ParticleSystem.Burst(0.0001f, StatSet[Stat.Projectiles]);
        emission.SetBurst(0, burst);
    }

    private void SetBulletSize()
    {
        var main = shooterParticle.main;
        main.startSize = StatSet[Stat.Size];
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        // hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    protected override void PlaySFX()
    {
        base.PlaySFX();
        // Invoke("PlayExplosion", shooterParticle.main.startLifetime.constant + shooterParticle.main.startDelay.constant);

    }

    private void PlayExplosion()
    {
        AudioManager.Main.RequestSFX(explosionSFX);
    }

    public override string DescriptionText()
    {
        string description = "shoots two bombs that explodes in " + StatColorHandler.StatPaint(StatSet[Stat.Projectiles].ToString()) + " projectiles that deals " + StatColorHandler.DamagePaint(fragEffect.StatSet[Stat.Damage].ToString()) + " damage on hit, each";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " damage + 50%";
        else return StatColorHandler.StatPaint("next level:") + " projectiles + 1";
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 5) GainDamage();
        else GainProjectile();
    }

    private void GainProjectile()
    {
        var projectiles = StatSet[Stat.Projectiles];
        projectiles += 1;
        SetStat(Stat.Projectiles, projectiles);
    }

    public void GainDamage()
    {
        var damage =fragEffect.StatSet[Stat.Damage];
        damage *= 1.5f;
        fragEffect.SetStat(Stat.Damage, damage);
    }
}