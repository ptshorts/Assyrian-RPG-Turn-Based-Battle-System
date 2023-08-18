using UnityEngine;

public class BattleMenu : MonoBehaviour
{
    private ControlManager.State DEFAULT_PREV_STATE = ControlManager.State.ActionSelection;
    private ControlManager.State DEFAULT_STATE = ControlManager.State.ActionSelection;

    [SerializeField] GameObject actionMenuBox;
    [SerializeField] GameObject castMenuBox;
    [SerializeField] GameObject summonMenuBox;
    [SerializeField] GameObject itemMenuBox;
    [SerializeField] GameObject targetMenuBox;

    [HideInInspector] 
    public PartyMenu party;

    [HideInInspector]
    public ActionMenu action;

    [HideInInspector]
    public SubMenu cast;

    [HideInInspector]
    public SubMenu summon;

    [HideInInspector]
    public SubMenu item;

    [HideInInspector]
    public TargetMenu target;

    public ControlManager controlManager;

    private ControlManager.State currentState;
    private ControlManager.State previousState;

    public void Init(ControlManager controlManager)
    {
        this.controlManager = controlManager;
        controlManager.state = ControlManager.State.None;
    }

    public void ShowPartyIndicator(int characterNumber)
    {
        if (characterNumber >= 0)
        {
            party.SetSelectedCharacter(characterNumber);
        }
    }

    public void ShowPartyMenu(bool show = true)
    {
        if (!show)
            party.HideAllOptions();
    }

    public void ShowActionMenu(bool show = true)
    {
        if (show)
            SetStateToAction();
        action.Open(show);
    }

    public void ShowCastMenu(bool show = true)
    {
        if (show)
            SetStateToCast();
        castMenuBox.SetActive(show);
    }

    public void ShowSummonMenu(bool show = true)
    {
        if (show) 
        {
            SetStateToSummon();
            summon.Open(); 
        }
        else
        {
            summonMenuBox.SetActive(show);
        }
    }

    public void ShowItemMenu(bool show = true)
    {
        if (show)
            SetStateToItem();
        itemMenuBox.SetActive(show);
    }

    public void ShowTargetMenu(ActionType actionType, bool show = true)
    {
        if (show)
        {
            target.actionType = actionType;
            controlManager.state = ControlManager.State.TargetSelection;
            SetStateToTarget();
        }
        target.Show(show);
    }

    /// <summary>
    /// Returns to the previous menu and clears the previous state back to the 
    /// default (action menu).
    /// </summary>
    public void SetStateToPrevious()
    {
        SetCurrentState(previousState);
        ResetPreviousState();
        UpdateState();
    }

    public void SetStateToAction()
    {
        UpdatePreviousState();
        SetCurrentState(ControlManager.State.ActionSelection);
        UpdateState();
    }

    public void SetStateToCast()
    {
        UpdatePreviousState();
        SetCurrentState(ControlManager.State.CastSelection);
        UpdateState();
    }

    public void SetStateToSummon()
    {
        UpdatePreviousState();
        SetCurrentState(ControlManager.State.SummonSelection);
        UpdateState();
    }

    public void SetStateToItem()
    {
        UpdatePreviousState();
        SetCurrentState(ControlManager.State.ItemSelection);
        UpdateState();
    }

    public void SetStateToTarget()
    {
        UpdatePreviousState();
        SetCurrentState(ControlManager.State.TargetSelection);
        UpdateState();
    }

    private void ResetPreviousState()
    {
        previousState = DEFAULT_PREV_STATE;
    }

    private void ResetCurrentState()
    {
        currentState = DEFAULT_STATE;
    }

    /// <summary>
    /// Update previous state, so that when the user wants to go back in the 
    /// menu, it knows where the previous place was in the menu.
    /// </summary>
    private void UpdatePreviousState()
    {
        previousState = currentState;
    }

    private void SetCurrentState(ControlManager.State state)
    {
        currentState = state;
    }

    /// <summary>
    /// Update the menu state.
    /// </summary>
    private void UpdateState()
    {
        controlManager.state = currentState;
    }

    /// <summary>
    /// Reset the action selection and the previous state, so that when the 
    /// menu opens up next time, it won't have the same choice and previous
    /// state selected.
    /// </summary>
    private void ResetMenu()
    {
        ResetPreviousState();
        action.ResetSelection();
    }

    /// <summary>
    /// Close all menus and reset the selection for next turn.
    /// </summary>
    public void CloseAll()
    {
        ResetMenu();
        ShowPartyMenu(false);
        ShowActionMenu(false);
        ShowCastMenu(false);
        ShowSummonMenu(false);
        ShowItemMenu(false);
        ShowTargetMenu(ActionType.None, false);
        controlManager.state = ControlManager.State.None;
    }
}
