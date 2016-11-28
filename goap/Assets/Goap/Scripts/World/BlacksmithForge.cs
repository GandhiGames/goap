using UnityEngine;
using System.Collections;

public class BlacksmithForge : WorldComponent
{
    void OnEnable()
    {
        COMPONENT_DATABASE.Register<BlacksmithForge>(this);
    }

    void OnDisable()
    {
        COMPONENT_DATABASE.UnRegister<BlacksmithForge>(this);
    }
}
