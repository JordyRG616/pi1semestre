using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : ActionController
{
    private DroneMovement movement;
    private WaitUntil waitDistance;
    private WaitForSecondsRealtime waitTime;

    public void StartComponent()
    {
        StartCoroutine(ManageActivation());
    }

    public void Configure(float level)
    {
        movement = GetComponent<DroneMovement>();

        GetTarget();

        var damage = shooters[0].StatSet[Stat.Damage];
        shooters[0].SetStat(Stat.Damage, damage * level);
        var rest = shooters[0].StatSet[Stat.Rest];
        shooters[0].SetStat(Stat.Rest, rest + (level/10));

        waitTime = new WaitForSecondsRealtime(shooters[0].StatSet[Stat.Rest]);
        waitDistance = new WaitUntil(() => Vector2.Distance(transform.position, target.transform.position) <= movement.GetDistance());
    }

    public override void Activate()
    {
        foreach(ActionEffect shooter in shooters)
        {
            shooter.Shoot();
        }
    }

    private void GetTarget()
    {
        target = movement.GetTarget().GetComponent<EnemyManager>();
        shooters[0].ReceiveTarget(target.gameObject);
    }

    protected override IEnumerator ManageActivation()
    {
        while(true)
        {
            yield return waitDistance;

            Activate();

            yield return waitTime;

        }
    }

    public void Stop()
    {
        StopShooters();
    }
}