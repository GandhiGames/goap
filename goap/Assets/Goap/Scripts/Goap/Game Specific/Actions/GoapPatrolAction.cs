using UnityEngine;
using System.Collections;
using System;

public class GoapPatrolAction : GoapAction
{
    public Transform[] patrolLocations;

    private int m_PatrolIndex;
    private bool m_CompletedPatrol;

    protected override void Start()
    {
        base.Start();

        m_PatrolIndex = 0;

        AddEffect("hasPatrolled", true);
    }

    protected override void DoReset()
    {
        m_CompletedPatrol = false;
    }

    public override bool IsDone()
    {
        return m_CompletedPatrol;
    }

    public override bool RequiresInRange()
    {
        return COMPONENT_DATABASE.RetrieveComponents<EnemyHealth>().Count > 0 ? false : true;
    }

    public override void SetTarget()
    {
        target = patrolLocations[m_PatrolIndex];

        m_PatrolIndex = (m_PatrolIndex + 1) % patrolLocations.Length;
    }

    public override bool CheckProceduralPrecondition()
    {
        return true;
    }

    public override bool Perform()
    {
        m_CompletedPatrol = true;

        return true;
    }
}
