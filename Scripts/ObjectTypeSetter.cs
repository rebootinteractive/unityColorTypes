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
                child.SetObjectType(objectTypes[randomize ? Random.Range(0, objectTypes.Length) : i % objectTypes.Length]);
            }
        }

    }
}