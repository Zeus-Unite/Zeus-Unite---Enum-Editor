#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ZeusUnite
{
    public partial class EnumEditorWindow : EditorWindow
    {
        string enumName = "";
        string enumEditName = "";
        bool byObject = true;
        bool editEnum = false;

        bool zeus = true;
        bool tutorial = true;

        public List<Object> objectList = new List<Object>();
        public List<string> stringList = new List<string>();

        public List<EditEnumObject> editEnumList = new List<EditEnumObject>();
        public List<EditEnumObject> editEnumListOriginal = new List<EditEnumObject>();

        Vector2 editorScrollPosition;
        Vector2 editorExtensionScrollPosition;
        int editSelectionIndex = -1;
        int editSelectionIndexLast = -1;

        string filePath = "/ZeusUnite/ZUData/Scripts/Enums/";
        string enumPath = "Assets/ZeusUnite/ZUData/Scripts/Enums/";
        string enumScriptType = ".cs";

        string path => enumPath + enumName + enumScriptType;
        string pathEdit => enumPath + enumEditName + enumScriptType;


        const string Header =
        #region Header
    "/// Check https://zeusunite.stussegames.com for more Information\n"
    + "namespace ZeusUnite\n"
    + "{\n"
    + "\t/// <summary>\n"
    + "\t/// </summary>\n"
    + "\tpublic enum ";

        string GetFullHeader => Header + enumName + "\n\t{";
        string GetFullEditHeader => Header + enumEditName + "\n\t{";
        #endregion



        [MenuItem("Tools/Zeus Unite/Enum Editor", priority = 52)]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow(typeof(EnumEditorWindow), false, "Zeus Unite - Enum Editor");
        }

        void OnGUI()
        {
            ZUDesign.SmallZeusHeader("Enum Generator", "v. 1.0.0", "https://assetstore.unity.com/packages/slug/228135#reviews");

            if (Application.isPlaying)
                return;
            GUILayout.Label("Enum Creator", ZUDesign.GetTitleStyle());
            GUILayout.Space(5);

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();

            if (!editEnum)
            {
                if (byObject && GUILayout.Button("Change to Create by String List", ZUDesign.GetButtonStyle(10)))
                {
                    byObject = false;
                }
                else if (!byObject && GUILayout.Button("Change to Create by Object List", ZUDesign.GetButtonStyle(10)))
                {
                    byObject = true;
                }
            }

            if (editEnum && GUILayout.Button("Add Enum List", ZUDesign.GetButtonStyle(10)))
            {
                editEnum = !editEnum;
            }
            else if (!editEnum && GUILayout.Button("Edit Enum List", ZUDesign.GetButtonStyle(10)))
            {
                editEnum = !editEnum;
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            editorScrollPosition = GUILayout.BeginScrollView(editorScrollPosition, GUILayout.MinHeight(200), GUILayout.MaxHeight(500));

            GUILayout.BeginVertical("HelpBox");
            if (byObject && !editEnum)
            {
                GUILayout.Label("Create Enum By Objects", ZUDesign.GetTitleStyle());
                GUILayout.Space(5);

                GUILayout.BeginHorizontal();
                GUILayout.Label("Enter the Enum Script Name for your Enum List", ZUDesign.GetTextField("ToolTip", 13));
                GUILayout.Space(10);
                enumName = EditorGUILayout.TextField(enumName, ZUDesign.GetTextField("TextField", 12));
                GUILayout.EndHorizontal();
                GUILayout.Space(5);

                if (enumName.Length > 0)
                {
                    GUILayout.Label("Enums are Saved inside ZeusUnite/ZUData/Scripts/Enums\nDrag and Drop Objects from your Project Folder in the List.", ZUDesign.GetHelpBoxStyle());
                    GUILayout.Space(5);

                    ScriptableObject target = this;
                    SerializedObject so = new SerializedObject(target);
                    SerializedProperty stringsProperty = so.FindProperty("objectList");

                    EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
                    so.ApplyModifiedProperties(); // Remember to apply modified properties
                }
            }
            else if (!byObject && !editEnum)
            {
                GUILayout.Label("Create Enum By Strings", ZUDesign.GetTitleStyle());
                GUILayout.Space(5);

                GUILayout.BeginHorizontal();

                GUILayout.Label("Enter the Enum Script Name for your Enum List", ZUDesign.GetTextField("ToolTip", 13));
                GUILayout.Space(10);

                enumName = EditorGUILayout.TextField(enumName, ZUDesign.GetTextField("TextField", 12));
                GUILayout.EndHorizontal();
                GUILayout.Space(5);

                if (enumName.Length > 0)
                {
                    GUILayout.Label("Enums are Saved inside ZeusUnite/ZUData/Scripts/Enums\nEnter the Enums you wish to Create inside the List.", ZUDesign.GetHelpBoxStyle());
                    GUILayout.Space(5);

                    ScriptableObject target = this;
                    SerializedObject so = new SerializedObject(target);
                    SerializedProperty stringsProperty = so.FindProperty("stringList");

                    EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
                    so.ApplyModifiedProperties(); // Remember to apply modified properties
                }
            }
            else if (editEnum)
            {
                if (!Directory.Exists(Application.dataPath + filePath))
                {
                    Debug.LogWarning("No Directory");
                    Directory.CreateDirectory(Application.dataPath + filePath);
                    AssetDatabase.Refresh();
                }

                var enumFilePaths = ZUEditorExtensions.GetFiles(filePath, "*.cs");

                Dictionary<string, string[]> fileAndEnumList = new Dictionary<string, string[]>();

                for (int i = 0; i < enumFilePaths.Length; i++)
                {
                    fileAndEnumList.Add(enumFilePaths[i], EnumGenerator.LoadEnumsFromFile(enumFilePaths[i]));
                }

                string[] selectionList = new string[enumFilePaths.Length + 1];
                selectionList[0] = "Select Enum Script";

                for (int i = 1; i < selectionList.Length; i++)
                {
                    int counter = enumFilePaths[i - 1].LastIndexOf("/");
                    string localChange = enumFilePaths[i - 1].Remove(0, counter + 1);
                    //Debug.Log(localChange);
                    //Debug.Log(enumFilePaths[i - 1]);
                    selectionList[i] = localChange;
                }

                editSelectionIndex = EditorGUILayout.Popup(editSelectionIndex, selectionList, ZUDesign.GetPopupStyle());

                GUILayout.Space(15);

                if (editSelectionIndex != -1 && editSelectionIndex != 0)
                {
                    if (editSelectionIndexLast != editSelectionIndex)
                    {
                        //Refresh
                        if (editEnumList == null)
                        {
                            editEnumList = new List<EditEnumObject>();
                            editEnumListOriginal = new List<EditEnumObject>();
                        }

                        editEnumList.Clear();
                        editEnumListOriginal.Clear();
                        objectList.Clear();

                        var workingString = fileAndEnumList[enumFilePaths[editSelectionIndex - 1]];

                        for (int i = 0; i < workingString.Length; i++)
                        {
                            EditEnumObject EEO = new EditEnumObject();

                            int counter = workingString[i].LastIndexOf("=");
                            //int endindex = workingString[i].LastIndexOf("=");
                            EEO.enumString = workingString[i].Remove(counter);
                            EEO.enumIndex = workingString[i].Remove(0, counter + 1);

                            var removedWhiteSpace = new string(EEO.enumString.ToCharArray()
        .Where(c => !Char.IsWhiteSpace(c))
        .ToArray());
                            var removedWhiteSpaceIndex = new string(EEO.enumIndex.ToCharArray()
        .Where(c => !Char.IsWhiteSpace(c))
        .ToArray());
                            EEO.enumString = removedWhiteSpace;
                            EEO.enumIndex = removedWhiteSpaceIndex;
                            Debug.Log(workingString[i]);
                            Debug.Log(EEO.enumString);
                            Debug.Log(EEO.enumIndex);
                            editEnumList.Add(EEO);
                            editEnumListOriginal.Add(EEO);
                        }
                        editSelectionIndexLast = editSelectionIndex;
                    }

                    //GUILayout.Label("Enums are Saved inside ZeusUnite/ZUData/Scripts/Enums\nDrag and Drop Objects from your Project Folder in the List.", ZUDesign.GetHelpBoxStyle());
                    GUILayout.Space(5);

                    ScriptableObject target = this;
                    SerializedObject so = new SerializedObject(target);
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    GUILayout.Label("Add Additional Objects from Project Folder", ZUDesign.GetTitleStyle(12));
                    GUILayout.Space(5);

                    SerializedObject sool = new SerializedObject(target);
                    SerializedProperty objectListProperty = sool.FindProperty("objectList");
                    EditorGUILayout.PropertyField(objectListProperty, true); // True means show children
                    sool.ApplyModifiedProperties(); // Remember to apply modified properties
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical();
                    GUILayout.Label("Add String Enums", ZUDesign.GetTitleStyle(12));
                    GUILayout.Label("Leave Value Field Empty for automatic Value assignment\nTip: Hold the \"Alt Key\" while open/close the Array to open/close all Elements", ZUDesign.GetHelpBoxStyle(10));
                    GUILayout.Space(5);
                    SerializedProperty editEnumProperty = so.FindProperty("editEnumList");
                    EditorGUILayout.PropertyField(editEnumProperty, true); // True means show children
                    so.ApplyModifiedProperties(); // Remember to apply modified properties
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }


            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();

            if (!editEnum && GUILayout.Button("Add New Enum List", ZUDesign.GetButtonStyle(14)))
            {
                if (byObject)
                {
                    if (!Regex.IsMatch(enumName, @"^[a-zA-Z]+$"))
                    {
                        Debug.LogWarning("Illegal Characters");

                    }
                    else if (System.IO.File.Exists(path))
                    {
                        Debug.LogWarning("File Already Exists");
                    }
                    else if (objectList == null)
                    {
                        Debug.LogWarning("List is Null");
                    }
                    else if (objectList.Count == 0)
                    {
                        Debug.LogWarning("List is Empty");
                    }
                    else
                    {
                        bool create = true;

                        for (int i = 0; i < objectList.Count; i++)
                        {
                            for (int y = 0; y < objectList.Count; y++)
                            {
                                if (i != y && objectList[i].name == objectList[y].name)
                                {
                                    //Duplicate Detected
                                    Debug.LogWarning("Duplicate in Row: " + i + " - " + y);
                                    create = false;
                                }
                            }


                            if (!Regex.IsMatch(objectList[i].name, @"^[a-zA-Z0-9_]+$"))
                            {
                                Debug.LogWarning("Illegal Characters in Row: " + i);
                                create = false;
                            }
                        }


                        if (create)
                        {
                            if (!Directory.Exists(Application.dataPath + filePath))
                            {
                                Debug.LogWarning("No Directory");
                                Directory.CreateDirectory(Application.dataPath + filePath);
                                AssetDatabase.Refresh();
                            }

                            System.IO.File.WriteAllText(path, "");

                            for (int i = 0; i < objectList.Count; i++)
                            {
                                EnumGenerator.AddEnum(path, GetFullHeader, objectList[i].name, objectList[i].GetHashCode());
                            }
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                        }
                        else
                        {
                            Debug.LogWarning("Duplicates in the List");
                        }
                    }
                }
                else if (!byObject)
                {
                    if (!Regex.IsMatch(enumName, @"^[a-zA-Z]+$"))
                    {
                        Debug.LogWarning("Illegal Characters");

                    }
                    else if (System.IO.File.Exists(path))
                    {
                        Debug.LogWarning("File Already Exists");
                    }
                    else if (stringList == null)
                    {
                        Debug.LogWarning("List is Null");
                    }
                    else if (stringList.Count == 0)
                    {
                        Debug.LogWarning("List is Empty");
                    }
                    else
                    {
                        bool create = true;

                        for (int i = 0; i < stringList.Count; i++)
                        {
                            for (int y = 0; y < stringList.Count; y++)
                            {
                                if (i != y && stringList[i] == stringList[y])
                                {
                                    //Duplicate Detected
                                    Debug.LogWarning("Duplicate in Row: " + i + " - " + y);
                                    create = false;
                                }
                            }

                            if (!Regex.IsMatch(stringList[i], @"^[a-zA-Z0-9_]+$"))
                            {
                                Debug.LogWarning("Illegal Characters in Row: " + i);
                                create = false;
                            }
                        }


                        if (create)
                        {
                            if (!Directory.Exists(Application.dataPath + filePath))
                            {
                                Debug.LogWarning("No Directory");
                                Directory.CreateDirectory(Application.dataPath + filePath);
                                AssetDatabase.Refresh();
                            }

                            System.IO.File.WriteAllText(path, "");

                            for (int i = 0; i < stringList.Count; i++)
                            {
                                EnumGenerator.AddEnum(path, GetFullHeader, stringList[i], stringList[i].GetHashCode());
                            }
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                        }
                        else
                        {
                            Debug.LogWarning("Duplicates in the List");
                        }
                    }
                }
            }
            else if (editEnum && GUILayout.Button("Edit Existing Enum List", ZUDesign.GetButtonStyle(14)))
            {

                //New Objects in the List
                //Check for Duplicates  to the Existing List

                bool duplicate = false;
                bool error = false;

                if (objectList != null && objectList.Count > 0)
                {
                    for (int i = 0; i < objectList.Count; i++)
                    {
                        for (int y = 0; y < editEnumList.Count; y++)
                        {
                            if (i == y)
                                continue;

                            if (editEnumList[y].enumString == objectList[i].name)
                            {
                                duplicate = true;
                                Debug.LogWarning("Duplicate in Row: " + i + " : " + y);
                            }
                        }
                    }

                    for (int y = 0; y < editEnumList.Count; y++)
                    {
                        for (int i = 0; i < editEnumList.Count; i++)
                        {
                            if (i == y)
                                continue;

                            if (editEnumList[y].enumString == editEnumList[i].enumString)
                            {
                                duplicate = true;
                                Debug.LogWarning("Duplicate in Row: " + i + " : " + y);
                            }
                        }
                    }

                    for (int i = 0; i < objectList.Count; i++)
                    {
                        if (!Regex.IsMatch(objectList[i].name, @"^[a-zA-Z0-9_]+$"))
                        {
                            Debug.LogWarning("Illegal Characters in Row: " + i);
                            error = true;
                        }
                    }

                    for (int i = 0; i < editEnumList.Count; i++)
                    {
                        if (!Regex.IsMatch(editEnumList[i].enumString, @"^[a-zA-Z0-9_]+$"))
                        {
                            Debug.LogWarning("Illegal Characters in Row: " + i);
                            error = true;
                        }
                    }
                }

                if (!duplicate && !error)
                {
                    var enumFilePaths = ZUEditorExtensions.GetFiles(filePath, "*.cs");
                    for (int i = 0; i < editEnumListOriginal.Count; i++)
                    {
                        EnumGenerator.EditEnum(pathEdit, GetFullEditHeader, editEnumListOriginal[i].enumString, editEnumList[i].enumString, Int32.Parse(editEnumList[i].enumIndex));
                    }

                    for (int i = 0; i < editEnumList.Count; i++)
                    {
                        if (editEnumList[i] == editEnumListOriginal[i])
                            continue;

                        EnumGenerator.AddEnum(pathEdit, GetFullEditHeader, editEnumList[i].enumString, editEnumList[i].enumIndex.Length == 0 ? editEnumList[i].GetHashCode() : Int32.Parse(editEnumList[i].enumIndex));
                    }

                    if (objectList != null && objectList.Count > 0)
                    {
                        for (int i = 0; i < objectList.Count; i++)
                        {
                            EnumGenerator.AddEnum(pathEdit, GetFullEditHeader, objectList[i].name, objectList[i].GetHashCode());
                        }
                    }


                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                else
                {
                    if (error)
                    {
                        Debug.LogWarning("Illegal Characters in the List");
                    }
                    else
                    {
                        Debug.LogWarning("Duplicates in the Lists");
                    }
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.Space(5);
            GUILayout.FlexibleSpace();
            if (tutorial)
            {
                GUILayout.BeginVertical("HelpBox");
                editorExtensionScrollPosition = GUILayout.BeginScrollView(editorExtensionScrollPosition, GUILayout.MinHeight(100), GUILayout.MaxHeight(200));
                GUILayout.Label("Zeus Unite - How To Use", ZUDesign.GetTitleStyle());
                ZUDesign.InfoBox("Add Enums by Objects", "Choose the Create by Enum Object Option\n" +
"Enter Enum Type Name.\n" +
"The Name will be converted to a File and Enum Type, it shouldn't contain WhiteSpace Characters!\n" +

    "You can Drag any Object from the Project  Folder inside the Array.\n" +
    "The Object Name will be converted to an Enum, it should'nt contain WhiteSpace!\n");
                GUILayout.Space(5);

                ZUDesign.InfoBox("Add Enums by Strings", "Choose the Create by Enum Strings Option\n" +
"Enter Enum Type Name.\n" +
"The Name will be converted to a File and Enum Type, it shouldn't contain WhiteSpace Characters!\n" +
"Enter the prefered Enum Names to the Array.\n" +
"The String will be converted to an Enum, it should'nt contain WhiteSpace Characters!\n");
                GUILayout.Space(5);

                ZUDesign.InfoBox("Edit Enums", "Choose the Option Edit Enum List\n" +
"Select an Enum Script in the Popup Window.\n" +
"You can Edit existing Enums, Change their Type and Value\n" +
"Leave Value Empty to automatic assign a Random Value\n" +
"You can also add additional Objects from your Project Folder\n" +
"Objects and Strings should'nt contain Whitespace or Special Characters.");
                GUILayout.Space(5);

                GUILayout.Label("Zeus Unite - Extensions", ZUDesign.GetTitleStyle());

                GUILayout.BeginVertical("ToolTip");
                GUILayout.Label("Zeus Unite - Extensions are available in the Unity Asset Store.");
                GUILayout.Label("Once the Extensions are Registered the additional Features are available");
                GUILayout.EndVertical();

#if ZU_IDM
            ShowItemGenerator();
#else
                ZUDesign.InfoBox("Item Enums", "Generate a List of all Existing Items within the Item Databases.\n" +
                    "You need the Zeus Unite - Item Database Asset.");
#endif
#if ZU_IDM
            ShowBlueprintGenerator();
#else
                ZUDesign.InfoBox("Blueprint Enums", "Generate a List of all Existing Blueprint within the Blueprint Databases.\n" +
                    "You need the Zeus Unite - Item Database Asset.");
#endif
#if ZU_Monsters
                ShowMonsterGenerator();
#else
            ZUDesign.InfoBox("Monster Enums", "Generate a List of all Existing Monster/Units/NPC's within the Monster Databases.\n" +
                "You need the Zeus Unite - Monsters Asset.");
#endif
                GUILayout.EndVertical();
                GUILayout.EndScrollView();
            }
            ZUDesign.Footer(ref zeus, ref tutorial);

        }
    }

    [Serializable]
    public class EditEnumObject
    {
        public string enumString;
        public string enumIndex;
    }

}
#endif