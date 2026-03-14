using System;

namespace JigSpace.Utilities
{
	[Flags]
	public enum TransformChannel
	{
		None = 0,
		Position = 1,
		PositionX = 2,
		PositionY = 4,
		PositionZ = 8,
		AllPosition = 0xF,
		Scale = 0x10,
		ScaleX = 0x20,
		ScaleY = 0x40,
		ScaleZ = 0x80,
		AllScale = 0xF0,
		Rotation = 0x100,
		RotationX = 0x200,
		RotationY = 0x400,
		RotationZ = 0x800,
		RotationW = 0x1000,
		AllRotation = 0x1F00,
		PositionAndScale = 0xFF,
		PositionAndRotation = 0x1F0F,
		ScaleAndRotation = 0x1FF0,
		AllAxis = 0x1FFF
	}
}
