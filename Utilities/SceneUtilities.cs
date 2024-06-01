// ---------------------------------------------
// ScenenUtilities - by The Illusion
// ---------------------------------------------
// Reusage Rights ------------------------------
// You are free to use this script or portions of it in your own mods, provided you give me credit in your description and maintain this section of comments in any released source code
//
// Warning !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
// Ensure you change the namespace to whatever namespace your mod uses, so it doesnt conflict with other mods
// ---------------------------------------------

namespace DynamicTrees.Utilities
{
	/// <summary>
	/// 
	/// </summary>
	public class SceneUtilities
	{

		/// <summary>
		/// Get if the current scene is indoor
		/// </summary>
		/// <param name="scene">true if you want the scene, false if you want the environment</param>
		/// <returns></returns>
		public static bool IsSceneIndoor(bool scene)
		{
			return (GameManager.GetWeatherComponent().IsIndoorScene() && scene) || GameManager.GetWeatherComponent().IsIndoorEnvironment();
		}

		/// <summary>
		/// Used to check if the current scene is EMPTY
		/// </summary>
		/// <param name="sceneName">The name of the scene to check, if null will use <c>GameManager.m_ActiveScene</c></param>
		/// <returns></returns>
		public static bool IsSceneEmpty(string? sceneName = null)
		{
			sceneName ??= GameManager.m_ActiveScene;

			return sceneName != null && sceneName.Contains("Empty", StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Used to check if the current scene is BOOT
		/// </summary>
		/// <param name="sceneName">The name of the scene to check, if null will use <c>GameManager.m_ActiveScene</c></param>
		/// <returns></returns>
		public static bool IsSceneBoot(string? sceneName = null)
		{
			sceneName ??= GameManager.m_ActiveScene;

			return sceneName != null && sceneName.Contains("Boot", StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Used to check if the current scene is a main menu scene
		/// </summary>
		/// <param name="sceneName">The name of the scene to check, if null will use <c>GameManager.m_ActiveScene</c></param>
		/// <returns></returns>
		public static bool IsSceneMenu(string? sceneName = null)
		{
			sceneName ??= GameManager.m_ActiveScene;

			return sceneName != null && sceneName.StartsWith("MainMenu", StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Used to check if the current scene is a playable scene. Use this for most needs
		/// </summary>
		/// <param name="sceneName">The name of the scene to check, if null will use <c>GameManager.m_ActiveScene</c></param>
		/// <returns></returns>
		public static bool IsScenePlayable(string? sceneName = null)
		{
			sceneName ??= GameManager.m_ActiveScene;

			return sceneName != null && !IsSceneEmpty(sceneName) && !IsSceneBoot(sceneName) && !IsSceneMenu(sceneName);
		}

		/// <summary>
		/// Used to check if the current scene is a base scene (Zone or Region)
		/// </summary>
		/// <param name="sceneName">The name of the scene to check, if null will use <c>GameManager.m_ActiveScene</c></param>
		/// <returns></returns>
		public static bool IsSceneBase(string? sceneName = null)
		{
			sceneName ??= GameManager.m_ActiveScene;

			return sceneName != null && (sceneName.Contains("Region", StringComparison.InvariantCultureIgnoreCase) || sceneName.Contains("Zone", StringComparison.InvariantCultureIgnoreCase));
		}

		/// <summary>
		/// Used to check if the current scene is a sandbox scene
		/// </summary>
		/// <param name="sceneName">The name of the scene to check, if null will use <c>GameManager.m_ActiveScene</c></param>
		/// <returns></returns>
		public static bool IsSceneSandbox(string? sceneName = null)
		{
			sceneName ??= GameManager.m_ActiveScene;

			return sceneName != null && sceneName.EndsWith("SANDBOX", StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Used to check if the current scene is a DLC01 scene
		/// </summary>
		/// <param name="sceneName">The name of the scene to check, if null will use <c>GameManager.m_ActiveScene</c></param>
		/// <returns></returns>
		public static bool IsSceneDLC01(string? sceneName = null)
		{
			sceneName ??= GameManager.m_ActiveScene;

			return sceneName != null && sceneName.EndsWith("DLC01", StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Used to check if the current scene is a DARKWALKER scene
		/// </summary>
		/// <param name="sceneName">The name of the scene to check, if null will use <c>GameManager.m_ActiveScene</c></param>
		/// <returns></returns>
		public static bool IsSceneDarkWalker(string? sceneName = null)
		{
			sceneName ??= GameManager.m_ActiveScene;

			return sceneName != null && sceneName.Contains("DARKWALKER", StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Used to check if the current scene is an additive scene, like sandbox or DLC scenes added to the base scene
		/// </summary>
		/// <param name="sceneName">The name of the scene to check, if null will use <c>GameManager.m_ActiveScene</c></param>
		/// <returns></returns>
		public static bool IsSceneAdditive(string? sceneName = null)
		{
			sceneName ??= GameManager.m_ActiveScene;

			return sceneName != null && (IsSceneSandbox(sceneName) || IsSceneDLC01(sceneName) || IsSceneDarkWalker(sceneName));
		}

		/// <summary>
		/// Used to check if the current scene is valid for weather
		/// </summary>
		/// <param name="sceneName">The name of the scene to check, if null will use <c>GameManager.m_ActiveScene</c></param>
		/// <param name="IndoorOverride"></param>
		/// <returns></returns>
		public static bool IsValidSceneForWeather(string sceneName, bool IndoorOverride)
		{
			sceneName ??= GameManager.m_ActiveScene;

			// this is done this way to make it easier to see the logic
			return sceneName != null
				&& (
					( IsSceneBase(sceneName) && !(IsSceneAdditive(sceneName)) ) && !GameManager.GetWeatherComponent().IsIndoorScene()
					)
				|| (GameManager.GetWeatherComponent().IsIndoorScene() && IndoorOverride);
		}
	}
}
