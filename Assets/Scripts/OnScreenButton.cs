using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnScreenButton : MonoBehaviour, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler
{
    public Button.ButtonClickedEvent clickEvent;
    public Action action;


    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    /// <summary>
    /// Mouse starts hovering over the button.
    /// </summary>
    private void OnMouseEnter()
    {

    }

    /// <summary>
    /// Mouse releases up from click (if not released from the button bounds, 
    /// then does not apply the click).
    /// </summary>
    private void OnMouseUpAsButton()
    {
        OnClick();
    }

    private void OnClick()
    {
        PlayClickAnimation();
        clickEvent.Invoke();
    }

    private void PlayClickAnimation()
    {
        GetComponent<Animator>().Play("ButtonClick", -1, 0f);
    }
}
