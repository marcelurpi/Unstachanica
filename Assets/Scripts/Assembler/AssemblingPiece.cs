using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AssemblingPiece : MonoBehaviour
{
    [SerializeField] private Item ingredient = null;
    [SerializeField] private Button button = null;
    [SerializeField] private TextMeshProUGUI slotText = null;

    private bool hasPiece = false;

    public ExpireItem GetIngredient() => new ExpireItem(ingredient);

    private void Start()
    {
        button.SetInteractable(!hasPiece);

        slotText.text = ingredient.GetName();

        button.OnClick += () =>
        {
            if (CanAddPiece()) AddPiece();
        };

        Manual.OnManualOpened += () => button.SetInteractable(false);
        Manual.OnManualClosed += () => button.SetInteractable(!hasPiece);
    }

    public void ForceAddPiece()
    {
        hasPiece = true;
        button.SetInteractable(false);
        Assembler.current.AddPiece();
    }

    private bool CanAddPiece()
    {
        if (hasPiece || InventorySlot.selected == null) return false;
        return InventorySlot.selected.GetItem() == new ExpireItem(ingredient);
    }

    private void AddPiece()
    {
        hasPiece = true;
        button.SetInteractable(false);
        InventorySlot.selected.Spend();
        Assembler.current.AddPiece();
    }
}
