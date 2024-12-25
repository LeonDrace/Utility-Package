using LeonDrace.Utility.Editor;
using UnityEditor;
using UnityEngine;

namespace LeonDrace.Utility
{
    [CustomPropertyDrawer(typeof(SerializableGuid))]
    public class SerializableGuidDrawer : PropertyDrawer
    {
        private const float m_LabelWidthPercentage = 0.8f;
        private const float m_CopyWidthPercentage = 0.1f;
        private const float m_NewGuidWidthPercentage = 0.1f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            
            var idProperty = property.FindPropertyRelative("m_Id");
            
            if (string.IsNullOrEmpty(idProperty.stringValue))
            {
                idProperty.stringValue = SerializableGuid.NewGuidAsString();
            }
            
            DrawGuid(position,idProperty);
            DrawCopyToClipboardButton(position, idProperty);
            DrawNewGuidButton(position, idProperty);
            
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.EndProperty();
        }

        private static void DrawNewGuidButton(Rect position, SerializedProperty idProperty)
        {
            var newGuidPosition = position;
            newGuidPosition.x += position.width * m_LabelWidthPercentage + position.width * m_CopyWidthPercentage;
            newGuidPosition.width = position.width * m_NewGuidWidthPercentage;
            if(GUI.Button(newGuidPosition, GeneralEditorHelper.GetInternalIcon("Loading")))
            {
                if (GeneralEditorHelper.DisplayDialog("Create New Guid", "Are you sure you want to create a new Guid?",
                        "Yes", "No"))
                    idProperty.stringValue = SerializableGuid.NewGuidAsString();
            }
        }

        private static void DrawCopyToClipboardButton(Rect position, SerializedProperty idProperty)
        {
            var copyPosition = position;
            copyPosition.x += position.width * m_LabelWidthPercentage;
            copyPosition.width = position.width * m_CopyWidthPercentage;
            if(GUI.Button(copyPosition, GeneralEditorHelper.GetInternalIcon("Clipboard")))
            {
                GUIUtility.systemCopyBuffer = idProperty.stringValue;
            }
        }

        private static void DrawGuid(Rect position, SerializedProperty idProperty)
        {
            var labelPosition = position;
            labelPosition.width *= m_LabelWidthPercentage;
            var defaultColor = GUI.color;
            GUI.contentColor = Color.gray;
            EditorGUI.LabelField(labelPosition, idProperty.stringValue, EditorStyles.textField);
            GUI.contentColor = defaultColor;
        }
    }
}


