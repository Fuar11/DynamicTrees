using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using MelonLoader;
using DynamicTrees.DynamicTreesComponent;
using DynamicTrees.Utilities.JSON;
using MelonLoader.Utils;

namespace DynamicTrees.Utilities
{
    public class SaveDataManager
    {
        //ModDataManager dm = new ModDataManager("Improved Trees", false);

        public async Task Save(DynamicTreeSaveDataProxy data)
        {
            await JsonFile.SaveAsync<DynamicTreeSaveDataProxy>($"{MelonEnvironment.ModsDirectory}/DynamicTrees.json", data);

            //string? dataString;
            //dataString = await JsonSerializer.SerializeAsync<DynamicTreeSaveDataProxy>(data);
            //dm.Save(dataString);
        }


		public async Task<DynamicTreeSaveDataProxy?> Load()
        {
            //string? dataString = dm.Load();
            //if (dataString is null) return null;

            //DynamicTreeSaveDataProxy? data = await JsonSerializer.DeserializeAsync<DynamicTreeSaveDataProxy>(dataString);
            //return data;
            return await JsonFile.LoadAsync<DynamicTreeSaveDataProxy>($"{MelonEnvironment.ModsDirectory}/DynamicTrees.json", true);
        }

    }
}
