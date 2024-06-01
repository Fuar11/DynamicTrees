// ---------------------------------------------
// JsonFile - by The Illusion
// ---------------------------------------------
// Reusage Rights ------------------------------
// You are free to use this script or portions of it in your own mods, provided you give me credit in your description and maintain this section of comments in any released source code
//
// Warning !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
// Ensure you change the namespace to whatever namespace your mod uses, so it doesnt conflict with other mods
// ---------------------------------------------
using System.Text.Json;
using System.Text.Json.Serialization;

using DynamicTrees.Utilities.Exceptions;

namespace DynamicTrees.Utilities.JSON
{
	/// <summary>
	/// 
	/// </summary>
	public class JsonFile
	{
		/// <summary>
		/// Gets the default set of options
		/// </summary>
		/// <returns></returns>
		public static JsonSerializerOptions GetDefaultOptions()
		{
			return new JsonSerializerOptions()
			{
				WriteIndented = true,   // pretty print
				IncludeFields = true    // use [JsonInclude] on properties you want to include, otherwise it wont be
			};
		}

		#region Syncronous
		/// <summary>
		/// Save the JSON file
		/// </summary>
		/// <typeparam name="T">The class reference</typeparam>
		/// <param name="configFileName">The absolute path of the file to save</param>
		/// <param name="Tinput">An instance of <typeparamref name="T"/> with the data you want to save</param>
		/// <param name="options">OPTIONAL: Options to use when saving the file. You MUST use the same options to load the file</param>
		/// <exception cref="BadMemeException"></exception>
		public static void Save<T>(string configFileName, T Tinput, JsonSerializerOptions? options = null)
		{
			try
			{
				options ??= GetDefaultOptions();
				using FileStream file = File.Open(configFileName, FileMode.Create, FileAccess.Write, FileShare.None);
				JsonSerializer.Serialize<T>(file, Tinput, options);
				file.Dispose();
			}
			catch (System.Exception e)
			{
				throw new BadMemeException($"Attempting to save {configFileName} failed", e);
			}
		}

		/// <summary>
		/// Load a JSON file
		/// </summary>
		/// <typeparam name="T">The class reference</typeparam>
		/// <param name="configFileName">The absolute path of the file to save</param>
		/// <param name="createFile">Create the file if it doesnt exist</param>
		/// <param name="options">OPTIONAL: Options to use when saving the file. You MUST use the same options to save the file</param>
		/// <returns>An instance of <typeparamref name="T"/> with the data desearalized from the JSON file</returns>
		/// <exception cref="BadMemeException"></exception>
		public static T? Load<T>(string configFileName, bool createFile = false, JsonSerializerOptions? options = null)
		{
			if (!File.Exists(configFileName))
			{
				if (createFile)
				{
					Save<T>(configFileName, default, options);
				}
				else
				{
					throw new BadMemeException($"JsonFile.Load({configFileName}, {createFile})::Requested JSON file does not exist");
				}
			}
			try
			{
				options ??= GetDefaultOptions();
				using FileStream file = File.Open(configFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
				var output = JsonSerializer.Deserialize<T>(file, options);
				file.Dispose();
				return output;
			}
			catch (System.Exception e)
			{
				throw new BadMemeException($"Attempting to load the config file failed, file: {configFileName}", e);
			}
		}

		#endregion
		#region Async
		/// <summary>
		/// Async load a JSON file
		/// </summary>
		/// <typeparam name="T">The class reference</typeparam>
		/// <param name="configFileName">The absolute path of the file to save</param>
		/// <param name="createFile">Create the file if it doesnt exist</param>
		/// <param name="options">OPTIONAL: Options to use when saving the file. You MUST use the same options to save the file</param>
		/// <returns>An instance of <typeparamref name="T"/> with the data desearalized from the JSON file</returns>
		/// <exception cref="BadMemeException"></exception>
		public static async Task<T?> LoadAsync<T>(string configFileName, bool createFile = false, JsonSerializerOptions? options = null)
		{
			return await Task.Run(() => Load<T>(configFileName, createFile, options));
		}

		/// <summary>
		/// Async save the JSON file
		/// </summary>
		/// <typeparam name="T">The class reference</typeparam>
		/// <param name="configFileName">The absolute path of the file to save</param>
		/// <param name="Tinput">An instance of <typeparamref name="T"/> with the data you want to save</param>
		/// <param name="options">OPTIONAL: Options to use when saving the file. You MUST use the same options to load the file</param>
		/// <exception cref="BadMemeException"></exception>
		public static async Task SaveAsync<T>(string configFileName, T Tinput, JsonSerializerOptions? options = null)
		{
			await Task.Run(() => Save<T>(configFileName, Tinput, options));
		}
		#endregion
	}
}
