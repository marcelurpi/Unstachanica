using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(BoxCollider2D))]
public class Button : MonoBehaviour
{
    public event System.Action OnClick = null;
    public event System.Action OnSelect = null;
    public event System.Action OnDeselect = null;

    private static event System.Action DeselectAll = null;

    private static bool insideAny = false;

    [SerializeField] private bool interactable = false;
    [SerializeField] private bool toggle = false;
    [SerializeField] private Image image = null;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightedColor = Color.white;
    [SerializeField] private Color pressedColor = Color.white;
    [SerializeField] private Color selectedColor = Color.white;
    [SerializeField] private Color disabledColor = Color.white;

    private bool sizeSet = false;
    private bool inside = false;
    private bool selected = false;
    private RectTransform rectTransform = null;
    private BoxCollider2D buttonCollider = null;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        buttonCollider = GetComponent<BoxCollider2D>();

        DeselectAll += Deselect;
    }

    private void OnEnable()
    {
        if (interactable) image.color = normalColor;
    }

    private void Update()
    {
        if(!sizeSet)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetLocalCorners(corners);
            float width = Vector3.Distance(corners[1], corners[2]);
            float height = Vector3.Distance(corners[0], corners[1]);

            if (width > 0 && height > 0)
            {
                sizeSet = true;
                buttonCollider.size = new Vector2(width, height);
            }
        }

        if (Input.GetMouseButtonUp(0) && !insideAny) Deselect();
    }

    public void SetInteractable(bool interactable)
    {
        if (this.interactable == interactable) return;

        this.interactable = interactable;
        if (interactable)
        {
            buttonCollider.enabled = true;
            if (inside) image.color = highlightedColor;
            else image.color = normalColor;
        }
        else
        {
            buttonCollider.enabled = false;
            Deselect();
            image.color = disabledColor;
        }
    }

    public void SetToggle(bool toggle)
    {
        this.toggle = toggle;
    }

    public void Deselect()
    {
        if (selected)
        {
            selected = false;
            if (inside) image.color = highlightedColor;
            else image.color = normalColor;

            OnDeselect?.Invoke();
        }
    }

    private void OnMouseEnter()
    {
        inside = true;
        insideAny = true;
        if (interactable && !selected) image.color = highlightedColor;
    }

    private void OnMouseExit()
    {
        inside = false;
        insideAny = false;
        if (interactable && !selected) image.color = normalColor;
    }

    private void OnMouseDown()
    {
        if (interactable) image.color = pressedColor;
    }

    private void OnMouseUpAsButton()
    {
        if (!interactable) return;

        image.color = highlightedColor;

        if (!toggle)
        {
            OnClick?.Invoke();
            DeselectAll?.Invoke();
            return;
        }

        if (!selected)
        {
            selected = true;
            image.color = selectedColor;
            OnSelect?.Invoke();
        }
        else DeselectAll?.Invoke();
    }
}
