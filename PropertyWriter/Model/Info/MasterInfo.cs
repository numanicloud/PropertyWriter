using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Model.Instance;

namespace PropertyWriter.Model
{
	class MasterInfo
	{
		public Type Type { get; private set; }
		public IPropertyModel Master { get; private set; }

		public MasterInfo(Type type, IPropertyModel master)
		{
			Type = type;
			Master = master;
		}        

		internal static MasterInfo ForGlobal(Type type, ModelFactory modelFactory)
		{
			return new MasterInfo(type, modelFactory.Create(type));
		}

		public static MasterInfo ForMaster(Type type, ModelFactory modelFactory)
		{
			var collectionType = typeof(IEnumerable<>).MakeGenericType(type);
			return new MasterInfo(type, modelFactory.Create(collectionType));
		}
	}
}
