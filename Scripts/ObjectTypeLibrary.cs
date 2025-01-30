using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectType
{
    [CreateAssetMenu(fileName = nameof(ObjectTypeLibrary), menuName = "ObjectType/" + nameof(ObjectTypeLibrary))]
    public class ObjectTypeLibrary : ScriptableObject
    {
        private static ObjectTypeLibrary _cachedInstance;
        public bool isDefault;
        public ObjectTypeController[] prefabs;
        public Type[] objectTypes;

        public static ObjectTypeLibrary Find(string name = nameof(ObjectTypeLibrary))
        {
            if (_cachedInstance != null) return _cachedInstance;

            var allLibraries = Resources.LoadAll<ObjectTypeLibrary>("");
            foreach (var library in allLibraries)
            {
                if(library.isDefault)
                {
                    _cachedInstance = library;
                    return _cachedInstance;
                }
            }

            if (allLibraries.Length > 0)
            {
                Debug.LogWarning("No default library found, returning the first one found");
                _cachedInstance = allLibraries[0];
                return _cachedInstance;
            }
            
#if UNITY_EDITOR
            if (allLibraries.Length == 0)
            {
                //Create the library
                var library = CreateInstance<ObjectTypeLibrary>();
                library.objectTypes = Array.Empty<Type>();
                library.isDefault = true;

                //Save the library
                var path = "Assets/Resources/" + name + ".asset";
                UnityEditor.AssetDatabase.CreateAsset(library, path);

                //Refresh the database
                UnityEditor.AssetDatabase.Refresh();
                _cachedInstance = library;
                return _cachedInstance;
            }
#endif

            return null;
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        public void ClearLibraryCache()
        {
            _cachedInstance = null;
        }

        // Call this when you need to force a reload of the library
        public static void ClearCache()
        {
            _cachedInstance = null;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Clear cache in editor when the asset is modified
            _cachedInstance = null;
        }
#endif

        public Type FindObjectType(string typeName)
        {
            foreach (var objectType in objectTypes)
            {
                if (objectType.typeName == typeName)
                {
                    return objectType;
                }
            }

            return new Type();
        }

        public string[] GetObjectTypeNames()
        {
            var names = new string[objectTypes.Length];
            for (var i = 0; i < objectTypes.Length; i++)
            {
                names[i] = objectTypes[i].typeName;
            }

            return names;
        }

        public Type GetRandomType()
        {
            return objectTypes[UnityEngine.Random.Range(0, objectTypes.Length)];
        }
        
        #if UNITY_EDITOR
        
        public void ReadMaterialFromAssets(string folderPath, int materialIndex)
        {
            // Convert the folder path into a path that Unity can understand for searching
            // For example, if folderPath is something like "Assets/MyMaterials",
            // then we can directly use that path in FindAssets.
    
            // Find all material assets in the given folder
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:Material", new[] {folderPath});
            List<Material> materialList = new List<Material>();

            // Load each material from its GUID
            foreach (string guid in guids)
            {
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                Material mat = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(assetPath);
                if (mat != null)
                {
                    materialList.Add(mat);
                }
            }
            
            if(materialList.Count==0)
            {
                Debug.LogError("No material found in the path: "+folderPath);
                return;
            }

            for (int index = 0; index < objectTypes.Length; index++)
            {
                Type type = objectTypes[index];
                //Find the material that includes type name
                foreach (var material in materialList)
                {
                    if (material.name.ToLower().Contains(type.typeName.ToLower()))
                    {
                        //If array needs, resize it
                        if (type.materials.Length <= materialIndex)
                        {
                            var typeMaterials = type.materials;
                            Array.Resize(ref typeMaterials, materialIndex + 1);
                            type.materials = typeMaterials;
                        }
                        
                        //Assign the material
                        type.materials[materialIndex] = material;
                        Debug.Log("Material assigned to " + type.typeName);
                        break;
                    }
                }
                
                objectTypes[index] = type;
            }
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}