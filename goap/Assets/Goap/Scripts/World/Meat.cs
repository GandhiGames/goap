using UnityEngine;
using System.Collections;

public class Meat : WorldComponent
{
    void OnEnable()
    {
        COMPONENT_DATABASE.Register<Meat>(this);
    }

    void OnDisable()
    {
        COMPONENT_DATABASE.UnRegister<Meat>(this);
    }
}
