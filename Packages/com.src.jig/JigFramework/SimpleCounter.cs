using UnityEngine;

public class SimpleCounter
{
	private float duration;

	private float maxTime;

	public void Start(float duration)
	{
		this.duration = duration;
		maxTime = 0f;
	}

	public void Reset()
	{
		maxTime = Time.time + duration;
	}

	public bool Ended()
	{
		return Time.time > maxTime;
	}
}
