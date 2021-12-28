using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject, IManualEntry
{
    public enum AssemblingZone
    {
        Table,
        Workshop,
        Garage,
    }

    [SerializeField] private string itemName = null;
    [SerializeField] [TextArea] private string description = null;
    [SerializeField] private int secondsToExpire = 0;

    [SerializeField] protected ObtainedFrom obtainedFrom = ObtainedFrom.Shop;

    [ObtainedFrom(ObtainedFrom.Shop)] [SerializeField] private int shopPrice = 0;

    [ObtainedFrom(ObtainedFrom.MachineCrafting)] [SerializeField] private Machine machineToCraft = null;
    [ObtainedFrom(ObtainedFrom.MachineCrafting)] [SerializeField] private Item primaryIngredientToCraft = null;
    [ObtainedFrom(ObtainedFrom.MachineCrafting)] [SerializeField] private Item secondaryIngredientToCraft = null;
    [ObtainedFrom(ObtainedFrom.MachineCrafting)] [SerializeField] private float secondsToCraft = 0;

    [ObtainedFrom(ObtainedFrom.Assembling)] [SerializeField] protected AssemblingZone assemblingZone = AssemblingZone.Table;
    [ObtainedFrom(ObtainedFrom.Assembling)] [SerializeField] private GameObject assemblingLayout = null;
    [ObtainedFrom(ObtainedFrom.Assembling)] [SerializeField] private float secondsToAssemble = 0;


    public string GetName() => itemName;
    public string GetDescription() => description;
    public int GetSecondsToExpire() => secondsToExpire;
    public virtual ObtainedFrom GetObtainedFrom() => obtainedFrom;
    public ExpireItem GetItem() => new ExpireItem(this);
    public int GetShopPrice() => shopPrice;
    public Machine GetMachineToCraft() => machineToCraft;
    public ExpireItem GetPrimaryIngredientToCraft() => new ExpireItem(primaryIngredientToCraft);
    public ExpireItem GetSecondaryIngredientToCraft() => new ExpireItem(secondaryIngredientToCraft);
    public float GetSecondsToCraft() => secondsToCraft;
    public virtual AssemblingZone GetAssemblingZone() => assemblingZone;
    public GameObject GetAssemblingLayout() => assemblingLayout;
    public float GetSecondsToAssemble() => secondsToAssemble;

    private void OnValidate()
    {
        if (obtainedFrom != ObtainedFrom.Shop) shopPrice = 0;
        if (obtainedFrom != ObtainedFrom.MachineCrafting)
        {
            machineToCraft = null;
            primaryIngredientToCraft = null;
            secondaryIngredientToCraft = null;
        }
        if (obtainedFrom != ObtainedFrom.Assembling)
        {
            assemblingZone = AssemblingZone.Table;
            assemblingLayout = null;
        }
    }
}
