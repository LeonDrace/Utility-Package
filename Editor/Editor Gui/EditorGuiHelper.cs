using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LeonDrace.Utility.Editor
{
    public static class EditorGuiHelper
    {
        public static string AddWhiteSpaces(string text)
        {
            return string.Concat(text
                .Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
        }

        public static string GetPropertyManagedTypeAsLabelName(SerializedProperty property)
        {
            return AddWhiteSpaces(property.managedReferenceValue.GetType().Name);
        }
        
        public static string GetPropertyTypeAsLabelName(SerializedProperty property)
        {
            return AddWhiteSpaces(property.objectReferenceValue.GetType().Name);
        }

        public static Rect GetFieldRect(Rect position)
        {
            position.width -= EditorGUIUtility.labelWidth;
            position.x += EditorGUIUtility.labelWidth;
            return position;
        }

        public static Rect GetLabelRect(Rect position)
        {
            position.width = EditorGUIUtility.labelWidth - 2f;
            return position;
        }

        public static Rect GetFullRect(Rect position)
        {
            var valueRect = GetFieldRect(position);
            var labelRect = GetLabelRect(position);
            var fullWidthRect = new Rect(labelRect);
            fullWidthRect.width += valueRect.width;
            return fullWidthRect;
        }
    }
}