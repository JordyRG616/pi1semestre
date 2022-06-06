using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StringHandler;

public class SpawnerEffect : DeployerActionEffect
{
    [SerializeField] private float droneLevel;

    public override Stat specializedStat => Stat.DroneLevel;

    public override Stat secondaryStat => Stat.Capacity;


    public override void SetData()
    {
        StatSet.Add(Stat.Capacity, capacity);
        StatSet.Add(Stat.DroneLevel, droneLevel);

        base.SetData();
    }

    public override void Initiate()
    {
        base.Initiate();
    }

    public override void Shoot()
    {
        if(pool.GetDeployedObjectCount() == StatSet[Stat.Capacity]) return;
        var deployable = pool.RequestDeployable();
        deployable.transform.position = transform.position;
        deployable.Born();
        var droneShooter = deployable.GetComponent<ActionEffect>();
        droneShooter.Initiate();
        droneShooter.SetStat(Stat.Damage, StatSet[Stat.DroneLevel] * 5);
        pool.GetModel().SetLifetime((StatSet[Stat.DroneLevel] * 10) + 15);
    }

    public override void ApplyEffect(HitManager hitManager)
    {
        
    }

    public override string DescriptionText()
    {
        string description = "spawn up to " + StatColorHandler.StatPaint(StatSet[Stat.Capacity].ToString()) + " drones of level " + StatColorHandler.StatPaint(StatSet[Stat.DroneLevel].ToString()) + ". (Deals " + StatColorHandler.DamagePaint(StatSet[Stat.DroneLevel] * 5) + " damage and lasts for " + StatColorHandler.StatPaint((StatSet[Stat.DroneLevel] * 10) + 15) + " seconds)";
        return description;
    }

    public override string upgradeText(int nextLevel)
    {
        if(nextLevel == 3 || nextLevel == 5) return StatColorHandler.StatPaint("next level:") + " capacity + 1";
        if(nextLevel == 2 || nextLevel == 4) return StatColorHandler.StatPaint("next level:") + " drone level + 1";
        return "no bonus next level";
    }

    public override void LevelUp(int toLevel)
    {
    }
}