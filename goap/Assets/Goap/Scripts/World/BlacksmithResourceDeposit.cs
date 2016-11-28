using UnityEngine;
using System.Collections;

public class BlacksmithResourceDeposit : WorldComponent 
{
	public int logs;

    void OnEnable()
    {
        COMPONENT_DATABASE.Register<BlacksmithResourceDeposit>(this);
    }

    void OnDisable()
    {
        COMPONENT_DATABASE.UnRegister<BlacksmithResourceDeposit>(this);
    }
}
