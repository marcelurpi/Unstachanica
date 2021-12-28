using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Spaceship : Item
{
    public override ObtainedFrom GetObtainedFrom() => ObtainedFrom.Assembling;
    public override AssemblingZone GetAssemblingZone() => AssemblingZone.Garage;

    private void OnValidate()
    {
        if (obtainedFrom != ObtainedFrom.Assembling) obtainedFrom = ObtainedFrom.Assembling;
        if (assemblingZone != AssemblingZone.Garage) assemblingZone = AssemblingZone.Garage;
    }
}
