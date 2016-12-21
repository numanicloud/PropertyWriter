using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Models.Properties;

namespace PropertyWriter.ViewModels.Properties
{
	class MultilineStringViewModel : StringViewModel
	{
		public MultilineStringViewModel(StringProperty property)
			: base(property)
		{
		}
	}
}
