using System;

namespace JigSpace.Utilities
{
	[Flags]
	public enum SpaceInfoPerAxis
	{
		AllLocal = 0,
		ScaleLossy = 1,
		PositionGlobal = 2,
		RotationGlobal = 4,
		GlobalPosAndRot = 6,
		AllGlobal = 7
	}
}
