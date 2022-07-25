#if UNITY_EDITOR
using UnityEngine;

namespace ZeusUnite
{
    [CreateAssetMenu(fileName = "ZUDesignStyle", menuName = "Zeus-Unite/ZUDesignStyle", order = 1)]
    public class ZUDesignStyle : ScriptableObject
    {
        public Color boxBackgroundColor;
        public Color fontNormalColor;
        public float fontMultiplier;
        public Color titleColor;
        public Color titleFontColor;
        public Color buttonColor;
        public Color buttonHoverColor;
        public Color buttonFontColor;
        public Color buttonFontHoverColor;
        public Color infoBoxBackgroundColor;
        public Color infoBoxFontColor;
        public Color acceptButtonColor;
        public Color acceptButtonFontColor;
        public Color declineButtonColor;
        public Color declineButtonFontColor;

        public bool IsActive;
    }
}
#endif