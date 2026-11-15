using HarmonyLib;

namespace MadelineInPharloom;

/// <summary>
/// The game's managers, for Silkeste's own code: the instance each one registered in its Awake, or null. Never a scene
/// search. The game's own getters (GameManager.SilentInstance / instance, GameCameras.instance, UIManager.instance,
/// ManagerSingleton.Instance) search the whole scene on every call while there is none, and two of them log an error
/// each time: asked dozens of times a frame on the start screen, that was its stutter. 2.4.0 throttled those getters
/// themselves, which also starved the game's own start-up code of them (GameManager.Awake reads GameCameras.instance
/// before GameCameras has woken) and soft-locked the main menu after Save and Quit (player report 2026-11-13). So the
/// game's getters are left alone, and Silkeste asks here instead.
/// </summary>
public static class Lookup
{
	private static readonly AccessTools.FieldRef<GameManager> GmField = AccessTools.StaticFieldRefAccess<GameManager>(AccessTools.Field(typeof(GameManager), "_instance"));

	private static readonly AccessTools.FieldRef<GameCameras> CamsField = AccessTools.StaticFieldRefAccess<GameCameras>(AccessTools.Field(typeof(GameCameras), "_instance"));

	private static readonly AccessTools.FieldRef<UIManager> UiField = AccessTools.StaticFieldRefAccess<UIManager>(AccessTools.Field(typeof(UIManager), "_instance"));

	public static GameManager Gm
	{
		get
		{
			GameManager g = GmField();
			return (g != null) ? g : null;   // a destroyed one counts as none
		}
	}

	public static GameCameras Cams
	{
		get
		{
			GameCameras c = CamsField();
			return (c != null) ? c : null;
		}
	}

	public static UIManager Ui
	{
		get
		{
			UIManager u = UiField();
			return (u != null) ? u : null;
		}
	}

	public static InputHandler Input
	{
		get
		{
			InputHandler i = ManagerSingleton<InputHandler>.UnsafeInstance;
			return (i != null) ? i : null;
		}
	}
}
