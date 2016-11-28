using UnityEngine;
using System.Collections;

public class GoapCollectMeatFromMeatTableAction : GoapAction
{
    public int maxMeatToCollect = 1;

    private MeatTable m_TargetStack;
    private bool m_Collected = false;
    private Inventory m_Inventory;

    void Awake()
    { 
        m_Inventory = GetComponent<Inventory>();
    }

    protected override void Start()
    {
        base.Start();

        AddPrecondition("hasMeat", false);
        AddEffect("hasMeat", true);
    }

    protected override void DoReset()
    {
        m_Collected = false;
    }

    public override bool IsDone()
    {
        return m_Collected;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void SetTarget()
    {
        var closest = GetClosest();
        if (closest != null)
        {
            m_TargetStack = closest;
            target = m_TargetStack.transform;
        }
    }

    public override bool CheckProceduralPrecondition()
    {
        var tables = COMPONENT_DATABASE.RetrieveComponents<MeatTable>();

        foreach(var s in tables)
        {
            var table = (MeatTable)s;

            if(table.count > 0)
            {
                return true;
            }
        }

        return false;
    }

    public override bool Perform()
    {

      int woodCount = Mathf.Min(maxMeatToCollect, m_TargetStack.count);

      m_Inventory.IncrementResourceCount(ResourceType.Meat, woodCount);

      m_TargetStack.count -= woodCount;

      m_Collected = true;
        

        return true;
    }


    private MeatTable GetClosest()
    {
        var stacks = COMPONENT_DATABASE.RetrieveComponents<MeatTable>();

        if (stacks.Count == 0)
        {
            return null;
        }

        MeatTable closest = null;
        float closestDist = float.MaxValue;

        foreach (var s in stacks)
        {

            var stack = (MeatTable)s;

            if (stack.count <= 0)
            {
                continue;
            }

            float dist = (stack.gameObject.transform.position - transform.position).magnitude;

            if (dist < closestDist)
            {
                closest = stack;
                closestDist = dist;
            }
        }

        return closest;
    }

}
