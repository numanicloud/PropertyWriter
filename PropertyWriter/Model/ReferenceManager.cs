using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Model.Instance;
using Reactive.Bindings;

namespace PropertyWriter.Model
{
	class ReferenceManager
	{
		public static void SetReferenceForMaster(MasterInfo[] masters)
		{
			foreach (var masterInfo in masters)
			{
				SetReferenceForMaster(masterInfo.Master, masters);
			}
		}

		private static void SetReferenceForMaster(IPropertyModel master, MasterInfo[] masters)
		{
			var classModel = master as ClassModel;
			if (classModel != null)
			{
				foreach (var info in classModel.Members)
				{
					SetReferenceForMaster(info.Model, masters);
				}
				return;
			}

			var structModel = master as StructModel;
			if (structModel != null)
			{
				foreach (var info in structModel.Members)
				{
					SetReferenceForMaster(info.Model, masters);
				}
				return;
			}

			var basicCollectionModel = master as BasicCollectionModel;
			if (basicCollectionModel != null)
			{
				foreach (var model in basicCollectionModel.Collection)
				{
					SetReferenceForMaster(model, masters);
				}
				return;
			}

			var complicateCollectionModel = master as ComplicateCollectionModel;
			if (complicateCollectionModel != null)
			{
				foreach (var model in complicateCollectionModel.Collection)
				{
					SetReferenceForMaster(model, masters);
				}
				return;
			}

			var referenceIntModel = master as ReferenceByIntModel;
			if (referenceIntModel != null)
			{
				var collectionModel = masters.First(x => x.Type == referenceIntModel.Type).Master as ComplicateCollectionModel;
				if (collectionModel != null)
				{
					referenceIntModel.Source = collectionModel.Collection.ToReadOnlyReactiveCollection(x => (object)x);
				}
				return;
			}
		}
	}
}
