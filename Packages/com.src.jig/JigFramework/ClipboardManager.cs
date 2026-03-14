using UnityEngine;

public class ClipboardManager : JigSingleton<ClipboardManager>
{
	private Vector3 _position;

	private Quaternion _rotation;

	private Vector3 _scale;

	public override void Awake()
	{
		base.Awake();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void CopyTransform(GameObject source)
	{
		if (source == null)
		{
			Debug.Log("No nobject selected");
			return;
		}
		_position = source.transform.position;
		_rotation = source.transform.rotation;
		_scale = source.transform.lossyScale;
	}

	public void PasteTransform(GameObject target)
	{
		if (target == null)
		{
			Debug.Log("No nobject selected");
			return;
		}
		target.transform.position = _position;
		target.transform.rotation = _rotation;
		target.transform.localScale = Vector3.one;
		target.transform.localScale = new Vector3(_scale.x / target.transform.lossyScale.x, _scale.y / target.transform.lossyScale.y, _scale.z / target.transform.lossyScale.z);
	}
}
