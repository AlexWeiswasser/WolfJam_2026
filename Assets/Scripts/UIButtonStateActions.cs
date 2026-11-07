using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "UIButtonStateActions", menuName = "Scriptable Objects/UIButtonStateActions")]
public class UIButtonStateActions : ScriptableObject
{
    [Header("Colors")]
    public Color baseColor;
    public Color altColor;
    
    [Header("Default State")]
    public ButtonState defaultState;

    [Header("Hovered State")]
    public ButtonState hoveredState;

    [Header("Pressed State")]
    public ButtonState pressedState;

    [Header("Disabled State")]
    public ButtonState disabledState;

    [System.Serializable]
    public class ButtonState
    {
        [Header("Change Sprite")]
        public bool changeSprite;
        public Sprite newSprite;

        [Header("Text Offset")]
        public bool useTextOffset;
        public int yOffset;

        [Header("Change Sprite Color")]
        public bool changeSpriteColor;
        public Color spriteColor = Color.white;
        public float colorChangeTime = 0.2f;

        [Header("Modify Sprite Color")]
        public bool modifySpriteColor;
        public float hue;
        public float saturation;
        public float value;
        public float colorModTime = 0.2f;

        [Header("Change Text Color")]
        public bool changeTextColor;
        public Color textColor = Color.white;
        public float textColorChangeTime = 0.2f;

        [Header("Scale")]
        public bool changeScale;
        public Vector2 newScale = Vector2.one;
        public float scaleChangeTime = 0.1f;
        public Ease scaleEase = Ease.InOutSine;

        [Header("Audio")]
        public bool playAudio;
        public AudioClip audioClip;

    }
}
