using System;
using System.Collections.Generic;
using System.Linq;
using JigSpace;
using UnityEngine;

public class DataManager : JigSingleton<DataManager>
{

	public string LastSaveMd5 { get; private set; }


	public override void Awake()
	{
		base.Awake();
	}

	

	private void ConvertStageDataToSerializable()
	{
	}

	private void ConvertStageDataFromSerializable()
	{
	}

	private void UpdateFormatVersion()
	{
	}


	private void OnEnable()
	{
		EventManager.StartListening(Events.JigSaved, OnJigSaved);
		EventManager.StartListening(Events.ModelLoaded, OnModelLoaded);
	}

	private void OnDisable()
	{
		EventManager.StopListening(Events.JigSaved, OnJigSaved);
		EventManager.StopListening(Events.ModelLoaded, OnModelLoaded);
	}

	private void OnModelLoaded(object arg0, object arg1)
	{
	}

	private void OnJigSaved(object arg0, object arg1)
	{
			Color color = Color.black;
			Texture2D texture2D = JigSingleton<GameManager>.Instance.ThumbnailCamera.CaptureThumbnail(GlobalSettings.JIG_THUMBNAIL_ASPECT_RATIO, true, new ValueTuple<Color, Color>(color, color));
			EventManager.TriggerEvent(Events.ThumbnailTaken, null, texture2D);
	}

}
