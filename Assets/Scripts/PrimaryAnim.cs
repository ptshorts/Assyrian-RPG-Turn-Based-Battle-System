using System;
using UnityEngine;

/// <summary>
/// This class is used to induce a chain of animations, such as the summon
/// animation where separate components (or sprites) are animated. (Example:
/// Ishtar hovers into the scene, then battle chariots start charging. And once
/// that's done, Ishtar hovers back out of the scene.)
/// </summary>
public class PrimaryAnim : MonoBehaviour
{
    public Animator firstAnim;
    public Animator secondAnim;

    // additional scene effects to call (such as roses populating in the field)
    public Action SceneEffectCallback;

    // the function to call once the final animation ends
    private Action EndOfPlaybackCallback;

    private float prevAnimSpeed;

    private bool hasInvokedEndOfPlayback = false;

    /// <summary>
    /// Play the first animation in a chain of animations.
    /// </summary>
    /// <param name="Callback"></param>
    public void PlayAnimation(Action EndOfPlaybackCallback)
    {
        this.EndOfPlaybackCallback = EndOfPlaybackCallback;
        firstAnim.Play("Beginning", -1, 0);
    }

    /// <summary>
    /// Play the second animation in a chain of animations.
    /// </summary>
    public void PlaySecondAnimation()
    {
        PauseAnim(firstAnim);
        secondAnim.Play("Main");
    }

    /// <summary>
    /// Stop the ending animation (which is the last animation in the chain of
    /// animations).
    /// </summary>
    public void StopEndingAnimation()
    {
        firstAnim.StopPlayback();
        firstAnim.enabled = false;
        if (!hasInvokedEndOfPlayback)
        {
            InvokeEndOfPlaybackCallback();
        }
        else
        {
            DeactivateParentObject();
        }
    }

    public void InvokeEndOfPlaybackCallback()
    {
        hasInvokedEndOfPlayback = true;
        EndOfPlaybackCallback.Invoke();
    }

    /// <summary>
    /// Pause the animator. This can only affect the beginning and ending 
    /// animations. It does not affect the second animation, since that's
    /// played by another animator. This is used to keep the first animation's
    /// frame in place while the second animation plays. Otherwise, if the
    /// animation ends, then the SpriteRenderer is emptied and the sprite
    /// on the screen disappears.
    /// </summary>
    /// <param name="anim">The animator to pause.</param>
    private void PauseAnim(Animator anim)
    {
        prevAnimSpeed = anim.speed;
        anim.speed = 0;
    }

    /// <summary>
    /// Resume the animation after having paused it. This can only affect the 
    /// beginning and ending animations. It does not affect the second 
    /// animation, since that's played by another animator.
    /// </summary>
    /// <param name="anim">The paused animator.</param>
    private void ResumeAnim(Animator anim)
    {
        anim.speed = prevAnimSpeed;
    }

    public void PlaySceneEffect()
    {
        AnimSceneEffect sceneEffect = GetComponent<AnimSceneEffect>();
        if (sceneEffect != null)
        {
            sceneEffect.GetSceneEffect()?.Invoke();
        }
    }

    public void ReverseSceneEffect()
    {
        AnimSceneEffect sceneEffect = GetComponent<AnimSceneEffect>();
        if (sceneEffect != null)
        {
            sceneEffect.GetReverseSceneEffect()?.Invoke();
        }
    }

    private void DeactivateParentObject()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
