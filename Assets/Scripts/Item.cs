using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, SubMenuItem
{

    /**
     * <summary>
     * The name of the item.
     * </summary>
     */
    [Tooltip("Name of the item.")]
    public string shimma;

    /**
     * <summary>
     * An image of the name text used an alternative to the name string.
     * </summary>
     */
    [Tooltip("Optional image of the name text.")]
    public Sprite nameSprite;

    public Sprite GetNameSprite()
    {
        return nameSprite;
    }
}
