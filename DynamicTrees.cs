global using DynamicTrees.Utilities.JSON;

using MelonLoader.Utils;
using ComplexLogger;

using DynamicTrees.Utilities;
using DynamicTrees.DynamicTreesComponent;

namespace DynamicTrees
{
	public class Main : MelonMod
	{
		public static string SaveDataFile { get; } = Path.Combine(MelonEnvironment.ModsDirectory, "DynamicTrees", "DynamicTrees.json");

		internal static AssetBundle TexturesBundle { get; set; }
		internal static SaveDataManager SaveDataManager;
		internal static ComplexLogger<Main> Logger = new();
		internal static DynamicTreeData DynamicTreeData;
		internal static List<DynamicTreeTexture> TreeTextures { get; set; } = [];

		public override async void OnInitializeMelon()
		{
			SaveDataManager ??= new();

			if (SaveDataManager == null)
			{
				Logger.Log("SaveDataManager remains null", FlaggedLoggingLevel.Error);
				return;
			}

			if (!Directory.Exists(Path.Combine(MelonEnvironment.ModsDirectory, "DynamicTrees"))) Directory.CreateDirectory(Path.Combine(MelonEnvironment.ModsDirectory, "DynamicTrees"));

			TexturesBundle = LoadAssetBundle("DynamicTrees.Textures.dynamictrees");
			if (TexturesBundle == null) Logger.Log($"TexturesBundle is null", FlaggedLoggingLevel.Error);
			await LoadAllTreeTextures();
		}

		public override void OnSceneWasInitialized(int buildIndex, string sceneName)
		{
			if (SceneUtilities.IsScenePlayable(sceneName))
			{
				GameObject DynamicTreeObject = new() { name = "DynamicTreeObject", layer = vp_Layer.Default };
				UnityEngine.Object.Instantiate(DynamicTreeObject, GameManager.GetVpFPSPlayer().transform);
				GameObject.DontDestroyOnLoad(DynamicTreeObject);
				DynamicTreeData ??= DynamicTreeObject.AddComponent<DynamicTreeData>();
			}

			TextureHelper.ReplaceTreeTextures(sceneName);
		}

		public override async void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			if (SaveDataManager != null && DynamicTreeData != null && DynamicTreeData.SaveDataProxy != null) await SaveDataManager.Save(DynamicTreeData.SaveDataProxy);
		}

		public override void OnApplicationQuit()
		{
			SaveDataManager.Save(DynamicTreeData.SaveDataProxy);
			base.OnApplicationQuit();
		}

		public static AssetBundle LoadAssetBundle(string name)
		{
			Logger.Log($"Attempting to load an AssetBundle with name: {name}", FlaggedLoggingLevel.Debug);
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
			{
				MemoryStream? memory = new();
				stream.CopyTo(memory);
				return AssetBundle.LoadFromMemory(memory.ToArray());
			};
		}

		public static async Task LoadAllTreeTextures()
		{
			foreach (string item in TextureHelper.pineTrees)
			{
				await LoadTreeTexture(item);
			}
			foreach (string item in TextureHelper.cedarTrees)
			{
				await LoadTreeTexture(item);
			}
		}

		public static async Task LoadTreeTexture(string name)
		{
			Logger.Log($"LoadTreeTexture({name})", FlaggedLoggingLevel.Debug);
			TreeTextures.Add(new DynamicTreeTexture() { Name = name, Texture = TexturesBundle?.LoadAsset<Texture>(name) });
		}
	}
}
