using UnityEditor;
using UnityEngine;

namespace ObjectType
{
    [CustomEditor(typeof(ObjectTypeController),true)]
    public class ObjectTypeControllerEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var objectType = (ObjectTypeController) target;
            if (GUILayout.Button("Set Type"))
            {
                objectType.SetObjectType(ObjectTypeLibrary.Find().FindObjectType(objectType.type.typeName));
                EditorUtility.SetDirty(target);
                
                var listeners=objectType.GetComponentsInChildren<IObjectTypeListener>();
                foreach (var listener in listeners)
                {
                    var listenerObject = (MonoBehaviour) listener;
                    EditorUtility.SetDirty(listenerObject);
                }
                
            }
        }
    }
}