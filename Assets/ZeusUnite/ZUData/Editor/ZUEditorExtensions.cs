#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;


namespace ZeusUnite
{
    public static partial class ZUEditorExtensions
    {
        public static string[] PathOfDatabases(string path) => GetFiles(path);

        public static string[] GetFiles(string path, string sufix = "*.asset")
        {
            return Directory.GetFiles(Application.dataPath + "/" + path, sufix, SearchOption.AllDirectories);
        }       
    }
}
#endif