using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using PropertyWriter.Model.Instance;
using PropertyWriter.ViewModel;
using PropertyWriter.ViewModel.Instance;

namespace PropertyWriter.Model
{
	class JsonSerializer
	{
		public static async Task SaveData(RootViewModel root, string savePath)
		{
			using(var file = new StreamWriter(savePath))
			{
				var json = JsonConvert.SerializeObject(root.Structure.Value.Value, Formatting.Indented);
				await file.WriteLineAsync(json);
			}
		}

        public static async Task LoadData(RootViewModel root, string savePath)
        {
            object obj;
            using (var file = new StreamReader(savePath))
            {
                obj = JsonConvert.DeserializeObject(await file.ReadToEndAsync(), root.Type);
            }
            await ModelConverter.LoadValueToRoot(root, obj);
        }
	}
}
