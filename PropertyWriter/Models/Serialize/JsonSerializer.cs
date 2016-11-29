using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyWriter.Models.Serialize
{
    class JsonSerializer
	{
		private static readonly DataContractJsonSerializerSettings Settings =
			new DataContractJsonSerializerSettings
			{
				UseSimpleDictionaryFormat = true,
			};

		public static async Task SaveDataAsync(PropertyRoot root, string savePath)
		{
			var tempFilePath = savePath + ".tmp";
			try
			{
				using (var file = File.Open(tempFilePath, FileMode.Create))
				{
					var currentCulture = Thread.CurrentThread.CurrentCulture;
					Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
					try
					{
						using (var writer = JsonReaderWriterFactory.CreateJsonWriter(
							file, Encoding.UTF8, true, true, "\t"))
						{
							var serializer = new DataContractJsonSerializer(root.Type, Settings);
							serializer.WriteObject(writer, root.Structure.Value.Value);
							await Task.Run(() => writer.Flush());
						}
					}
					finally
					{
						Thread.CurrentThread.CurrentCulture = currentCulture;
					}
				}
				File.Delete(savePath);
				File.Move(tempFilePath, savePath);
			}
			finally
			{
				//File.Delete(tempFilePath);
			}
		}

        public static async Task LoadDataAsync(PropertyRoot root, string savePath)
		{
			object obj;
			using (var file = File.OpenRead(savePath))
			{
				var currentCulture = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
				try
				{
					var serializer = new DataContractJsonSerializer(root.Type, Settings);
					obj = serializer.ReadObject(file);
					if(obj == null)
					{
						throw new Exception("読み込みに失敗しました。");
					}
				}
				finally
				{
					Thread.CurrentThread.CurrentCulture = currentCulture;
				}
			}
            await ModelConverter.LoadValueToRootAsync(root, obj);
        }
	}
}
