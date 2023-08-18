using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMenu : MonoBehaviour
{
    [HideInInspector]
    private Action[] callbacks;

    public SpriteRenderer[] arrows;
    public SpriteRenderer[] slots;
    public Sprite[] selectionWords;

    private SpriteRenderer currentArrow;

    private int currentSelection = 0;

    public void Init(Action[] callbacks)
    {
        this.callbacks = callbacks;
        Setup();
        ResetSelection();
    }

    /// <summary>
    /// Set the words of the choices in the menu.
    /// </summary>
    public void Setup()
    {
        int count = 0;
        foreach (var word in selectionWords)
        {
            if (count < slots.Length)
            {
                slots[count].sprite = word;
                count++;
            }
        }
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

    private void HideAllOptions()
    {
        foreach (var arrow in arrows)
        {
            arrow.enabled = false;
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Open this menu.
    /// </summary>
    public void Open(bool show = true)
    {
        if (show) 
            ResetSelection();
        gameObject.SetActive(show);
    }

    /// <summary>
    /// Return to the previous menu.
    /// </summary>
    public void Cancel()
    {
        ResetSelection();
        gameObject.SetActive(false);
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
        if (currentSelection > -1 && currentSelection < callbacks.Length)
        {
            callbacks[currentSelection].Invoke();
        }
    }

    public int GetSelection()
    {
        return currentSelection;
    }
}
