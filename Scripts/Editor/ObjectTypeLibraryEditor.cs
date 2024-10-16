using UnityEngine;

namespace ObjectType
{
    [UnityEditor.CustomEditor(typeof(ObjectTypeLibrary))]
    public class ObjectTypeLibraryEditor: UnityEditor.Editor
    {
        
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
        }
    }
    
        
    
}