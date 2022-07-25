#if UNITY_EDITOR
/// <summary> **** Zeus Unite Info ****
///
///	EnumGenerator.cs
///	https://twitter.com/StusseGames
///	http://zeusunite.idm.stussegames.com
///
///	ENUM Generator, Create Visually Enums inside th Zeus Unite - Enum Editor.
/// Contains Methods required by the Zeus Unite - Item Database Management
/// To Create ItemTypes based on configured Databases.
/// </summary>  **** All Rights Reserved, � Zeus Unite ****

using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;


namespace ZeusUnite
{
    public static partial class EnumGenerator
    {
        const string zeusCreated = "/// </summary> **** Automatic Generated with EnumGenerator, The Software is served as it is.\n" +
            "/// The Enum Generator is � Copyright by Zeus Unite ****\n\n";

#if ZU_IDM
        const string ItemTypeHeader =
        #region ItemTypeHeader
    "/// You can Extend the ItemTypes with additional Types by Creating new Databases in the Editor\n"
    + "/// PLEASE DON'T EDIT THIS FILE THE IDM SYSTEM MIGHT NOT RECOGNIZE THE CHANGES\n"
    + "/// THE ITEMTYPE CS IS REQUIRED TO CREATE IDM DATABASES\n"
    + "/// THIS OBJECT  IS AUTO GENERATED BY ZEUS UNITE - ENUM GENERATOR\n"
    + "///\n"
    + "/// Every Game needs specific Types of Items for Equipment and others.\n"
    + "/// To Keep a nice and Clean Structure the ItemTypes are Identical with the Item Databases\n"
    + "/// So Every Item can be found in their ItemType Database.\n"
    + "///\n"
    + "/// a Inventory System based on the Item Database Management System.\n"
    + "/// Check https://zeusunite.idm.stussegames.com for more Information\n"
    + "/// </summary>  **** All Rights Reserved � Zeus Unite ****\n\n"
    + "namespace ZeusUnite\n"
    + "{\n"
    + "\t/// <summary>\n"
    + "\t/// ItemTypes define the Type of an Item.\n"
    + "\t/// ItemType get used to define Behaviour and Actions of the Item.\n"
    + "\t/// </summary>\n"
    + "\tpublic enum ItemType\n"
    + "\t{";
        #endregion
        const string ItemTagHeader =
        #region ItemTagHeader
    "/// You can Extend the Item Tags with additional Tags in the Item Database Management Editor\n"
    + "/// PLEASE DON'T EDIT THIS FILE THE IDM SYSTEM MIGHT NOT RECOGNIZE THE CHANGES\n"
    + "/// THE ITEMTAG CS IS REQUIRED FOR ITEM TAG INSIDE ITEMS\n"
    + "/// THIS OBJECT  IS AUTO GENERATED BY ZEUS UNITE - ENUM GENERATOR\n"
    + "///\n"
    + "///\n"
    + "/// Check https://zeusunite.idm.stussegames.com for more Information\n"
    + "/// </summary>  **** All Rights Reserved � Zeus Unite ****\n\n"
    + "namespace ZeusUnite\n"
    + "{\n"
    + "\t/// <summary>\n"
    + "\t/// ItemTags define the Tags of an Item.\n"
    + "\t/// ItemTags get used to define additional Behaviour and Actions of Items.\n"
    + "\t/// You can declare Tags like: QuestItem, NotForSale, TwoHanded and whatever you need.\n"
    + "\t/// </summary>\n"
    + "\tpublic enum ItemTag\n"
    + "\t{";
        #endregion
        const string CraftingTypeHeader =
        #region CraftingTypeHeader
    "/// You can Extend the Crafting Types with additional Types in the Blueprint Editor\n"
    + "/// Check https://zeusunite.idm.stussegames.com for more Information\n"
    + "/// </summary>  **** Automatic Generated with EnumGenerator All Rights Reserved � Zeus Unite ****\n\n"
    + "namespace ZeusUnite\n"
    + "{\n"
    + "\t/// <summary>\n"
    + "\t/// CraftingTypes\n"
    + "\t/// CraftingType can be used to add Behaviour to Crafting\n"
    + "\t/// </summary>\n"
    + "\tpublic enum CraftingType\n"
    + "\t{";
        #endregion


    static string itemDatabasePath = "Assets/ZeusUnite/IDM/Scripts/Enums/ItemType.cs";
    static string itemTagPath = "Assets/ZeusUnite/IDM/Scripts/Enums/ItemTag.cs";
    static string craftingTypePath = "Assets/ZeusUnite/IDM/Scripts/Enums/CraftingType.cs";

        #region ItemTag
    public static void AddItemTag(string newEnum, int id)
    {
        AddEnum(itemTagPath, ItemTagHeader,newEnum,id);
    }

    public static void EditItemTag(string enumEdit, string enumNew, int id)
    {
        EditEnum(itemTagPath, ItemTagHeader, enumEdit, enumNew, id);
    }

    public static void RemoveItemTag(string enumRemove)
    {
        RemoveEnum(itemTagPath, ItemTagHeader, enumRemove);
    }
        #endregion

        #region ItemDatabase
    public static void AddItemDatabase(string newEnum, int id)
    {
        AddEnum(itemDatabasePath, ItemTypeHeader, newEnum, id);        
    }

    public static void EditItemDatabase(string enumEdit, string enumNew, int id)
    {
        EditEnum(itemDatabasePath, ItemTypeHeader, enumEdit,enumNew,id);
    }

    public static void RemoveItemDatabase(string enumRemove)
    {
        RemoveEnum(itemDatabasePath, ItemTypeHeader, enumRemove);
    }
        #endregion

        #region CraftingDatabase
    public static void AddCraftingDatabase(string entry, int id)
    {
        AddEnum(craftingTypePath, CraftingTypeHeader, entry, id);
    }
    public static void EditCraftingDatabase(string enumEdit, string enumNew, int id)
    {
        EditEnum(craftingTypePath, CraftingTypeHeader, enumEdit, enumNew, id);
    }
    public static void RemoveCraftingDatabase(string enumRemove)
    {
        RemoveEnum(craftingTypePath, CraftingTypeHeader, enumRemove);
    }
        #endregion
#endif

        public static void AddEnum(string path, string header, string newEnum, int id)
        {
            string[] enumEntries = LoadEnumsFromFile(path);

            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                streamWriter.WriteLine(zeusCreated);
                streamWriter.Write(header);
                if (enumEntries != null)
                {
                    for (int i = 0; i < enumEntries.Length; i++)
                    {
                        streamWriter.Write("\t\t" + enumEntries[i] + ",");
                    }
                }
                streamWriter.WriteLine("\n\t\t" + newEnum + " = " + id + ",");
                streamWriter.WriteLine("\t}");
                streamWriter.WriteLine("}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        public static void EditEnum(string path, string header, string enumEdit, string enumNew, int id)
        {
            string[] enumEntries = LoadEnumsFromFile(path);

            for (int i = 0; i < enumEntries.Length; i++)
            {
                if (enumEntries[i].Contains(enumEdit))
                {
                    string news = "\n\t\t" + enumNew + " = " + id;
                    enumEntries[i] = news;
                    break;
                }
            }

            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                streamWriter.WriteLine(zeusCreated);

                streamWriter.Write(header);

                for (int i = 0; i < enumEntries.Length; i++)
                {
                    streamWriter.Write("\t\t" + enumEntries[i] + ",");
                }
                streamWriter.WriteLine("\n\t}");
                streamWriter.WriteLine("}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        public static void RemoveEnum(string path, string header, string enumRemove)
        {
            string[] enumEntries = LoadEnumsFromFile(path);
            List<string> newEntries = new List<string>();

            foreach (var item in enumEntries)
            {
                if (!item.Contains(enumRemove))
                {
                    newEntries.Add(item);
                }
            }

            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                streamWriter.WriteLine(zeusCreated);

                streamWriter.Write(header);

                for (int i = 0; i < newEntries.Count; i++)
                {
                    streamWriter.Write("\t\t" + newEntries[i] + ",");
                }
                streamWriter.WriteLine("\n\t}");
                streamWriter.WriteLine("}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        public static string[] LoadEnumsFromFile(string path)
        {
            //The path Folder must exist
            string filePathAndName = path;
            //We write now every Enum as a String
            using (StreamReader streamWriter = new StreamReader(filePathAndName))
            {
                var x = streamWriter.ReadToEnd();
                int endIndex = 0;

                //We Find the Opening of the enum First by going through and find the latest { Opening Bracket
                for (int i = 0; i < x.Length; i++)
                {
                    if (x[i] == '{')
                    {
                        endIndex = i + 1;
                    }
                }

                //The Latest { Opening Bracket defines the Start of the Enum List
                var removed = x.Remove(0, endIndex);
                int newEnd = 0;

                //We Scan now for the latest Enum Entry which should have an , Comma as Seperator.
                for (int i = 0; i < removed.Length; i++)
                {
                    if (removed[i] == ',')
                    {
                        newEnd = i;
                    }
                }

                //We Remove now everything past the latest Enum Entry
                if (newEnd != 0)
                {
                    int length = removed.Length - newEnd;
                    var replace = removed.Remove(newEnd, length);
                    var read = replace.Split(',');

                    return read;
                }
                else
                {

                    return null;
                }
            }
        }
    }
}
#endif