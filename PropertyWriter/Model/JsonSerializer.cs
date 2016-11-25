using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PropertyWriter.Model
{
    class JsonSerializer
	{
		public static async Task SaveDataAsync(PropertyRoot root, string savePath)
		{
			using(var file = new StreamWriter(savePath))
			{
				var json = JsonConvert.SerializeObject(root.Structure.Value.Value, Formatting.Indented);
				await file.WriteLineAsync(json);
			}
		}

        public static async Task LoadDataAsync(PropertyRoot root, string savePath)
        {
            object obj;
            using (var file = new StreamReader(savePath))
            {
                obj = JsonConvert.DeserializeObject(await file.ReadToEndAsync(), root.Type);
            }
            await ModelConverter.LoadValueToRootAsync(root, obj);
        }
	}
}
