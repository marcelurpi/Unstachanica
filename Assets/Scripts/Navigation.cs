using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    private enum Zone
    {
        Shop,
        Table,
        Workshop,
        Garage,
    }

    [SerializeField] private Zone zone = Zone.Shop;
    [SerializeField] private Button leftButton = null;
    [SerializeField] private Button rightButton = null;
    [SerializeField] private TextMeshProUGUI leftButtonText = null;
    [SerializeField] private TextMeshProUGUI rightButtonText = null;
    [SerializeField] private GameObject[] zoneParents = null;

    private void Start()
    {
        foreach (GameObject parent in zoneParents)
        {
            parent.SetActive(false);
        }
        zoneParents[(int)zone].SetActive(true);
        UpdateButtons();

        leftButton.OnClick += () =>
        {
            if (zone != Zone.Shop)
            {
                zoneParents[(int)zone].SetActive(false);
                zone--;
                UpdateButtons();
                zoneParents[(int)zone].SetActive(true);
            }
        };

        rightButton.OnClick += () =>
        {
            if (zone != Zone.Garage)
            {
                zoneParents[(int)zone].SetActive(false);
                zone++;
                UpdateButtons();
                zoneParents[(int)zone].SetActive(true);
            }
        };

        Manual.OnManualOpened += () =>
        {
            leftButton.SetInteractable(false);
            rightButton.SetInteractable(false);
        };

        Manual.OnManualClosed += () =>
        {
            leftButton.SetInteractable(true);
            rightButton.SetInteractable(true);
        };
    }

    private void OnValidate()
    {
        foreach (GameObject parent in zoneParents)
        {
            parent.SetActive(false);
        }
        zoneParents[(int)zone].SetActive(true);
    }

    public bool CurrentZoneEqualsAssemblingZone(Item.AssemblingZone assemblingZone)
    {
        if (assemblingZone == Item.AssemblingZone.Table && zone == Zone.Table) return true;
        if (assemblingZone == Item.AssemblingZone.Workshop && zone == Zone.Workshop) return true;
        if (assemblingZone == Item.AssemblingZone.Garage && zone == Zone.Garage) return true;
        return false;
    }

    private void UpdateButtons()
    {
        if (zone == Zone.Shop)
        {
            leftButton.SetInteractable(false);
            leftButtonText.text = "";
        }
        else
        {
            leftButton.SetInteractable(true);
            leftButtonText.text = $"To\n{ zone - 1 }";
        }

        if (zone == Zone.Garage)
        {
            rightButton.SetInteractable(false);
            rightButtonText.text = "";
        }
        else
        {
            rightButton.SetInteractable(true);
            rightButtonText.text = $"To\n{ zone + 1 }";
        }
    }
}
