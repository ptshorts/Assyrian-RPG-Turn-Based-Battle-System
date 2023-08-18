using System;
using UnityEngine;

public class TargetMenu : MonoBehaviour
{
    [HideInInspector]
    public Party enemyParty;

    [HideInInspector]
    public Action<ActionType, int[]> Callback;
    
    private SpriteRenderer currentArrow;

    private int currentSelection = 0;

    public ActionType actionType;

    public void Init(Party enemyParty, Action<ActionType, int[]> Callback, ActionType actionId)
    {
        this.enemyParty = enemyParty;
        this.Callback = Callback;
        this.actionType = actionId;
        if (actionType == ActionType.Summon)
        {
            SelectAll();
        }
        else
        {
            // set selection to the first enemy character
            ResetSelection();
        }
    }

    public void ResetSelection()
    {
        currentSelection = 0;
        int count = 0;
        foreach (var arrow in enemyParty.arrows)
        {
            if (count != 0) { 
                arrow.enabled = false;
            } 
            else
            {
                UpdateCurrentArrow();
            }
            count++;
        }
    }

    public void UpdateSelection()
    {
        foreach (var arrow in enemyParty.arrows)
        {
            if (arrow != currentArrow)
            {
                arrow.enabled = false;
            } 
            else if (gameObject.activeSelf)
                arrow.enabled = true;
        }
    }

    private void HideAllOptions()
    {
        foreach (var arrow in enemyParty.arrows)
        {
            arrow.enabled = false;
        }
    }

    private void UpdateCurrentArrow()
    {
        currentArrow = enemyParty.arrows[currentSelection];
        UpdateSelection();
    }

    public void Show(bool show = true)
    {
        gameObject.SetActive(show);
        if (show) 
            ResetSelection();
    }

    public void Cancel()
    {
        HideAllOptions();
        // 0 is for canceling
        Callback(0, null);
        gameObject.SetActive(false);
    }

    public void Previous()
    {
        if (currentSelection <= 0)
            currentSelection = enemyParty.arrows.Length - 1;
        else
            currentSelection--;
        UpdateCurrentArrow();
    }

    public void Next()
    {
        if (currentSelection >= enemyParty.arrows.Length - 1)
            currentSelection = 0;
        else 
            currentSelection++;
        UpdateCurrentArrow();
    }

    public void SelectAll()
    {
        foreach (var arrow in enemyParty.arrows)
        {
            arrow.enabled = true;
        }
    }

    public void Execute()
    {
        HideAllOptions();
        Callback(actionType, new int[] { currentSelection });
    }

    public int GetSelection()
    {
        return currentSelection;
    }
}
