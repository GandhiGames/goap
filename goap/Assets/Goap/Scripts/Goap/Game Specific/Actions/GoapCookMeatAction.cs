using UnityEngine;
using System.Collections;
using System;

public class GoapCookMeatAction : GoapAction
{
    public float cookDurationSecs = 5f;

    private CookingStation m_TargetStation;
    private bool m_Cooked = false;
    private float m_StartTime = 0;
    private GoapLabourerAnimator m_Animator;
    private Inventory m_Inventory;

    void Awake()
    {
        m_Animator = GetComponent<GoapLabourerAnimator>();
        m_Inventory = GetComponent<Inventory>();
    }

    protected override void Start()
    {
        base.Start();

        AddPrecondition("hasMeat", true);
        AddEffect("hasCookedMeat", true);
    }

    protected override void DoReset()
    {
        m_TargetStation = null;
        m_Cooked = false;
        m_StartTime = 0f;
    }

    public override bool IsDone()
    {
        return m_Cooked;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override bool Perform()
    {
        if (m_StartTime == 0)
        {
            m_StartTime = Time.time;
            m_Animator.PlayThrust();
        }


        if (Time.time - m_StartTime > cookDurationSecs)
        {
            m_Animator.StopThrust();

            m_Inventory.IncrementResourceCount(ResourceType.Meat, -1);

            m_Inventory.IncrementResourceCount(ResourceType.CookedMeat, 1);

            m_Cooked = true;
        }

        return true;
    }

    public override void SetTarget()
    {
        var closest = GetClosest();

        if (closest != null)
        {
            m_TargetStation = closest;
            target = m_TargetStation.transform;
        }
    }

    public override bool CheckProceduralPrecondition()
    {
        return COMPONENT_DATABASE.RetrieveComponents<CookingStation>().Count > 0;
    }

    private CookingStation GetClosest()
    {
        var stations = COMPONENT_DATABASE.RetrieveComponents<CookingStation>();

        if (stations.Count == 0)
        {
            return null;
        }

        CookingStation closest = null;
        float closestDist = float.MaxValue;

        foreach (var tree in stations)
        {
            float dist = (tree.gameObject.transform.position - transform.position).magnitude;

            if (dist < closestDist)
            {
                closest = (CookingStation)tree;
                closestDist = dist;
            }
        }

        return closest;
    }
}
