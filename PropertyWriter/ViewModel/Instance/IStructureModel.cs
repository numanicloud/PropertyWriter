using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Model;

namespace PropertyWriter.ViewModel.Instance
{
	interface IStructureModel
	{
		InstanceAndMemberInfo[] Members { get; }
	}
}
