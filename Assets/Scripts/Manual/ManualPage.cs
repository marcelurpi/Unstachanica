using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ManualPage : MonoBehaviour
{
    [Header("Left Page")]
    [SerializeField] private TextMeshProUGUI leftPageChapterText = null;
    [SerializeField] private TextMeshProUGUI leftPageTitleText = null;
    [SerializeField] private TextMeshProUGUI leftPageDescriptionText = null;
    [SerializeField] private Button previousPageButton = null;

    [Header("Right Page")]
    [SerializeField] private TextMeshProUGUI rightPageChapterText = null;
    [SerializeField] private TextMeshProUGUI rightPageTitleText = null;
    [SerializeField] private TextMeshProUGUI rightPageDescriptionText = null;
    [SerializeField] private TextMeshProUGUI rightPageOtherInfoText = null;
    [SerializeField] private TextMeshProUGUI rightPageSelectMessageText = null;
    [SerializeField] private GameObject rightPageImage = null;
    [SerializeField] private Button selectAssemblingRecipeButton = null;
    [SerializeField] private Button nextPageButton = null;

    [SerializeField] private Navigation navigation = null;

    private bool doublePage = false;
    private int leftPageEntry = 0;
    private Manual manual = null;
    private IManualEntry[] chapterEntries = null;

    private void Start()
    {
        previousPageButton.OnClick += () =>
        {
            if (doublePage)
            {
                leftPageEntry--;
                DisplayEntryPerDoublePage();
            }
            else
            {
                leftPageEntry -= 2;
                DisplayEntryPerSinglePerPage();
            }
        };

        nextPageButton.OnClick += () =>
        {
            if (doublePage)
            {
                leftPageEntry++;
                DisplayEntryPerDoublePage();
            }
            else
            {
                leftPageEntry += 2;
                DisplayEntryPerSinglePerPage();
            }
        };

        selectAssemblingRecipeButton.OnClick += () =>
        {
            manual.CloseManual();
            Item itemEntry = (Item)chapterEntries[leftPageEntry];
            Assembler.current.SetAssemblingRecipe(itemEntry);
        };
    }

    public void SetupPage(Manual.Chapter chapter, Manual manual)
    {
        this.manual = manual;
        chapterEntries = GetEntriesForChapter(chapter);

        leftPageChapterText.text = chapter.ToString();
        rightPageChapterText.text = chapter.ToString();

        if (chapterEntries.Length == 0)
        {
            leftPageTitleText.text = "";
            leftPageDescriptionText.text = "";
            rightPageTitleText.text = "";
            rightPageDescriptionText.text = "";
        }
        else
        {
            leftPageEntry = 0;
            doublePage = chapter != Manual.Chapter.Machines;
            if (doublePage) DisplayEntryPerDoublePage();
            else DisplayEntryPerSinglePerPage();
        }
    }

    private void DisplayEntryPerSinglePerPage()
    {
        previousPageButton.gameObject.SetActive(leftPageEntry > 0);
        nextPageButton.gameObject.SetActive(chapterEntries.Length > leftPageEntry + 2);

        rightPageImage.SetActive(true);
        rightPageOtherInfoText.text = "";
        rightPageSelectMessageText.text = "";
        selectAssemblingRecipeButton.gameObject.SetActive(false);

        leftPageTitleText.text = chapterEntries[leftPageEntry].GetName();
        leftPageDescriptionText.text = chapterEntries[leftPageEntry].GetDescription();
        if (leftPageEntry + 1 == chapterEntries.Length)
        {
            rightPageTitleText.text = "";
            rightPageDescriptionText.text = "";
            return;
        }
        rightPageTitleText.text = chapterEntries[leftPageEntry + 1].GetName();
        rightPageDescriptionText.text = chapterEntries[leftPageEntry + 1].GetDescription();
    }

    private void DisplayEntryPerDoublePage()
    {
        previousPageButton.gameObject.SetActive(leftPageEntry > 0);
        nextPageButton.gameObject.SetActive(chapterEntries.Length > leftPageEntry + 1);

        rightPageImage.SetActive(false);
        selectAssemblingRecipeButton.gameObject.SetActive(false);

        Item itemEntry = (Item)chapterEntries[leftPageEntry];

        leftPageTitleText.text = itemEntry.GetName();
        leftPageDescriptionText.text = itemEntry.GetDescription();

        rightPageTitleText.text = "";
        rightPageDescriptionText.text = "";
        rightPageSelectMessageText.text = "";

        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        if(itemEntry.GetSecondsToExpire() == 0) builder.Append("This item doesn't expire\n\n");
        else builder.AppendFormat("This item expires after\n<b>{0} seconds</b>\n\n", itemEntry.GetSecondsToExpire());

        builder.Append("<b><size=50>How to obtain:</size></b>\n\n");
        switch (itemEntry.GetObtainedFrom())
        {
            case ObtainedFrom.Shop:
                builder.AppendFormat("Can be bought from the shop for <b>{0} Space Coins</b>", itemEntry.GetShopPrice());
                break;

            case ObtainedFrom.MachineCrafting:
                builder.AppendFormat("Can be crafted using a <b>{0}</b>", itemEntry.GetMachineToCraft().GetName());
                builder.AppendFormat(" and it requires <b>{0}</b>", itemEntry.GetPrimaryIngredientToCraft().ToString());
                bool hasSecondary = !itemEntry.GetSecondaryIngredientToCraft().IsEmpty();
                if (hasSecondary) builder.AppendFormat(" and <b>{0}</b>", itemEntry.GetSecondaryIngredientToCraft().ToString());
                break;

            case ObtainedFrom.Assembling:
                builder.Append("Can be assembled and its recipe requires:");
                foreach (KeyValuePair<string, int> ingredient in GetAssemblingIngredientDictionary(itemEntry))
                {
                    builder.AppendFormat("\n<b>{0} {1}</b>", ingredient.Value, ingredient.Key);
                }
                if(navigation.CurrentZoneEqualsAssemblingZone(itemEntry.GetAssemblingZone()))
                {
                    if (Assembler.current.GetCurrentResultItem() == itemEntry.GetItem()) rightPageSelectMessageText.text = "Recipe already selected";
                    else selectAssemblingRecipeButton.gameObject.SetActive(true);
                }
                else rightPageSelectMessageText.text = $"This recipe has to be assembled in the { itemEntry.GetAssemblingZone().ToString() }";
                break;

            default: break;
        }
        rightPageOtherInfoText.text = builder.ToString();
    }

    private Dictionary<string, int> GetAssemblingIngredientDictionary(Item itemEntry)
    {
        Dictionary<string, int> ingredients = new Dictionary<string, int>();
        foreach (Transform piece in itemEntry.GetAssemblingLayout().transform)
        {
            ExpireItem ingredient = piece.GetComponent<AssemblingPiece>().GetIngredient();
            if (ingredients.ContainsKey(ingredient.ToString())) ingredients[ingredient.ToString()]++;
            else ingredients.Add(ingredient.ToString(), 1);
        }
        return ingredients;
    }

    private IManualEntry[] GetEntriesForChapter(Manual.Chapter chapter)
    {
        switch (chapter)
        {
            case Manual.Chapter.Spaceships: return GetEntryArray<Spaceship>();
            case Manual.Chapter.Components: return GetEntryArray<SpaceComponent>();
            case Manual.Chapter.Pieces: return GetEntryArray<Piece>();
            case Manual.Chapter.Materials: return GetEntryArray<Material>();
            case Manual.Chapter.Machines: return GetEntryArray<Machine>();
            default: return null;
        }
    }

    private IManualEntry[] GetEntryArray<T>() where T : IManualEntry
    {
        return Resources.FindObjectsOfTypeAll<ScriptableObject>().Where(s => s is T).Cast<IManualEntry>().OrderBy(m => m.GetName()).ToArray();
    }
}
