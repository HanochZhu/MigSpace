using System.Collections.Generic;
using JigSpace.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace JigSpace.Extensions
{
	public static class MaterialExtension
	{
		public static bool AllowsCustomTextures(this Material material)
		{
			if (material.HasProperty("_ForceTextures"))
			{
				return material.GetFloat("_ForceTextures") == 0f;
			}
			return true;
		}


		public static bool TryGetInt(this Material material, string name, out int value)
		{
			if (material.HasInt(name))
			{
				value = material.GetInt(name);
				return true;
			}
			value = 0;
			return false;
		}

		public static bool TryGetFloat(this Material material, string name, out float value)
		{
			if (material.HasFloat(name))
			{
				value = material.GetFloat(name);
				return true;
			}
			value = 0f;
			return false;
		}

		public static bool TryGetVector(this Material material, string name, out Vector4 value)
		{
			if (material.HasVector(name))
			{
				value = material.GetVector(name);
				return true;
			}
			value = default(Vector4);
			return false;
		}

		public static bool TryGetColor(this Material material, string name, out Color color)
		{
			if (material.HasColor(name))
			{
				color = material.GetColor(name);
				return true;
			}
			color = default(Color);
			return false;
		}

		public static bool TryGetTexture(this Material material, string name, out Texture texture)
		{
			if (material.HasTexture(name))
			{
				texture = material.GetTexture(name);
				return texture != null;
			}
			texture = null;
			return false;
		}
	}
}
