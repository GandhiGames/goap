using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Inventory))]
public class ToolDispenser : MonoBehaviour
{
    public int woodenAxeCount
    {
        get
        {
            return m_Inventory.GetResourceCount(ResourceType.WoodenAxe);
        }
    }

    private Inventory m_Inventory;

    void Awake()
    {
        m_Inventory = GetComponent<Inventory>();
    }

    public void RetrievedWoodenAxe()
    {
        if (woodenAxeCount > 0)
        {
            m_Inventory.IncrementResourceCount(ResourceType.WoodenAxe, -1);
        }
    }
}
