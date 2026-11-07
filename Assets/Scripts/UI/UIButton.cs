using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    enum ButtonStates
    {
        Default,
        Hovered,
        Pressed,
        Disabled
    }

    [SerializeField] UIButtonStateActions stateActions;
    [SerializeField] bool disabled;

    bool grouped = false;
    bool hovered = false;
    bool useAltColor = false;

    ButtonStates lastState = ButtonStates.Default;

    public UnityEvent onClick;

    public Action<UIButton> clicked;

    Image buttonImage;
    RectTransform buttonImageRect;
    RectTransform buttonRect;

    bool usingText;
    CustomText buttonText;
    RectTransform buttonTextRect;
    

    void Awake()
    {
        buttonText = GetComponentInChildren<CustomText>();
        usingText = buttonText != null;
        buttonRect = GetComponent<RectTransform>();
        buttonImage = GetComponentInChildren<Image>();
        buttonImageRect = buttonImage.gameObject.GetComponent<RectTransform>();
        buttonTextRect = buttonText.GetComponent<RectTransform>();
        buttonImage.color = stateActions.baseColor;

        SetEnabled(!disabled);
    }

    public void SetEnabled(bool enabled)
    {
        disabled = !enabled;
        ParseStateAction(disabled ? stateActions.disabledState : stateActions.defaultState, state: disabled ? ButtonStates.Disabled : ButtonStates.Default, instant: true);
    }

    public void UseAltColor(bool use)
    {
        useAltColor = use;
        RefreshState(false);
    }

    public void SetGrouped(bool group)
    {
        grouped = group;
    }

    public bool GetEnabled() { return !disabled; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (disabled) return;

        //Debug.Log("Cursor hovered over the button!");
        ParseStateAction(stateActions.hoveredState, ButtonStates.Hovered);
        hovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (disabled) return;

        //Debug.Log("Cursor left the button.");
        ParseStateAction(stateActions.defaultState, ButtonStates.Default);
        hovered = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (disabled) return;

        //Debug.Log("Button pressed down!");
        ParseStateAction(stateActions.pressedState, ButtonStates.Pressed);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (disabled) return;
        if (!hovered) return;

        //Debug.Log("Button unpressed Up!");
        ParseStateAction(stateActions.hoveredState, ButtonStates.Hovered);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (disabled) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            onClick?.Invoke();
            if (!grouped) ParseStateAction(stateActions.hoveredState, ButtonStates.Hovered);
            clicked?.Invoke(this);
        }
    }

    public void ManualInvoke()
    {
        onClick?.Invoke();
    }

    private void RefreshState(bool instant)
    {
        switch (lastState)
        {
            case ButtonStates.Default:
                ParseStateAction(stateActions.defaultState, ButtonStates.Default, instant);
                break;
            case ButtonStates.Hovered:
                ParseStateAction(stateActions.hoveredState, ButtonStates.Hovered, instant);
                break;
            case ButtonStates.Pressed:
                ParseStateAction(stateActions.hoveredState, ButtonStates.Hovered, instant);
                break;
            case ButtonStates.Disabled:
                ParseStateAction(stateActions.disabledState, ButtonStates.Disabled, instant);
                break;
        }
    }

    private void ParseStateAction(UIButtonStateActions.ButtonState bState, ButtonStates state, bool instant = false)
    {
        KillTweens();

        lastState = state;
        
        if (bState.changeSprite)
            buttonImage.sprite = bState.newSprite;

        if (usingText && bState.useTextOffset && buttonTextRect)
            buttonTextRect.anchoredPosition = new Vector2(buttonTextRect.anchoredPosition.x, bState.yOffset);

        if (bState.changeSpriteColor)
            Play(buttonImage.DOColor(bState.spriteColor, bState.colorChangeTime).SetUpdate(true), instant);

        if (bState.modifySpriteColor)
        {
            Color.RGBToHSV(useAltColor ? stateActions.altColor : stateActions.baseColor, out float H, out float S, out float V);
            H = Mathf.Repeat(H + bState.hue, 1f);
            S = Mathf.Clamp01(S + bState.saturation);
            V = Mathf.Clamp01(V + bState.value);

            Play(buttonImage.DOColor(Color.HSVToRGB(H, S, V), bState.colorModTime).SetUpdate(true), instant);
        }

        if (usingText && bState.changeTextColor)
            Play(buttonText.DOColor(bState.textColor, bState.textColorChangeTime).SetUpdate(true), instant);

        if (bState.changeScale)
            Play(buttonImageRect.DOScale(bState.newScale, bState.scaleChangeTime).SetEase(bState.scaleEase).SetUpdate(true), instant);

        if (bState.playAudio)
        {
            
        }
           
    }

    void KillTweens()
    {
        buttonImage.DOKill();
        buttonImageRect.DOKill();
        if (usingText) buttonText.DOKill();
    }

    void Play(Tween t, bool instant)
    {
        if (instant) t.Complete();
    }

}
