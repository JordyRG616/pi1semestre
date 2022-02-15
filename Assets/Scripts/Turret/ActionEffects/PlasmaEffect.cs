using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class PlasmaEffect : ActionEffect
{
    [SerializeField] private float duration;
    [SerializeField] private float initialBulletSpeed;
    private FMOD.Studio.EventInstance instance;

    public override void SetData()
    {
        StatSet.Add(Stat.Duration, duration);
        SetDuration();
        StatSet.Add(Stat.BulletSpeed, initialBulletSpeed);
        SetBulletSpeed();

        base.SetData();
    }

    public override void SetStat(Stat statName, float value)
    {
        base.SetStat(statName, value);
        SetBulletSpeed();
        SetDuration();
    }

    private void SetDuration()
    {
        var main = shooterParticle.main;
        main.duration = StatSet[Stat.Duration];
    }

    private void SetBulletSpeed()
    {
        var main = shooterParticle.main;
        main.startSpeed = StatSet[Stat.BulletSpeed];
    }

    public override void Shoot()
    {
        // StartCoroutine(PlaySFX(StatSet[Stat.Duration]));
        AudioManager.Main.RequestSFX(onShootSFX, out sfxInstance);
        shooterParticle.Play();
    }

    private IEnumerator PlaySFX(float duration)
    {
        AudioManager.Main.RequestSFX(onShootSFX, out var instance);

        yield return new WaitForSeconds(duration + 1);

        AudioManager.Main.StopSFX(instance);
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        hitManager.HealthInterface.UpdateHealth(-StatSet[Stat.Damage]);
        ApplyStatusEffect<Slug>(hitManager, 2f, new float[] {.66f});
    }

    public override string DescriptionText()
    {
        string description = "releases a stream of particles for " + StatColorHandler.StatPaint(StatSet[Stat.Duration].ToString()) + " seconds that deals " + StatColorHandler.DamagePaint(StatSet[Stat.Damage].ToString()) + " to all enemies in the area and applies " + KeywordHandler.KeywordPaint(keyword);
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " bullet speed +10";
        else return StatColorHandler.StatPaint("next level:") + " duration + 25%";
        
    }

    public override void LevelUp(int toLevel)
    {
        if(toLevel == 3 || toLevel == 5)
        {
            GainSpeed();
        }
        else
        {
            GainDuration();
        }
    }

    private void GainDuration()
    {
        var _duration = StatSet[Stat.Duration];
        _duration *= 1.25f;
        SetStat(Stat.Duration, _duration);
    }

    private void GainSpeed()
    {
        var speed = StatSet[Stat.BulletSpeed];
        speed += 10f;
        SetStat(Stat.BulletSpeed, speed);
    }
}
