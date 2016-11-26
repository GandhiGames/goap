using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ResourceType
{
	Wood,
	Iron
}

public class Resource
{
    public int count { get; set; }

    public Resource(int count)
    {
        this.count = count;
    }
}

public class Inventory : MonoBehaviour
{
    public Tool equippedTool;

	private Dictionary<ResourceType, Resource> m_Resources;

	void Start ()
	{
		m_Resources = new Dictionary<ResourceType, Resource> ();
	}

    public bool HasToolEquipped(ToolType toolType)
    {
        return equippedTool != null && equippedTool.name.Equals(toolType.ToString());
    }

	public void IncrementResourceCount (ResourceType resourceType, int count)
	{
		if (m_Resources.ContainsKey (resourceType)) {
			m_Resources [resourceType].count += count;
		} else {
			m_Resources.Add (resourceType, new Resource (count));
		}
	}

	public void SetResourceCount (ResourceType resourceType, int count)
	{
		if (m_Resources.ContainsKey (resourceType)) {
			m_Resources [resourceType].count = count;
		} else {
			m_Resources.Add (resourceType, new Resource (count));
		}
	}

	public int GetResourceCount (ResourceType resourceType)
	{
		if (m_Resources.ContainsKey (resourceType)) {
			return m_Resources [resourceType].count;
		}

		return 0;
	}

	public bool HasResource (ResourceType resourceType)
	{
		if(!m_Resources.ContainsKey (resourceType))
        {
            return false;
        }

        return m_Resources[resourceType].count > 0;
	}

}
