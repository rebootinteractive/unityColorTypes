using System;
using UnityEditor;
using UnityEngine;

namespace ObjectType
{
    [CustomPropertyDrawer(typeof(ObjectTypeEnum))]
    public class ObjectTypeEnumEditor : PropertyDrawer
    {
        // Draw the dropdown
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //If multiple objects are selected and they have different types, just draw the default property field
            if (property.serializedObject.isEditingMultipleObjects)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }
            
            var typeNames = ObjectTypeLibrary.Find().GetObjectTypeNames();
            if (typeNames.Length == 0)
            {
                typeNames = new[] {"No Type"};
            }
            
            EditorGUI.BeginProperty(position, label, property);

            // Find the typeName property
            SerializedProperty typeNameProp = property.FindPropertyRelative("typeName");

            // Get the current type name and find its index in the list
            string currentTypeName = typeNameProp.stringValue;
            int currentIndex = Array.IndexOf(typeNames, currentTypeName);
            if (currentIndex == -1) currentIndex = 0; // Default to first item if not found

            // Draw the dropdown
            int index = EditorGUI.Popup(position, label.text, currentIndex, typeNames);
            if (index != currentIndex)
            {
                typeNameProp.stringValue = typeNames[index];
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }        
            
            EditorGUI.EndProperty();
        }
    }
}