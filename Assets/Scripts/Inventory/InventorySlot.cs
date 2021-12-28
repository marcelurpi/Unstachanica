using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    private static event System.Action OnSlotSelected = null;
    private static event System.Action OnSlotDeselected = null;

    public static InventorySlot selected = null;

    [SerializeField] private Button button = null;
    [SerializeField] private Image fillImage = null;
    [SerializeField] private TextMeshProUGUI slotText = null;

    private bool hasItem = false;
    private int slotIndex = 0;
    private Inventory inventory = null;

    private void Start()
    {
        button.OnClick += () =>
        {
            inventory.MoveFromAnotherInventory(selected.inventory, selected.slotIndex, slotIndex);
        };

        button.OnSelect += () =>
        {
            if (hasItem)
            {
                selected = this;
                OnSlotSelected?.Invoke();
            }
            else button.Deselect();
        };

        button.OnDeselect += () =>
        {
            if(selected == this)
            {
                selected = null;
                OnSlotDeselected?.Invoke();
            }
        };

        OnSlotSelected += () =>
        {
            if(selected != this)
            {
                if(!hasItem) button.SetToggle(false);
            }
        };

        OnSlotDeselected += () =>
        {
            button.SetToggle(true);
        };

        Manual.OnManualOpened += () =>
        {
            button.SetInteractable(false);
        };

        Manual.OnManualClosed += () =>
        {
            button.SetInteractable(true);
        };
    }

    public void Construct(int slotIndex, Inventory inventory)
    {
        this.slotIndex = slotIndex;
        this.inventory = inventory;
        hasItem = false;
    }

    public void DisplayItem(ExpireItem item)
    {
        slotText.text = item.ToString();
        fillImage.fillAmount = item.GetFillAmount();
        hasItem = !item.IsEmpty();

        if (selected && !hasItem) button.Deselect();
    }

    public ExpireItem GetItem()
    {
        return inventory.GetItemAt(slotIndex);
    }

    public void Spend()
    {
        inventory.SpendAt(slotIndex);
    }
}
