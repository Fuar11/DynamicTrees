namespace ImprovedTrees
{
    public class Main : MelonMod
    {
        private static AssetBundle? assetBundle;
        internal static AssetBundle TexturesBundle
        {
            get => assetBundle ?? throw new System.NullReferenceException(nameof(assetBundle));
        }
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Improved Trees is online");

            assetBundle = LoadAssetBundle("ImprovedTrees.Textures.improvedtrees");

        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (GameManager.GetWeatherComponent().IsIndoorScene()) return;

            Utilities.TextureHelper.ReplaceTreeTextures();
            
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
