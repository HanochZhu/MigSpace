using UnityEngine;

namespace JigSpace.Extensions
{
	public static class TextureExtensions
	{
		public static Texture2D GetCopy(this Texture sourceTexture)
		{
			Texture2D texture2D = new Texture2D(sourceTexture.width, sourceTexture.height, TextureFormat.RGBA32, false);
			Graphics.CopyTexture(sourceTexture, texture2D);
			return texture2D;
		}
	}
}
