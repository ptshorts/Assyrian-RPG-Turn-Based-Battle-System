using System.Collections;
using UnityEngine;

/// <summary>
/// All characters in a battle scene are attackable. This class handles the
/// reception of damage; the damage text that displays above the character;
/// and the physical targeting of the character (such as a location for 
/// long-ranged objects to approach or for close-quarter units to place into
/// while attacking the character). It also serves the selection arrow of
/// the character to the targeting menu.
/// </summary>
public class Attackable : MonoBehaviour
{
    private Character character;
    [SerializeField] private TextMesh damageText;
    [SerializeField] private GameObject standOffPosition;
    [SerializeField] private GameObject longRangeTarget;
    public SpriteRenderer selectionArrow;

    // weapon is required so that it's also terminated along with the character
    private Weapon weapon;

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        character = GetComponent<Character>();
        weapon = gameObject.GetComponentInChildren<Weapon>();
    }

    /// <summary>
    /// Handles enduring an attack. If the character's HP falls down to 0 or 
    /// below, then the termination is triggered from here. The reception of
    /// damage is adjusted based on the character's armor. Also updates the HP
    /// status bar and invokes the damage text display (above the character).
    /// </summary>
    /// <param name="attackPoints">The amount of damage currently to be endured.</param>
    public void Endure(int attackPoints)
    {
        int enduredPoints = AdjustAttackToArmor(attackPoints);
        character.hp -= enduredPoints;
        if (character.hp <= 0)
        {
            // in case HP goes below 0, reset it to 0
            character.hp = 0;
            character.Terminate();
            if (weapon != null)
            {
                weapon.Terminate(character.fadesOffScene);
            }
        }
        character.UpdateHpBar();
        DisplayDamageAmount(enduredPoints);
    }

    /**
     * <summary>
     * Reduce the attack points that are to be endured by this character based
     * on the worn armor, if any.
     * </summary>
     */
    public int AdjustAttackToArmor(int attackPoints)
    {
        return attackPoints - (int)(attackPoints * character.GetArmorRate());
    }

    public Vector3 GetPosition()
    {
        return GetComponent<Transform>().position;
    }

    /**
     * <summary>
     * Provides the position where an attacker should be placed in a close 
     * quarter combat situation, in front of this attackable character.
     * </summary>
     */
    public Vector3 GetStandOffPosition()
    {
        return standOffPosition.transform.position;
    }

    /**
     * <summary>
     * Provides the position where a projectile would go to hit this attackable
     * character.
     * </summary>
     */
    public Vector3 GetLongRangeTarget()
    {
        return longRangeTarget.transform.position;
    }

    /// <summary>
    /// Displays the damage text above the character.
    /// </summary>
    /// <param name="damagePoints">The amount of damage incurred by the current
    /// attack against this character.</param>
    private void DisplayDamageAmount(int damagePoints)
    {
        if (damageText)
        {
            damageText.text = damagePoints.ToString();
            StartCoroutine(ShowDamageText());
        }
    }

    private IEnumerator ShowDamageText()
    {
        damageText.GetComponent<MeshRenderer>().enabled = true;
        yield return new WaitForSeconds(2);
        damageText.GetComponent<MeshRenderer>().enabled = false;
    }

    public bool IsTerminated()
    {
        return character.IsTerminated();
    }
}
