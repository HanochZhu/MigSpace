using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mig.Utils
{
    public static class TextureUtils
    {
        public static Sprite ConvertToSprite(this Texture2D self)
        {
            return Sprite.Create(self, new Rect(0,0,self.width, self.height), new Vector2(0.5f,0.5f));
        }

    }

}
