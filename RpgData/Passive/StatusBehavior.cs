using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Annotation;

namespace RpgData.Passive
{
	[PwSubtyping]
	public class PassiveBehavior
	{
		[PwMember("名前")]
		public string Name { get; set; }
		[PwMember("説明")]
		public string Description { get; set; }
	}
}
