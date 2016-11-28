using UnityEngine;
using System.Collections;

public class GoapDepositMeatToMeatTableAction : GoapAction
{

    private bool m_DepositedMeat = false;
    private MeatTable m_TargetDeposit;
    private Inventory m_Inventory;

    void Awake()
    {
        m_Inventory = GetComponent<Inventory>();
    }

    protected override void Start()
    {
        base.Start();

        AddPrecondition("hasMeat", true); // can't drop off firewood if we don't already have some
        AddEffect("hasMeat", false); // we now have no firewood
        AddEffect("collectMeat", true); // we collected firewood
    }

    protected override void DoReset()
    {
        m_DepositedMeat = false;
        m_TargetDeposit = null;
    }

    public override bool IsDone()
    {
        return m_DepositedMeat;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void SetTarget()
    {
        MeatTable closest = GetClosest();

        if (closest != null)
        {
            m_TargetDeposit = closest;
            target = m_TargetDeposit.transform;
        }
    }

    public override bool CheckProceduralPrecondition()
    {
        return COMPONENT_DATABASE.RetrieveComponents<MeatTable>().Count > 0;
    }

    public override bool Perform()
    {
        m_TargetDeposit.count += m_Inventory.GetResourceCount(ResourceType.Meat);
        m_Inventory.SetResourceCount(ResourceType.Meat, 0);
        m_DepositedMeat = true;

        return true;
    }

    private MeatTable GetClosest()
    {
        var tables = COMPONENT_DATABASE.RetrieveComponents<MeatTable>();

        MeatTable closest = null;
        float closestDist = float.MaxValue;

        foreach (var tree in tables)
        {
            float dist = (tree.gameObject.transform.position - transform.position).magnitude;

            if (dist < closestDist)
            {
                closest = (MeatTable)tree;
                closestDist = dist;
            }
        }

        return closest;
    }
}
