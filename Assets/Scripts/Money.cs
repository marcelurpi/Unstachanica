using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] private int amount = 0;
    [SerializeField] private Button button = null;
    [SerializeField] private TextMeshProUGUI moneyText = null;

    private void Start()
    {
        moneyText.text = $"{ amount } Space Coins";

        Manual.OnManualOpened += () => button.SetInteractable(false);

        Manual.OnManualClosed += () => button.SetInteractable(true);
    }

    public bool HasEnough(int amount)
    {
        return this.amount >= amount;
    }

    public void Earn(int amount)
    {
        this.amount += amount;
        moneyText.text = $"{ this.amount } Space Coins";
    }

    public void Spend(int amount)
    {
        this.amount -= amount;
        moneyText.text = $"{ this.amount } Space Coins";
    }
}
