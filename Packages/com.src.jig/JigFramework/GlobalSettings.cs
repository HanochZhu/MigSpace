using System.Collections.Generic;
using UnityEngine;

public static class GlobalSettings
{
	public static readonly Color DefaultBackgroundColor = Color.white;

	public static float LegacyGlobalScale = 0.045f;

	public static float TweenSuperFast = 0.07f;

	public static float TweenFast = 0.4f;

	public static float TweenSlow = 0.8f;

	public const float BUTTON_FEEDBACK_DURATION = 1.8f;

	public static float LongPressDuration = 0.55f;

	public const int OBJECT_CAPTURE_MIN_IMAGE_COUNT = 3;

	public const int OBJECT_CAPTURE_MAX_IMAGE_COUNT = 50;

	public const float AutoPlayStepDelayMin = 0f;

	public const float AutoPlayStepDelayMax = 999f;

	public static string History = "history";

	public static string Science = "science";

	public static string Machines = "machines";

	public static string Space = "space";

	public static string HowTo = "howTo";

	public static string JigPacks = "jigPack";

	public static string Featured = "featured";

	public static string JigPackTiles = "jigPackTiles";

	public static string MyJigs = "myJigs";

	public static List<string> CategoryStrings = new List<string> { History, Science, Machines, Space, HowTo, JigPacks, JigPackTiles, MyJigs };

	public const float DefaultUITweenDuration = 0.3f;

	public const float FastUITweenDuration = 0.15f;

	public const float DefaultExplodeDuration = 0.8f;

	public const float UI_SWITCH_TWEEN_DURATION = 0.1f;

	public const float ON_HOVER_SATURATION_PERCENT = 0.14f;

	public const float ON_PRESS_SATURATION_PERCENT = 0.28f;

	public const int JIG_THUMBNAIL_WIDTH = 512;

	public const int JIG_THUMBNAIL_HEIGHT = 386;

	public const int JIG_GRID_COLUMN_COUNT = -1;

	public static float JIG_THUMBNAIL_ASPECT_RATIO => 1.32642484f;
}
