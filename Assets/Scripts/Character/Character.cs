using System;
using UnityEngine;

[SelectionBase]
public class Character : MonoBehaviour
{
    /**
     * <summary>
     * The index of the character object in the party array.
     * </summary>
     */
    [HideInInspector]
    public int memberId;

    /**
     * <summary>
     * The image of the character.
     * </summary>
     */
    [Tooltip("What the character looks like.")]
    [SerializeField] private Sprite sprite;

    /**
     * <summary>
     * The image of the character when HP reaches 0 (and the character is 
     * knocked out). Not required if the character a fades off the scene.
     * </summary>
     */
    [Tooltip("What the character looks like after getting knocked out / terminated.")]
    [SerializeField] private Sprite knockoutSprite;

    /**
     * <summary>
     * This indicates that the character fades off of the screen upon HP level
     * reaching 0 (terminated in battle).
     * </summary>
     */
    [Tooltip("Whether the character will fade off the screen upon termination.")]
    public bool fadesOffScene;

    /**
     * <summary>
     * The name of the character.
     * </summary>
     */
    [Tooltip("Name of the character.")]
    public string shimma;

    /**
     * <summary>
     * An image of the name of the character.
     * </summary>
     */
    [Tooltip("Optional image of the character's name.")]
    public Sprite nameSprite;

    /**
     * <summary>
     * The current experience level of the character.
     * </summary>
     */
    [HideInInspector]
    private int level;

    /**
     * <summary>
     * Current health points.
     * </summary>
     */
    //[HideInInspector]
    public int hp;

    /**
     * <summary>
     * Current max amount of health points (HP).
     * </summary>
     */
    [HideInInspector]
    public int maxHp;

    /**
     * <summary>
     * The max HP amount with which the character starts at the beginning of 
     * the game.
     * </summary>
     */
    [Tooltip("Max HP with which character starts at the beginning of the game.")]
    [field: SerializeField] private int initialHp;

    /**
     * <summary>
     * Current mana points.
     * </summary>
     */
    [HideInInspector]
    public int mp;

    /**
     * <summary>
     * Current max amount of mana points (MP).
     * </summary>
     */
    [HideInInspector]
    public int maxMp;

    /**
     * <summary>
     * The max MP amount with which the character starts at the beginning of 
     * the game.
     * </summary>
     */
    [Tooltip("Max MP with which character starts at the beginning of the game.")]
    [SerializeField] private int initialMp;

    /**
     * <summary>
     * Experience points.
     * </summary>
     */
    [HideInInspector]
    public int xp;

    /**
     * <summary>
     * The experience points required for the character to advance to level 2.
     * This will be used as the base value for all levels with an offset added.
     * </summary>
     */
    [Tooltip("XP required for character to advance to level 2.")]
    [SerializeField] private int nextLevelXp;

    /**
     * <summary>
     * The rate that's used for incrementally raising the required experience
     * points to level up the character, so that it's not always the same 
     * amount, which would make leveling up too easy later in the game when
     * enemies provide greater experience points.
     * </summary>
     */
    [Tooltip("The rate at which required XP grows as the character levels up.")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float levelUpGrowthRate;

    /**
     * <summary>
     * Character type.
     * </summary>
     */
    public Archetype type;

    /**
     * <summary>
     * The percentage at which an attacker's attack points are reduced.
     * </summary>
     */
    [HideInInspector]
    private float armorRate = 0.0f;

    /**
     * <summary>
     * The idle position of the character in the scene.
     * </summary>
     */
    [HideInInspector]
    public Vector3 battlePosition;

    public Cast[] casts;
    public Summon[] summons;
    public Item[] items;

    /// <summary>
    /// 
    /// </summary>
    [HideInInspector]
    private StatusBar hpBar;
    private StatusBar mpBar;

    private Action<int> RemoveFromParty;

    private bool isEnemy;

    public void Start()
    {
        LoadSprite(this.sprite);
        // TODO: evaluate whether this is necessary, once the battle system is
        //  setting this variable
        battlePosition = new Vector3(0, 0, 0);
        hp = initialHp;
        maxHp = initialHp;
        mp = initialMp;
        maxMp = initialMp;
    }

    public void Init(Action<int> RemoveFromParty, bool isEnemy = true, StatusBar hpBar = null, StatusBar mpBar = null)
    {
        this.RemoveFromParty = RemoveFromParty;
        this.isEnemy = isEnemy;
        this.hpBar = hpBar;
        this.mpBar = mpBar;
        if (hpBar != null && mpBar != null)
            SetupStatusBars();
    }

    private void SetupStatusBars()
    {
        SetupHpBar();
        SetupMpBar();
    }

    private void SetupHpBar()
    {
        hpBar.currentValue = hp;
        hpBar.maxValue = maxHp;
    }

    private void SetupMpBar()
    {
        if (mpBar != null)
        {
            mpBar.currentValue = mp;
            mpBar.maxValue = maxMp;
        }
    }

    public void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.currentValue = hp;
            hpBar.UpdateBar();
        }
    }

    public void UpdateMpBar()
    {
        if (mpBar != null)
        {
            mpBar.currentValue = mp;
            mpBar.UpdateBar();
        }
    }

    /**
     * <summary>
     * Apply the provided Sprite to the Character object.
     * </summary>
     */
    private void LoadSprite(Sprite sprite)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer) 
            spriteRenderer.sprite = sprite;
    }

    /**
     * <summary>
     * Set the character object in its battle position. This method is meant to
     * be called by outside callers that manage the positioning of characters.
     * </summary>
     */
    public void SetInBattlePosition()
    {
        transform.localPosition = battlePosition;
    }

    /**
     * <summary>
     * Move the character to the provided position in the parameter.
     * </summary>
     */
    public void MoveToPosition(Vector3 position)
    {
        transform.position = position;
    }

    /**
     * <summary>
     * Animate the character's termination. Usually a sprite showing the 
     * character is knocked out or fading the sprite out of the scene.
     * </summary>
     */
    public void Terminate()
    {
        RemoveFromParty.Invoke(memberId);
        if (fadesOffScene)
        {
            FadeOut();
        }
        else
        {
            DisplayAsKnockedOut();
        }
    }

    /**
     * <summary>
     * Animate the fading out of the character after getting terminated.
     * </summary>
     */
    private void FadeOut()
    {
        var anim = GetComponent<Animator>();
        anim.enabled = true;
        anim.Play("Fade Out", -1, 0f);
    }

    /**
     * <summary>
     * Change the character's current sprite to the one set for the character's
     * knockout look.
     * </summary>
     */
    private void DisplayAsKnockedOut()
    {
        LoadSprite(knockoutSprite);
    }

    public float GetArmorRate()
    {
        return armorRate;
    }

    public bool IsTerminated()
    {
        return hp <= 0;
    }

    public void Hide()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public bool HasCasts()
    {
        return casts.Length > 0;
    }

    public bool HasSummons()
    {
        return summons.Length > 0;
    }

    public bool HasItems()
    {
        return items.Length > 0;
    }
}
