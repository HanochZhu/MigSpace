using System;
using JigSpace.Extensions;
using UnityEngine;
using UnityEngine.Video;

namespace JigSpace.Utilities
{
	[RequireComponent(typeof(VideoPlayer))]
	public class VideoFrameExtractor : MonoBehaviour
	{
		public Action<Texture2D> _extractVideoFrameCallback;

		public static void ExtractFrame(string url, long frameIndex, Action<Texture2D> callback)
		{
			new GameObject().AddComponent<VideoFrameExtractor>().Init(url, frameIndex, callback);
		}

		public void Init(string url, long frameIndex, Action<Texture2D> callback)
		{
			_extractVideoFrameCallback = callback;
			VideoPlayer component = GetComponent<VideoPlayer>();
			component.Stop();
			component.url = url;
			component.renderMode = VideoRenderMode.APIOnly;
			component.playOnAwake = false;
			component.frame = frameIndex;
			component.sendFrameReadyEvents = true;
			component.frameReady += OnFrameReady;
			component.Prepare();
		}

		private void OnFrameReady(VideoPlayer source, long frameIndex)
		{
			Texture2D copy = source.texture.GetCopy();
			_extractVideoFrameCallback?.Invoke(copy);
			source.Stop();
			UnityEngine.Object.Destroy(source.gameObject);
		}
	}
}
