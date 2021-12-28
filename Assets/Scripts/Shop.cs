using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private static Item[] buyableItems = null;

    [SerializeField] private Inventory inventory = null;
    [SerializeField] private Money money = null;
    [SerializeField] private ItemManager itemManager = null;
    [SerializeField] private Button button = null;
    [SerializeField] private TextMeshProUGUI priceText = null;
    [SerializeField] private TextMeshProUGUI goodsText = null;

    private Item shopItem = null;
    private int emptySlotIndex = 0;

    private void Start()
    {
        if (buyableItems == null) buyableItems = itemManager.GetBuyableItems();

        if (transform.GetSiblingIndex() >= buyableItems.Length)
        {
            priceText.text = "";
            goodsText.text = "";
            button.SetInteractable(false);
            return;
        }

        shopItem = buyableItems[transform.GetSiblingIndex()];

        priceText.text = $"{ shopItem.GetShopPrice() } Space Coins";
        goodsText.text = shopItem.GetItem().ToString();

        button.OnClick += () =>
        {
            if (CanBuy()) Buy();
        };

        Manual.OnManualOpened += () => button.SetInteractable(false);
        Manual.OnManualClosed += () => button.SetInteractable(true);
    }

    private bool CanBuy()
    {
        if (!money.HasEnough(shopItem.GetShopPrice())) return false;

        emptySlotIndex = inventory.GetEmptySlot();
        return emptySlotIndex != -1;
    }

    private void Buy()
    {
        money.Spend(shopItem.GetShopPrice());
        inventory.StoreAt(shopItem.GetItem(), emptySlotIndex);
    }
}
