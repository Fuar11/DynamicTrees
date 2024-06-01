using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2Cpp;
using DynamicTrees.DynamicTreesComponent;
using DynamicTrees.Utilities;
using Unity.VisualScripting;
using UnityEngine.Analytics;

namespace DynamicTrees.Patches
{
	[HarmonyPatch(typeof(RenderObjectInstance), nameof(RenderObjectInstance.Setup))]
	public class RenderObjectInstance_Setup
	{
		public static async void Postfix(RenderObjectInstance __instance)
		{
			if (__instance.m_Category != RenderObjectInstance.Category.Tree) return;

			Main.Logger?.Log("Render Object Instance for trees is alive!", ComplexLogger.FlaggedLoggingLevel.Debug);
			await TextureHelper.ReplaceInstancedTreeTextures(GameManager.m_ActiveScene);

			if (Main.DynamicTreeData != null) Main.DynamicTreeData.hasInstancedTrees = true;
		}

	}
}
