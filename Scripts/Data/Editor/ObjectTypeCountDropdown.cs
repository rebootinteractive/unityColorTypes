using System;
using UnityEditor;
using UnityEngine;

namespace ObjectType
{
    // Generic, reusable IMGUI dropdown that binds to an int index of ObjectTypeLibrary.objectTypes
    // Features:
    // - Popup shows type names; value is the selected index
    // - Background tinted with the selected type's color (first color slot)
    // - Previous/Next buttons to navigate types quickly
    public static class ObjectTypeCountDropdown
    {
        // Layout version
        public static int DrawLayout(int currentIndex, string label = null, int colorIndex = 0)
        {
            var library = ObjectTypeLibrary.Find();
            if (library == null || library.objectTypes == null || library.objectTypes.Length == 0)
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.Popup(label ?? "Type", 0, new[] {"No Types"});
                }
                return -1;
            }

            var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
            return Draw(rect, currentIndex, label, colorIndex);
        }

        // Absolute positioning version
        public static int Draw(Rect position, int currentIndex, string label = null, int colorIndex = 0)
        {
            // Reserve space for label if provided
            if (!string.IsNullOrEmpty(label))
            {
                position = EditorGUI.PrefixLabel(position, new GUIContent(label));
            }

            var library = ObjectTypeLibrary.Find();
            if (library == null || library.objectTypes == null || library.objectTypes.Length == 0)
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUI.Popup(position, 0, new[] {"No Types"});
                }
                return -1;
            }

            var names = library.GetObjectTypeNames();
            currentIndex = Mathf.Clamp(currentIndex, 0, names.Length - 1);

            // Layout: [<] [popup] [>]
            const float buttonWidth = 24f;
            Rect leftButtonRect = new Rect(position.x, position.y, buttonWidth, position.height);
            Rect rightButtonRect = new Rect(position.xMax - buttonWidth, position.y, buttonWidth, position.height);
            Rect popupRect = new Rect(leftButtonRect.xMax + 2f, position.y, Mathf.Max(0, rightButtonRect.xMin - leftButtonRect.xMax - 4f), position.height);

            // Determine the selected color (first available color), fallback to a neutral tint
            var selectedType = library.objectTypes[currentIndex];
            Color tint = new Color(0.23f, 0.23f, 0.23f, 0.25f);
            if (selectedType.colors != null && selectedType.colors.Length > colorIndex)
            {
                var c = selectedType.colors[colorIndex];
                tint = new Color(c.r, c.g, c.b, 0.25f);
            }

            // Draw background tint behind the popup area
            EditorGUI.DrawRect(popupRect, tint);

            // Prev / Next with wrap-around
            if (GUI.Button(leftButtonRect, "<"))
            {
                currentIndex = (currentIndex - 1 + names.Length) % names.Length;
            }
            if (GUI.Button(rightButtonRect, ">"))
            {
                currentIndex = (currentIndex + 1) % names.Length;
            }

            // Leave space for a color swatch inside the popup area
            var swatchSize = position.height - 4f;
            var labelPadding = swatchSize + 6f;
            var popupInnerRect = new Rect(popupRect.x + labelPadding, popupRect.y, Mathf.Max(0, popupRect.width - labelPadding), popupRect.height);

            // Draw the popup (no label overload to keep control over content rect)
            int newIndex = EditorGUI.Popup(popupInnerRect, currentIndex, names);
            if (newIndex != currentIndex)
            {
                currentIndex = newIndex;
            }

            // Optional color swatch at far left of popup (inside popup rect)
            var swatchRect = new Rect(popupRect.x + 2f, popupRect.y + 2f, swatchSize, swatchSize);
            if (selectedType.colors != null && selectedType.colors.Length > colorIndex)
            {
                EditorGUI.DrawRect(swatchRect, selectedType.colors[colorIndex]);
            }
            else
            {
                EditorGUI.DrawRect(swatchRect, new Color(0.5f, 0.5f, 0.5f, 1f));
            }

            return currentIndex;
        }
    }
}

