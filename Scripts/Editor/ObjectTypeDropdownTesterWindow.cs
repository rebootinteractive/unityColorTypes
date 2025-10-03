using UnityEditor;
using UnityEngine;

namespace ObjectType
{
    public class ObjectTypeDropdownTesterWindow : EditorWindow
    {
        private int _selectedIndex;
        private int _colorIndex;

        [MenuItem("Tools/ObjectType/Dropdown Tester")] 
        private static void Open()
        {
            var window = GetWindow<ObjectTypeDropdownTesterWindow>(true, "ObjectType Dropdown Tester");
            window.minSize = new Vector2(360, 120);
            window.Show();
        }

        private void OnGUI()
        {
            var library = ObjectTypeLibrary.Find();
            if (library == null)
            {
                EditorGUILayout.HelpBox("ObjectTypeLibrary not found. Create one under Resources and mark as default.", MessageType.Warning);
                return;
            }

            EditorGUILayout.Space();
            _colorIndex = EditorGUILayout.IntSlider("Color Index", _colorIndex, 0, 8);
            _selectedIndex = ObjectTypeCountDropdown.DrawLayout(_selectedIndex, "Object Type", _colorIndex);

            EditorGUILayout.Space();
            if (library.objectTypes != null && library.objectTypes.Length > 0 && _selectedIndex >= 0 && _selectedIndex < library.objectTypes.Length)
            {
                var type = library.objectTypes[_selectedIndex];
                EditorGUILayout.LabelField("Selected Index", _selectedIndex.ToString());
                EditorGUILayout.LabelField("Selected Name", type.typeName ?? "");
            }
        }
    }
}


