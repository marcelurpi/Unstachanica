using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event System.Action OnItemsChanged = null;

    [SerializeField] private Item[] startingItems = null;
    [SerializeField] private InventorySlot[] slots = null;
    [SerializeField] private ItemManager itemManager = null;

    private bool freezeExpire = false;
    private ExpireItem[] items = null;

    private void Awake()
    {
        items = new ExpireItem[slots.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Construct(i, this);

            ExpireItem item;
            if (i < startingItems.Length)
            {
                item = new ExpireItem(startingItems[i]);
            }
            else item = ExpireItem.empty;
            StoreAt(item, i);
        }

        Manual.OnManualOpened += () => freezeExpire = true;
        Manual.OnManualClosed += () => freezeExpire = false;

        itemManager.CallUpdateEvenWhenHidden(this);
    }

    public void UpdateEvenWhenHidden()
    {
        if (freezeExpire) return;

        for (int i = 0; i < slots.Length; i++)
        {
            items[i].UpdateSecondsLeftToExpire();
            StoreAt(items[i], i);
        }
    }

    public ExpireItem GetItemAt(int slotIndex)
    {
        return items[slotIndex];
    }

    public bool HasSecondarySlot()
    {
        return slots.Length > 1;
    }

    public int GetEmptySlot()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].IsEmpty()) return i;
        }
        return -1;
    }

    public int GetEmptySlotAfterSpending(ExpireItem item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].IsEmpty()) return i;
            if (items[i] == item) return i;
        }
        return -1;
    }

    public void StoreAt(ExpireItem item, int slotIndex)
    {
        items[slotIndex] = item;
        slots[slotIndex].DisplayItem(items[slotIndex]);
        OnItemsChanged?.Invoke();
    }

    public int GetSlotWith(ExpireItem item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item) return i;
        }
        return -1;
    }

    public void SpendAt(int slotIndex)
    {
        StoreAt(ExpireItem.empty, slotIndex);
    }

    public void MoveFromAnotherInventory(Inventory from, int fromSlot, int toSlot)
    {
        StoreAt(from.items[fromSlot], toSlot);
        from.SpendAt(fromSlot);
    }
}