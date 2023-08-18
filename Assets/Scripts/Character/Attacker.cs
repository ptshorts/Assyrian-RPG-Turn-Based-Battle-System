using UnityEngine;

/**
 * A weapon-centric system of managing attacks from a character.
 */
public class Attacker : MonoBehaviour
{
    private Character character;

    public Weapon weapon;
    public Summon summon;
    private Attackable target;
    private Character[] targets;
    [SerializeField] private TurnBasedSystem turnSystem;

    private Summon currentSummon;

    public void Start()
    {
        character = GetComponent<Character>();
        SetTurnBasedSystem();
    }

    /**
     * <summary>
     * Find
     * </summary>
     */
    public void SetTurnBasedSystem()
    {
        turnSystem = GameObject.Find("Turn Based System")?.GetComponent<TurnBasedSystem>();
    }

    /**
     * <summary>
     * Attack an Attackable target.
     * </summary>
     */
    public void Attack(Attackable target)
    {
        this.target = target;
        if (!weapon.IsLongRange())
            MoveAndStrike();
        else
            Strike();
    }

    /**
     * <summary>
     * Move the attacker to the target's close quarter position, then strike.
     * </summary>
     */
    private void MoveAndStrike()
    {
        if (character == null)
            character = GetComponent<Character>();
        character.MoveToPosition(target.GetStandOffPosition());
        Strike();
    }

    /**
     * <summary>
     * Strike the Attackable target. (Can only attack one target at a time.)
     * </summary>
     */
    private void Strike()
    {
        if (weapon.IsLongRange())
            weapon.SetCallbackOnProjectile(PostAttackEffects);
        weapon.PlayAttackAnimation(PostAttackEffects, target);
    }

    public void Summon(Character[] targets, int id)
    {
        this.targets = targets;
        if (id < character.summons.Length) {
            UpdateSummon(id);
            ActivateSummon(true);
            weapon.PlaySummonAnimation(PostSummoningAnimation);
        }
    }

    /**
     * <summary>
     * Handle all the effects that are supposed to happen after an attack is 
     * animated. This method is meant to be used by an outside caller, such as 
     * animation event.
     * </summary>
     * 
     * TODO: redesign this so that the method is not getting called for nothing
     */
    public void PostAttackEffects(int callerId)
    {
        // if it's a long-range weapon and the method calling this method
        // is the StopAttackAnimation, then it's too early to do this;
        // another method will call this (the one where the projectile makes
        // contact with the target object)
        if (!(weapon.IsLongRange() && callerId == 1))
        {
            RecoverToBattlePosition();
            InflictAttackDamage();
            turnSystem.DoneWithTurn();
        }
    }

    public void PostSummoningAnimation()
    {
        currentSummon.PlayAnimation(PostSummonEffects);
    }

    public void PostSummonEffects()
    {
        InflictSummonDamage();
        //ActivateSummon(false);
        turnSystem.DoneWithTurn();
    }

    /**
     * <summary>
     * Go back to this character's idle position.
     * <4/summary>
     */
    private void RecoverToBattlePosition()
    {
        if (weapon.IsLongRange())
            weapon.SetInBattlePosition();
        else
            character.SetInBattlePosition();
    }

    /**
     * <summary>
     * Inflict the due damages on the Attackable target.
     * </summary>
     */
    private void InflictAttackDamage()
    {
        target.Endure(weapon.GetAttackPoints());
    }

    /**
     * <summary>
     * Inflict the due damages on the Attackable target.
     * </summary>
     */
    private void InflictCastDamage()
    {
        // TODO: develop the InflictCastDamage method
        //target.Endure(weapon.GetCastPoints());
    }

    /**
     * <summary>
     * Inflict the due damages on the Attackable target.
     * </summary>
     */
    private void InflictSummonDamage()
    {
        foreach (var target in targets)
        {
            target.GetComponent<Attackable>()?.Endure(currentSummon.GetAttackPoints());
        }
    }

    private void UpdateSummon(int id)
    {
        currentSummon = character.summons[id];
    }

    private void ActivateSummon(bool active)
    {
        currentSummon.gameObject.SetActive(active);
    }
}
