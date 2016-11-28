using UnityEngine;
using System.Collections;

public class MeatTable : WorldComponent
{
    public int count;

    void OnEnable()
    {
        COMPONENT_DATABASE.Register<MeatTable>(this);
    }

    void OnDisable()
    {
        COMPONENT_DATABASE.UnRegister<MeatTable>(this);
    }
}
