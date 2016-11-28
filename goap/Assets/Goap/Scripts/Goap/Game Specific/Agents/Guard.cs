using UnityEngine;
using System.Collections.Generic;

public class Guard : GoapLabourer
{
    public override Dictionary<string, object> CreateGoalState()
    {
        var goal = new Dictionary<string, object>();

        goal.Add("areaSafe", true);

        return goal;
    }
}
