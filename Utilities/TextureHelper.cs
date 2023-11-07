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
using ImprovedTrees;

namespace ImprovedTrees.Utilities
{
    internal class TextureHelper
    {

        public static void ReplaceTreeTextures()
        {
            //check if scene is outdoors
            //todo

            if(GameManager.m_ActiveScene == "AshCanyonRegion" || GameManager.m_ActiveScene == "BlackrockRegion" || GameManager.m_ActiveScene == "LongRailTransitionZone" || GameManager.m_ActiveScene == "HubRegion" || GameManager.m_ActiveScene == "AirfieldRegion")
            {
                MelonLogger.Msg("Scene is newer than 1.56. Hinterland did things weirdly here so I'll deal with it later.");
                return;
            }

            TerrainData terrainData = GetTerrainDataPerScene();
            if (terrainData != null)
            {
                MelonLogger.Msg("Terrain component found.");
                Array treeObjects = terrainData.treePrototypes;

                foreach(TreePrototype treePrototype in treeObjects)
                {
                    MeshRenderer meshRenderer = treePrototype.m_Prefab.GetComponent<MeshRenderer>();
                    Material mat = meshRenderer.material;
                    if(mat.mainTexture.name == "TRN_TreeBarkPine_Snow_A")
                    {
                        MelonLogger.Msg("Main texture can be replaced");
                        mat.mainTexture = Main.TexturesBundle.LoadAsset<Texture>("TRN_TreeBarkPine_Clear");
                        MelonLogger.Msg("Main texture has been replaced");
                    }
                }

                terrainData.RefreshPrototypes();

            }

        }

        private static TerrainData GetTerrainDataPerScene()
        {

            string scene = GameManager.m_ActiveScene;
            TerrainData terrainData = null;

            if (scene == "LakeRegion")
            {
                terrainData = GameObject.Find("Art/Terrains/Terrain").GetComponent<Terrain>().terrainData;
            }
            else if(scene == "RuralRegion")
            {
                terrainData = GameObject.Find("Art/Terrain_RuralMain").GetComponent<Terrain>().terrainData;
            }
            else if(scene == "CoastalRegion")
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

    }
}
