using System;
using JigSpace.Utilities;
using UnityEngine;

namespace JigSpace.Extensions
{
	public static class RectExtensions
	{
		public static Rect Edit(this Rect rect, RectEdit[] edits, params RectEdit[] editParams)
		{
			Rect output = new Rect(rect);
			RectEdit[] array = edits;
			foreach (RectEdit pair in array)
			{
				DoEdit(ref output, pair);
			}
			array = editParams;
			foreach (RectEdit pair2 in array)
			{
				DoEdit(ref output, pair2);
			}
			return output;
		}

		public static Rect Edit(this Rect rect, RectEdit edits, params RectEdit[] editParams)
		{
			Rect output = new Rect(rect);
			DoEdit(ref output, edits);
			foreach (RectEdit pair in editParams)
			{
				DoEdit(ref output, pair);
			}
			return output;
		}

		private static void DoEdit(ref Rect output, RectEdit pair)
		{
			switch (pair.Type)
			{
			case RectEdit.RectEditType.Add:
				RectEdit.Add(pair, ref output);
				break;
			case RectEdit.RectEditType.Change:
				RectEdit.Change(pair, ref output);
				break;
			case RectEdit.RectEditType.Set:
				RectEdit.Set(pair, ref output);
				break;
			case RectEdit.RectEditType.Multiply:
				RectEdit.Multiply(pair, ref output);
				break;
			case RectEdit.RectEditType.Subtract:
				RectEdit.Subtract(pair, ref output);
				break;
			case RectEdit.RectEditType.Divide:
				RectEdit.Divide(pair, ref output);
				break;
			case RectEdit.RectEditType.Modulo:
				RectEdit.Modulo(pair, ref output);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}
}
