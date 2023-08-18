using System;
using UnityEngine;

public class SubMenu : MonoBehaviour
{
    [HideInInspector]
    public SubMenuType type;

    [HideInInspector]
    public Character character;

    [HideInInspector]
    private Action<int> OnExecute;
    private Action OnBack;

    public SpriteRenderer[] arrows;
    public SpriteRenderer[] slots;

    private SpriteRenderer currentArrow;

    private int currentSelection = 0;

    public void Init(Character character, SubMenuType type, Action<int> OnExecute, Action OnBack)
    {
        this.type = type;
        this.character = character;
        this.OnExecute = OnExecute;
        this.OnBack = OnBack;
        Setup(type);
        ResetSelection();
    }

    /// <summary>
    /// Setup the submenu choices based on the type of the submenu and what
    /// the character possesses.
    /// </summary>
    /// <param name="type">The type of the submenu.</param>
    public void Setup(SubMenuType type)
    {
        int count = 0;
        var items = GetMenuItems(type);
        foreach (var item in items)
        {
            if (count < slots.Length)
            {
                slots[count].sprite = item.GetNameSprite();
                count++;
            }
        }
    }
    
    /// <summary>
    /// Get the submenu items from the character based on what type of submenu
    /// this is.
    /// </summary>
    /// <param name="type">The type of the submenu.</param>
    /// <returns></returns>
    private SubMenuItem[] GetMenuItems(SubMenuType type)
    {
        switch (type)
        {
            case SubMenuType.Cast:
                {
                    return character.casts;
                }
            case SubMenuType.Summon:
                {
                    return character.summons;
                }
            case SubMenuType.Item:
                {
                    return character.items;
                }
            default:
                {
                    return null;
                }
        }
    }

    /// <summary>
    /// Update the selected character and set this submenu up based on that
    /// character's assets (casts, summons, items).
    /// </summary>
    /// <param name="character"></param>
    public void UpdateCharacter(Character character)
    {
        this.character = character;
        Setup(type);
    }

    /// <summary>
    /// Set the first choice as the selected one.
    /// </summary>
    public void ResetSelection()
    {
        currentSelection = 0;
        int count = 0;
        foreach (var arrow in arrows)
        {
            if (count > 0)
                arrow.enabled = false;
            else
                arrow.enabled = true;
            count++;
        }
    }

    /// <summary>
    /// Set the current arrow based on the current selection.
    /// </summary>
    private void UpdateCurrentArrow()
    {
        currentArrow = arrows[currentSelection];
        UpdateSelection();
    }

    /// <summary>
    /// Show the arrow of the currently-selected choice.
    /// </summary>
    public void UpdateSelection()
    {
        foreach (var arrow in arrows)
        {
            if (arrow != currentArrow)
                arrow.enabled = false;
            else
                arrow.enabled = true;
        }
    }

    private void HideAllArrows()
    {
        foreach (var arrow in arrows)
        {
            arrow.enabled = false;
        }
    }

    /// <summary>
    /// Hide this submenu.
    /// </summary>
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Open this submenu.
    /// </summary>
    public void Open()
    {
        gameObject.SetActive(true);
        ResetSelection();
    }

    /// <summary>
    /// Return to the previous menu.
    /// </summary>
    public void Cancel()
    {
        HideAllArrows();
        Hide();
        OnBack.Invoke();
    }

    /// <summary>
    /// Select the previous choice.
    /// </summary>
    public void Previous()
    {
        if (currentSelection <= 0)
            currentSelection = arrows.Length - 1;
        else
            currentSelection--;
        UpdateCurrentArrow();
    }

    /// <summary>
    /// Select the next choice.
    /// </summary>
    public void Next()
    {
        if (currentSelection >= arrows.Length - 1)
            currentSelection = 0;
        else
            currentSelection++;
        UpdateCurrentArrow();
    }

    /// <summary>
    /// Proceed with the current selection.
    /// </summary>
    public void Execute()
    {
        if (currentSelection >= 0)
        {
            OnExecute.Invoke(currentSelection);
        }
    }

    public int GetSelection()
    {
        return currentSelection;
    }
}
