using System;
using Lean.Pool;
using UnityEngine;

namespace ObjectType
{
    public class ObjectTypeController : MonoBehaviour
    {
        public bool pooling;
        public ObjectTypeEnum defaultType;
        
        [SerializeField]private bool setInactiveListeners;
        [SerializeField]private bool useDefaultType;

        public string TypeName
        {
            get
            {
                if (Type.IsNull())
                {
                    return null;
                }

                return Type.typeName;
            }
        }
        
        public Type Type
        {
            get;
            private set;

        }
        public bool Pooled { get; private set; }

        public virtual void SetObjectType(int typeIndex)
        {
            var library = ObjectTypeLibrary.Find();
            if (library==null)
            {
                Debug.LogError("ObjectTypeLibrary is null");
                return;
            }
            var objectTypes=library.objectTypes;
            if (typeIndex < 0 || typeIndex >= objectTypes.Length)
            {
                Debug.LogError("Type index is out of range");
                return;
            }
            SetObjectType(objectTypes[typeIndex]);
        }

        public virtual void SetObjectType(Type type)
        {
            if (type.IsNull())
            {
                Debug.LogError("Type is null");
                return;
            }

            Type = type;
            var listeners = GetComponentsInChildren<IObjectTypeListener>(setInactiveListeners);
            foreach (var listener in listeners)
            {
                listener.OnObjectTypeChanged(type);
            }
        }

        protected void Start()
        {
            if (useDefaultType)
            {
                var library = ObjectTypeLibrary.Find();
                var typeNames = library.GetObjectTypeNames();
                var name = string.IsNullOrEmpty(defaultType?.typeName) && typeNames.Length > 0
                    ? typeNames[0]
                    : defaultType?.typeName;
                if (!string.IsNullOrEmpty(name))
                {
                    SetObjectType(library.FindObjectType(name));
                }
            }
        }

        public virtual void Destroy()
        {
            if (Pooled)
            {
                LeanPool.Despawn(gameObject);
            }
            else
            {
                if(Application.isPlaying)
                    Destroy(gameObject);
                else
                    DestroyImmediate(gameObject);
            }
        }
        
        public static ObjectTypeController Spawn(string typeName, int prefabIndex)
        {
            var objectType = ObjectTypeLibrary.Find().FindObjectType(typeName);
            if (objectType.IsNull())
            {
                return null;
            }

            var prefabs = ObjectTypeLibrary.Find().prefabs;
            if (prefabIndex < 0 || prefabIndex >= prefabs.Length)
            {
                return null;
            }
            return Spawn(objectType, prefabs[prefabIndex]);
        }

        public static ObjectTypeController Spawn(Type objectType, ObjectTypeController prefab)
        {
            ObjectTypeController instance = null;

#if UNITY_EDITOR

            if (!Application.isPlaying)
            {
                instance = (UnityEditor.PrefabUtility.InstantiatePrefab(prefab.gameObject) as GameObject)?.GetComponent<ObjectTypeController>();
                if (instance == null)
                {
                    Debug.LogError("Prefab does not have ObjectTypeController component");
                    return null;
                }

                instance.defaultType = ObjectTypeEnum.GetEnum(objectType.typeName);
            }

#endif
            if (instance == null)
            {
                if (prefab.pooling)
                {
                    instance = LeanPool.Spawn(prefab.gameObject).GetComponent<ObjectTypeController>();
                    instance.Pooled = true;
                }
                else
                {
                    instance = Instantiate(prefab).GetComponent<ObjectTypeController>();
                }
            }

            instance.SetObjectType(objectType);
            return instance;
        }
    }
}