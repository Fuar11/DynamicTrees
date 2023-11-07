using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2Cpp;

namespace ImprovedTrees.Patches
{
    internal class MainPatches
    {

        [HarmonyPatch(typeof(QualitySettingsManager), nameof(QualitySettingsManager.ApplyCurrentQualitySettings))]

        public class ReplaceTrees
        {

            public static void Postfix()
            {
               
            }

        }

    }
}
