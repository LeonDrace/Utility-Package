using System;
using LeonDrace.Utility;
using LeonDrace.Utility.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.Serializer
{
    [CustomPropertyDrawer(typeof(SerializableGuidArrayAttribute))]
    public class SerializableGuidArrayDrawer : PropertyDrawer
    {
        SerializableGuidArrayAttribute SerializableGuidArrayAttribute => (SerializableGuidArrayAttribute)attribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
           
            EditorGUI.BeginChangeCheck();
            CompareLastTwo(property);
            EditorGUI.PropertyField(position, property, label,true);
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.isExpanded ? 
                EditorGUI.GetPropertyHeight(property, label, property.isExpanded) : EditorGUIUtility.singleLineHeight;
        }

        private void CompareLastTwo(SerializedProperty property)
        {
            var parentProperty = EditorGuiHelper.FindParentProperty(property);

            if (parentProperty is not { isArray: true, arraySize: > 1 }) return;
            
            var count = parentProperty.arraySize;
            
            var secondLastElement = parentProperty.GetArrayElementAtIndex(count - 2)
                .FindPropertyRelative(SerializableGuidArrayAttribute.GuidPath).FindPropertyRelative("m_Id");
                
            var lastElement = parentProperty.GetArrayElementAtIndex(count - 1)
                .FindPropertyRelative(SerializableGuidArrayAttribute.GuidPath).FindPropertyRelative("m_Id");

            if (secondLastElement.stringValue == lastElement.stringValue)
                lastElement.stringValue = SerializableGuid.NewGuidAsString();
        }
    }
}