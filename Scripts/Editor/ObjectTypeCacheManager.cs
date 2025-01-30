using UnityEngine;

namespace ObjectType
{
    public static class ObjectTypeCacheManager
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            // Clear cache on startup
            ObjectTypeLibrary.ClearCache();
            
            // Register for application quit
            Application.quitting += OnApplicationQuit;
        }

        private static void OnApplicationQuit()
        {
            ObjectTypeLibrary.ClearCache();
        }
    }
}