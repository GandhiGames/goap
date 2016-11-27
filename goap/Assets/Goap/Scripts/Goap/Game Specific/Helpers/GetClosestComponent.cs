using UnityEngine;
using System.Collections;

public class GetClosestComponent
{
	public T GetClosest<T>(GameObject agent) where T : MonoBehaviour
	{
		var stacks = UnityEngine.GameObject.FindObjectsOfType<T>();

		if(stacks.Length == 0)
		{
			return null;
		}

		return CalculateClosest(stacks, agent);
	}

	private T CalculateClosest<T>(T[] stacks, GameObject agent) where T : MonoBehaviour
	{
		T closest = null;

		float closestDist = float.MaxValue;

		foreach (var stack in stacks)
		{
			float dist = (stack.gameObject.transform.position - agent.transform.position).magnitude;

			if (dist < closestDist)
			{
				closest = stack;
				closestDist = dist;
			}
		}

		return closest;
	}
}
