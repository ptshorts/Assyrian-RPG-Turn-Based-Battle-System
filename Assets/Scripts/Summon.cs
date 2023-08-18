using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour, SubMenuItem
{
    /**
     * <summary>
     * The name of the summon.
     * </summary>
     */
    [Tooltip("Name of the summon.")]
    public string shimma;

    /**
     * <summary>
     * An image of the name text used an alternative to the name string.
     * </summary>
     */
    [Tooltip("Optional image of the summon's name.")]
    public Sprite nameSprite;


    /**
     * <summary>
     * An instance of PrimaryAnim that sets the first animation of the summon.
     * </summary>
     */
    [Tooltip("An instance of PrimaryAnim that sets the first animation of the summon.")]
    public PrimaryAnim primaryAnim;

    /**
     * <summary>
     * Used for a second animation of the summoning, as needed. Such as 
     * animation of sprites other than the summon's own sprite.
     * </summary>
     */
    [Tooltip("A second animation of the summoning, as needed.")]
    public Animator secondAnimation;

    /**
     * <summary>
     * The amount of mana points that this summon costs.
     * </summary>
     */
    [Tooltip("The amount of mana points that this summon costs.")]
    public int mpCost;

    /**
     * <summary>
     * The attack points inflicted upon every target.
     * </summary>
     */
    [Tooltip("The attack points inflicted upon every target.")]
    public int ap;

    /// <summary>
    /// Get the sprite version of the summon's name.
    /// </summary>
    /// <returns></returns>
    public Sprite GetNameSprite()
    {
        return nameSprite;
    }

    /// <summary>
    /// Play the summon's chain of animations.
    /// </summary>
    /// <param name="Callback"></param>
    public void PlayAnimation(Action Callback)
    {
        primaryAnim.PlayAnimation(Callback);
    }

    /// <summary>
    /// Returns the attack points inflicted upon every target.
    /// </summary>
    public int GetAttackPoints()
    {
        return ap;
    }
}
