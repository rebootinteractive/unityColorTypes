using UnityEngine;

namespace ObjectType
{
    public class ObjectTypeRandomizer: MonoBehaviour
    {
        public ObjectTypeEnum[] objectTypes;

#if ODIN_INSPECTOR
        [Button]
#else
        [ContextMenu("Randomize Children Object Types")]
#endif
        public void RandomizeChildrenObjectTypes(){
            var children = GetComponentsInChildren<ObjectTypeController>();
            foreach (var child in children)
            {
                child.SetObjectType(objectTypes[Random.Range(0, objectTypes.Length)]);
            }
        }
        
    }
}