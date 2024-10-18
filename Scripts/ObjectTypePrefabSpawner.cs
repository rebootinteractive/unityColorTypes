using UnityEngine;

namespace ObjectType
{
    public class ObjectTypePrefabSpawner:MonoBehaviour,IObjectTypeListener
    {
        public GameObject spawnedInstance;
        public int prefabIndex;
        
        public void OnObjectTypeChanged(Type type)
        {
            DestroySpawnedInstance();
            if (type.prefabs.Length > prefabIndex && type.prefabs[prefabIndex]!=null)
            {
                if (Application.isPlaying)
                {
                    spawnedInstance = Instantiate(type.prefabs[prefabIndex], transform);
                    spawnedInstance.transform.localPosition = Vector3.zero;
                }
                else
                {
                    #if UNITY_EDITOR
                    spawnedInstance = (UnityEditor.PrefabUtility.InstantiatePrefab(type.prefabs[prefabIndex].gameObject) as GameObject);
                    spawnedInstance.transform.SetParent(transform);
                    spawnedInstance.transform.localPosition = Vector3.zero;
                    UnityEditor.EditorUtility.SetDirty(gameObject);
                    #endif
                }
            }
        }

        private void DestroySpawnedInstance()
        {
            if (spawnedInstance)
            {
                if (Application.isPlaying)
                {
                    Destroy(spawnedInstance);
                }
                else
                {
                    DestroyImmediate(spawnedInstance);
                    spawnedInstance = null;
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
                }
            }
        }
    }
}