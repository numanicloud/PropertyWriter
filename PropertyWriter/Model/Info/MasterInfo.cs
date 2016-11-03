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
		public string Name { get; private set; }
		public IPropertyModel Master { get; private set; }

		public MasterInfo(Type type, IPropertyModel master, string name)
		{
			Type = type;
			Master = master;
			Name = name ?? type.Name;
		}        

		internal static MasterInfo ForGlobal(Type type, ModelFactory modelFactory, string name)
		{
			return new MasterInfo(type, modelFactory.Create(type), name);
		}

		public static MasterInfo ForMaster(Type type, ModelFactory modelFactory, string name)
		{
			var collectionType = typeof(IEnumerable<>).MakeGenericType(type);
			return new MasterInfo(type, modelFactory.Create(collectionType), name);
		}
	}
}
