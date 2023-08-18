using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMenu : MonoBehaviour
{
    [HideInInspector]
    public Party party;

    public SpriteRenderer[] slots;
    public SpriteRenderer[] arrows;

    private SpriteRenderer currentArrow;

    private int currentSelection = 0;

    /// <summary>
    /// Initialize this instance of PartyMenu.
    /// </summary>
    /// <param name="playerParty">The Party object containing the player party.</param>
    /// <param name="rowCount">The amount of name rows.</param>
    public void Init(Party playerParty, int rowCount)
    {
        this.party = playerParty;
        SetupNameSlots(rowCount);
        AddNamesToLayout();
    }

    private void SetupNameSlots(int rowCount)
    {
        slots = new SpriteRenderer[rowCount];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = GameObject.Find("Name Row " + (i + 1).ToString())?.GetComponent<SpriteRenderer>();
        }
    }

    private void AddNamesToLayout()
    {
        int count = 0;
        foreach (var character in party.characters)
        {
            if (count < slots.Length)
            {
                slots[count].sprite = character.nameSprite;
                count++;
            }
        }
    }

    public void SetSelectedCharacter(int selection)
    {
        currentSelection = selection;
        UpdateCurrentArrow();
    }

    /// <summary>
    /// Set the first choice as the selected one.
    /// </summary>
    public void ResetSelection()
    {
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

    public void HideAllOptions()
    {
        foreach (var arrow in arrows)
        {
            arrow.enabled = false;
        }
        gameObject.SetActive(false);
    }
}
