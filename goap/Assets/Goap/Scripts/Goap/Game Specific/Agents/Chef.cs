using UnityEngine;
using System.Collections.Generic;

public class Chef : GoapLabourer
{
    public override Dictionary<string, object> CreateGoalState()
    {
        var goal = new Dictionary<string, object>();

        goal.Add("hasCookedMeat", true);

        return goal;
    }
}
