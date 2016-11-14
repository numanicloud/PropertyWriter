using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model
{
	class PwJsonDeserializeException : Exception
	{
	}

	class PwObjectMissmatchException : PwJsonDeserializeException
	{
		public PwObjectMissmatchException(string modelName, string propertyName)
		{
			ObjectName = modelName;
			PropertyName = propertyName;
		}

		public string ObjectName { get; }
		public string PropertyName { get; }

		public override string Message => $"<{ObjectName}> の \"{PropertyName}\" に対応するデータがJson内に見つかりませんでした。";
	}

	class PwSubtypeNotFoundException : PwJsonDeserializeException
	{
		public PwSubtypeNotFoundException(string propertyName, string typeName)
		{
			PropertyName = propertyName;
			TypeName = typeName;
		}

		public string PropertyName { get; }
		public string TypeName { get; private set; }

		public override string Message => $"<{PropertyName}> に設定できる \"{TypeName}\" 型のオブジェクトがJson内に見つかりませんでした。";
	}
}
