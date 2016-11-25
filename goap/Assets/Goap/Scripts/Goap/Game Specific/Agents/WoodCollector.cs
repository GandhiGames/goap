using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class WoodCollector : GoapLabourer
{
    public override Dictionary<string, object> CreateGoalState()
    {
        var goal = new Dictionary<string, object>();

        goal.Add("collectLogs", true);

        return goal;
    }
}
