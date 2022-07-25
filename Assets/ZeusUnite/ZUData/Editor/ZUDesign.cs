#if UNITY_EDITOR
/// <summary> **** Zeus Unite Info ****
///
///	Item.cs
///	https://twitter.com/StusseGames
///	http://zeusunite.idm.stussegames.com
///
/// If you want more Infos about Editor Scripting:
/// Check https://zeusunite.stussegames.com Out
/// We provide alot of Useful Information about Game Design and Unity
/// </summary>  **** All Rights Reserved ® Zeus Unite ****

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if ZU_Monsters
using ZeusUnite.Monsters;
#endif
namespace ZeusUnite
{
    public class ZUDesign
    {
        public static void ZeusUniteExtensions()
        {
#if InventoryManager_Extension
            GUI.color = Color.green;
            GUILayout.Label("Inventory Manager is Present");
            GUI.color = Color.white;            
#else
            GUI.color = Color.red;
            GUILayout.Label("Inventory Manager is not Present");
            GUI.color = Color.white;
#endif
        }

        static ZUDesignStyle designStyle;

        public static ZUDesignStyle DesignStyle 
        { 
            get 
            {
                if (designStyle == null)
                    designStyle = GetActiveStyle();

                return designStyle; 
            } 
            set => designStyle = value; 
        }

        public static ZUDesignStyle GetActiveStyle()
        {
            string[] styles = ZUEditorExtensions.PathOfDatabases("ZeusUnite/ZUData/Resources/ZUStyles/");

            ZUDesignStyle[] Styles = GetDesignsAtPath(styles);

            for (int i = 0; i < Styles.Length; i++)
            {
                if (Styles[i].IsActive)
                {
                    return Styles[i];
                }
            }

            return null;

            ZUDesignStyle[] GetDesignsAtPath(string[] paths)
            {
                string[] localPaths = paths;
                ZUDesignStyle[] databaseBlueprints = new ZUDesignStyle[paths.Length];

                for (int i = 0; i < localPaths.Length; i++)
                {
                    int counter = localPaths[i].LastIndexOf("/");
                    string path = localPaths[i].Remove(0, counter + 1);
                    string endPath = path.Replace(".asset", "");

                    databaseBlueprints[i] = Resources.Load<ZUDesignStyle>("ZUStyles/" + endPath) as ZUDesignStyle;
                }

                return databaseBlueprints;
            }
        }

        public static void SmallZeusHeader(string title, string version, string assetURL = "https://assetstore.unity.com/publishers/68127")
        {
            var zeusImage = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ZeusUnite/ZUData/Images/ZeusUniteBrand.png") as Texture2D;

            if (DesignStyle == null)
                return;

            GUILayout.BeginHorizontal("HelpBox");
            GUIStyle boxstyle = new GUIStyle();
            boxstyle.alignment = TextAnchor.MiddleLeft;
            boxstyle.fixedHeight = 90;
            boxstyle.fixedWidth = 170;
            GUILayout.Label(zeusImage, boxstyle);


            GUILayout.BeginVertical();
            GUILayout.Label("Zeus Unite - " + title, GetTitleStyle(16));
            if (GUILayout.Button("Please Rate This Asset", ZUDesign.GetButtonStyle(12)))
            {
                Application.OpenURL(assetURL);
            }
            GUILayout.Label("This Version: " + version, ZUDesign.GetTitleStyle(10));
            if (GUILayout.Button("Check Out Zeus Unite, in Asset Store", ZUDesign.GetButtonStyle(10)))
            {
                Application.OpenURL("https://assetstore.unity.com/publishers/68127");
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        public static void ZeusHeader(string title, string version, string assetURL = "https://assetstore.unity.com/publishers/68127")
        {
            var zeusImage = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ZeusUnite/ZUData/Images/ZeusUniteBrand.png") as Texture2D;

            if (DesignStyle == null)
                return;

            GUILayout.BeginHorizontal("HelpBox");
            GUIStyle boxstyle = new GUIStyle();
            boxstyle.alignment = TextAnchor.MiddleLeft;
            boxstyle.fixedHeight = 90;
            boxstyle.fixedWidth = 170;
            GUILayout.Label(zeusImage, boxstyle);


            GUILayout.BeginVertical();
            GUIStyle style = new GUIStyle("Button");
            style.alignment = TextAnchor.LowerCenter;
            GUILayout.Label("Zeus Unite - " + title, GetTitleStyle(16));
            if (GUILayout.Button("Please Rate This Asset", ZUDesign.GetButtonStyle(12)))
            {
                Application.OpenURL(assetURL);
            }
            GUILayout.Label("This Version: " + version, ZUDesign.GetTitleStyle(10));
            if (GUILayout.Button("Check Out Zeus Unite, in Asset Store", ZUDesign.GetButtonStyle(10)))
            {
                Application.OpenURL("https://assetstore.unity.com/publishers/68127");
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Support: We Prefer to provide you Support via our Discord");

            style.alignment = TextAnchor.UpperRight;
            if (GUILayout.Button("support@zeusunite.stussegames.com", style))
            {
                Application.OpenURL("mailto:support@zeusunite.stussegames.com");
            }
            if (GUILayout.Button("Join Our Discord Server", style))
            {
                Application.OpenURL("https://discord.gg/t9peX8dCDa");

            }
            if (GUILayout.Button("Visit Zeus Unite a Brand of StusseGames.com", style))
            {
                Application.OpenURL("https://zeusunite.stussegames.com");

            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        public static void Footer(ref bool zeus, ref bool tutorial)
        {
            if (DesignStyle == null)
                return;

            GUIStyle showZeusUnit = new GUIStyle("Toggle");
            showZeusUnit.fixedWidth = 400;
            GUIStyle foot = new GUIStyle("ToolTip");
            foot.margin = new RectOffset(25, 25, 0, 0);
            //showZeusUnit.alignment = TextAnchor.LowerRight;
            GUILayout.BeginHorizontal(foot);
            zeus = EditorGUILayout.Toggle("Show Zeus Header", zeus, showZeusUnit);
            GUILayout.FlexibleSpace();
            GUILayout.Box("® All Rights Reserved © Zeus Unite, Suggestions, Feedback and Bugreport on Discord");
            GUILayout.FlexibleSpace();
            tutorial = EditorGUILayout.Toggle("Show How to Use", tutorial, showZeusUnit);
            GUILayout.EndHorizontal();
            ZUDesign.ZeusUniteExtensions();
        }

        public static void InfoBox(string title, string infoText, bool blue = false)
        {
            if (DesignStyle == null)
                return;

            var infoImage = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ZeusUnite/ZUData/Images/HazardIcon.png") as Texture2D;
            GUIStyle imageStyle = new GUIStyle();
            imageStyle.alignment = TextAnchor.MiddleRight;
            imageStyle.margin = new RectOffset(30, 1, 1, 1);
            imageStyle.fixedHeight = 52;
            imageStyle.fixedWidth = 52;

            GUIStyle outerBox = new GUIStyle("Box");

            outerBox.normal.background = MakeTex(1, 1, DesignStyle.infoBoxBackgroundColor);

            GUIStyle innerBox = new GUIStyle("Box");
            innerBox.normal.background = MakeTex(1, 1, new Color(0f, 0f, 0f, .6f));
            innerBox.margin = new RectOffset(1, 1, 1, 1);
            innerBox.padding = new RectOffset(8, 8, 8, 8);
            innerBox.fontStyle = FontStyle.Italic;
            innerBox.stretchWidth = true;
            innerBox.normal.textColor = DesignStyle.infoBoxFontColor;

            GUIStyle titleStyle = new GUIStyle();
            titleStyle.fontSize = 18;
            titleStyle.alignment = TextAnchor.LowerCenter;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.normal.textColor = DesignStyle.infoBoxFontColor;

            GUIStyle textStyle = new GUIStyle();
            textStyle.fontSize = 12;
            textStyle.normal.textColor = DesignStyle.infoBoxFontColor;

            GUILayout.BeginVertical(outerBox);
            GUILayout.BeginVertical(innerBox);
            GUILayout.BeginHorizontal();
            GUILayout.Label(infoImage, imageStyle);
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label(title, titleStyle);
            GUILayout.Space(10);
            GUILayout.Label(infoText, textStyle);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndVertical();
        }

        public static GUIStyle GetBoxAcceptOrDecline(bool failed = false, int fontsize = 12, bool centerText = true)
        {
            if (DesignStyle == null)
                return new GUIStyle("Box");

            var style = new GUIStyle("HelpBox");
            style.padding = new RectOffset(fontsize / 2, fontsize / 2, fontsize / 2, fontsize / 2);
            if(centerText)
            style.alignment = TextAnchor.MiddleCenter;
            else
            style.alignment = TextAnchor.MiddleLeft;
            style.fontStyle = FontStyle.Italic;

            style.fontSize = (int)(fontsize * DesignStyle.fontMultiplier);

            if (failed)
            {
                style.normal.background = MakeTex(1, 1, DesignStyle.declineButtonColor);
                style.normal.textColor = DesignStyle.declineButtonFontColor;
            }
            else
            {
                style.normal.background = MakeTex(1, 1, DesignStyle.acceptButtonColor);
                style.normal.textColor = DesignStyle.acceptButtonFontColor;
            }


            return style;
        }


        public static GUIStyle GetTitleStyle(int fontsize = 14)
        {
            if (DesignStyle == null)
                return new GUIStyle("Box");


            var style = new GUIStyle(GUI.skin.button);
            style.padding = new RectOffset(5, 5, 5, 5);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.Bold;
            style.normal.background = MakeTex(600, 1, DesignStyle.titleColor);
            style.normal.textColor = DesignStyle.titleFontColor;
            style.fontSize = (int)(fontsize * DesignStyle.fontMultiplier);

            return style;
        }

        public static GUIStyle GetButtonStyle(int fontSize = 12)
        {
            if (DesignStyle == null)
                return new GUIStyle("Box");

            var style = new GUIStyle(GUI.skin.button);
            style.padding = new RectOffset(5, 5, 5, 5);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.Bold;
            style.fontSize = (int)(fontSize * DesignStyle.fontMultiplier);
            style.stretchWidth = true;

            //Changing Normal Hover and Active Behaviour of the Button
            style.normal.background = MakeTex(600, 1, DesignStyle.buttonColor);
            style.normal.textColor = DesignStyle.buttonFontColor;
            style.hover.background = MakeTex(600, 1, DesignStyle.buttonHoverColor);
            style.hover.textColor = DesignStyle.buttonFontHoverColor;
            style.active.background = MakeTex(600, 1, DesignStyle.buttonColor);
            style.active.textColor = DesignStyle.buttonFontColor;

            return style;
        }

        public static GUIStyle GetTextField(string _style = null,int fontSize = 12)
        {
            if (DesignStyle == null)
                return new GUIStyle("Box");

            GUIStyle style;

            if (_style == null)
                style = new GUIStyle();
            else
                style = new GUIStyle(_style);

            style.alignment = TextAnchor.MiddleLeft;
            style.padding = new RectOffset(5, 5, 5, 5);
            style.fontSize = (int)(fontSize * DesignStyle.fontMultiplier);
            style.stretchWidth = true;
            style.fixedHeight = (fontSize * DesignStyle.fontMultiplier) * 2;

            style.normal.textColor = DesignStyle.fontNormalColor;

            return style;
        }

        public static Texture2D MakeTex(int width, int height, Color col)
        {
            if (DesignStyle == null)
                return null;

            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

        public static GUIStyle GetPopupStyle(int fontsize = 14)
        {
            if (DesignStyle == null)
                return new GUIStyle("Box");

            var style = new GUIStyle("PopUp");
            //style.margin = new RectOffset(0, 0, 5, 0);
            style.fontSize = (int)(fontsize * DesignStyle.fontMultiplier);
            style.normal.textColor = DesignStyle.fontNormalColor;
            style.fixedHeight = fontsize * 2;
            return style;
        }

        public static GUIStyle GetHelpBoxStyle(int fontsize = 12)
        {
            if (DesignStyle == null)
                return new GUIStyle("Box");

            var style = new GUIStyle("HelpBox");
            style.fontSize = (int)(fontsize * DesignStyle.fontMultiplier);
            style.padding = new RectOffset(5, 5, 5, 5);
            style.normal.textColor = DesignStyle.fontNormalColor;
            return style;
        }

        public static GUIStyle GetTextArea(int fontsize = 12)
        {
            if (DesignStyle == null)
                return new GUIStyle("Box");

            GUIStyle textArea = new GUIStyle("TextArea");
            textArea.padding = new RectOffset(15, 15, 15, 15);
            textArea.stretchHeight = true;
            textArea.fontSize = (int)(12 * ZUDesign.DesignStyle.fontMultiplier);

            return textArea;
        }

        public static GUIStyle LayoutBackground()
        {
            if (DesignStyle == null)
                return new GUIStyle("Box");

            var style = new GUIStyle(GUI.skin.box);
            style.padding = new RectOffset(4, 4, 6, 6);
            style.alignment = TextAnchor.UpperCenter;
            style.stretchWidth = true;
            style.stretchHeight = true;
            style.normal.background = MakeTex(600, 1, DesignStyle.boxBackgroundColor);
            return style;
        }
    }
}
#endif