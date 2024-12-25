using System;
using System.Linq;
using System.Text.RegularExpressions;
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
        
        public static SerializedProperty FindParentProperty(SerializedProperty serializedProperty)
        {
            var propertyPaths = serializedProperty.propertyPath.Split('.');
            if (propertyPaths.Length <= 1)
            {
                return default;
            }

            var parentSerializedProperty = serializedProperty.serializedObject.FindProperty(propertyPaths.First());
            for (var index = 1; index < propertyPaths.Length - 1; index++)
            {
                if (propertyPaths[index] == "Array")
                {
                    if (index + 1 == propertyPaths.Length - 1)
                    {
                        break;
                    }
                    
                    if (propertyPaths.Length > index + 1 && Regex.IsMatch(propertyPaths[index + 1], "^data\\[\\d+\\]$"))
                    {
                        var match = Regex.Match(propertyPaths[index + 1], "^data\\[(\\d+)\\]$");
                        var arrayIndex = int.Parse(match.Groups[1].Value);
                        parentSerializedProperty = parentSerializedProperty.GetArrayElementAtIndex(arrayIndex);
                        index++;
                    }
                }
                else
                {
                    parentSerializedProperty = parentSerializedProperty.FindPropertyRelative(propertyPaths[index]);
                }
            }

            return parentSerializedProperty;
        }

        public static int GetIndexInParent(SerializedProperty serializedProperty)
        {
            var parent = FindParentProperty(serializedProperty);
            
            if (parent != serializedProperty)
            {
                for (int i = 0; i < parent.arraySize; i++)
                {
                    var prop = parent.GetArrayElementAtIndex(i);
                    if (SerializedProperty.EqualContents(prop, serializedProperty))
                        return i;
                }
            }
            
            return -1;
        }
    }
}