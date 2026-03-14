using UnityEngine;

namespace JigSpace.Utilities
{
	public static class TransformDataExtensions
	{
		public static bool HasFlag(this SpaceInfoPerAxis @enum, SpaceInfoPerAxis valueToContain)
		{
			return (@enum & valueToContain) == valueToContain;
		}

		public static bool HasFlag(this int space, int valueToContain)
		{
			return (space & valueToContain) == valueToContain;
		}

		public static void SetFromTransform(this Transform applyTransform, Transform other, SpaceInfoPerAxis otherAxisOptions)
		{
			applyTransform.SetFromTransform(other, otherAxisOptions, otherAxisOptions);
		}

		public static void SetFromTransform(this Transform applyTransform, Transform other, SpaceInfoPerAxis otherAxisOptions, SpaceInfoPerAxis applyToAxisOptions)
		{
			TransformData transformData = new TransformData(other, otherAxisOptions);
			applyTransform.localScale = transformData._Scale;
			if (applyToAxisOptions.HasFlag(SpaceInfoPerAxis.PositionGlobal))
			{
				applyTransform.position = transformData._Position;
			}
			else
			{
				applyTransform.localPosition = transformData._Position;
			}
			if (applyToAxisOptions.HasFlag(SpaceInfoPerAxis.RotationGlobal))
			{
				applyTransform.rotation = transformData._Rotation;
			}
			else
			{
				applyTransform.localRotation = transformData._Rotation;
			}
		}

		public static void SetFromTD(this Transform transform, TransformData data, SpaceInfoPerAxis options = SpaceInfoPerAxis.GlobalPosAndRot)
		{
			data.ApplyData(transform, options);
		}

		public static TransformData GetTD(this Transform transform, SpaceInfoPerAxis options = SpaceInfoPerAxis.AllLocal)
		{
			return new TransformData(transform, options);
		}
	}
}
