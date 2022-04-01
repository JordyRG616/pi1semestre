using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class ShellEffect : ActionEffect
{
    [SerializeField] private float initialBulletSize;
    [SerializeField] private float initialBurstSize;

    public override Stat specializedStat => Stat.BurstSize;

    public override Stat secondaryStat => Stat.Size;

    public override void SetData()
    {
        StatSet.Add(Stat.Size, initialBulletSize);
        SetBulletSize();
        StatSet.Add(Stat.BurstSize, initialBurstSize);
        SetBurstSize();

        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetBulletSize();
        SetBurstSize();
    }

    private void SetBulletSize()
    {
        var main = shooterParticle.main;
        main.startSize = StatSet[Stat.Size];
    }

    private void SetBurstSize()
    {
        var module = shooterParticle.emission;
        var burst = module.GetBurst(0);
        burst.cycleCount = (int)StatSet[Stat.BurstSize];
        module.SetBurst(0, burst);
        // main.duration = ;
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
    }

    public override string DescriptionText()
    {
        string description = "shoots " + StatColorHandler.StatPaint(StatSet[Stat.BurstSize].ToString()) + " burst of bullets. Each bullet deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " damage on hit";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) 
            return StatColorHandler.StatPaint("next level:") + " +1 burst";

        else 
            return StatColorHandler.StatPaint("next level:") + " damage +15%";
    }

    public override void Shoot()
    {
        AudioManager.Main.RequestSFX(onShootSFX, out var sfxInstance);
        shooterParticle.Play();
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 3 || toLevel == 5) 
            GainBurst();
        else 
            GainDamage();
    }

    private void GainBurst()
    {
        var projectiles = StatSet[Stat.BurstSize];
        projectiles += 1;
        SetStat(Stat.BurstSize, projectiles);
    }

    public void GainDamage()
    {
        var damage = StatSet[Stat.Damage];
        damage *= 1.15f;
        SetStat(Stat.Damage, damage);
    }
}