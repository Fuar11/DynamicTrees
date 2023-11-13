using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using ModData;
using MelonLoader;
using DynamicTrees.DynamicTreesComponent;

namespace DynamicTrees.Utilities
{
    internal class SaveDataManager
    {
        ModDataManager dm = new ModDataManager("Improved Trees", false);

        public void Save(DynamicTreeSaveDataProxy data)
        {
            string? dataString;
            dataString = JsonSerializer.Serialize<DynamicTreeSaveDataProxy>(data);
            dm.Save(dataString);
        }

        public DynamicTreeSaveDataProxy Load()
        {
            string? dataString = dm.Load();
            if (dataString is null) return null;

            DynamicTreeSaveDataProxy? data = JsonSerializer.Deserialize<DynamicTreeSaveDataProxy>(dataString);
            return data;
        }

    }
}
