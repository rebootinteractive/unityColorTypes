using UnityEngine;

namespace ObjectType
{
    public class ObjectTypeSpriteSetter:MonoBehaviour,IObjectTypeListener
    {
        public int spriteIndex;
        
        public void OnObjectTypeChanged(Type type)
        {
            var rend = GetComponent<SpriteRenderer>();
            if (rend == null) return;
            
            if (type.sprites.Length > spriteIndex)
            {
                rend.sprite = type.sprites[spriteIndex];
            }
            else
            {
                rend.sprite = null;
            }
        }
        
    }
    
        
    
}