using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecallAction : BossAction
{
    [SerializeField] private Transform recallPosition;
    [Header("Effects")]
    [SerializeField] private ParticleSystem trackBeam;
    [SerializeField] [FMODUnity.EventRef] private string beamSFX;
    [SerializeField] private ParticleSystem levelUpVFX;
    [SerializeField] [FMODUnity.EventRef] private string levelUpSFX;
    private float _ogDuration;
    private float vfxTime;
    private WaitForSeconds waitTime;
    private ForgeController forgeController;

    void Start()
    {
        forgeController = GetComponent<ForgeController>();
        vfxTime = levelUpVFX.main.duration - .5f;
        waitTime = new WaitForSeconds(0.01f);
        _ogDuration = actionDuration;
    }

    

    private IEnumerator Recall(Transform formationHead)
    {
        var formation = formationHead.parent.GetComponent<FormationManager>();

        formationHead.GetComponent<Rigidbody2D>().Sleep();

        foreach(EnemyManager enemy in formation.Children)
        {
            var _body = enemy.GetComponent<Rigidbody2D>();
            _body.Sleep();
            var attackController = enemy.attackController;
            attackController.Sleeping = true;
        }

        var position = formationHead.position;
        float step = 0;

        trackBeam.transform.position = position;
        trackBeam.Play();
        AudioManager.Main.RequestSFX(beamSFX, out var instance);

        while(step <= 2)
        {
            if(formationHead == null) StopAllCoroutines();
            var newPos = Vector2.Lerp(position, recallPosition.position, step / 2);
            formationHead.position = newPos;
            trackBeam.transform.position = formationHead.position;
            step += 0.01f;
            yield return waitTime;
        }

        trackBeam.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        AudioManager.Main.StopSFX(instance);
        levelUpVFX.Play();
        AudioManager.Main.RequestSFX(levelUpSFX, out instance);

        step = 0;

        while(step <= vfxTime)
        {
            formationHead.position = recallPosition.position;
            step += 0.01f;
            yield return waitTime;
        }
        
        AudioManager.Main.StopSFX(instance);

        formationHead.GetComponent<Rigidbody2D>().WakeUp();

        foreach(EnemyManager enemy in formation.Children)
        {
            enemy.AdjustLevel(enemy.level + 1);
            enemy.GetComponent<Rigidbody2D>().WakeUp();
            enemy.GetComponent<EnemyAttackController>().Sleeping = false;
        }
    }


    public override void StartAction()
    {
        actionDuration = _ogDuration;
        if(forgeController.Children.Count == 0) 
        {
            actionDuration = .1f;
            return;
        }
        var childs = forgeController.Children.FindAll(x => x.Children[0].GetComponent<EnemyHealthController>().GetHealthPercentage() >= 0.7f);
        var rdm = UnityEngine.Random.Range(0, childs.Count);
        var child = childs[rdm];
        if(child == null) 
        {
            actionDuration = .1f;
            return;
        }
        var head = child.transform.Find("Head");
        StartCoroutine(Recall(head));
    }

    public override void Action()
    {

    }

    public override void EndAction()
    {

    }

    public override void DoActionMove()
    {

    }
}
