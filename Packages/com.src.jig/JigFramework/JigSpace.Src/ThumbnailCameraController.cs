using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ThumbnailCameraController : MonoBehaviour
{
	[SerializeField]
	private Renderer _backgroundPlaneRenderer;

	[SerializeField]
	private Renderer _gradientdPlaneRenderer;

	[SerializeField]
	private GameObject _shadowPlane;

	private Camera _camera;

	public Camera Camera
	{
		get
		{
			if (_camera == null)
			{
				_camera = GetComponent<Camera>();
			}
			return _camera;
		}
	}

	private void LateUpdate()
	{
		base.transform.SetPositionAndRotation(Camera.main.transform.position, Camera.main.transform.rotation);
	}

	public Texture2D CaptureThumbnail(float aspectRatio, bool autoDisableCamera = false, ValueTuple<Color, Color>? backgroundColor = null)
	{
		Camera.aspect = aspectRatio;
		Texture2D result = CaptureThumbnail(1f, Camera.pixelWidth, Camera.pixelHeight, 0, 0, autoDisableCamera, backgroundColor);
		Camera.ResetAspect();
		return result;
	}

	public Texture2D CaptureThumbnail(float scale, int capturedZoneWidth, int capturedZoneHeight, int captureOriginX = 0, int captureOriginY = 0, bool autoDisableCamera = false, ValueTuple<Color, Color>? backgroundColor = null)
	{
		if (backgroundColor.HasValue)
		{
			SetBackgroundColor(backgroundColor.Value.Item1, backgroundColor.Value.Item2);
		}
		base.gameObject.SetActive(true);
		base.transform.SetPositionAndRotation(Camera.main.transform.position, Camera.main.transform.rotation);
		RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24)
		{
			antiAliasing = 8,
			mipMapBias = -0.5f,
			anisoLevel = 9,
			filterMode = FilterMode.Trilinear
		};
		Camera.targetTexture = renderTexture;
		Camera.Render();
		Camera.targetTexture = null;
		RenderTexture.active = renderTexture;
		capturedZoneWidth = (int)((float)capturedZoneWidth * scale);
		capturedZoneHeight = (int)((float)capturedZoneHeight * scale);
		captureOriginX = (int)((float)captureOriginX * scale);
		captureOriginY = (int)((float)captureOriginY * scale);
		Texture2D texture2D = new Texture2D(capturedZoneWidth, capturedZoneHeight, TextureFormat.RGB24, false);
		captureOriginY = Screen.height - captureOriginY - capturedZoneHeight;
		Rect source = new Rect(captureOriginX, captureOriginY, capturedZoneWidth, capturedZoneHeight);
		texture2D.ReadPixels(source, 0, 0);
		texture2D.Apply();
		Camera.main.targetTexture = null;
		RenderTexture.active = null;
		UnityEngine.Object.Destroy(renderTexture);
		if (autoDisableCamera)
		{
			base.gameObject.SetActive(false);
		}
		return texture2D;
	}

	public void SetBackgroundColor(Color colorA, Color colorB)
	{
		_backgroundPlaneRenderer.material.color = colorA;
		_gradientdPlaneRenderer.material.SetColor("_ColorInner", colorA);
		_gradientdPlaneRenderer.material.SetColor("_ColorOuter", colorB);
	}

	private void OnEnable()
	{
		_shadowPlane.SetActive(true);
	}

	private void OnDisable()
	{
		_shadowPlane.SetActive(false);
	}
}
