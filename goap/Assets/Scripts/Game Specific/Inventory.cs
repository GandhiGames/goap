using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ResourceType
{
	Wood,
	Iron}
;

public class Inventory : MonoBehaviour
{
	private Dictionary<ResourceType, Resource> m_Resources;

	void Start ()
	{
		m_Resources = new Dictionary<ResourceType, Resource> ();
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
		return m_Resources.ContainsKey (resourceType);
	}

	private class Resource
	{
		public int count { get; set; }

		public Resource (int count)
		{
			this.count = count;
		}
	}

}
