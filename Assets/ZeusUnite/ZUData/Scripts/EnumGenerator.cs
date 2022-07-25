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
/// </summary>  **** All Rights Reserved, © Zeus Unite ****

using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;


namespace ZeusUnite
{
    public static partial class EnumGenerator
    {
        const string zeusCreated = "/// </summary> **** Automatic Generated with EnumGenerator, The Software is served as it is.\n" +
            "/// The Enum Generator is © Copyright by Zeus Unite ****\n\n";

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