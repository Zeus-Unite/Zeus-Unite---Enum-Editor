#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ZeusUnite
{
    public class ZUStyleWindow : EditorWindow
    {
        [MenuItem("Tools/Zeus Unite/Display Settings", priority = 99)]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow(typeof(ZUStyleWindow), false, "Zeus Unite - Display Settings");
        }

        Vector2 scrollPosition;

        int styleIndex = -1;
        int oldStyleIndex = 0;
        void OnGUI()
        {
            if (Application.isPlaying)
            {
                GUILayout.Label("Application is Playing, Prevent Runtime Overhead\nStop the Editor Playing to resume Editing!", ZUDesign.GetTitleStyle(18));
                return;
            }

            if (ZUDesign.DesignStyle == null)
            {
                GUI.color = Color.red;
                GUILayout.Label("No Active Design, Go To the ZUStyles Resource Folder and Set an Active Design\n" +
                    "If there are no Styles in the Folder, you can Reimport the Asset or Create a new Scriptable Object of ZUDesignStyle\n\n" +
                    "Right Click the ZUStyles Folder in ZeusUnite/ZUData/Resources/ZUStyles\n\n" +
                    "Navigate to Create -> Zeus Unite -> ZUDesignStyle\n\n" +
                    "Edit the Style inside our Display Settings Editor\n\n" +
                    "The System automatic detects the Designs inside the Folder\n" +
                    "Enjoy");
                GUI.color = Color.white;

                Debug.LogWarning("No Active Design, Go To the ZUStyles Resource Folder and Set an Active Design");
                return;
            }

            if (GUILayout.Button("Refresh GUI"))
            {
                _ = ZUDesign.DesignStyle;
            }

            var zeusImage = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ZeusUnite/ZUData/Images/ZeusUniteBrand.png") as Texture2D;


            GUILayout.BeginHorizontal("HelpBox");
            GUIStyle boxstyle = new GUIStyle();
            boxstyle.alignment = TextAnchor.MiddleLeft;
            boxstyle.fixedHeight = 90;
            boxstyle.fixedWidth = 170;
            GUILayout.Label(zeusImage, boxstyle);


            GUILayout.BeginVertical();
            GUIStyle style = new GUIStyle("Button");
            style.alignment = TextAnchor.LowerCenter;
            GUILayout.Label("Zeus Unite - Display Settings", ZUDesign.GetTitleStyle(16));
            GUILayout.Label("1.0");
            GUILayout.Label("");
            if (GUILayout.Button("Check Out Zeus Unite, in Asset Store", style))
            {
                Application.OpenURL("https://assetstore.unity.com/publishers/68127");
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUIStyle centerText = new GUIStyle();
            centerText.alignment = TextAnchor.UpperCenter;
            centerText.normal.textColor = ZUDesign.DesignStyle.fontNormalColor;
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            GUILayout.BeginVertical(ZUDesign.LayoutBackground());
            GUILayout.Label("Box Base Style Settings", ZUDesign.LayoutBackground(), GUILayout.MaxHeight(20));
            ZUDesign.DesignStyle.boxBackgroundColor = EditorGUILayout.ColorField("Box Background Color: ", ZUDesign.DesignStyle.boxBackgroundColor);
            ZUDesign.DesignStyle.fontNormalColor = EditorGUILayout.ColorField("Font Normal Color: ", ZUDesign.DesignStyle.fontNormalColor);
            ZUDesign.DesignStyle.fontMultiplier = EditorGUILayout.Slider("Font Size Multiplier: ", ZUDesign.DesignStyle.fontMultiplier, 1, 3);
            GUILayout.Space(10);
            GUILayout.Label("Title Box Style", centerText);
            ZUDesign.DesignStyle.titleColor = EditorGUILayout.ColorField("Title Box Color: ", ZUDesign.DesignStyle.titleColor);
            ZUDesign.DesignStyle.titleFontColor = EditorGUILayout.ColorField("Title Font Color: ", ZUDesign.DesignStyle.titleFontColor);
            GUILayout.Label("Title Box Style", ZUDesign.GetTitleStyle(12));
            GUILayout.Space(10);
            GUILayout.Label("Button Box Style", centerText);
            ZUDesign.DesignStyle.buttonColor = EditorGUILayout.ColorField("Button Color: ", ZUDesign.DesignStyle.buttonColor);
            ZUDesign.DesignStyle.buttonFontColor = EditorGUILayout.ColorField("Button Font Color: ", ZUDesign.DesignStyle.buttonFontColor);
            ZUDesign.DesignStyle.buttonHoverColor = EditorGUILayout.ColorField("Button Hover Color: ", ZUDesign.DesignStyle.buttonHoverColor);
            ZUDesign.DesignStyle.buttonFontHoverColor = EditorGUILayout.ColorField("Button Font Hover Color: ", ZUDesign.DesignStyle.buttonFontHoverColor);
            GUILayout.Label("Button Box Style", ZUDesign.GetButtonStyle(12));
            GUILayout.Space(10);
            GUILayout.Label("Info Box Style", centerText);
            ZUDesign.DesignStyle.infoBoxBackgroundColor = EditorGUILayout.ColorField("Info Box Color: ", ZUDesign.DesignStyle.infoBoxBackgroundColor);
            ZUDesign.DesignStyle.infoBoxFontColor = EditorGUILayout.ColorField("Info Box Font Color: ", ZUDesign.DesignStyle.infoBoxFontColor);
            ZUDesign.InfoBox("This is an Info Box", "Change the Color of the Info Box");
            GUILayout.Space(10);
            GUILayout.Label("Confirm Buttons", centerText);
            ZUDesign.DesignStyle.acceptButtonColor = EditorGUILayout.ColorField("Accept Button Color: ", ZUDesign.DesignStyle.acceptButtonColor);
            ZUDesign.DesignStyle.acceptButtonFontColor = EditorGUILayout.ColorField("Accept Button Font Color: ", ZUDesign.DesignStyle.acceptButtonFontColor);
            ZUDesign.DesignStyle.declineButtonColor = EditorGUILayout.ColorField("Decline Button Color: ", ZUDesign.DesignStyle.declineButtonColor);
            ZUDesign.DesignStyle.declineButtonFontColor = EditorGUILayout.ColorField("Decline Button Font Color: ", ZUDesign.DesignStyle.declineButtonFontColor);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Accept Button and Success Message Style", ZUDesign.GetBoxAcceptOrDecline());
            GUILayout.Label("Decline Button and Failure Message Style", ZUDesign.GetBoxAcceptOrDecline(true));
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            CheckForDesign();
            GUILayout.Space(10);

            void CheckForDesign()
            {
                string[] styles = ZUEditorExtensions.PathOfDatabases("ZeusUnite/ZUData/Resources/ZUStyles/");

                ZUDesignStyle[] Styles = GetDesignsAtPath(styles);

                if (styleIndex == -1)
                {
                    if (Styles.Length > 1)
                    {
                        for (int i = 0; i < Styles.Length; i++)
                        {
                            if (Styles[i].IsActive)
                            {
                                styleIndex = i + 1;
                                oldStyleIndex = styleIndex;
                                break;
                            }
                        }
                    }
                }

                string[] styleList = StyleList();

                styleIndex = EditorGUILayout.Popup(styleIndex, styleList, ZUDesign.GetPopupStyle());

                if (styleIndex != oldStyleIndex)
                {
                    oldStyleIndex = styleIndex;
                    EditorUtility.SetDirty(ZUDesign.DesignStyle);
                    ZUDesign.DesignStyle.IsActive = false;

                    for (int i = 1; i < styleList.Length; i++)
                    {
                        string item = styleList[i];

                        if (item == styleList[styleIndex])
                        {
                            EditorUtility.SetDirty(Styles[i - 1]);
                            Styles[i - 1].IsActive = true;


                            ZUDesign.DesignStyle = Styles[i - 1];
                        }
                    }
                }


                string[] StyleList()
                {
                    string[] selectList = new string[styles.Length + 1];

                    selectList[0] = "Select Style";

                    for (int i = 0; i < styles.Length; i++)
                    {
                        int counter = styles[i].LastIndexOf("/");
                        string path = styles[i].Remove(0, counter + 1);
                        string endPath = path.Replace(".asset", "");
                        selectList[i + 1] = endPath;
                    }

                    return selectList;
                }

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
        }
    }
}
#endif