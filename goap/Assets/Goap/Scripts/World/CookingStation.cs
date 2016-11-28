using UnityEngine;
using System.Collections;

public class CookingStation : WorldComponent
{
    void OnEnable()
    {
        COMPONENT_DATABASE.Register<CookingStation>(this);
    }

    void OnDisable()
    {
        COMPONENT_DATABASE.UnRegister<CookingStation>(this);
    }
}
