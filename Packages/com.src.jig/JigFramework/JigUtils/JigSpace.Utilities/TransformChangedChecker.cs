using System;
using UnityEngine;

namespace JigSpace.Utilities
{
	[Serializable]
	public class TransformChangedChecker
	{
		public Transform _Transform;

		public TransformData _Cache;

		public SpaceInfoPerAxis _CacheSpaceInfo;

		public TransformChangedChecker(Transform transform, TransformData cache)
		{
			_Transform = transform;
			_Cache = cache;
			_CacheSpaceInfo = SpaceInfoPerAxis.GlobalPosAndRot;
		}

		public TransformChangedChecker(Transform transform, SpaceInfoPerAxis space = SpaceInfoPerAxis.GlobalPosAndRot)
		{
			_Transform = transform;
			_Cache = TransformData.Create(transform, space);
			_CacheSpaceInfo = space;
		}

		public TransformChannel HasTransformChanged(bool autoUpdateCache = true)
		{
			return HasTransformChanged(_CacheSpaceInfo, autoUpdateCache);
		}

		public TransformChannel HasTransformChanged(SpaceInfoPerAxis space, bool autoUpdateCache = true)
		{
			TransformChannel transformChannel = TransformChannel.None;
			if (!_Transform)
			{
				return transformChannel;
			}
			transformChannel |= HasPositionChanged(_Transform, _Cache, space, transformChannel);
			transformChannel |= HasRotationChanged(_Transform, _Cache, space, transformChannel);
			transformChannel |= HasScaleChanged(_Transform, _Cache, space, transformChannel);
			if (autoUpdateCache)
			{
				UpdateCache();
			}
			return transformChannel;
		}

		private void UpdateCache()
		{
			_Cache.SetData(_Transform, _CacheSpaceInfo);
		}

		public static TransformChannel HasTransformChanged(Transform t, TransformData cache, SpaceInfoPerAxis space = SpaceInfoPerAxis.GlobalPosAndRot)
		{
			TransformChannel transformChannel = TransformChannel.None;
			transformChannel |= HasPositionChanged(t, cache, space, transformChannel);
			transformChannel |= HasRotationChanged(t, cache, space, transformChannel);
			return transformChannel | HasScaleChanged(t, cache, space, transformChannel);
		}

		private static TransformChannel HasScaleChanged(Transform t, TransformData cache, SpaceInfoPerAxis space, TransformChannel tVC)
		{
			Vector3 vector = (space.HasFlag(SpaceInfoPerAxis.ScaleLossy) ? t.lossyScale : t.localScale);
			if (vector != cache._Scale)
			{
				tVC = TransformChannel.Scale;
				if (vector.x != cache._Scale.x)
				{
					tVC |= TransformChannel.ScaleX;
				}
				if (vector.y != cache._Scale.y)
				{
					tVC |= TransformChannel.ScaleY;
				}
				if (vector.z != cache._Scale.z)
				{
					tVC |= TransformChannel.ScaleZ;
				}
			}
			return tVC;
		}

		private static TransformChannel HasRotationChanged(Transform t, TransformData cache, SpaceInfoPerAxis space, TransformChannel tVC)
		{
			Quaternion quaternion = (space.HasFlag(SpaceInfoPerAxis.RotationGlobal) ? t.rotation : t.localRotation);
			if (quaternion != cache._Rotation)
			{
				tVC = TransformChannel.Rotation;
				if (quaternion.x != cache._Rotation.x)
				{
					tVC |= TransformChannel.RotationX;
				}
				if (quaternion.y != cache._Rotation.y)
				{
					tVC |= TransformChannel.RotationY;
				}
				if (quaternion.z != cache._Rotation.z)
				{
					tVC |= TransformChannel.RotationZ;
				}
				if (quaternion.w != cache._Rotation.w)
				{
					tVC |= TransformChannel.RotationW;
				}
			}
			return tVC;
		}

		private static TransformChannel HasPositionChanged(Transform t, TransformData cache, SpaceInfoPerAxis space, TransformChannel tVC)
		{
			Vector3 vector = (space.HasFlag(SpaceInfoPerAxis.PositionGlobal) ? t.position : t.localPosition);
			if (vector != cache._Position)
			{
				tVC = TransformChannel.Position;
				if (vector.x != cache._Position.x)
				{
					tVC |= TransformChannel.PositionX;
				}
				if (vector.y != cache._Position.y)
				{
					tVC |= TransformChannel.PositionY;
				}
				if (vector.z != cache._Position.z)
				{
					tVC |= TransformChannel.PositionZ;
				}
			}
			return tVC;
		}
	}
}
