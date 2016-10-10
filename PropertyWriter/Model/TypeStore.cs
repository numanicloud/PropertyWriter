using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model
{
	class TypeStore
	{
		public Assembly Assembly { get; set; }

		private Dictionary<Type, List<Type>> typeList_;

		public IEnumerable<Type> GetDerivedType( Type baseType )
		{
			return Assembly.DefinedTypes
					.Where( _ => _.IsAssignableFrom( baseType ) )
					.ToArray();
		}
	}
}
