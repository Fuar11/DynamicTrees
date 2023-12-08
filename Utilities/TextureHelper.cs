using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine.AddressableAssets;
using UnityEngine;
using Il2CppTLD.AddressableAssets;
using Il2Cpp;
using DynamicTrees.DynamicTreesComponent;
using Unity.VisualScripting;
using UnityEngine.Analytics;
using Random = System.Random;

namespace DynamicTrees.Utilities
{
    internal class TextureHelper
    {

        public static string[] pineTrees = { "TRN_TreeBarkPine_Clear", "TRN_TreeBarkPine_Lowest", "TRN_TreeBarkPine_Low", "TRN_TreeBarkPine_LowMedium", "TRN_TreeBarkPine_Medium", "TRN_TreeBarkPine_HighMedium", "TRN_TreeBarkPine_High", "TRN_TreeBarkPine_Highest", "TRN_TreeBarkPine_Snow_A" };
        public static string[] cedarTrees = { "TRN_TreeBarkCedar_Clear", "TRN_TreeBarkCedar_Low", "TRN_TreeBarkCedar_LowMedium", "TRN_TreeBarkCedar_Medium", "TRN_TreeBarkCedar_HighMedium", "TRN_TreeBarkCedar_High", "TRN_TreeBarkCedar_Highest", "TRN_TreeBarkCedar_Highest", "TRN_TreeCedarBark_Snow_B" };

        //entry
        public static void ReplaceTreeTextures(string scene, bool runInstancedTrees = false)
        {
            if (runInstancedTrees) ReplaceInstancedTreeTextures(scene);

            ReplaceTerrainTreeTextures(scene);
            //ReplaceInSceneTreeTextures(scene);
        }

        //for regions still using Unity Terrain
        private static void ReplaceTerrainTreeTextures(string scene)
        {
            TerrainData terrainData = GetTerrainDataPerScene(scene);
            if (terrainData != null)
            {
                Array treeObjects = terrainData.treePrototypes;

                foreach (TreePrototype treePrototype in treeObjects)
                {
                    MeshRenderer meshRenderer = treePrototype.m_Prefab.GetComponent<MeshRenderer>();
                    if (meshRenderer == null)
                    {
                        continue;
                    }
                    Material mat = meshRenderer.material;
                    ReplaceMainTexture(mat);
                }

                terrainData.RefreshPrototypes();

            }

        }
        public static TerrainData GetTerrainDataPerScene(string scene)
        {

            TerrainData terrainData = null;

            if (scene == "DamRiverTransitionZoneB")
            {
                terrainData = GameObject.Find("Art/Terrain/DamRiverTerrain_main").GetComponent<Terrain>().terrainData;
            }
            
            return terrainData;
        }

        //for trees not part of terrain or instancing
        private static GameObject GetInSceneTreesPerScene(string scene)
        {

            GameObject trees = new GameObject();

            if (scene == "MarshRegion")
            {
                trees = GameObject.Find("Art/Terrain/Trees");
            }
            else if (scene == "LakeRegion")
            {
                trees = GameObject.Find("Art/NavmeshHelpers");
            }
            else if (scene == "TracksRegion")
            {
                trees = GameObject.Find("Art/TreesLogs");
            }
            else if (scene == "CrashMountainRegion")
            {
                trees = GameObject.Find("Art/Terrain/TRN_MountainDistant_Group/Trees");
            }
            else if (scene == "BlackrockRegion_SANDBOX")
            {
                trees = GameObject.Find("Root/Art/Sandbox_Trees");
            }
            else if (scene == "LongRailTransitionZone")
            {
                trees = GameObject.Find("Art/Trees_NonInstanced");
            }
            else if (scene == "MountainTownRegion")
            {
                trees = GameObject.Find("Art/Terrain/Trees");
            }
            else if (scene == "CanneryRegion")
            {
                trees = GameObject.Find("Art/Trees");
            }
            else if (scene == "RavineTransitionZone")
            {
                trees = GameObject.Find("Art/Terrain");
            }

            //Ash Canyon subregions (which don't cover anything but it's a minor amt of trees)
            if (scene == "AshCanyonRegion_CentralPeak")
            {
                trees = GameObject.Find("Root/Art/GrayBlock/Central_Peak_Logs");
            }
            else if (scene == "AshCanyonRegion_WestPassage")
            {
                trees = GameObject.Find("Root/Art/GrayBlock/WestPassage_Logs");
            }
            else if (scene == "AshCanyonRegion_WestRidge")
            {
                trees = GameObject.Find("Root/Art/Trees");
            }

            return trees;
        }
        private static void ReplaceInSceneTreeTextures(string scene)
        {
            GameObject Trees = GetInSceneTreesPerScene(scene);
            Transform treeObjects = Trees.transform;

            for (int i = 0; i < treeObjects.childCount; i++)
            {
                Transform tree = treeObjects.GetChild(i);


                //check to make sure the tree is actually a tree prefab and not some other object, in which case skip to the next one
                if (tree.gameObject.transform.childCount == 0 || !tree.gameObject.transform.GetChild(0).name.ToLowerInvariant().Contains("lod")) continue;

                MeshRenderer renderer = tree.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
                if (renderer)
                {
                    ReplaceMainTexture(renderer.material);
                }
            }


        }

        public static void ReplaceInstancedTreeTextures(string scene)
        {
            RenderObjectInstance TreeRenderer = GetInstancedObject(scene);

            if (TreeRenderer != null && TreeRenderer.m_Category == RenderObjectInstance.Category.Tree)
            {
                MelonLogger.Msg("Render object instance is tree category");

                RenderObjectInstanceBatches.PerBatch TreeBatches = TreeRenderer.m_Batches.m_Batches;               
                RenderObjectInstanceBatches.PerBatch.RenderInfo[] TreeBatchRenderInfos = TreeBatches.m_RenderInfos;

                foreach (var Renderer in TreeBatchRenderInfos)
                {

                    if (Renderer.m_Materials.Count < 1) continue;

                    ReplaceMainTexture(Renderer.m_Materials.ElementAt(0));
                }
            }

        }

        private static RenderObjectInstance GetInstancedObject(string scene)
        {

            RenderObjectInstance[] rois = null;

            if (scene == "AshCanyonRegion" || scene == "BlackrockRegion" || scene == "MiningRegion" || scene == "RuralRegion" || scene == "WhalingStationRegion" || scene == "TracksRegion")
            {
                rois = GameObject.Find("ArtRenderer").GetComponents<RenderObjectInstance>();
            }
            else if (scene == "HubRegion") rois = GameObject.Find("ArtRenderers/Instancing").GetComponents<RenderObjectInstance>();
            else if (scene == "AirfieldRegion") rois = GameObject.Find("ArtRenderer/Instanceing").GetComponents<RenderObjectInstance>();
            else if (scene == "LongRailTransitionZone") rois = GameObject.Find("Art_Renderers/Instancing").GetComponents<RenderObjectInstance>();
            else if (scene == "CanneryRegion") rois = GameObject.Find("Art/Art_Rendering").GetComponents<RenderObjectInstance>();
            else if (scene == "CanyonRoadTransitionZone" || scene == "BlackrockTransitionZone") rois = GameObject.Find("ArtRenderers/Instancing").GetComponents<RenderObjectInstance>();
            else if (scene == "LakeRegion" || scene == "CoastalRegion" || scene == "MarshRegion" || scene == "RiverValleyRegion" || scene == "RavineTransitionZone") rois = GameObject.Find("ArtRenderers").GetComponents<RenderObjectInstance>();
            else if (scene == "HighwayTransitionZone") rois = GameObject.Find("ArtInstancing").GetComponents<RenderObjectInstance>();
            else if (scene == "CrashMountainRegion") rois = GameObject.Find("Art/ArtRenderers").GetComponents<RenderObjectInstance>();
            else if (scene == "MountainTownRegion") rois = GameObject.Find("Art/Art_Renderers").GetComponents<RenderObjectInstance>();


            if (rois == null || rois.Length == 0) return null;
            return rois.FirstOrDefault(roi => roi.m_Category == RenderObjectInstance.Category.Tree);

        }

        //main replace method
        public static void ReplaceMainTexture(Material mat)
        {
            //check for all pine tree textures here
            if (pineTrees.Contains(mat.mainTexture.name))
            {
                mat.mainTexture = Main.TexturesBundle.LoadAsset<Texture>(GetTextureBasedOnWeather(pineTrees));
            }
            //check for all cedar tree textures here
            else if (cedarTrees.Contains(mat.mainTexture.name))
            {
                mat.mainTexture = Main.TexturesBundle.LoadAsset<Texture>(GetTextureBasedOnWeather(cedarTrees));
            }

        }

        public static string GetTextureBasedOnWeather(string[] textures)
        {

            DynamicTreeData dtd = GameObject.Find("SCRIPT_EnvironmentSystems").GetComponent<DynamicTreeData>();

            if (dtd == null)
            {
                return textures[0];
            }
            float acc = dtd.GetCurrentAccumulation();

            Random rand = new System.Random();

            int lowVariation = rand.Next(0, 1);
            int midVariation = rand.Next(-1, 1);
            int highVariation = rand.Next(-1, 0);

            if (acc >= dtd.clearAccumulation && acc < dtd.lowestAccumulation) return textures[0 + lowVariation];
            else if (acc >= dtd.lowestAccumulation && acc < dtd.lowAccumulation) return textures[1 + midVariation];
            else if (acc >= dtd.lowAccumulation && acc < dtd.lowMediumAccumulation) return textures[2 + midVariation];
            else if (acc >= dtd.lowMediumAccumulation && acc < dtd.mediumAccumulation) return textures[3 + midVariation];
            else if (acc >= dtd.mediumAccumulation && acc < dtd.mediumHighAccumulation) return textures[4 + midVariation];
            else if (acc >= dtd.mediumHighAccumulation && acc < dtd.highAccumulation) return textures[5 + midVariation];
            else if (acc >= dtd.highAccumulation && acc < dtd.highestAccumulation) return textures[6 + midVariation ];
            else if (acc >= dtd.highestAccumulation && acc < dtd.fullAccumulation) return textures[7 + highVariation];
            else return textures[7];
        }

    }
}
