using UnityEngine;

namespace OrbCreationExtensions
{
	public static class ObjectExtensions
	{
		public static string MakeString(this object anObject)
		{
			if (anObject == null)
			{
				return "";
			}
			if (anObject.GetType() == typeof(string))
			{
				return (string)anObject;
			}
			if (anObject.GetType() == typeof(bool))
			{
				return ((bool)anObject).MakeString();
			}
			if (anObject.GetType() == typeof(int))
			{
				return ((int)anObject).MakeString();
			}
			if (anObject.GetType() == typeof(float))
			{
				return ((float)anObject).MakeString();
			}
			if (anObject.GetType() == typeof(double))
			{
				return ((double)anObject).MakeString();
			}
			if (anObject.GetType() == typeof(string))
			{
				return ((string)anObject).MakeString();
			}
			return anObject?.ToString() ?? "";
		}

		public static int MakeInt(this object anObject)
		{
			if (anObject == null)
			{
				return 0;
			}
			if (anObject.GetType() == typeof(string))
			{
				return ((string)anObject).MakeInt();
			}
			if (anObject.GetType() == typeof(bool))
			{
				return ((bool)anObject).MakeInt();
			}
			if (anObject.GetType() == typeof(int))
			{
				return ((int)anObject).MakeInt();
			}
			if (anObject.GetType() == typeof(float))
			{
				return ((float)anObject).MakeInt();
			}
			if (anObject.GetType() == typeof(double))
			{
				return ((double)anObject).MakeInt();
			}
			if (anObject.GetType() == typeof(string))
			{
				return ((string)anObject).MakeInt();
			}
			return 0;
		}

		public static bool MakeBool(this object anObject)
		{
			if (anObject == null)
			{
				return false;
			}
			if (anObject.GetType() == typeof(string))
			{
				return ((string)anObject).MakeBool();
			}
			if (anObject.GetType() == typeof(bool))
			{
				return ((bool)anObject).MakeBool();
			}
			if (anObject.GetType() == typeof(int))
			{
				return ((int)anObject).MakeBool();
			}
			if (anObject.GetType() == typeof(float))
			{
				return ((float)anObject).MakeBool();
			}
			if (anObject.GetType() == typeof(double))
			{
				return ((double)anObject).MakeBool();
			}
			if (anObject.GetType() == typeof(string))
			{
				return ((string)anObject).MakeBool();
			}
			return true;
		}

		public static float MakeFloat(this object anObject)
		{
			if (anObject == null)
			{
				return 0f;
			}
			if (anObject.GetType() == typeof(string))
			{
				return ((string)anObject).MakeFloat();
			}
			if (anObject.GetType() == typeof(bool))
			{
				return ((bool)anObject).MakeFloat();
			}
			if (anObject.GetType() == typeof(int))
			{
				return ((int)anObject).MakeFloat();
			}
			if (anObject.GetType() == typeof(float))
			{
				return ((float)anObject).MakeFloat();
			}
			if (anObject.GetType() == typeof(double))
			{
				return ((double)anObject).MakeFloat();
			}
			if (anObject.GetType() == typeof(string))
			{
				return ((string)anObject).MakeFloat();
			}
			return 0f;
		}

		public static double MakeDouble(this object anObject)
		{
			if (anObject == null)
			{
				return 0.0;
			}
			if (anObject.GetType() == typeof(string))
			{
				return ((string)anObject).MakeDouble();
			}
			if (anObject.GetType() == typeof(bool))
			{
				return ((bool)anObject).MakeDouble();
			}
			if (anObject.GetType() == typeof(int))
			{
				return ((int)anObject).MakeDouble();
			}
			if (anObject.GetType() == typeof(float))
			{
				return ((float)anObject).MakeDouble();
			}
			if (anObject.GetType() == typeof(double))
			{
				return ((double)anObject).MakeDouble();
			}
			if (anObject.GetType() == typeof(string))
			{
				return ((string)anObject).MakeDouble();
			}
			return 0.0;
		}

		public static Color MakeColor(this object anObject)
		{
			if (anObject == null)
			{
				new Color(0f, 0f, 0f, 0f);
			}
			else
			{
				if (anObject.GetType() == typeof(Color))
				{
					return ((Color)anObject).MakeColor();
				}
				if (anObject.GetType() == typeof(string))
				{
					return ((string)anObject).MakeColor();
				}
				if (anObject.GetType() == typeof(Vector3))
				{
					return ((Vector3)anObject).MakeColor();
				}
				if (anObject.GetType() == typeof(Vector4))
				{
					return ((Vector4)anObject).MakeColor();
				}
			}
			return new Color(0f, 0f, 0f, 0f);
		}
	}
}
