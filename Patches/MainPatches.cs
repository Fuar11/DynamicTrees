using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2Cpp;
using ImprovedTrees.DynamicTrees;
using ImprovedTrees.Utilities;
using Unity.VisualScripting;

namespace ImprovedTrees.Patches
{
    internal class MainPatches
    {

        [HarmonyPatch(typeof(RenderObjectInstanceBatches.PerBatch), nameof(RenderObjectInstanceBatches.PerBatch.SetRenderInfo))]

        public class ReplaceTreeTextureRenderer
        {
            public static void Prefix(ref int batchIndex, ref Renderer renderer, ref Mesh mesh, ref int lod, ref bool forceOnlyLod0Shadow, RenderObjectInstanceBatches.PerBatch __instance)
            {
                Material testMat = renderer.material;

                TextureHelper.ReplaceMainTexture(testMat);

            }
        }



        [HarmonyPatch(typeof(GearItem), nameof(GearItem.Serialize))]
        public class SaveDynamicTreeDataPrompt
        {
            public static void Postfix()
            {
                MelonLogger.Msg("GearItem Serialize called");

                DynamicTreeData dynamicTreeData = GameObject.Find("SCRIPT_EnvironmentSystems").GetComponent<DynamicTreeData>();
                if (dynamicTreeData != null) dynamicTreeData.SaveData();
            }

        }

    }
}
