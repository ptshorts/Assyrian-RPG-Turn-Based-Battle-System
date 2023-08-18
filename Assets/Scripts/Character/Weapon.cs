using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /**
     * <summary>
     * The idle image of the weapon, which is not required for weapons that 
     * are not always active (meaning always shown).
     * </summary>
     */
    [Tooltip("What the weapon looks like when idle. Not required if weapon isn't always active.")]
    [SerializeField] private Sprite sprite;

    /**
     * <summary>
     * The image of the weapon when HP reaches 0 (and the character is 
     * knocked out). Not required if the weapon fades off the scene.
     * </summary> 
     */
    [Tooltip("What the weapon looks like after character has collapsed/fallen.")]
    [SerializeField] private Sprite fallenSprite;

    /**
     * <summary>
     * The attack animation showing the weapon carry out a strike.
     * </summary>
     */
    [Tooltip("Attack animation of the weapon.")]
    [SerializeField] private Animator attackAnim;

    /**
     * <summary>
     * A Projectile object will carry a SpriteRenderer (to enable at the 
     * beginning of an attack animation) and an Animator (to begin the
     * animation of the airborne projectile).
     * </summary>
     */
    [Tooltip("An instance of a Projectile object (like for an arrow or bullet).")]
    [SerializeField] private Projectile projectile;

    /**
     * <summary>
     * Indicates whether the weapon is a ranged one that can strike a target 
     * from a longer distance away.
     * </summary>
     */
    [Tooltip("Can this weapon be used from far away?")]
    public bool longRange = false;

    /**
     * <summary>
     * Is the idle sprite of this weapon always shown, such a held sword? Or is 
     * it something that's put away, such as the ready position of the bow and 
     * arrow, or the prayer hands of the sorcerer.
     * </summary>
     */
    [Tooltip("Is the idle state of this sprite always shown or is it hidden once an attack is done?")]
    public bool alwaysActive = true;

    /**
     * <summary>
     * The attack points that a target will endure upon getting struck by this 
     * character.
     * </summary>
     */
    private int ap;

    /**
     * <summary>
     * The initial Attack Points (AP) amount.
     * </summary>
     */
    public int initialAp;

    private Action<int> Callback;
    private Action SummonCallback;

    private Attackable target;

    private Vector3 battlePosition;

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        ap = initialAp;
        if (IsLongRange())
            battlePosition = projectile.transform.localPosition;
    }

    /**
     * <summary>
     * Apply the provided Sprite to the Weon object.
     * </summary>
     */
    private void LoadSprite(Sprite sprite)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
            spriteRenderer.sprite = sprite;
    }

    public int GetAttackPoints()
    {
        return ap;
    }

    public bool IsLongRange()
    {
        return longRange;
    }

    public void PlayAttackAnimation(Action<int> Callback, Attackable target)
    {
        this.Callback = Callback;
        this.target = target;
        attackAnim.enabled = true;
        attackAnim.Play("Attack", -1, 0f);
    }

    public void StopAttackAnimation()
    {
        attackAnim.StopPlayback();
        attackAnim.enabled = false;
        Callback?.Invoke(1);
    }

    public void PlaySummonAnimation(Action SummonCallback)
    {
        this.SummonCallback = SummonCallback;
        attackAnim.enabled = true;
        attackAnim.Play("Summon", -1, 0f);
    }

    public void PlayPostSummoningAnim()
    {
        SummonCallback.Invoke();
    }

    public void HideWeapon()
    {
        GetComponent<SpriteRenderer>().sprite = null;
    }

    public void DisplayProjectile()
    {
        projectile.spriteRenderer.enabled = true;
    }

    public void MoveProjectileToTarget()
    {
        projectile.SetTarget(target.GetLongRangeTarget());
        projectile.Move();
    }

    public void SetCallbackOnProjectile(Action<int> callback)
    {
        projectile.SetCallback(callback);
    }

    /**
     * <summary>
     * Set the projectile object in its battle position. This method is meant to
     * be called by outside callers that manage the positioning of characters.
     * </summary>
     */
    public void SetInBattlePosition()
    {
        projectile.transform.localPosition = battlePosition;
    }

    /**
     * <summary>
     * Animate the weapon's termination. Usually a sprite showing the 
     * weapon has fallen down or fading the sprite out of the scene.
     * </summary>
     */
    public void Terminate(bool fadesOffScene)
    {
        if (fadesOffScene)
        {
            FadeOutWeapon();
        }
        else
        {
            DisplayAsFallen();
        }
    }

    /**
     * <summary>
     * Animate the fading out of the character after getting terminated.
     * </summary>
     */
    private void FadeOutWeapon()
    {
        attackAnim.enabled = true;
        attackAnim.Play("Fade Out", -1, 0f);
    }

    /**
     * <summary>
     * Change the weapon's current sprite to the one set for the weapon's
     * fallen/collapsed look.
     * </summary>
     */
    private void DisplayAsFallen()
    {
        LoadSprite(fallenSprite);
    }

    public void Hide()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
