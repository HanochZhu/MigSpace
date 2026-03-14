using JigSpace.Extensions;
using UnityEngine;

namespace JigSpace.Utilities
{
	public sealed class RectVerticalScope : RectLayoutScope
	{
		public RectVerticalScope(int count, Rect rect)
			: base(count, rect)
		{
		}

		protected override Rect InitNextRect()
		{
			Rect rect = base.Rect;
			rect.height /= base.Count;
			return rect;
		}

		protected override void DoNextRect()
		{
			Rect nextRect = base.NextRect;
			nextRect.y += nextRect.height;
			base.NextRect = nextRect;
			CurrentIndex++;
		}

		protected override Rect DoGetNextAmount(int amount, Rect retVal)
		{
			retVal = retVal.Edit(RectEdit.SetHeight(retVal.height * (float)amount));
			return retVal;
		}
	}
}
