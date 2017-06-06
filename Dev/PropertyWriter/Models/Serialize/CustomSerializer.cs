using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using PropertyWriter.Models.Exceptions;

namespace PropertyWriter.Models.Serialize
{
	class CustomSerializer
	{
		public static async Task LoadDataAsync(MethodInfo deserializer, PropertyRoot root, string savePath)
		{
			object value;
			using (var file = File.OpenRead(savePath))
			{
				try
				{
					value = deserializer.Invoke(null, new object[] { file });
				}
				catch (ArgumentException)
				{
					throw new PwSerializeMethodException("デシリアライズ メソッドのシグネチャが不正です。");
				}
				catch (TargetParameterCountException)
				{
					throw new PwSerializeMethodException("デシリアライズ メソッドのシグネチャが不正です。");
				}

				if (value is Task t)
				{
					await t;
					var resultProperty = typeof(Task<>).MakeGenericType(root.Type)
						.GetProperty(nameof(Task<object>.Result));
					value = resultProperty.GetValue(t);
				}
				if (value == null)
				{
					throw new Exception("データ ファイルが壊れています。");
				}
			}
			var converter = new ModelConverter();
			await converter.LoadValueToRootAsync(root, value);
		}

		public static async Task SaveDataAsync(MethodInfo serializer, PropertyRoot root, string savePath)
		{
			var tempFilePath = savePath + ".tmp";
			using (var file = File.OpenWrite(tempFilePath))
			{
				var value = root.Structure.Value.Value;
				try
				{
					var ret = serializer.Invoke(null, new object[] { value, file });
					if (ret != null && ret is Task t)
					{
						await t;
					}
				}
				catch (ArgumentException)
				{
					throw new PwSerializeMethodException("シリアライズ用メソッドのシグネチャが不正です。");
				}
			}
			File.Delete(savePath);
			File.Move(tempFilePath, savePath);
		}
	}
}
