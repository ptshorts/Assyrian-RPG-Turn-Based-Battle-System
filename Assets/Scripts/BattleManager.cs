using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the battle scene manager. It ensures that everything is 
/// instantiated, or set, in the right order, so that nothing is assigned 
/// prior to its dependencies having been instantiated and/or initialized. 
/// It also serves as the parent to most of the code in the battle scene,
/// that way it's always clear where to go to make things happen
/// </summary>
public class BattleManager : MonoBehaviour
{
    [SerializeField] TurnBasedSystem turnSystem;
    [SerializeField] Party playerParty;
    [SerializeField] Party enemyParty;

    // characters
    public Character[] players;
    public Character[] enemies;

    private Character currentCharacter;

    // UI
    [SerializeField] GameObject battleUi;
    [SerializeField] BattleResultsBox battleResults;

    // menu
    [SerializeField] BattleMenu battleMenu;
    [SerializeField] PartyMenu partyMenu;
    [SerializeField] ActionMenu actionMenu;
    [SerializeField] SubMenu castMenu;
    [SerializeField] SubMenu summonMenu;
    [SerializeField] SubMenu itemMenu;
    [SerializeField] TargetMenu targetMenu;

    // status bars
    [SerializeField] StatusBar[] hpBars;
    [SerializeField] StatusBar[] mpBars;

    // message boxes
    [SerializeField] GameObject missedMsgBox;
    [SerializeField] SpriteRenderer errorIndicator;

    // controls
    [SerializeField] ControlManager controlManager;

    // animators
    [SerializeField] Animator victoryDanceAnim;

    // variables
    private int subMenuSelection;
    private int earnedGold;
    private int earnedExperience;


    private void Start()
    {
        Debug.Log("BattleManager -- Start()");
        Init();
    }

    /// <summary>
    /// Setup BattleManager. The order of this initialization matters.
    /// </summary>
    private void Init()
    {
        // party characters
        SetupPlayerParty();
        SetupEnemyParty();
        SetFirstCharacter();
        // menus
        SetupPartyMenu();
        SetupActionMenu();
        SetupCastMenu();
        SetupSummonMenu();
        SetupItemMenu();
        SetupTargetMenu();
        SetupBattleMenu();
        // turn-based system
        SetupTurnBasedSystem();
        // controls
        SetupControlManager();
        // rewards
        SetupRewards();
        // start game
        StartFirstTurn();
    }

    /// <summary>
    /// Setup the instance of the Party class that contains the player's 
    /// characters along with some of their associated UI components, such as
    /// the HP and MP bars in the scene.
    /// </summary>
    private void SetupPlayerParty()
    {
        playerParty.Init(players, isEnemy: false, hpBars, mpBars);
    }


    /// <summary>
    /// Setup the instance of the Party class that contains the enemy 
    /// characters.
    /// </summary>
    private void SetupEnemyParty()
    {
        enemyParty.Init(enemies, isEnemy: true, null, null);
    }

    /// <summary>
    /// Set the first current character.
    /// </summary>
    private void SetFirstCharacter()
    {
        if (playerParty.characters.Length > 0)
            currentCharacter = playerParty.characters[0];
    }

    /// <summary>
    /// Setup the system that handles the turn-based battle mode.
    /// </summary>
    private void SetupTurnBasedSystem()
    {
        turnSystem.Init(
            playerParty, 
            enemyParty, 
            battleMenu, 
            victoryDanceAnim,
            SetCurrentCharacter, 
            SetupEndOfBattleUi, 
            Defeat
       );
    }
    
    /// <summary>
    /// Setup the menu that shows the character names and indicates which 
    /// character is selected by showing an arrow next to the name.
    /// </summary>
    private void SetupPartyMenu()
    {
        partyMenu.Init(playerParty, rowCount: 3);
    }

    /// <summary>
    /// Setup the main battle menu where actions are selected (attack, cast, 
    /// summon, item).
    /// </summary>
    private void SetupActionMenu()
    {
        Action[] callbacks = new Action[4]
        {
            AttackMenuCallback,
            CastMenuCallback,
            SummonMenuCallback,
            ItemMenuCallback
        };
        actionMenu.Init(callbacks);
    }

    /// <summary>
    /// Setup the menu used for selecting enemy targets.
    /// </summary>
    private void SetupTargetMenu()
    {
        // no actionId yet, so passing 0 (TODO: find a better way to handle this)
        targetMenu.Init(enemyParty, TargetCallback, 0);
    }

    /// <summary>
    /// Setup the menu from which casts are selected.
    /// </summary>
    private void SetupCastMenu()
    {
        castMenu.Init(currentCharacter, SubMenuType.Cast, PostCastCallback, GoBackToActionMenu);
    }

    /// <summary>
    /// Setup the menu from which summons are selected.
    /// </summary>
    private void SetupSummonMenu()
    {
        summonMenu.Init(currentCharacter, SubMenuType.Summon, PostSummonCallback, GoBackToActionMenu);
    }

    /// <summary>
    /// Setup the menu from which items are selected.
    /// </summary>
    private void SetupItemMenu()
    {
        itemMenu.Init(currentCharacter, SubMenuType.Item, PostItemCallback, GoBackToActionMenu);
    }

    /// <summary>
    /// Set the battle menu dependencies and initialize it.
    /// </summary>
    private void SetupBattleMenu()
    {
        battleMenu.Init(controlManager);
        battleMenu.party = partyMenu;
        battleMenu.action = actionMenu;
        battleMenu.cast = castMenu;
        battleMenu.summon = summonMenu;
        battleMenu.item = itemMenu;
        battleMenu.target = targetMenu;
    }

    /// <summary>
    /// Set up the code that handles the onscreen control buttons.
    /// </summary>
    private void SetupControlManager()
    {
        controlManager.Init(battleMenu);
    }

    /// <summary>
    /// Set the things that are to be collected after the battle is won.
    /// </summary>
    private void SetupRewards()
    {
        // TODO: the game system should plug into this and decide these things.
        earnedGold = enemies.Length * 11;
        earnedExperience = enemies.Length * 27;
    }

    /// <summary>
    /// Begin the first turn in the turn-based battle mode.
    /// </summary>
    private void StartFirstTurn()
    {
        turnSystem.Next();
    }

    /// <summary>
    /// Set the current character (from the player's party), so that the battle
    /// menu could function based on that character.
    /// </summary>
    /// <param name="character">The Character instance that's to be set as the current character.</param>
    private void SetCurrentCharacter(Character character)
    {
        currentCharacter = character;
    }

    /// <summary>
    /// Go straight into the target selection menu.
    /// </summary>
    private void AttackMenuCallback()
    {
        battleMenu.ShowTargetMenu(ActionType.Attack);
    }

    /// <summary>
    /// Shows the menu for the list of casts based on the character.
    /// </summary>
    private void CastMenuCallback()
    {
        battleMenu.cast.UpdateCharacter(currentCharacter);
        battleMenu.ShowCastMenu();
    }

    /// <summary>
    /// Shows the menu for the list of summons based on the character.
    /// </summary>
    private void SummonMenuCallback()
    {
        battleMenu.summon.UpdateCharacter(currentCharacter);
        battleMenu.ShowSummonMenu();
    }

    /// <summary>
    /// Shows the menu for the list of items based on the character.
    /// </summary>
    private void ItemMenuCallback()
    {
        battleMenu.item.UpdateCharacter(currentCharacter);
        battleMenu.ShowItemMenu();
        // TODO: develop item callback (type of item, selected member from party)
    }

    /// <summary>
    /// Goes back to the main action menu (attack, cast, summon, item).
    /// </summary>
    private void GoBackToActionMenu()
    {
        battleMenu.SetStateToAction();
    }

    /// <summary>
    /// Transitions the menu UI from cast selection SubMenu to target menu.
    /// </summary>
    /// <param name="castId">The index of the cast selected from the character's list of casts.</param>
    private void PostCastCallback(int castId)
    {
        // TODO: develop the PostCastCallback method
        if (currentCharacter.HasCasts())
        {
            subMenuSelection = castId;
            battleMenu.SetStateToTarget();
            battleMenu.target.ResetSelection();
        // base next action on the cast ID
        }
        else
        {
            // TODO: indicate error (with visual & sound) in PostCastCallback
            ShowErrorIndicator();
        }
    }

    /// <summary>
    /// Transitions the menu UI from summon selection SubMenu to target menu.
    /// </summary>
    /// <param name="summonId">The index of the summon selected from the character's list of summons.</param>
    private void PostSummonCallback(int summonId)
    {
        if (currentCharacter.HasSummons() && currentCharacter.mp >= currentCharacter.summons[summonId].mpCost) {
            subMenuSelection = summonId;
            battleMenu.SetStateToTarget();
            battleMenu.target.actionType = ActionType.Summon;
            battleMenu.target.SelectAll();
        }
        else
        {
            // TODO: indicate error (with visual & sound) in PostSummonCallback
            ShowErrorIndicator();
        }
    }

    /// <summary>
    /// Transitions the menu UI from item selection SubMenu to player party menu.
    /// </summary>
    /// <param name="itemId">The index of the item selected from the character's list of items.</param>
    private void PostItemCallback(int itemId)
    {
        // TODO: develop the PostItemCallback method
        if (currentCharacter.HasItems())
        {
            subMenuSelection = itemId;
            //battleMenu.SetStateToPartyTarget();
            battleMenu.target.actionType = ActionType.UseItem;
            //battleMenu.partyTarget.ResetSelection();
        }
        else
        {
            // TODO: indicate error (with visual & sound) in PostItemCallback
            ShowErrorIndicator();
        }
    }

    /// <summary>
    /// Handles what happens after selecting a target from the target menu.
    /// </summary>
    /// <param name="actionType">Whether it's attack, cast, or summon.</param>
    /// <param name="targets">The index of the target in the enemy characters array.</param>
    /// <param name="id">The cast id or summon id.</param>
    private void TargetCallback(ActionType actionType, int[] targets)
    {
        switch (actionType)
        {
            case ActionType.None:
                {
                    // cancel
                    battleMenu.SetStateToPrevious();
                    break;
                }
            case ActionType.Attack:
                {
                    battleMenu.CloseAll();
                    if (targets.Length > 0 && enemyParty.characters.Length > targets[0])
                    {
                        Attackable target = enemyParty.characters[targets[0]].GetComponent<Attackable>();
                        currentCharacter.GetComponent<Attacker>().Attack(target);
                    }
                    break;
                }
            case ActionType.Cast:
                {
                    // TODO: develop cast outcome after target has been selected
                    //currentCharacter.GetComponent<Attacker>().Cast(targets, subMenuSelection);
                    break;
                }
            case ActionType.Summon:
                {
                    battleMenu.CloseAll();
                    if (targets.Length > 0 && enemyParty.characters.Length > targets[0])
                    {
                        currentCharacter.mp = currentCharacter.mp - currentCharacter.summons[subMenuSelection].mpCost;
                        currentCharacter.UpdateMpBar();
                        currentCharacter.GetComponent<Attacker>().Summon(enemyParty.characters, subMenuSelection);
                    }
                    break;
                }
        }
    }

    /// <summary>
    /// Show the error indicator when an impossible selection is being made,
    /// such as trying to select from an empty menu.
    /// </summary>
    private void ShowErrorIndicator()
    {
        StartCoroutine(IndicateError());
    }

    /// <summary>
    /// Transition from battle mode to results mode.
    /// </summary>
    /// <param name="victorious">Whether the battle has been won by the player's party.</param>
    private void SetupEndOfBattleUi(bool victorious)
    {
        HideControls();
        HideBattleMenu();
        if (victorious)
        {
            CollectRewards();
            ShowResults();
        }
    }

    /// <summary>
    /// Hide the on-screen control buttons.
    /// </summary>
    private void HideControls()
    {
        controlManager.gameObject.SetActive(false);
    }

    /// <summary>
    /// Hide the battle menu UI 
    /// </summary>
    private void HideBattleMenu()
    {
        battleUi.gameObject.SetActive(false);
    }

    /// <summary>
    /// Starts a chain of battle results that starts with the gold gains, then
    /// transitions to the experience points earned.
    /// </summary>
    private void ShowResults()
    {
        StartCoroutine(ShowGoldEarnings());
    }

    /// <summary>
    /// Show the results UI box with the gold gains, then transition to showing
    /// the experience points earned.
    /// </summary>
    private IEnumerator ShowGoldEarnings()
    {
        battleResults.gameObject.SetActive(true);
        battleResults.ShowGold(earnedGold);
        yield return new WaitForSeconds(3);
        StartCoroutine(ShowExperienceGains());
    }

    /// <summary>
    /// Show the experience points earned, then hide the results UI box.
    /// </summary>
    private IEnumerator ShowExperienceGains()
    {
        battleResults.ShowExperience(earnedExperience);
        yield return new WaitForSeconds(3);
        battleResults.gameObject.SetActive(false);
        EndOfBattle();
    }

    /// <summary>
    /// Show the error indicator for a short amount of time.
    /// </summary>
    private IEnumerator IndicateError()
    {
        errorIndicator.enabled = true;
        yield return new WaitForSeconds(1);
        errorIndicator.enabled = false;
    }

    /// <summary>
    /// Adds things like gold and experience points to the inventory and 
    /// character attributes via the main game system, which does not exist
    /// here and needs to be plugged in.
    /// </summary>
    private void CollectRewards()
    {
        // TODO: develop the method to add things like gold and experience
        //  points to the game's system.
        Debug.Log("CollectRewards()");
    }


    /// <summary>
    /// Do what needs to happen after the results are shown, post victory.
    /// </summary>
    private void EndOfBattle()
    {
        // TODO: develop the EndOfBattle method that's invoked after victory.
        Debug.Log("EndOfBattle() — Still doin' the khigga!");
    }


    /// <summary>
    /// Do what needs to happen after the results are shown.
    /// </summary>
    private void Defeat()
    {
        // TODO: develop the defeat outcome.
        Debug.Log("Defeat()");
    }
}
