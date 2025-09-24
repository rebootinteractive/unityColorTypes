using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ObjectType
{
    public class ObjectTypeSetter : MonoBehaviour
    {
        public ObjectTypeEnum[] objectTypes;
        public bool randomize;
        public bool setDefaultType;

#if ODIN_INSPECTOR
        [Button]
#else
        [ContextMenu("Set Children Object Types")]
#endif
        public void SetChildrenObjectTypes()
        {
            var children = GetComponentsInChildren<ObjectTypeController>();
            for (int i = 0; i < children.Length; i++)
            {
                ObjectTypeController child = children[i];
                var type = objectTypes[randomize ? Random.Range(0, objectTypes.Length) : i % objectTypes.Length];
                if (setDefaultType)
                {
                    child.defaultType = type;
                }
                child.SetObjectType(type);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(child);
#endif
            }
        }

    }
}