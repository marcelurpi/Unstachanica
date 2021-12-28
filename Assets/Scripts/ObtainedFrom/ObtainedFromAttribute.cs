using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class ObtainedFromAttribute : PropertyAttribute
{
    private readonly ObtainedFrom obtainedFrom = ObtainedFrom.Shop;
    public ObtainedFrom GetObtainedFrom() => obtainedFrom;

    public ObtainedFromAttribute(ObtainedFrom obtainedFrom)
    {
        this.obtainedFrom = obtainedFrom;
    }
}

public enum ObtainedFrom
{
    Shop,
    MachineCrafting,
    Assembling,
}
