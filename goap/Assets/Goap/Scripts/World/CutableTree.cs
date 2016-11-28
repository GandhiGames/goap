using UnityEngine;
using System.Collections;

public class CutableTree : WorldComponent
{
    void OnEnable()
    {
        COMPONENT_DATABASE.Register<CutableTree>(this);
    }

    void OnDisable()
    {
        COMPONENT_DATABASE.UnRegister<CutableTree>(this);
    }
}
