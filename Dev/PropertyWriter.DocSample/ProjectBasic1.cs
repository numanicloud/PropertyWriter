using PropertyWriter.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.DocSample.ProjectBasic1
{
	[PwProject]
	public class MyProject
	{
		[PwMaster]
		public int Data { get; set; }
		[PwMaster]
		public string Message { get; set; }
	}
}
