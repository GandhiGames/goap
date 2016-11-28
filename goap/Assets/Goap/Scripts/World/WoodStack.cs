using UnityEngine;
using System.Collections;

public class WoodStack : WorldComponent
{
    public int count = 1;

    void OnEnable()
    {
        COMPONENT_DATABASE.Register<WoodStack>(this);
    }

    void OnDisable()
    {
        COMPONENT_DATABASE.UnRegister<WoodStack>(this);
    }
}
