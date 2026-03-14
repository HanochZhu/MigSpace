using System;
using UnityEngine;

namespace OrbCreationExtensions
{
	public static class Texture2DExtensions
	{
		public static bool HasTransparency(this Texture2D tex)
		{
			Color[] pixels;
			try
			{
				pixels = tex.GetPixels(0);
			}
			catch (Exception message)
			{
				Debug.Log(message);
				return false;
			}
			for (int i = 0; i < pixels.Length; i++)
			{
				if (pixels[i].a < 1f)
				{
					return true;
				}
			}
			return false;
		}
	}
}
