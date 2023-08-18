using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the turns in the turn-based battle mode.
/// </summary>
public class TurnBasedSystem : MonoBehaviour
{
    [HideInInspector]
    public Party playerParty;

    [HideInInspector]
    public Party enemyParty;

    BattleMenu battleMenu;

    private int playerTurn = 0;
    private int enemyTurn = 0;

    private Animator victoryDanceAnim;

    public Action<Character> SetCurrentCharacter;
    public Action<bool> SetEndSceneUi;
    public Action DefeatCallback;


    public void Init(Party playerParty, Party enemyParty, BattleMenu battleMenu,
        Animator victoryDance, Action<Character> SetCurrentCharacter, 
        Action<bool> SetEndSceneUi, Action DefeatCallback)
    {
        this.playerParty = playerParty;
        this.enemyParty = enemyParty;
        this.battleMenu = battleMenu;
        this.victoryDanceAnim = victoryDance;
        this.SetCurrentCharacter = SetCurrentCharacter;
        this.SetEndSceneUi = SetEndSceneUi;
        this.DefeatCallback = DefeatCallback;
    }

    /// <summary>
    /// Invoke the next turn. (The first turn is also a next turn.)
    /// </summary>
    public void Next()
    {
        if (enemyParty.characters == null || enemyParty.characters.Length < 1)
        {
            Victory();
        }
        else if (playerParty.characters == null || playerParty.characters.Length < 1)
        {
            Defeat();
        }
        else if (playerTurn < playerParty.characters.Length)
        {
            SetCurrentCharacter.Invoke(playerParty.characters[playerTurn]);
            SetCharacterIndicator(playerTurn);
            ShowActionMenu();
            playerTurn++;
        }
        else
        {
            if (enemyTurn > enemyParty.characters.Length - 1)
            {
                ResetCounters();
                Next();
            }
            else
            {
                StartCoroutine(EnemyTurn(seconds: 2));
            }
        }
    }

    private void SetCharacterIndicator(int characterNumber)
    {
        battleMenu.ShowPartyIndicator(characterNumber);
    }

    private void ShowActionMenu()
    {
        battleMenu.ShowActionMenu();
    }

    private void EnemyAction()
    {
        Attackable member = GetRandomPartyMember();
        if (enemyTurn < enemyParty.characters.Length)
            enemyParty.characters[enemyTurn].GetComponent<Attacker>().Attack(member);
    }

    private Attackable GetRandomPartyMember()
    {
        int random = UnityEngine.Random.Range(0, playerParty.characters.Length - 1);
        return playerParty.characters[random].GetComponent<Attackable>();
    }

    private void ResetCounters()
    {
        playerTurn = 0;
        enemyTurn = 0;
    }

    public void DoneWithTurn()
    {
        Next();
    }

    private void Victory()
    {
        StartCoroutine(EndBattle(3, true));
    }

    private void Defeat()
    {
        SetEndSceneUi.Invoke(false);
        StartCoroutine(EndBattle(3, false));
    }

    private void VictoryDance()
    {
        victoryDanceAnim.enabled = true;
        victoryDanceAnim.Play("Victory Dance", -1, 0);
    }

    private IEnumerator EnemyTurn(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        EnemyAction();
        enemyTurn++;
    }

    private IEnumerator EndBattle(int seconds, bool victorious)
    {
        yield return new WaitForSeconds(seconds);
        if (victorious)
        {
            playerParty.gameObject.SetActive(false);
            enemyParty.gameObject.SetActive(false);
            SetEndSceneUi.Invoke(true);
            VictoryDance();
        }
        else
        {
            DefeatCallback.Invoke();
        }
    }
}
