using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class ExplodeEffect
{
	protected struct ExplodedElement
	{
		public Vector3 OriginalLocalPosition;

		public Transform OriginalTransformParent;
	}

	protected List<ExplodedElement> _explodedElementsList = new List<ExplodedElement>();

	protected Transform _explodeCenterTransform;

	private Sequence _explodeJigSequence;

	public bool IsJigExploded { get; protected set; }

	public float ExplodedValue { get; protected set; }

	
	public void EnableExplode(bool enable)
	{
		IsJigExploded = enable;
	}

	public void ExplodeJig(float explodeValue, float duration, Action onTweenComplete = null)
	{
		ExplodedValue = explodeValue;
		int count = _explodedElementsList.Count;
		Vector3[] array = new Vector3[count];
		for (int i = 0; i < count; i++)
		{
			Vector3 vector = (array[i] = GetNewLocalPosition(_explodedElementsList[i], explodeValue));
		}
		if (_explodeJigSequence != null)
		{
			_explodeJigSequence.Complete();
			_explodeJigSequence = null;
		}
		_explodeJigSequence = DOTween.Sequence();
		for (int j = 0; j < count; j++)
		{
			Vector3 vector = array[j];
		}
		if (duration > 0f)
		{
			TweenCallback action = delegate
			{
				_explodeJigSequence = null;
				onTweenComplete?.Invoke();
			};
			_explodeJigSequence.OnComplete(action);
		}
		else
		{
			onTweenComplete?.Invoke();
		}
	}

	protected abstract Vector3 GetNewLocalPosition(ExplodedElement explodedElement, float explodeValue);

	public void Destroy()
	{
		if (_explodeCenterTransform != null)
		{
			UnityEngine.Object.Destroy(_explodeCenterTransform.gameObject);
		}
	}

	public void ResetExplodedJig(bool transition, Action OnCompleteAction)
	{
		if (!IsJigExploded)
		{
			return;
		}
		IsJigExploded = false;
		if (transition)
		{
			ExplodeJig(0f, 0.8f, delegate
			{
				OnCompleteAction?.Invoke();
			});
		}
		else
		{
			ExplodeJig(0f, 0f, delegate
			{
				OnCompleteAction?.Invoke();
			});
		}
	}

	
}
