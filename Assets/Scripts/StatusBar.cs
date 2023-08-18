using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is used for the HP and MP statuses.
/// </summary>
public class StatusBar : MonoBehaviour
{
    public SpriteRenderer bar;

    public float currentValue;
    public float maxValue;

    public void Start()
    {
        UpdateBar();
    }

    public void UpdateBar()
    {
        if (maxValue != 0)
        {
            bar.transform.localScale = new Vector3((currentValue / maxValue), 1, 1);
        }
    }
}
