using System;
using UnityEngine;

namespace JigSpace.Utilities
{
	[Serializable]
	public struct TransformData
	{
		public struct OperatorOptions
		{
			public enum ScaleOptions
			{
				DontOverride,
				OverrideValue,
				DoOperation
			}

			public ScaleOptions _ScaleOptions;

			public bool DoPosition;

			public bool DoRotation;

			public static OperatorOptions Default
			{
				get
				{
					OperatorOptions result = default(OperatorOptions);
					result._ScaleOptions = ScaleOptions.DontOverride;
					result.DoPosition = true;
					result.DoRotation = true;
					return result;
				}
			}
		}

		public Vector3 _Position;

		public Quaternion _Rotation;

		public Vector3 _Scale;

		public Vector3 _EulerAngles
		{
			get
			{
				return _Rotation.eulerAngles;
			}
			set
			{
				_Rotation.eulerAngles = value;
			}
		}

		public static TransformData Empty => new TransformData(Vector3.zero, Quaternion.identity, Vector3.one);

		public static TransformData PositiveInfinity => new TransformData(Vector3.positiveInfinity, Quaternion.identity, Vector3.positiveInfinity);

		public static TransformData NegativeInfinity => new TransformData(Vector3.negativeInfinity, Quaternion.identity, Vector3.negativeInfinity);

		public TransformData(GameObject gameObject, SpaceInfoPerAxis spaceInfoPerAxis = SpaceInfoPerAxis.GlobalPosAndRot)
			: this(gameObject.transform, spaceInfoPerAxis)
		{
		}

		public TransformData(Transform transform, SpaceInfoPerAxis spaceInfoPerAxis = SpaceInfoPerAxis.GlobalPosAndRot)
		{
			_Rotation = (spaceInfoPerAxis.HasFlag(SpaceInfoPerAxis.RotationGlobal) ? transform.rotation : transform.localRotation);
			_Scale = (spaceInfoPerAxis.HasFlag(SpaceInfoPerAxis.ScaleLossy) ? transform.lossyScale : transform.localScale);
			_Position = (spaceInfoPerAxis.HasFlag(SpaceInfoPerAxis.PositionGlobal) ? transform.position : transform.localPosition);
		}

		public TransformData(TransformData transform)
			: this(transform._Position, transform._Rotation, transform._Scale)
		{
		}

		public TransformData(Vector3 position)
			: this(position, Quaternion.identity, Vector3.one)
		{
		}

		public TransformData(Vector3 position, Quaternion rotation)
			: this(position, rotation, Vector3.one)
		{
		}

		public TransformData(Vector3 position, Vector3 eulerAngles, Vector3 scale)
			: this(position, Quaternion.Euler(eulerAngles), scale)
		{
		}

		public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			_Rotation = rotation;
			_Scale = scale;
			_Position = position;
		}

		public TransformData SetData(Transform transform, SpaceInfoPerAxis spaceInfoPerAxis = SpaceInfoPerAxis.AllLocal)
		{
			_Rotation = (spaceInfoPerAxis.HasFlag(SpaceInfoPerAxis.RotationGlobal) ? transform.rotation : transform.localRotation);
			_Scale = (spaceInfoPerAxis.HasFlag(SpaceInfoPerAxis.ScaleLossy) ? transform.lossyScale : transform.localScale);
			_Position = (spaceInfoPerAxis.HasFlag(SpaceInfoPerAxis.PositionGlobal) ? transform.position : transform.localPosition);
			return this;
		}

		public TransformData SetData(TransformData transform)
		{
			_Rotation = transform._Rotation;
			_Scale = transform._Scale;
			_Position = transform._Position;
			return this;
		}

		public TransformData ApplyData(Transform t, SpaceInfoPerAxis spaceInfoPerAxis = SpaceInfoPerAxis.GlobalPosAndRot)
		{
			t.localScale = _Scale;
			if (spaceInfoPerAxis.HasFlag(SpaceInfoPerAxis.PositionGlobal))
			{
				t.position = _Position;
			}
			else
			{
				t.localPosition = _Position;
			}
			if (spaceInfoPerAxis.HasFlag(SpaceInfoPerAxis.RotationGlobal))
			{
				t.rotation = _Rotation;
			}
			else
			{
				t.localRotation = _Rotation;
			}
			return this;
		}

		public static TransformData Create(Transform transform, SpaceInfoPerAxis spaceInfoPerAxis = SpaceInfoPerAxis.AllLocal)
		{
			return new TransformData(transform, spaceInfoPerAxis);
		}

		public TransformData Add(TransformData other)
		{
			Add(other, OperatorOptions.Default);
			return this;
		}

		public void Add(TransformData other, OperatorOptions options)
		{
			switch (options._ScaleOptions)
			{
			case OperatorOptions.ScaleOptions.OverrideValue:
				_Scale = other._Scale;
				break;
			case OperatorOptions.ScaleOptions.DoOperation:
				_Scale += other._Scale;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case OperatorOptions.ScaleOptions.DontOverride:
				break;
			}
			if (options.DoPosition)
			{
				_Position += other._Position;
			}
			if (options.DoRotation)
			{
				_Rotation *= other._Rotation;
			}
		}

		public TransformData Subtract(TransformData other)
		{
			Subtract(other, OperatorOptions.Default);
			return this;
		}

		public void Subtract(TransformData other, OperatorOptions options)
		{
			switch (options._ScaleOptions)
			{
			case OperatorOptions.ScaleOptions.OverrideValue:
				_Scale = other._Scale;
				break;
			case OperatorOptions.ScaleOptions.DoOperation:
				_Scale -= other._Scale;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case OperatorOptions.ScaleOptions.DontOverride:
				break;
			}
			if (options.DoPosition)
			{
				_Position -= other._Position;
			}
			if (options.DoRotation)
			{
				_Rotation *= Quaternion.Inverse(other._Rotation);
			}
		}

		public static implicit operator TransformData(Transform input)
		{
			return new TransformData(input);
		}

		public static implicit operator TransformData(GameObject input)
		{
			return new TransformData(input.transform);
		}

		public static bool operator ==(TransformData a, TransformData b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(TransformData a, TransformData b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is TransformData))
			{
				return false;
			}
			return Equals((TransformData)obj);
		}

		public bool Equals(TransformData other)
		{
			if (_Position == other._Position && _Rotation == other._Rotation)
			{
				return _Scale == other._Scale;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (((_Position.GetHashCode() * 397) ^ _Rotation.GetHashCode()) * 397) ^ _Scale.GetHashCode();
		}
	}
}
