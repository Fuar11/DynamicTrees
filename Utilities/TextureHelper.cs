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

namespace DynamicTrees.Utilities
{
    internal class TextureHelper
    {

        public static string[] pineTrees = { "TRN_TreeBarkPine_Clear", "TRN_TreeBarkPine_Lowest", "TRN_TreeBarkPine_Low", "TRN_TreeBarkPine_LowMedium", "TRN_TreeBarkPine_Medium", "TRN_TreeBarkPine_HighMedium", "TRN_TreeBarkPine_High", "TRN_TreeBarkPine_Highest", "TRN_TreeBarkPine_Snow_A" };
        public static string[] spruceTrees = { };

        //entry
        public static void ReplaceTreeTextures(string scene)
        {
            ReplaceTerrainTreeTextures(scene);
            ReplaceInSceneTreeTextures(scene);
        }

        //for old regions
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
        private static TerrainData GetTerrainDataPerScene(string scene)
        {

            TerrainData terrainData = null;

            if (scene == "LakeRegion")
            {
                terrainData = GameObject.Find("Art/Terrains/Terrain").GetComponent<Terrain>().terrainData;
            }
            else if (scene == "MarshRegion")
            {
                terrainData = GameObject.Find("Art/Terrain/Terrain_main").GetComponent<Terrain>().terrainData;
            }
            else if (scene == "RuralRegion")
            {
                terrainData = GameObject.Find("Art/Terrain_RuralMain").GetComponent<Terrain>().terrainData;
            }
            else if (scene == "CoastalRegion")
            {
                terrainData = GameObject.Find("Art/Terrain_CoastalMain").GetComponent<Terrain>().terrainData;
            }
            else if (scene == "WhalingStationRegion")
            {
                terrainData = GameObject.Find("Art/Terrain_WhaleMain").GetComponent<Terrain>().terrainData;
            }
            else if (scene == "HighwayTransitionZone")
            {
                terrainData = GameObject.Find("Art/HighwayTerrain_main").GetComponent<Terrain>().terrainData;
            }
            else if (scene == "CrashMountainRegion")
            {
                terrainData = GameObject.Find("Art/Terrain/CrashMountainTerrain_main").GetComponent<Terrain>().terrainData;
            }
            else if (scene == "MarshRegion")
            {
                terrainData = GameObject.Find("Art/Terrain/Terrain_main").GetComponent<Terrain>().terrainData;
            }
            else if (scene == "TracksRegion")
            {
                terrainData = GameObject.Find("Art/Terrain/TracksTerrain_main").GetComponent<Terrain>().terrainData;
            }
            else if (scene == "RavineTransitionZone")
            {
                terrainData = GameObject.Find("Art/TerrainRavineExtensionMain").GetComponent<Terrain>().terrainData;
            }
            else if (scene == "DamRiverTransitionZoneB")
            {
                terrainData = GameObject.Find("Art/Terrain/DamRiverTerrain_main").GetComponent<Terrain>().terrainData;
            }
            else if (scene == "MountainTownRegion")
            {
                terrainData = GameObject.Find("Art/Terrain/Terrains/MountainTown_main_terrain").GetComponent<Terrain>().terrainData;
            }
            else if (scene == "RiverValleyRegion")
            {
                terrainData = GameObject.Find("Art/Terrains/RiverValleyTerrain_Main").GetComponent<Terrain>().terrainData;
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

        //for new regions
        public static void ReplaceInstancedTreeTextures(string scene)
        {
            RenderObjectInstance TreeRenderer = GetInstancedObject(scene);
            if (TreeRenderer != null && TreeRenderer.m_Category == RenderObjectInstance.Category.Tree)
            {
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

            if (scene == "AshCanyonRegion" || scene == "BlackrockRegion")
            {
                return GameObject.Find("ArtRenderer").GetComponent<RenderObjectInstance>();
            }
            else if (scene == "HubRegion") return GameObject.Find("ArtRenderers/Instancing").GetComponent<RenderObjectInstance>();
            else if (scene == "AirfieldRegion") return GameObject.Find("ArtRenderer/Instanceing").GetComponent<RenderObjectInstance>();
            else if (scene == "LongRailTransitionZone") return GameObject.Find("Art_Renderers/Instancing").GetComponent<RenderObjectInstance>();
            else if (scene == "CanneryRegion") return GameObject.Find("Art/Art_Rendering").GetComponent<RenderObjectInstance>();
            else if (scene == "CanyonRoadTransitionZone" || scene == "BlackrockTransitionZone") return GameObject.Find("ArtRenderers/Instancing").GetComponent<RenderObjectInstance>();
            else return null;
        }

        //main replace method
        public static void ReplaceMainTexture(Material mat)
        {
            //check for all pine tree textures here
            if (pineTrees.Contains(mat.mainTexture.name))
            {
                mat.mainTexture = Main.TexturesBundle.LoadAsset<Texture>(GetTextureBasedOnWeather(pineTrees));
            }
            //else if(mat.mainTexture.name == "")

        }

        public static string GetTextureBasedOnWeather(string[] textures)
        {

            DynamicTreeData dtd = GameObject.Find("SCRIPT_EnvironmentSystems").GetComponent<DynamicTreeData>();

            if (dtd == null)
            {
                return textures[0];
            }
            float acc = dtd.GetCurrentAccumulation();

            if (acc >= dtd.clearAccumulation && acc < dtd.lowestAccumulation) return textures[0];
            else if (acc >= dtd.lowestAccumulation && acc < dtd.lowAccumulation) return textures[1];
            else if (acc >= dtd.lowAccumulation && acc < dtd.lowMediumAccumulation) return textures[2];
            else if (acc >= dtd.lowMediumAccumulation && acc < dtd.mediumAccumulation) return textures[3];
            else if (acc >= dtd.mediumAccumulation && acc < dtd.mediumHighAccumulation) return textures[4];
            else if (acc >= dtd.mediumHighAccumulation && acc < dtd.highAccumulation) return textures[5];
            else if (acc >= dtd.highAccumulation && acc < dtd.highestAccumulation) return textures[6];
            else if (acc >= dtd.highestAccumulation && acc < dtd.fullAccumulation) return textures[7];
            else return textures[8];
        }

    }
}
