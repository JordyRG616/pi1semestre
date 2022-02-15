using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour, IManager
{
    
    public BaseEffectTemplate baseEffect {get; private set;}
    public ActionController actionController {get; private set;}
    public Dictionary<Stat, float> Stats {get; protected set;} = new Dictionary<Stat, float>();
    public IntegrityManager integrityManager {get; private set;}

    public int maxLevel = 5;
    public int Level 
    {
        get
        {
            return _level;
        }
        private set
        {
            if(value > 5) value = 5;
            _level = value;
        }
    }
    private int _level = 0;

    public event EventHandler<LevelUpArgs> OnLevelUp;


    public void Initiate()
    {
        baseEffect = GetComponentInChildren<BaseEffectTemplate>();
        actionController = GetComponentInChildren<ActionController>();

        OnLevelUp += actionController.HandleLevelUp;

        GetStats();
        
        integrityManager = GetComponent<IntegrityManager>();
        integrityManager.Initiate(Stats[Stat.Health]);
    }

    private void GetStats()
    {
        Stats.Add(Stat.Cost, baseEffect.GetCost() + actionController.GetCost());
        Stats.Add(Stat.Health, actionController.GetHealth());

        var stats = actionController.GetShooters()[0].StatSet;

        foreach(Stat stat in stats.Keys)
        {
            Stats.Add(stat, stats[stat]);
        }

    }

    public void LevelUp()
    {
        Level ++;
        actionController.RaiseHealthByPercentage(.1f);
        OnLevelUp?.Invoke(this, new LevelUpArgs(Level));
    }

    public void DestroyManager()
    {
        GetComponentInParent<ShipManager>().RemoveTurret(this);
    }
}

public class LevelUpArgs : EventArgs
{
    public int toLevel;

    public LevelUpArgs(int level)
    {
        toLevel = level;
    }
}