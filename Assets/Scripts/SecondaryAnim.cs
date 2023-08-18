using UnityEngine;

/// <summary>
/// This is used to make a second animation happen in a chain of animations, 
/// such as that of the summons. Like the chariots that charge after Ishtar 
/// appears in the scene.
/// </summary>
public class SecondaryAnim : MonoBehaviour
{
    /// <summary>
    /// This is the same animator that plays the beginning animation. It's 
    /// set externally via inspector in the Unity editor.
    /// </summary>
    public Animator endingAnim;

    /// <summary>
    /// This is for triggering the ending animation after the second animation 
    /// ends. It's used by an animation event in the animator (see the 
    /// Animation pane in the Unity editor to find it).
    /// </summary>
    public void PlayEndingAnim()
    {
        StopAndDisableThisAnim();
        StartEndingAnim();
    }

    /// <summary>
    /// Stop and disable the animation so that it's not using up resources.
    /// </summary>
    private void StopAndDisableThisAnim()
    {
        Animator anim = GetComponent<Animator>();
        anim.StopPlayback();
        anim.enabled = false;
    }

    /// <summary>
    /// The ending animation is played by the primary animator, which also 
    /// plays the beginning animation. But that animator is paused right before
    /// playing the secondary animation, so it must be resumed by setting its
    /// speed back to normal. And then the ending animation can be played.
    /// </summary>
    private void StartEndingAnim()
    {
        endingAnim.speed = 1.0f;
        endingAnim.Play("Ending", -1, 0);
    }
}
