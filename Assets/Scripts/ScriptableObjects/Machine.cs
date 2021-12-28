using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Machine : ScriptableObject, IManualEntry
{
    [SerializeField] private string machineName = null;
    [SerializeField] [TextArea] private string description = null;
    public string GetName() => machineName;
    public string GetDescription() => description;
}
