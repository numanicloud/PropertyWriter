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

		public static MasterInfo ForGlobal(Type type)
		{
			return new MasterInfo(type, InstanceFactory.Create(type));
		}

		public static MasterInfo ForMaster(Type type)
		{
			var collectionType = typeof(IEnumerable<>).MakeGenericType(type);
			return new MasterInfo(type, InstanceFactory.Create(collectionType));
		}

		public static MasterInfo ForSubtypingMaster(Type type)
		{
			var model = new BasicCollectionModel(type);
			return new MasterInfo(type, model);
		}
	}
}
