using UnityEngine;
using System.Collections.Generic;

public interface Tool
{
    string name { get; }
    void Damage();
    bool IsDestroyed();
}

public class WoodenAxe : Tool
{
    public string name { get; private set; }
    private int m_Durability;
    private int m_DamagePercentPerUse;

    public WoodenAxe()
    {
        name = ToolType.WoodenAxe.ToString();
        m_DamagePercentPerUse = 34;
        m_Durability = 100;
    }

    public void Damage()
    {
        m_Durability -= m_DamagePercentPerUse;
    }

    public bool IsDestroyed()
    {
        return m_Durability <= 0;
    }
}

public enum ToolType
{
    WoodenAxe
}

[RequireComponent(typeof(Inventory))]
public class ToolDispenser : WorldComponent
{
    private Dictionary<ToolType, Resource> m_Tools;

    void Start()
    {
        m_Tools = new Dictionary<ToolType, Resource>();
    }

    void OnEnable()
    {
        COMPONENT_DATABASE.Register<ToolDispenser>(this);
    }

    void OnDisable()
    {
        COMPONENT_DATABASE.UnRegister<ToolDispenser>(this);
    }

    public void IncrementToolCount(ToolType toolType, int count)
    {
        if (m_Tools.ContainsKey(toolType))
        {
            m_Tools[toolType].count += count;
        }
        else
        {
            m_Tools.Add(toolType, new Resource(count));
        }
    }

    public int GetResourceCount(ToolType toolType)
    {
        if (m_Tools.ContainsKey(toolType))
        {
            return m_Tools[toolType].count;
        }

        return 0;
    }

    public Tool RetrieveWoodenAxe()
    {
        if (GetResourceCount(ToolType.WoodenAxe) > 0)
        {
            IncrementToolCount(ToolType.WoodenAxe, -1);

            return new WoodenAxe();
        }

        return null;
    }
}
