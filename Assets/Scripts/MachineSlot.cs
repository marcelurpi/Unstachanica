using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MachineSlot : MonoBehaviour
{
    [SerializeField] private Machine machine = null;
    [SerializeField] private Inventory inventory = null;
    [SerializeField] private ItemManager itemManager = null;
    [SerializeField] private Button button = null;
    [SerializeField] private Image fillImage = null;
    [SerializeField] private TextMeshProUGUI machineText = null;

    private bool usesSecondary = false;
    private bool isCrafting = false;
    private int resultSlotIndex = 0;
    private float secondsToCraft = 0;
    private float totalSecondsToCraft = 0;
    private ExpireItem itemToCraft = ExpireItem.empty;

    private void Start()
    {
        machineText.text = machine.GetName();

        button.OnClick += () =>
        {
            if (CanCraft()) StartCrafting();
        };

        Manual.OnManualOpened += () => button.SetInteractable(false);
        Manual.OnManualClosed += () => button.SetInteractable(true);
    }

    public void UpdateEvenWhenHidden()
    {
        if (isCrafting)
        {
            secondsToCraft -= Time.deltaTime;
            fillImage.fillAmount = secondsToCraft / totalSecondsToCraft;
            if (secondsToCraft <= 0)
            {
                isCrafting = false;
                itemManager.StopCallingUpdateEvenWhenHidden(this);
                inventory.StoreAt(itemToCraft, resultSlotIndex);
            }
        }
    }

    private bool CanCraft()
    {
        if (isCrafting) return false;

        ExpireItem primaryIngredient = inventory.GetItemAt(0);
        if (primaryIngredient.IsEmpty()) return false;

        ExpireItem secondaryIngredient = inventory.HasSecondarySlot() ? inventory.GetItemAt(1) : ExpireItem.empty;

        itemToCraft = itemManager.GetResultFromMachineAndIngredients(machine, primaryIngredient, secondaryIngredient, ref usesSecondary, ref totalSecondsToCraft);
        if (itemToCraft.IsEmpty()) return false;

        secondsToCraft = totalSecondsToCraft;

        resultSlotIndex = inventory.GetEmptySlotAfterSpending(primaryIngredient);
        return resultSlotIndex != -1;
    }

    private void StartCrafting()
    {
        inventory.SpendAt(0);
        if (usesSecondary) inventory.SpendAt(1);

        isCrafting = true;
        itemManager.CallUpdateEvenWhenHidden(this);
    }
}
