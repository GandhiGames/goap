using UnityEngine;
using System.Collections.Generic;

public class Blacksmith : GoapLabourer
{
    public override Dictionary<string, object> CreateGoalState()
    {
        var goal = new Dictionary<string, object>();

        goal.Add("hasNewWoodenAxe", true);
        return goal;
    }
}
