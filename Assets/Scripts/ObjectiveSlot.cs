using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveSlot : MonoBehaviour
{
    [SerializeField] private int moneyRewardedPerPiece = 0;
    [SerializeField] private Objective objective = null;
    [SerializeField] private Inventory inventory = null;
    [SerializeField] private Money money = null;
    [SerializeField] private Button button = null;
    [SerializeField] private TextMeshProUGUI itemText = null;
    [SerializeField] private TextMeshProUGUI rewardText = null;

    private bool recipeSelected = false;
    private int objectiveItemSlot = 0;

    private void Start()
    {
        if(objective.GetSpaceship() == null)
        {
            itemText.text = "";
            rewardText.text = "";
            button.SetInteractable(false);
        }
        else
        {
            itemText.text = objective.GetSpaceship().GetName();
            rewardText.text = $"+{ objective.GetPiecesLeft() * moneyRewardedPerPiece }\nSpace Coins";
        }

        button.OnClick += () =>
        {
            if(!recipeSelected)
            {
                recipeSelected = true;
                Assembler.current.SetAssemblingRecipe(objective.GetSpaceship());
                Assembler.current.AddAllExcept(objective.GetPiecesLeft());
            }
            else if (CanComplete()) Complete();
        };

        Manual.OnManualOpened += () => button.SetInteractable(false);
        Manual.OnManualClosed += () => button.SetInteractable(true);
    }

    private bool CanComplete()
    {
        if (objective.GetSpaceship() == null) return false;

        objectiveItemSlot = inventory.GetSlotWith(objective.GetSpaceship().GetItem());
        return objectiveItemSlot != -1;
    }

    private void Complete()
    {
        inventory.SpendAt(objectiveItemSlot);
        money.Earn(objective.GetPiecesLeft() * moneyRewardedPerPiece);
        recipeSelected = false;
    }
}
