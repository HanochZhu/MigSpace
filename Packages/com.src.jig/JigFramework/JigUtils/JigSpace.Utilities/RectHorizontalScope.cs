using JigSpace.Extensions;
using UnityEngine;

namespace JigSpace.Utilities
{
	public sealed class RectHorizontalScope : RectLayoutScope
	{
		public RectHorizontalScope(int count, Rect rect)
			: base(count, rect)
		{
		}

		protected override Rect InitNextRect()
		{
			Rect rect = base.Rect;
			rect.width /= base.Count;
			return rect;
		}

		protected override void DoNextRect()
		{
			Rect nextRect = base.NextRect;
			nextRect.x += nextRect.width;
			base.NextRect = nextRect;
			CurrentIndex++;
		}

		protected override Rect DoGetNextAmount(int amount, Rect retVal)
		{
			retVal = retVal.Edit(RectEdit.SetWidth(retVal.width * (float)amount));
			return retVal;
		}
	}
}
