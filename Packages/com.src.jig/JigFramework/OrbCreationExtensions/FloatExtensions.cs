using UnityEngine;

namespace OrbCreationExtensions
{
	public static class FloatExtensions
	{
		public static string MakeString(this float aFloat)
		{
			return aFloat.ToString() ?? "";
		}

		public static string MakeString(this float aFloat, int decimals)
		{
			if (decimals <= 0)
			{
				return Mathf.RoundToInt(aFloat).ToString() ?? "";
			}
			return string.Format("{0:F" + decimals + "}", aFloat);
		}

		public static int MakeInt(this float aFloat)
		{
			return Mathf.FloorToInt(aFloat);
		}

		public static bool MakeBool(this float aFloat)
		{
			return aFloat > 0f;
		}

		public static float MakeFloat(this float aFloat)
		{
			return aFloat;
		}

		public static double MakeDouble(this float aFloat)
		{
			return aFloat;
		}

		public static string MakeString(this double aDouble)
		{
			return aDouble.ToString() ?? "";
		}

		public static string MakeString(this double aDouble, int decimals)
		{
			if (decimals <= 0)
			{
				int num = (int)aDouble;
				if (num >= 0 && aDouble - (double)num >= 0.5)
				{
					num++;
				}
				if (num < 0 && aDouble - (double)num <= -0.5)
				{
					num--;
				}
				return num.ToString() ?? "";
			}
			return string.Format("{0:F" + decimals + "}", aDouble);
		}

		public static int MakeInt(this double aDouble)
		{
			return (int)aDouble;
		}

		public static bool MakeBool(this double aDouble)
		{
			return aDouble > 0.0;
		}

		public static float MakeFloat(this double aDouble)
		{
			return (float)aDouble;
		}

		public static double MakeDouble(this double aDouble)
		{
			return aDouble;
		}

		public static float To180Angle(this float f)
		{
			while (f <= -180f)
			{
				f += 360f;
			}
			while (f > 180f)
			{
				f -= 360f;
			}
			return f;
		}

		public static float To360Angle(this float f)
		{
			while (f < 0f)
			{
				f += 360f;
			}
			while (f >= 360f)
			{
				f -= 360f;
			}
			return f;
		}

		public static float RadToCompassAngle(this float rad)
		{
			return (rad * 57.29578f).DegreesToCompassAngle();
		}

		public static float DegreesToCompassAngle(this float angle)
		{
			angle = 90f - angle;
			return angle.To360Angle();
		}

		public static float CompassAngleLerp(this float from, float to, float portion)
		{
			float num = (to - from).To180Angle();
			num *= Mathf.Clamp01(portion);
			return (from + num).To360Angle();
		}

		public static float RelativePositionBetweenAngles(this float angle, float from, float to)
		{
			from = from.To360Angle();
			to = to.To360Angle();
			if (from - to > 180f)
			{
				from -= 360f;
			}
			if (to - from > 180f)
			{
				to -= 360f;
			}
			angle = angle.To360Angle();
			if (from < to)
			{
				if (angle >= from && angle < to)
				{
					return (angle - from) / (to - from);
				}
				if (angle - 360f >= from && angle - 360f < to)
				{
					return (angle - 360f - from) / (to - from);
				}
			}
			if (from > to)
			{
				if (angle < from && angle >= to)
				{
					return (angle - to) / (from - to);
				}
				if (angle - 360f < from && angle - 360f >= to)
				{
					return (angle - 360f - to) / (from - to);
				}
			}
			return -1f;
		}

		public static float Distance(this float f1, float f2)
		{
			return Mathf.Abs(f1 - f2);
		}

		public static float Round(this float f, int decimals)
		{
			float num = Mathf.Pow(10f, decimals);
			f = Mathf.Round(f * num);
			return f / num;
		}

		public static float Cube(this float f)
		{
			return f * f;
		}
	}
}
