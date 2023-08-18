using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The roses are spawned in multiple rows, where each row conforms to a sprite
/// order which fits cohesively into layers along with other objects on the 
/// scene (namely, the characters).
/// </summary>
public class RoseSpawnField : MonoBehaviour
{
    // distance between roses on the x-axis
    public float deltaX;

    // x-axis starting point from the left
    public float initialX;

    // x-axis ending point on the right
    public float finalX;

    // the y upper limit is set by the y-axis point of one of the rows that
    // contains characters in it, so that the layering will be considered
    public GameObject upperLimitY;

    // the order to be set in the SpriteRenderer
    public int spriteOrder;

    // the last x-axis point where a rose was placed; used to figure the
    // position of the next rose using the deltaX value
    [HideInInspector]
    public float? lastX = null;

    public float GetNextX()
    {
        if (lastX == null)
            lastX = initialX;
        lastX += deltaX;
        // just to rig the twisted requirements, placing initialX as substitute
        return lastX ?? initialX;
    }

    public float GetY()
    {
        return upperLimitY.transform.localPosition.y;
    }
}
