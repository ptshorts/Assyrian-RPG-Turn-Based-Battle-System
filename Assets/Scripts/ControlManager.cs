using System;
using UnityEngine;

/// <summary>
/// The code for the onscreen control buttons.
/// </summary>
public class ControlManager : MonoBehaviour
{
    [HideInInspector]
    public BattleMenu menu;

    public enum State
    {
        None,
        PartySelection,
        ActionSelection,
        CastSelection,
        SummonSelection,
        ItemSelection,
        TargetSelection,
    }

    public State state;

    public void Init(BattleMenu battleMenu)
    {
        this.menu = battleMenu;
        this.state = State.ActionSelection;
    }

    public void OnClickUp()
    {
        switch (state)
        {
            case State.PartySelection:
                {
                    // TODO: develop party selection, as needed (OnClickUp)
                    break;
                }
            case State.ActionSelection:
                {
                    menu.action.Previous();
                    break;
                }
            case State.CastSelection:
                {
                    menu.cast.Previous();
                    break;
                }
            case State.SummonSelection:
                {
                    menu.summon.Previous();
                    break;
                }
            case State.ItemSelection:
                {
                    menu.item.Previous();
                    break;
                }
            case State.TargetSelection:
                {
                    menu.target.Previous();
                    break;
                }
        }
    }

    public void OnClickDown()
    {
        switch (state)
        {
            case State.PartySelection:
                {
                    // TODO: develop party selection, as needed (OnClickDown)
                    break;
                }
            case State.ActionSelection:
                {
                    menu.action.Next();
                    break;
                }
            case State.CastSelection:
                {
                    menu.cast.Next();
                    break;
                }
            case State.SummonSelection:
                {
                    menu.summon.Next();
                    break;
                }
            case State.ItemSelection:
                {
                    menu.item.Next();
                    break;
                }
            case State.TargetSelection:
                {
                    menu.target.Next();
                    break;
                }
        }
    }

    public void OnClickExecute()
    {
        switch (state)
        {
            case State.PartySelection:
                {
                    // TODO: develop party selection, as needed (OnClickExecute)
                    break;
                }
            case State.ActionSelection:
                {
                    menu.action.Execute();
                    break;
                }
            case State.CastSelection:
                {
                    menu.cast.Execute();
                    break;
                }
            case State.SummonSelection:
                {
                    menu.summon.Execute();
                    break;
                }
            case State.ItemSelection:
                {
                    menu.item.Execute();
                    break;
                }
            case State.TargetSelection:
                {
                    menu.target.Execute();
                    break;
                }
        }
    }

    public void OnClickCancel()
    {
        switch (state)
        {
            case State.PartySelection:
                {
                    // TODO: develop party selection, as needed (OnClickCancel)
                    break;
                }
            case State.ActionSelection:
                {
                    // do nothing, unless party selection is implemented; in
                    // such case, menu.action.Cancel() must take menu back to
                    // party selection
                    break;
                }
            case State.CastSelection:
                {
                    menu.cast.Cancel();
                    break;
                }
            case State.SummonSelection:
                {
                    menu.summon.Cancel();
                    break;
                }
            case State.ItemSelection:
                {
                    menu.item.Cancel();
                    break;
                }
            case State.TargetSelection:
                {
                    menu.target.Cancel();
                    break;
                }
        }
    }
}
