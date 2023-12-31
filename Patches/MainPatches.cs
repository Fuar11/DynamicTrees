﻿using System;
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
    internal class MainPatches
    {

        [HarmonyPatch(typeof(RenderObjectInstance), nameof(RenderObjectInstance.Setup))]

        public class ChangeTextureOfInstancedTreesAfterInit
        {

            public static void Postfix(RenderObjectInstance __instance)
            {
                if (__instance.m_Category != RenderObjectInstance.Category.Tree) return;

                MelonLogger.Msg("Render Object Instance for trees is alive!");
                TextureHelper.ReplaceInstancedTreeTextures(GameManager.m_ActiveScene);
                DynamicTreeData dtd = GameObject.Find("SCRIPT_EnvironmentSystems").GetComponent<DynamicTreeData>();
                if (dtd != null) dtd.hasInstancedTrees = true;
            }

        }

        [HarmonyPatch(typeof(GearItem), nameof(GearItem.Serialize))]
        public class SaveDynamicTreeDataPrompt
        {
            public static void Postfix()
            {
                DynamicTreeData dynamicTreeData = GameObject.Find("SCRIPT_EnvironmentSystems").GetComponent<DynamicTreeData>();
                if (dynamicTreeData != null) dynamicTreeData.SaveData();
            }

        }
    }
}
