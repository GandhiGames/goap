using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class WoodCollector : GoapLabourer
{
    /**
     * Our only goal will ever be to chop logs.
     * The ChopFirewoodAction will be able to fulfill this goal.
     */
    public override Dictionary<string, object> CreateGoalState()
    {
        var goal = new Dictionary<string, object>();

        goal.Add("collectLogs", true);

        return goal;
    }
}
