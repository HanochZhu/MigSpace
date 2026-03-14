using UnityEngine;

public static class ColorExtensions
{
	public static Color DuplicateWithAlpha(this Color color, float alpha)
	{
		return new Color(color.r, color.g, color.b, alpha);
	}

	public static bool Equals(this Color color, Color color2, bool ignoreAlpha, float tolerance = 0.01f)
	{
		if (Mathf.Abs(color.r - color2.r) < tolerance && Mathf.Abs(color.g - color2.g) < tolerance && Mathf.Abs(color.b - color2.b) < tolerance)
		{
			if (!ignoreAlpha)
			{
				return Mathf.Abs(color.a - color2.a) < tolerance;
			}
			return true;
		}
		return false;
	}
}
