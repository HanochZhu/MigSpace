using UnityEngine;

public class JigSingleton<T> : MonoBehaviour where T : Component
{
	private static T _instance;

	private static object _lock = new object();

	public static bool applicationIsQuitting;

	public static T Instance
	{
		get
		{
			if (applicationIsQuitting)
			{
				return null;
			}
			lock (_lock)
			{
				if ((Object)_instance == (Object)null)
				{
					_instance = (T)Object.FindObjectOfType(typeof(T));
					if (Object.FindObjectsOfType(typeof(T)).Length > 1)
					{
						Debug.LogError("[Singleton] Something went really wrong  - there should never be more than 1 singleton! Reopening the scene might fix it.");
						return _instance;
					}
					if ((Object)_instance == (Object)null)
					{
						GameObject obj = new GameObject();
						_instance = obj.AddComponent<T>();
						obj.name = "(singleton) " + typeof(T);
					}
				}
				return _instance;
			}
		}
	}

	public virtual void Awake()
	{
		Application.quitting += OnApplicationQuitting;
	}

    private void OnDestroy()
    {
		_instance = null;
    }

    private static void OnApplicationQuitting()
	{
		applicationIsQuitting = true;
	}
}
