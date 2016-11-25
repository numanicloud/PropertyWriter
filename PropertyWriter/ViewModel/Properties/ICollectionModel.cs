using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Model.Instance;

namespace PropertyWriter.ViewModel.Instance
{
	interface ICollectionModel
	{
		IPropertyViewModel AddNewElement();
	}
}
