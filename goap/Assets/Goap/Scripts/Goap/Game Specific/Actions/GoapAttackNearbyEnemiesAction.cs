using UnityEngine;
using System.Collections;

public class GoapAttackNearbyEnemiesAction : GoapAction
{
    public float hitTime = 1f;

    private EnemyHealth m_TargetEnemy;
    private float m_StartTime = 0f;
    private bool m_Killed = false;
    private GoapLabourerAnimator m_Animator;

    void Awake()
    {
        m_Animator = GetComponent<GoapLabourerAnimator>();
    }

    protected override void Start()
    {
        base.Start();

        // AddPrecondition("enemyNearby", true);
        AddPrecondition("hasPatrolled", true);
        AddEffect("areaSafe", true);
    }

    protected override void DoReset()
    {
        m_StartTime = 0f;
        m_Killed = false;
    }

    public override bool IsDone()
    {
        return m_Killed;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void SetTarget()
    {
        // Stops thrusting animation (used when 
        // another NPC kills the target enemy and Perform is not called).
        m_Animator.StopThrust();

        var closest = GetClosest();
        if (closest != null)
        {
            m_TargetEnemy = closest;
            target = m_TargetEnemy.transform;
        }
    }

    public override bool CheckProceduralPrecondition()
    {
        return true;// COMPONENT_DATABASE.RetrieveComponents<EnemyHealth>().Count > 0;
    }

    public override bool Perform()
    {
        // Someone else killed the enemy
        if(m_TargetEnemy == null || m_TargetEnemy.IsDead)
        {
            m_Killed = true;
            m_Animator.StopThrust();
            return true;
        }

        if(m_StartTime == 0f)
        {
            m_Animator.PlayThrust();
            m_StartTime = Time.time;
        }

        if (Time.time - m_StartTime > hitTime)
        {
            m_TargetEnemy.DoDamage();

            if(m_TargetEnemy == null || m_TargetEnemy.IsDead)
            {
                m_Animator.StopThrust();
                m_Killed = true;
            }

            m_StartTime = 0f;
        }

         return true;
    }


    private EnemyHealth GetClosest()
    {
        var stacks = COMPONENT_DATABASE.RetrieveComponents<EnemyHealth>();

        if (stacks.Count == 0)
        {
            return null;
        }

        WorldComponent closest = null;
        float closestDist = float.MaxValue;

        foreach (var stack in stacks)
        {

            float dist = (stack.gameObject.transform.position - transform.position).magnitude;

            if (dist < closestDist)
            {
                closest = stack;
                closestDist = dist;
            }
        }

        return (EnemyHealth)closest;
    }

}
