using UnityEngine;
using System.Collections.Generic;

public class AnimalFarmer : GoapLabourer
{
	public override Dictionary<string, object> CreateGoalState()
	{
		var goal = new Dictionary<string, object>();

		goal.Add("spawnMeat", true);
		goal.Add ("hasMeat", true);

		return goal;
	}
}
