using UnityEngine;

namespace ObjectType
{
    [UnityEditor.CustomEditor(typeof(ObjectTypeLibrary))]
    public class ObjectTypeLibraryEditor: UnityEditor.Editor
    {

        private string _materialReaderPath;
        private int _materialReaderIndex;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var library = (ObjectTypeLibrary) target;
            
            if (GUILayout.Button("Set As Default"))
            {
                library.isDefault = true;
                UnityEditor.EditorUtility.SetDirty(target);
                
                var allLibraries =Resources.FindObjectsOfTypeAll<ObjectTypeLibrary>();
                foreach (var objectTypeLibrary in allLibraries)
                {
                    if (objectTypeLibrary != library)
                    {
                        objectTypeLibrary.isDefault = false;
                        UnityEditor.EditorUtility.SetDirty(objectTypeLibrary);
                    }
                }
            }
            
            _materialReaderPath = UnityEditor.EditorGUILayout.TextField("Material Reader Path", _materialReaderPath);
            _materialReaderIndex = UnityEditor.EditorGUILayout.IntField("Material Index", _materialReaderIndex);
            if (GUILayout.Button("Read Materials"))
            {
                library.ReadMaterialFromAssets(_materialReaderPath, _materialReaderIndex);
            }
        }
    }
    
        
    
}