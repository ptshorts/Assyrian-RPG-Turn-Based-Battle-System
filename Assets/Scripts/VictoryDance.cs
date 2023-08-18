using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is for removing the animation of the victory dance after it reaches
/// a certain place in the scene.
/// </summary>
public class VictoryDance : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Animator anim = collision.gameObject.GetComponent<Animator>();
        if (anim != null)
        {
            anim.StopPlayback();
            anim.enabled = false;
            collision.gameObject.SetActive(false);
        }
    }
}
