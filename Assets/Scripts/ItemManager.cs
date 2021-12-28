using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private bool awoken = false;
    private Item[] buyableItems = null;
    private Item[] craftableItems = null;
    private List<Inventory> inventoriesToUpdate = null;
    private List<MachineSlot> machinesToUpdate = null;
    private List<Assembler> assemblersToUpdate = null;

    public Item[] GetBuyableItems() => buyableItems;

    private void Awake()
    {
        if (awoken) return;
        awoken = true;
        List<Item> buyable = new List<Item>();
        List<Item> craftable = new List<Item>();
        inventoriesToUpdate = new List<Inventory>();
        machinesToUpdate = new List<MachineSlot>();
        assemblersToUpdate = new List<Assembler>();
        Item[] items = Resources.FindObjectsOfTypeAll<Item>();
        foreach (Item item in items)
        {
            if (item.GetObtainedFrom() == ObtainedFrom.Shop) buyable.Add(item);
            else if (item.GetObtainedFrom() == ObtainedFrom.MachineCrafting) craftable.Add(item);
        }
        buyableItems = buyable.ToArray();
        craftableItems = craftable.ToArray();
    }

    private void Update()
    {
        List<Inventory> inventories = new List<Inventory>(inventoriesToUpdate);
        foreach (Inventory inventory in inventories) inventory.UpdateEvenWhenHidden(); 

        List<MachineSlot> machines = new List<MachineSlot>(machinesToUpdate);
        foreach (MachineSlot machine in machines) machine.UpdateEvenWhenHidden();

        List<Assembler> assemblers = new List<Assembler>(assemblersToUpdate);
        foreach (Assembler assemble in assemblers) assemble.UpdateEvenWhenHidden();
    }

    public ExpireItem GetResultFromMachineAndIngredients(Machine machine, ExpireItem primaryIngredient, ExpireItem secondaryIngredient, ref bool usesSecondary, ref float secondsToCraft)
    {
        secondsToCraft = 0;
        foreach (Item item in craftableItems)
        {
            bool secondaryEmptyOrSame = item.GetSecondaryIngredientToCraft().IsEmpty() || item.GetSecondaryIngredientToCraft() == secondaryIngredient;
            if (item.GetMachineToCraft() == machine && item.GetPrimaryIngredientToCraft() == primaryIngredient && secondaryEmptyOrSame)
            {
                usesSecondary = !item.GetSecondaryIngredientToCraft().IsEmpty();
                secondsToCraft = item.GetSecondsToCraft();
                return item.GetItem();
            }
        }
        return ExpireItem.empty;
    }

    public void CallUpdateEvenWhenHidden(Inventory inventory)
    {
        if (!awoken) Awake();
        inventoriesToUpdate.Add(inventory);
    }

    public void CallUpdateEvenWhenHidden(MachineSlot machine)
    {
        machinesToUpdate.Add(machine);
    }

    public void CallUpdateEvenWhenHidden(Assembler assembler)
    {
        assemblersToUpdate.Add(assembler);
    }

    public void StopCallingUpdateEvenWhenHidden(MachineSlot machine)
    {
        machinesToUpdate.Remove(machine);
    }
}
