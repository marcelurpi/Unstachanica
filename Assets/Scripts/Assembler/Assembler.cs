using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Assembler : MonoBehaviour
{
    public static Assembler current = null;

    [SerializeField] private Inventory inventory = null;
    [SerializeField] private Button assembleButton = null;
    [SerializeField] private Image fillImage = null;
    [SerializeField] private ItemManager itemManager = null;

    private bool isAssembling = false;
    private int piecesLeft = 0;
    private int resultSlotIndex = 0;
    private float secondsToAssemble = 0;
    private float totalSecondsToAssemble = 0;
    private GameObject layout = null;
    private ExpireItem resultItem = ExpireItem.empty;

    public ExpireItem GetCurrentResultItem() => resultItem;

    private void Start()
    {
        assembleButton.OnClick += () =>
        {
            if (CanAssemble()) StartAssembling();
        };

        Manual.OnManualOpened += () => assembleButton.SetInteractable(false);
        Manual.OnManualClosed += () => assembleButton.SetInteractable(true);
    }

    private void OnEnable()
    {
        current = this;
    }

    private void OnDisable()
    {
        current = null;
    }

    public void UpdateEvenWhenHidden()
    {
        if(isAssembling)
        {
            secondsToAssemble -= Time.deltaTime;
            fillImage.fillAmount = secondsToAssemble / totalSecondsToAssemble;
            if(secondsToAssemble <= 0)
            {
                isAssembling = false;

                Destroy(layout);
                layout = null;
                inventory.StoreAt(resultItem, resultSlotIndex);
            }
        }
    }

    public void SetAssemblingRecipe(Item item)
    {
        resultItem = new ExpireItem(item);
        if (layout != null) Destroy(layout);
        layout = Instantiate(item.GetAssemblingLayout(), transform);
        piecesLeft = layout.transform.childCount;
        totalSecondsToAssemble = item.GetSecondsToAssemble();
        secondsToAssemble = totalSecondsToAssemble;
    }

    public void AddPiece()
    {
        piecesLeft--;
    }

    public void AddAllExcept(int piecesLeft)
    {
        List<int> children = new List<int>();
        for (int i = 0; i < layout.transform.childCount; i++) children.Add(i);

        for(int i = 0; i < layout.transform.childCount - piecesLeft; i++)
        {
            int randomIndex = Random.Range(0, children.Count);
            layout.transform.GetChild(children[randomIndex]).GetComponent<AssemblingPiece>().ForceAddPiece();
            children.RemoveAt(randomIndex);
        }
    }

    private bool CanAssemble()
    {
        if (isAssembling || layout == null || piecesLeft > 0) return false;
        resultSlotIndex = inventory.GetEmptySlot();
        return resultSlotIndex != -1;
    }

    private void StartAssembling()
    {
        isAssembling = true;
        itemManager.CallUpdateEvenWhenHidden(this);
    }
}
