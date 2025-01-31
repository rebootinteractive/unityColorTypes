using System.Collections;
using UnityEngine;

namespace ObjectType
{
    public class ObjectTypeMaterialLerpSetter : MonoBehaviour, IObjectTypeListener
    {
        public float lerpDuration = 3;
        public bool lerpEnabled;

        public int objectTypeMaterialIndex;
        public int rendererMaterialIndex;
    
        protected Renderer Renderer;

    

        public void OnObjectTypeChanged(Type type)
        {
            if (Renderer == null)
            {
                Renderer = GetComponent<Renderer>();
                if (Renderer == null) Debug.LogError("Renderer not found");
            }
        
            if (type.materials.Length > objectTypeMaterialIndex)
            {
                if (Renderer != null)
                {
#if UNITY_EDITOR

                    if (!Application.isPlaying)
                    {
                        if (Renderer.sharedMaterials.Length > rendererMaterialIndex)
                        {
                            var newMaterials = Renderer.sharedMaterials;
                            newMaterials[rendererMaterialIndex] = type.materials[objectTypeMaterialIndex];
                            Renderer.sharedMaterials = newMaterials;
                            return;
                        }
                      
                        Debug.LogError("Renderer shared material index out of range");
                        return;
                    }

#endif
                    if (lerpEnabled)
                    {
                        StartCoroutine(LerpMaterial(type.materials[objectTypeMaterialIndex]));
                    }
                    else
                    {
                        if (Renderer.materials.Length > rendererMaterialIndex)
                        {
                            var newMaterials = Renderer.materials;
                            newMaterials[rendererMaterialIndex] = type.materials[objectTypeMaterialIndex];
                            Renderer.materials = newMaterials;
                        }
                        else
                        {
                            Debug.LogError("Renderer material index out of range");
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Material index out of range");
            }
        }

        protected virtual IEnumerator LerpMaterial(Material aimMaterial)
        {
            if (Renderer.materials.Length > rendererMaterialIndex)
            {
                var newMaterials = Renderer.materials;
                float elapsedTime = 0;
                Material startMaterial = newMaterials[rendererMaterialIndex];
                while (elapsedTime < lerpDuration)
                {
                    newMaterials[rendererMaterialIndex].Lerp(startMaterial, aimMaterial, elapsedTime / lerpDuration);
                    Renderer.materials = newMaterials;
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                    
            }
            else
            {
                Debug.LogError("Renderer material index out of range");
            }
        }
    }
}
