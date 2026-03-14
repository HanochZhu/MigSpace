using System;
using System.Collections;
using System.Collections.Generic;
using JigSpace.Extensions;
using UnityEngine;

namespace JigSpace.Utilities
{
	public abstract class RectLayoutScope : IDisposable, IEnumerable<Rect>, IEnumerable
	{
		protected int CurrentIndex;

		public int Count { get; private set; }

		public Rect Rect { get; private set; }

		public Rect? LastRect { get; protected set; }

		public Rect FirstRect { get; protected set; }

		protected Rect NextRect { get; set; }

		protected RectLayoutScope(int count, Rect rect)
		{
			Count = count;
			Rect = rect;
			LastRect = null;
			CurrentIndex = 0;
			FirstRect = (NextRect = InitNextRect());
		}

		protected abstract Rect InitNextRect();

		protected abstract void DoNextRect();

		public Rect GetNext()
		{
			if (CurrentIndex > Count)
			{
				throw new IndexOutOfRangeException("Trying to create a rect, that is no longer in bounds");
			}
			LastRect = NextRect;
			Rect nextRect = NextRect;
			DoNextRect();
			return nextRect;
		}

		public Rect GetNext(int amount)
		{
			if (CurrentIndex == Count || CurrentIndex + amount > Count)
			{
				throw new IndexOutOfRangeException("Trying to create a rect, that is no longer in bounds");
			}
			LastRect = NextRect;
			Rect nextRect = NextRect;
			nextRect = DoGetNextAmount(amount, nextRect);
			for (int i = 0; i < amount; i++)
			{
				DoNextRect();
			}
			return nextRect;
		}

		protected virtual Rect DoGetNextAmount(int amount, Rect retVal)
		{
			retVal = retVal.Edit(RectEdit.SetWidth(retVal.width * (float)amount));
			return retVal;
		}

		public Rect GetNext(RectEdit rectEdit)
		{
			if (CurrentIndex > Count)
			{
				throw new IndexOutOfRangeException("Trying to create a rect, that is no longer in bounds");
			}
			LastRect = NextRect;
			Rect nextRect = NextRect;
			DoNextRect();
			return nextRect.Edit(rectEdit);
		}

		public Rect GetNext(int amount, RectEdit rectEdit)
		{
			if (CurrentIndex == Count || CurrentIndex + amount > Count)
			{
				throw new IndexOutOfRangeException("Trying to create a rect, that is no longer in bounds");
			}
			return GetNext(amount).Edit(rectEdit);
		}

		public Rect GetNext(params RectEdit[] edits)
		{
			if (CurrentIndex > Count)
			{
				throw new IndexOutOfRangeException("Trying to create a rect, that is no longer in bounds");
			}
			LastRect = NextRect;
			Rect nextRect = NextRect;
			DoNextRect();
			return nextRect.Edit(edits);
		}

		public Rect GetNext(int amount, params RectEdit[] edits)
		{
			if (CurrentIndex == Count || CurrentIndex + amount > Count)
			{
				throw new IndexOutOfRangeException("Trying to create a rect, that is no longer in bounds");
			}
			return GetNext(amount).Edit(edits);
		}

		public void Dispose()
		{
		}

		public IEnumerator<Rect> GetEnumerator()
		{
			while (CurrentIndex < Count)
			{
				yield return GetNext();
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
