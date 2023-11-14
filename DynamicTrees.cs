using DynamicTrees.Utilities;
using Unity.VisualScripting;
using DynamicTrees.DynamicTreesComponent;

namespace DynamicTrees
{
    public class Main : MelonMod
    {
        private static AssetBundle? assetBundle;
        internal static AssetBundle TexturesBundle
        {
            get => assetBundle ?? throw new System.NullReferenceException(nameof(assetBundle));
        }

        internal static SaveDataManager sdm = new SaveDataManager();

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Dynamic Trees is online");

            assetBundle = LoadAssetBundle("DynamicTrees.Textures.dynamictrees");

        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (sceneName.ToLowerInvariant().Contains("menu") || sceneName.ToLowerInvariant().Contains("boot") || sceneName.ToLowerInvariant().Contains("empty")) return;

            if(!sceneName.Contains("_SANDBOX") && !sceneName.Contains("_DLC") && !sceneName.Contains("_WILDLIFE"))
            {
                if (!GameObject.Find("SCRIPT_EnvironmentSystems").GetComponent<DynamicTreeData>())
                {
                    GameObject.Find("SCRIPT_EnvironmentSystems").AddComponent<DynamicTreeData>();
                }
            }

            if (GameManager.GetWeatherComponent().IsIndoorScene()) return;

            TextureHelper.ReplaceTreeTextures(sceneName);
        }

        private static AssetBundle LoadAssetBundle(string path)
        {
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            MemoryStream memoryStream = new MemoryStream((int)stream.Length);
            stream.CopyTo(memoryStream);

            return memoryStream.Length != 0
                ? AssetBundle.LoadFromMemory(memoryStream.ToArray())
                : throw new System.Exception("No data loaded!");
        }
    }
}
