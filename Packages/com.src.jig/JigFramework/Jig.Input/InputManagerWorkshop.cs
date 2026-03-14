using System;
using UnityEngine;

namespace JigSpace
{
	public class InputManagerWorkshop : MonoBehaviour
	{
		public enum TargetStage
		{
			PreviousStages,
			PreviousStage,
			FollowingStages,
			FollowingStage,
			AllStages,
			Waiting,
			None
		}

		[NonSerialized]
		public TargetStage Target = TargetStage.None;
	}
}
