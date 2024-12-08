using System;
using LeonDrace.Utility.Helpers;
using UnityEditor;
using UnityEngine;

namespace LeonDrace.Utility.Editor
{
    /// <summary>
    /// Draw type options in a dropdown and add them to the serialize reference.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TypePickerDrawer<T>
    {
        private readonly Type _searchType = typeof(T);
        
        private string[] _options;
        private Type[] _optionTypes;
        private int _selectedOption;

        public TypePickerDrawer()
        {
            GetOptions();
        }
        
        /// <summary>
        /// Draw type options in a dropdown and add them to the serialize reference.
        /// Draws the instance once a type was added.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!DrawPicker(position, property))
            {
                DrawInstance(position, property, label);
            }
        }

        private bool DrawPicker(Rect position, SerializedProperty property)
        {
            if (property.managedReferenceValue != null) return false;
            
            _selectedOption = EditorGUI.Popup(EditorGuiHelper.GetFieldRect(position), _selectedOption, _options);
            if (GUI.Button(EditorGuiHelper.GetLabelRect(position), "Add Action"))
            {
                CreateInstance(property, _optionTypes[_selectedOption]);
            }
            return true;
        }

        private static void DrawInstance(Rect position, SerializedProperty property, GUIContent label)
        {
            label.text = EditorGuiHelper.GetPropertyManagedTypeAsLabelName(property);
            EditorGUI.BeginProperty(position, label, property);
            position.width -= 20;
            EditorGUI.PropertyField(position, property, label, true);
            ClearInstanceButton(position, property);
            EditorGUI.EndProperty();
                
            property.serializedObject.ApplyModifiedProperties();
        }
        
        private static void ClearInstanceButton(Rect position, SerializedProperty property)
        {
            var fullRect = EditorGuiHelper.GetFullRect(position);
            position.x = fullRect.x + fullRect.width;
            position.width = 20;
            if (GUI.Button(position,"X", EditorStyles.miniButtonRight))
            {
                property.managedReferenceValue = null;
            }
        }

        private static void CreateInstance(SerializedProperty property, System.Type type)
        {
            property.managedReferenceValue = Activator.CreateInstance(type);
        }

        private void GetOptions()
        {
            if(_options != null) return;

            var foundTypes = AssemblyHelper.GetTypes(_searchType);

            _options = new string[foundTypes.Count];
            _optionTypes = new Type[foundTypes.Count];
            
            int counter = 0;
            foreach (var foundType in foundTypes)
            {
                _options[counter] = EditorGuiHelper.AddWhiteSpaces(foundType.Name);
                _optionTypes[counter] = foundType;
                counter++;
            }
        }
    }
}