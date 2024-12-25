using System;
using UnityEditor;
using UnityEngine;

namespace LeonDrace.Utility.Editor
{
    public static class GeneralEditorHelper
    {
        public static GUIContent GetInternalIcon(string path)
        {
            return EditorGUIUtility.IconContent(path);
        }

        public static bool DisplayDialog(string title, string message, string okButtonText, string cancelButtonText)
        {
            return EditorUtility.DisplayDialog(title, message, okButtonText, cancelButtonText);
        }
    }
}